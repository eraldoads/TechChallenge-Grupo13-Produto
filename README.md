# TechChallenge-Grupo13-Produto
Este repositório é dedicado ao microsserviço de produtos, o qual foi desmembrado do monolito criado para a lanchonete durante a evolução da pós-graduação em Arquitetura de Software da FIAP.

Tanto o build e push para o repositório no ECR da AWS usando Terraform, quanto a análise de código e cobertura de testes utilizando SonarCloud são realizados via Github Actions.

## 🖥️ Grupo 13 - Integrantes
🧑🏻‍💻 *<b>RM352133</b>*: Eduardo de Jesus Coruja </br>
🧑🏻‍💻 *<b>RM352316</b>*: Eraldo Antonio Rodrigues </br>
🧑🏻‍💻 *<b>RM352032</b>*: Luís Felipe Amengual Tatsch </br>

## Arquitetura
Quando disparamos a Github Action, é realizado o build da aplicação e o push para o repositório criado previamente no Elastic Container Registry (ECS).
Ao final da action, é atualizada a Service no Elastic Container Service (ECS), executando assim a service que irá realizar a criação do container.

![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/47857203/e5c163d0-8d81-4f8d-8f6f-3d039cbe917b)

Para este microsserviço, utilizamos .NET 8.0, o que também representa uma evolução de tecnologia em relação ao monolito, o qual foi baseado no .NET 6.0 .

## Testes

Utilizamos a ferramenta SonarCloud para análise de código e cobertura de testes. Para este microsserviço, atingimos 100% de cobertura, conforme abaixo:

https://sonarcloud.io/summary/overall?id=eraldoads_TechChallenge-Grupo13-Produto

![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/47857203/7260d1c5-a352-4866-bd3e-6b576f4fe3a4)

### BDD – Behavior-Driven Development
Para organizar o código e armazenar os cenários de testes com a técnica BDD (Desenvolvimento Orientado por Comportamento) criou-se um projeto baseado na ferramenta SpecFlow que trata-se de uma implementação oficial do Cucumber para .NET que utiliza o Gherkin como analisador. Neste exemplo foi configurado em conjunto com o framework nUnit.

#### Organização do Teste
Um novo projeto, chamado <b>ProdutoTestBDD</b> foi adicionado à solução na pasta BDD dentro da estrutura de Testes.
Arquivo de configuração do projeto ProdutoTestBDD
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/de1ae9f1-35b3-4f41-a64d-7a1f52c97fde)

O arquivo <b>Produto.feature</b> armazena as funcionalidades (features) que serão utilizadas como base para a validação da API Produto. Para efeito de estudo, foi definido o cenário que realiza a busca de um Produto a partir de um ID informado.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/8e1b7d41-3e60-4c06-be73-bdebf98a900d)
 
O arquivo <b>GerenciarProdutosStepDefinitions.cs</b> contém os passos que serão executados para validar os cenários definidos nas features. No exemplo, há três métodos implementados para validar o cenário.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/7f03c3aa-84f2-4cfe-a74b-ff66079a8f80)
 
##### GivenQueUmProdutoJaFoiCadastradoAsync
Implementa os passos que serão realizados para atender o que foi estabelecido no <b>“Dado”</b>.
Neste exemplo o método é responsável por inserir um Produto para garantir que o Cenário de Busca seja concluído com sucesso.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/3782dd9c-759d-4b9f-892b-69cf06ee7772)
 
##### WhenRequisitarABuscaDoProdutoPorIDAsync
Implementa os passos que serão realizados para atender o que foi estabelecido no <b>“Quando”</b>.
Neste exemplo o método realiza a busca pelo Produto utilizando o ID.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/046fc703-56a5-4eb5-b6ac-8eba00168f69)
 
##### ThenOProdutoEExibidoComSucesso
Implementa os passos que serão realizados para atender o que foi estabelecido no <b>“Então”</b>.
Neste exemplo o método confere se o Produto exibido é realmente o cadastrado anteriormente.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/4a0d7c6c-515e-4273-abbe-1a1b490483c0)
 
#### Execução dos Testes
A imagem a seguir apresenta o resultado da execução de todos os testes que a solução possui, destacando o cenário definido em BDD BuscaProdutoPorID, bem como o detalhe dos passos realizados.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Produto/assets/149120484/a63fc77b-b495-4937-b276-a9f5174aa7e2)
 
