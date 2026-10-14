#include "TopOptNative.h"

extern "C" TOPOPT_API double ExecutarOtimizacao(DadosOtimizacao dados, double* resultados) {
    // Exemplo: preencher o array de resultados com um valor base
    for (int i = 0; i < dados.maxIteracoes; i++) {
        resultados[i] =+ 1;
    }
    // Retorna um valor de volume (exemplo)
    return dados.maxIteracoes;
}