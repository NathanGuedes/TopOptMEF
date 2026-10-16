using System.Globalization;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TopOptUI;

public partial class MainWindow
{
    private const int Limite = 500;

    public MainWindow()
    {
        InitializeComponent();
        btGerarMalha_Click(this, null);
    }

    // Aceita vírgula ou ponto como separador decimal
    private static bool Num(string? t, out double v) =>
        double.TryParse((t ?? "").Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out v);

    private static bool Positivo(double v) => v > 0 && !double.IsInfinity(v);

    private static void Aviso(string msg) =>
        MessageBox.Show(msg, "Valores inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);

    private void btGerarMalha_Click(object sender, RoutedEventArgs? e)
    {
        if (!int.TryParse(txtNelx.Text, out var nelx) || !int.TryParse(txtNely.Text, out var nely) ||
            nelx < 1 || nely < 1 || nelx > Limite || nely > Limite)
        {
            Aviso($"Informe inteiros entre 1 e {Limite} para nelx e nely.");
            return;
        }

        if (!Num(txtLx.Text, out var lx) || !Num(txtLy.Text, out var ly) || !Positivo(lx) || !Positivo(ly))
        {
            Aviso("Informe valores numéricos maiores que zero para o comprimento (Lx) e a largura (Ly).");
            return;
        }

        plotView.Model = CriarModeloMalha(nelx, nely, lx, ly);

        var ci = CultureInfo.CurrentCulture;
        lblInfo.Text =
            $"{nelx * nely} elementos\n" +
            $"{(nelx + 1) * (nely + 1)} nós\n" +
            $"Elemento: {(lx / nelx).ToString("G4", ci)} × {(ly / nely).ToString("G4", ci)}";
    }

    private static LineSeries Linha(OxyColor cor, double espessura) => new() { Color = cor, StrokeThickness = espessura };

    // Adiciona um segmento isolado (o ponto indefinido interrompe a linha)
    private static void Segmento(LineSeries s, double x1, double y1, double x2, double y2)
    {
        s.Points.Add(new DataPoint(x1, y1));
        s.Points.Add(new DataPoint(x2, y2));
        s.Points.Add(DataPoint.Undefined);
    }

    private static LinearAxis Eixo(AxisPosition pos, string titulo, bool invertido = false) =>
        new()
        {
            Position = pos,
            Title = titulo,
            MinimumPadding = 0.05,
            MaximumPadding = 0.05,
            StartPosition = invertido ? 1 : 0,
            EndPosition = invertido ? 0 : 1
        };

    private static PlotModel CriarModeloMalha(int nelx, int nely, double lx, double ly)
    {
        double dx = lx / nelx, dy = ly / nely;

        var modelo = new PlotModel
        {
            Title = $"Malha ({nelx} × {nely} elementos) — {lx:G4} × {ly:G4}",
            PlotType = PlotType.Cartesian // mantém a proporção real Lx : Ly
        };
        modelo.Axes.Add(Eixo(AxisPosition.Bottom, "x"));
        modelo.Axes.Add(Eixo(AxisPosition.Left, "y", invertido: true));

        // Linhas da malha
        var malha = Linha(OxyColors.LightGray, 1);
        for (var i = 0; i <= nelx; i++) Segmento(malha, i * dx, 0, i * dx, ly);
        for (var j = 0; j <= nely; j++) Segmento(malha, 0, j * dy, lx, j * dy);

        // Contorno do domínio
        var contorno = Linha(OxyColors.Black, 2);
        foreach (var (x, y) in new[] { (0.0, 0.0), (lx, 0), (lx, ly), (0, ly), (0, 0) })
            contorno.Points.Add(new DataPoint(x, y));

        // Engaste (parede esquerda, x = 0)
        var engaste = Linha(OxyColors.Firebrick, 5);
        Segmento(engaste, 0, 0, 0, ly);

        // Hachuras da parede fixa (no máximo 20)
        var hachuras = Linha(OxyColors.Firebrick, 1.5);
        var n = Math.Min(nely, 20);
        for (var k = 0; k <= n; k++)
        {
            var y = k * ly / n;
            Segmento(hachuras, 0, y, -lx * 0.02, y + ly * 0.02);
        }

        modelo.Series.Add(malha);
        modelo.Series.Add(contorno);
        modelo.Series.Add(engaste);
        modelo.Series.Add(hachuras);
        return modelo;
    }
}