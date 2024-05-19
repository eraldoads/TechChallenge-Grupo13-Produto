using Domain.Entities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ProdutoTestBDD.StepDefinitions
{
    [Binding]
    public class GerenciarProdutosStepDefinitions
    {     
        
        private String ENDPOINT_PRODUTO = "http://localhost:5008/produto";


        /// <summary>
        /// Método que realiza o cadastro de um Produto para que seja utilizado na busca posterior
        /// </summary>
        [Given(@"que um Produto já foi cadastrado")]
        public async Task GivenQueUmProdutoJaFoiCadastradoAsync()
        {            
            var client = new HttpClient();

            // prepara o request com o endpoint base
            var request = new HttpRequestMessage(HttpMethod.Post, ENDPOINT_PRODUTO);

            // prepara o conteúdo a ser enviado no POST
            var content = new StringContent("{\r\n    \"NomeProduto\":\"Hamburguer PiklesFood 01\",\r\n    \"DescricaoProduto\":\"Hamburguer tradicional com alface, queijo e molho especial\",\r\n    \"ImagemProduto\":\"http://piklesfastfood.com.br/images/hamburguer01.png\",\r\n    \"ValorProduto\":30\r\n,\r\n    \"IdCategoria\":1\r\n}", null, "application/json");
            request.Content = content;

            // realiza o cadastro do Produto
            var response = await client.SendAsync(request);

            // armazena na variável o retorno do POST
            var rtn = JsonConvert.DeserializeObject<Produto>(await response.Content.ReadAsStringAsync());

            // valida o status retornado no POST com o esperado (201)
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);

            // armazena no contexto o idProduto do Produto que foi cadastrado
            ScenarioContext.Current["idProduto"] = rtn.IdProduto;
                                   
        }

        /// <summary>
        /// Método que realiza a busca do Produto cadastrado por seu ID
        /// </summary>
        /// <returns></returns>
        [When(@"requisitar a busca do Produto por ID")]
        public async Task WhenRequisitarABuscaDoProdutoPorIDAsync()
        {            
            var client = new HttpClient();
            
            // inicializa a variável com o idProduto retornado no cadastro realizado anteriormente
            var id = ScenarioContext.Current["idProduto"];

            // monta o endpoint que será utilizado no GET, incluindo o parâmetro a ser utilizado na busca
            var request = new HttpRequestMessage(HttpMethod.Get, ENDPOINT_PRODUTO + "/" + id);

            // realiza o GET
            var response = await client.SendAsync(request);

            // converte o retorno para Produto
            var rtn = JsonConvert.DeserializeObject<Produto>(await response.Content.ReadAsStringAsync());

            // armazena no contexto o conteúdo retornado para utilizar posteriormente na validação
            ScenarioContext.Current["Produto"] = rtn;

            // verifica se o Status do retorno da requisição é o esperado (200)
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
                        
            //return rtn;
        }

        /// <summary>
        /// Método que valida se o Produto retornado é realmente o que foi cadastrado anteriormente
        /// </summary>
        [Then(@"o Produto é exibido com sucesso")]
        public void ThenOProdutoEExibidoComSucesso()
        {
            // inicializa a variável com o Produto retornado na consulta anterior
            var produtoRetornado = (Produto)ScenarioContext.Current["Produto"];

            // inicializa a variável com o idProduto do Produto cadastrado no primeiro step
            var idProdutoInicial = (int)ScenarioContext.Current["idProduto"];

            // valida se o id do Produto cadastrado inicialmente é igual ao id do Produto retornado na consulta
            Assert.True(produtoRetornado.IdProduto == idProdutoInicial);
        }

        /// <summary>
        /// Método executado após o Cenário @tag1 para eliminar o registro criado no primeiro step
        /// </summary>
        [AfterScenario("@tag1")]
        public async Task AfterScenarioAsync()
        {
            var client = new HttpClient();

            // inicializa a variável com o idCliente retornado no cadastro realizado anteriormente
            var id = ScenarioContext.Current["idCliente"];

            // monta o endpoint que será utilizado no DELETE, incluindo o parâmetro a ser utilizado
            var request = new HttpRequestMessage(HttpMethod.Delete, ENDPOINT_PRODUTO + "/" + id);

            // realiza o DELETE
            var response = await client.SendAsync(request);

            // verifica se o Status do retorno da requisição é o esperado (200)
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

        }

    }
}
