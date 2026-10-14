using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TopOptUI
{
    public partial class MainWindow : Window
    {
        private const int LimiteElementos = 500;

        public MainWindow()
        {
            InitializeComponent();
            btGerarMalha_Click(this, new RoutedEventArgs());
        }

        private bool LerMalha(out int nelx, out int nely)
        {
            nelx = nely = 0;
            if (!int.TryParse(txtNelx.Text, out nelx) || !int.TryParse(txtNely.Text, out nely) ||
                nelx < 1 || nely < 1 || nelx > LimiteElementos || nely > LimiteElementos)
            {
                MessageBox.Show(
                    $"Informe inteiros entre 1 e {LimiteElementos} para nelx e nely.",
                    "Valores inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btGerarMalha_Click(object sender, RoutedEventArgs e)
        {
            if (!LerMalha(out int nelx, out int nely)) return;

            plotView.Model = CriarModeloMalha(nelx, nely);
            lblInfo.Text = $"{nelx * nely} elementos\n{(nelx + 1) * (nely + 1)} nós";
        }

        private static PlotModel CriarModeloMalha(int nelx, int nely)
        {
            var modelo = new PlotModel
            {
                Title = $"Malha ({nelx} × {nely})",
                PlotType = PlotType.Cartesian 
            };

            // Paddings retornados para 0.05 já que a seta foi removida
            modelo.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "x",
                MinimumPadding = 0.05, 
                MaximumPadding = 0.05
            });
            modelo.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "y",
                StartPosition = 1, 
                EndPosition = 0,   
                MinimumPadding = 0.05,
                MaximumPadding = 0.05
            });

            // 1. Linhas internas da malha
            var interior = new LineSeries { Color = OxyColors.LightGray, StrokeThickness = 1 };
            for (int i = 0; i <= nelx; i++)
            {
                interior.Points.Add(new DataPoint(i, 0));
                interior.Points.Add(new DataPoint(i, nely));
                interior.Points.Add(DataPoint.Undefined);
            }
            for (int j = 0; j <= nely; j++)
            {
                interior.Points.Add(new DataPoint(0, j));
                interior.Points.Add(new DataPoint(nelx, j));
                interior.Points.Add(DataPoint.Undefined);
            }
            modelo.Series.Add(interior);

            // 2. Contorno do domínio
            var contorno = new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 };
            contorno.Points.Add(new DataPoint(0, 0));
            contorno.Points.Add(new DataPoint(nelx, 0));
            contorno.Points.Add(new DataPoint(nelx, nely));
            contorno.Points.Add(new DataPoint(0, nely));
            contorno.Points.Add(new DataPoint(0, 0));
            modelo.Series.Add(contorno);

            // 3. Condição de Contorno: Engaste (Parede do lado esquerdo x = 0)
            var engaste = new LineSeries 
            { 
                Color = OxyColors.Firebrick, 
                StrokeThickness = 5,
                Title = "Engaste" 
            };
            engaste.Points.Add(new DataPoint(0, 0));
            engaste.Points.Add(new DataPoint(0, nely));
            modelo.Series.Add(engaste);

            // Adiciona pequenos "hachurados" para ilustrar a parede fixa
            var hachuras = new LineSeries { Color = OxyColors.Firebrick, StrokeThickness = 1.5 };
            for (double y = 0; y <= nely; y += Math.Max(1, nely / 20.0))
            {
                hachuras.Points.Add(new DataPoint(0, y));
                hachuras.Points.Add(new DataPoint(-nelx * 0.02, y + nely * 0.02));
                hachuras.Points.Add(DataPoint.Undefined);
            }
            modelo.Series.Add(hachuras);

            return modelo;
        }
    }
}