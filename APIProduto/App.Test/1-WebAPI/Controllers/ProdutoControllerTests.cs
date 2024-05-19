using API.Controllers;
using Application.Interfaces;
using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Domain.Tests._1_WebAPI
{
    public class ProdutoControllerTests
    {
        private readonly ProdutoController _controller;
        private readonly Mock<IProdutoService> _AppService = new();

        public ProdutoControllerTests()
        {
            _controller = new ProdutoController(_AppService.Object);
        }

        #region [GET]
        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "BuscarListaProdutos OkResult")]
        public async Task GetProdutos_ReturnsOkResult_BuscarListaProdutos()
        {
            // Arrange
            _AppService.Setup(service => service.GetProdutos())
                .ReturnsAsync([new(), new()]);

            // Act
            var result = await _controller.GetProdutos();

            // Assert
            Assert.NotNull(result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "BuscarListaProdutos NoContentResult")]
        public async Task GetProdutos_ReturnsNoContentResult_BuscarListaProdutos()
        {
            // Arrange
            _AppService.Setup(service => service.GetProdutos())
                .ReturnsAsync([]);

            // Act
            var result = await _controller.GetProdutos();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProduto ReturnsOkResult")]
        public async Task GetProduto_ReturnsOkResult()
        {
            // Arrange
            int testId = 1;
            ProdutoLista produto = new()
            {
                IdProduto = 1,
                NomeProduto = "Produto Teste",
                ValorProduto = 10.99f,
                DescricaoProduto = "Descrição do produto teste",
                IdCategoria = 5,
                NomeCategoria = "Categoria Teste",
                ImagemProduto = "Imagem do produto teste"
            };

            _AppService.Setup(service => service.GetProdutoById(testId))
                .ReturnsAsync(produto);

            // Act
            var result = await _controller.GetProduto(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProdutoLista?>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(produto.IdProduto, actionResult.Value.IdProduto);
            Assert.Equal(produto.NomeProduto, actionResult.Value.NomeProduto);
            Assert.Equal(produto.ValorProduto, actionResult.Value.ValorProduto);
            Assert.Equal(produto.DescricaoProduto, actionResult.Value.DescricaoProduto);
            Assert.Equal(produto.IdCategoria, actionResult.Value.IdCategoria);
            Assert.Equal(produto.ImagemProduto, actionResult.Value.ImagemProduto);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProduto ReturnsNoContentResult")]
        public async Task GetProduto_ReturnsNoContentResult()
        {
            // Arrange
            int testId = 1;
            ProdutoLista? produto = null;
            _AppService.Setup(service => service.GetProdutoById(testId))
                .ReturnsAsync(produto);

            // Act
            var result = await _controller.GetProduto(testId);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }


        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProdutosPorIdCategoria ReturnsOkResult")]
        public async Task GetProdutosPorIdCategoria_ReturnsOkResult()
        {
            // Arrange
            EnumCategoria testIdCategoria = EnumCategoria.Lanche; // Substitua pelo valor correto
            List<Categoria> categorias =
            [
                new Categoria { IdCategoria = 1, NomeCategoria = "Lanche" },
                // Adicione mais categorias se necessário
            ];

            _AppService.Setup(service => service.GetProdutosByIdCategoria(testIdCategoria))
                .ReturnsAsync(categorias);

            // Act
            var result = await _controller.GetProdutosPorIdCategoria(testIdCategoria);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Categoria>>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(categorias.Count, actionResult.Value.Count);

        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProdutosPorIdCategoria ReturnsBadRequest")]
        public async Task GetProdutosPorIdCategoria_ReturnsBadRequest()
        {
            // Arrange
            EnumCategoria? testIdCategoria = null;

            // Act
            var result = await _controller.GetProdutosPorIdCategoria(testIdCategoria);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProdutosPorIdCategoria ReturnsNoContent")]
        public async Task GetProdutosPorIdCategoria_ReturnsNoContent()
        {
            // Arrange
            EnumCategoria testIdCategoria = EnumCategoria.Lanche; // Substitua pelo valor correto
            List<Categoria> categorias = new List<Categoria>();

            _AppService.Setup(service => service.GetProdutosByIdCategoria(testIdCategoria))
                .ReturnsAsync(categorias);

            // Act
            var result = await _controller.GetProdutosPorIdCategoria(testIdCategoria);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "GetProdutosPorIdCategoria ReturnsNoContent Null")]
        public async Task GetProdutosPorIdCategoria_ReturnsNoContent_WhenNull()
        {
            // Arrange
            EnumCategoria testIdCategoria = EnumCategoria.Lanche; // Substitua pelo valor correto
            List<Categoria> categorias = null;

            _AppService.Setup(service => service.GetProdutosByIdCategoria(testIdCategoria))
                .ReturnsAsync(categorias);

            // Act
            var result = await _controller.GetProdutosPorIdCategoria(testIdCategoria);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
        #endregion

        #region [POST]
        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PostProduto ReturnsBadRequest WhenNull")]
        public async Task PostProduto_ReturnsBadRequest_WhenNull()
        {
            // Arrange
            ProdutoDTO produtoDTO = null;

            // Act
            var result = await _controller.PostProduto(produtoDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PostProduto BadRequestResult ValidationException")]
        public async Task PostProduto_ReturnsBadRequestResult_ValidationException()
        {
            // Arrange
            var ProdutoDTO = new EntitiesDTO.ProdutoDTO
            {
                IdCategoria = 1,
                NomeProduto = "TesteNome",
                ValorProduto = 10.99f,
                DescricaoProduto = "Descrição do produto teste",
                ImagemProduto = "Imagem do produto teste"
            };

            _AppService.Setup(service => service.PostProduto(ProdutoDTO))
                .Throws(new ValidationException("Erro de validação"));

            // Act
            var result = await _controller.PostProduto(ProdutoDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Erro de validação", badRequestResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PostProduto ReturnsCreatedResult")]
        public async Task PostProduto_ReturnsCreatedResult()
        {
            // Arrange
            ProdutoDTO produtoDTO = new ProdutoDTO { /* preencha os campos necessários */ };
            Produto novoProduto = new Produto { /* preencha os campos necessários */ };

            _AppService.Setup(service => service.PostProduto(produtoDTO))
                .ReturnsAsync(novoProduto);

            // Act
            var result = await _controller.PostProduto(produtoDTO);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(novoProduto, actionResult.Value);
            // Adicione mais verificações se necessário
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PostProduto ReturnsStatusCodeResult WhenPreconditionFailedException")]
        public async Task PostProduto_ReturnsStatusCodeResult_WhenPreconditionFailedException()
        {
            // Arrange
            ProdutoDTO produtoDTO = new ProdutoDTO { /* preencha os campos necessários */ };

            _AppService.Setup(service => service.PostProduto(produtoDTO))
                .Throws(new PreconditionFailedException("Erro de pré-condição", "Detalhes do erro"));

            // Act
            var result = await _controller.PostProduto(produtoDTO);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(412, objectResult.StatusCode);
        }
        #endregion

        #region [PATCH]
        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PatchProduto NoContentResult")]
        public async Task PatchProduto_ReturnsNoContentResult()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var patchDoc = new JsonPatchDocument<Produto>(); // preencha com operações de patch de teste
            _AppService.Setup(service => service.PatchProduto(id, patchDoc))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PatchProduto(id, patchDoc);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PatchProduto NotFoundResult ResourceNotFoundException")]
        public async Task PatchProduto_ReturnsNotFoundResult_ResourceNotFoundException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var patchDoc = new JsonPatchDocument<Produto>(); // preencha com operações de patch de teste
            _AppService.Setup(service => service.PatchProduto(id, patchDoc))
                .Throws(new ResourceNotFoundException("Produto não encontrado"));

            // Act
            var result = await _controller.PatchProduto(id, patchDoc);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto não encontrado", notFoundResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PatchProduto BadRequestResult ValidationException")]
        public async Task PatchProduto_ReturnsBadRequestResult_ValidationException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var patchDoc = new JsonPatchDocument<Produto>(); // preencha com operações de patch de teste
            _AppService.Setup(service => service.PatchProduto(id, patchDoc))
                .Throws(new ValidationException("Erro de validação"));

            // Act
            var result = await _controller.PatchProduto(id, patchDoc);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro de validação", badRequestResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PatchProduto StatusCode500Result Exception")]
        public async Task PatchProduto_ReturnsStatusCode500Result_Exception()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var patchDoc = new JsonPatchDocument<Produto>(); // preencha com operações de patch de teste
            _AppService.Setup(service => service.PatchProduto(id, patchDoc))
                .Throws(new Exception());

            // Act
            var result = await _controller.PatchProduto(id, patchDoc);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
        #endregion

        #region [PUT]
        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PutProduto NoContentResult")]
        public async Task PutProduto_ReturnsNoContentResult()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var ProdutoDTO = new EntitiesDTO.ProdutoDTO(); // preencha com dados de teste
            _AppService.Setup(service => service.PutProduto(id, ProdutoDTO))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutProduto(id, ProdutoDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PutProduto ReturnsNotFound WhenResourceNotFoundException")]
        public async Task PutProduto_ReturnsNotFound_WhenResourceNotFoundException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            ProdutoDTO produtoDTO = new() { /* preencha os campos necessários */ };

            _AppService.Setup(service => service.PutProduto(id, produtoDTO))
                .Throws(new ResourceNotFoundException("Produto não encontrado"));

            // Act
            var result = await _controller.PutProduto(id, produtoDTO);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto não encontrado", notFoundResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PutProduto ReturnsBadRequest WhenValidationException")]
        public async Task PutProduto_ReturnsBadRequest_WhenValidationException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            ProdutoDTO produtoDTO = new() { /* preencha os campos necessários */ };

            _AppService.Setup(service => service.PutProduto(id, produtoDTO))
                .Throws(new ValidationException("Erro de validação"));

            // Act
            var result = await _controller.PutProduto(id, produtoDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro de validação", badRequestResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "PutProduto ReturnsStatusCode500 WhenException")]
        public async Task PutProduto_ReturnsStatusCode500_WhenException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            ProdutoDTO produtoDTO = new() { /* preencha os campos necessários */ };

            _AppService.Setup(service => service.PutProduto(id, produtoDTO))
                .Throws(new Exception());

            // Act
            var result = await _controller.PutProduto(id, produtoDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Ocorreu um erro interno. Por favor, tente novamente mais tarde.", statusCodeResult.Value);
        }

        #endregion

        #region [DELETE]
        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "DeleteProduto NotFoundResult KeyNotFoundException")]
        public async Task DeleteProduto_ReturnsNotFoundResult_KeyNotFoundException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            _AppService.Setup(service => service.DeleteProduto(id))
                .Throws(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<object>(notFoundResult.Value);
            Assert.Equal(new { id, error = "Produto não encontrado" }.ToString(), value.ToString());
        }


        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "DeleteProduto BadRequestResult ValidationException")]
        public async Task DeleteProduto_ReturnsBadRequestResult_ValidationException()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            _AppService.Setup(service => service.DeleteProduto(id))
                .Throws(new ValidationException("Erro de validação"));

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Erro de validação", badRequestResult.Value);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "DeleteProduto StatusCode500Result Exception")]
        public async Task DeleteProduto_ReturnsStatusCode500Result_Exception()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            _AppService.Setup(service => service.DeleteProduto(id))
                .Throws(new Exception());

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "DeleteProduto ReturnsNoContent WhenProdutoIsNull")]
        public async Task DeleteProduto_ReturnsNoContent_WhenProdutoIsNull()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            ProdutoLista deletedProduto = null;

            _AppService.Setup(service => service.DeleteProduto(id))
                .ReturnsAsync(deletedProduto);

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Trait("Categoria", "ProdutoController")]
        [Fact(DisplayName = "DeleteProduto ReturnsProduto WhenProdutoIsNotNull")]
        public async Task DeleteProduto_ReturnsProduto_WhenProdutoIsNotNull()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            ProdutoLista deletedProduto = new ProdutoLista
            {
                IdProduto = 1,
                NomeProduto = "Produto Teste",
                ValorProduto = 10.99f,
                DescricaoProduto = "Descrição do produto teste",
                IdCategoria = 5,
                NomeCategoria = "Categoria Teste",
                ImagemProduto = "Imagem do produto teste"
            };

            _AppService.Setup(service => service.DeleteProduto(id))
                .ReturnsAsync(deletedProduto);

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProdutoLista>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(deletedProduto, actionResult.Value);
        }

        #endregion
    }
}
