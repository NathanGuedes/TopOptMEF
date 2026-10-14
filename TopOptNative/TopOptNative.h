#pragma once

#ifdef TOPOPTNATIVE_EXPORTS
#define TOPOPT_API __declspec(dllexport)
#else
#define TOPOPT_API __declspec(dllimport)
#endif

// Estrutura de dados para sua pesquisa
struct DadosOtimizacao {
    double moduloElasticidade;
    double carga;
    int maxIteracoes;
};

// Exportação limpa para o C#
extern "C" {
    TOPOPT_API double ExecutarOtimizacao(DadosOtimizacao dados, double* resultados);
}