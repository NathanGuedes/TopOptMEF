# Guia de Colaboração

O fluxo é simples: cada pessoa trabalha na sua própria branch e, ao terminar, mescla (merge) direto na `main`. **Não usamos Pull Requests** e **não apagamos branches**, para que o histórico de cada contribuição fique registrado no repositório.

---

## Regras principais

1. **Nunca trabalhe direto na `main`.** Crie sempre uma branch para a sua tarefa.
2. **Não é necessário Pull Request.** Quando terminar, você mesmo mescla sua branch na `main`.
3. **Nunca apague branches**, nem local nem remotamente, mesmo depois do merge. Elas são o histórico do projeto.
4. **Você pode (e deve) atualizar sua branch com as mudanças da `main`** sempre que precisar.
5. **Não use `git push --force`** e evite `git rebase` em branches já enviadas. Isso reescreve o histórico.
6. **Antes de mesclar na `main`, teste** se o projeto continua funcionando.

---

## Passo a passo

### 1. Clonar o repositório (apenas na primeira vez)

Baixe o projeto para o seu computador:

```bash
git clone https://github.com/NathanGuedes/TopOptMEF.git
cd TopOptMEF
```

Depois, configure o Git para usar `merge` ao fazer `git pull` (combina com o fluxo deste guia):

```bash
git config pull.rebase false
```

> Se você já tem o projeto clonado, pule para o passo 2.

### 2. Criar sua branch

Sempre parta da `main` atualizada:

```bash
git checkout main
git pull origin main
git checkout -b seunome/descricao-da-tarefa
```

Exemplos de nomes: `maria/tela-de-login`, `joao/correcao-bug-pagamento`.

### 3. Trabalhar e fazer commits

```bash
git add .
git commit -m "Descreve o que foi feito"
git push origin seunome/descricao-da-tarefa
```

Dicas:
- Faça commits pequenos e frequentes.
- Escreva mensagens claras, explicando **o que** mudou e **por quê**.
- Envie (`push`) sua branch com frequência para não perder trabalho.

### 4. Atualizar sua branch com a `main`

Se outras pessoas mesclaram coisas na `main` enquanto você trabalhava, traga essas mudanças para a sua branch:

```bash
git checkout seunome/descricao-da-tarefa
git fetch origin
git merge origin/main
```

Se houver conflitos, veja a seção [Resolvendo conflitos](#resolvendo-conflitos). Depois envie:

```bash
git push origin seunome/descricao-da-tarefa
```

> Use `merge` (e não `rebase`) para atualizar sua branch. Assim o histórico é preservado.

### 5. Mesclar na `main` (ao terminar)

1. Atualize sua branch com a `main` (passo 4) e **teste tudo**.
2. Vá para a `main` e mescle:

```bash
git checkout main
git pull origin main
git merge --no-ff seunome/descricao-da-tarefa
git push origin main
```

A opção `--no-ff` cria um commit de merge e mantém visível no histórico onde a branch começou e terminou.

3. **Não apague a branch.** Ela continua no repositório e você pode voltar a usá-la.

### 6. Continuar trabalhando depois do merge

Pode reaproveitar a mesma branch ou criar uma nova a partir da `main`. Se reaproveitar, atualize-a primeiro:

```bash
git checkout seunome/descricao-da-tarefa
git merge origin/main
```

---

## Resolvendo conflitos

Conflitos acontecem quando duas pessoas alteram o mesmo trecho de código.

1. O Git avisa quais arquivos estão em conflito (`git status`).
2. Abra os arquivos e procure marcações como:

```
<<<<<<< HEAD
seu código
=======
código vindo da main
>>>>>>> origin/main
```

3. Decida o que manter, remova as marcações e salve.
4. Finalize:

```bash
git add .
git commit
```

Em caso de dúvida sobre qual código manter, **converse com quem fez a outra alteração**.

---

## Resumo rápido

| O que fazer | Comando |
|---|---|
| Clonar o projeto (1ª vez) | `git clone URL-DO-REPOSITORIO` |
| Criar branch | `git checkout -b seunome/tarefa` |
| Enviar alterações | `git push origin seunome/tarefa` |
| Atualizar com a main | `git merge origin/main` |
| Mesclar na main | `git checkout main && git pull && git merge --no-ff seunome/tarefa && git push` |

---

## Configuração do repositório (para administradores)

### Adicionar este guia ao repositório

Coloque o `README.md` na raiz do projeto e envie para a `main`:

```bash
git add README.md
git commit -m "Adiciona guia de colaboração"
git push origin main
```
