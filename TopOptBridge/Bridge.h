#pragma once

// Inclui o cabeçalho do motor nativo para conhecermos a struct e a função
#include "..\TopOptNative\TopOptNative.h" 

using namespace System;
using namespace System::Runtime::InteropServices;

namespace TopOptBridge {

    public ref class Solver
    {
    public:
        // Método que faz a ponte entre o C# (Managed) e o C++ (Native)
        static double Calcular(double modulo, double carga, int iteracoes, array<double>^ resultados)
        {
            // 1. Instancia a struct nativa definida no motor C++
            ::DadosOtimizacao dados;
            dados.moduloElasticidade = modulo;
            dados.carga = carga;
            dados.maxIteracoes = iteracoes;

            // 2. Fixa o array do C# na memória (pinning)
            // Isso impede que o Garbage Collector do .NET mova o array enquanto o C++ escreve nele
            pin_ptr<double> pResultados = &resultados[0];

            // 3. Chama a função real do motor nativo (TopOptNative)
            return ::ExecutarOtimizacao(dados, pResultados);
        }
    };
}