using Application.Interfaces;
using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace App.Test._2_Application.Services
{
    public class ProdutoServiceTests
    {
        private readonly IProdutoService _AppServices;
        private readonly Mock<IProdutoRepository> _Repository = new();

        public ProdutoServiceTests()
        {
            _AppServices = new ProdutoService(_Repository.Object);

        }

        #region [GET]
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "GetProdutos Returna Lista de Produtos OK")]
        public async Task GetProdutos_ReturnsListProdutos()
        {
            // Arrange
            _Repository.Setup(repo => repo.GetProdutos())
                .ReturnsAsync(new List<Produto>
                {
                    new Produto
                    {
                        IdProduto = 1,
                        NomeProduto = "Produto Teste",
                        DescricaoProduto = "Descrição do Produto",
                        ValorProduto = 10.00f,
                        IdCategoria = 1,
                        ImagemProduto = "imagem.jpg"
                    }
                });

            // Act
            var result = await _AppServices.GetProdutos();

            // Assert
            Assert.Single(result);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "GetProdutoById Returna Produto OK")]
        public async Task GetProdutoById_ReturnsProduto()
        {
            // Arrange
            _Repository.Setup(repo => repo.GetProdutoById(It.IsAny<int>()))
                .ReturnsAsync(new Produto
                {
                    IdProduto = 1,
                    NomeProduto = "Produto Teste",
                    DescricaoProduto = "Descrição do Produto",
                    ValorProduto = 10.00f,
                    IdCategoria = 1,
                    Categoria = new Categoria { IdCategoria = 1, NomeCategoria = "Lanche" },
                    ImagemProduto = "imagem.jpg"
                });

            // Act
            var result = await _AppServices.GetProdutoById(1);

            // Assert
            Assert.Equal(1, result.IdProduto);
            Assert.Equal("Produto Teste", result.NomeProduto);
            Assert.Equal("Descrição do Produto", result.DescricaoProduto);
            Assert.Equal(10.00f, result.ValorProduto);
            Assert.Equal(1, result.IdCategoria);
            Assert.Equal("imagem.jpg", result.ImagemProduto);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "GetProdutoById ReturnsNull WhenProdutoIsNull")]
        public async Task GetProdutoById_ReturnsNull_WhenProdutoIsNull()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            Produto produtoBase = null;

            _Repository.Setup(repo => repo.GetProdutoById(id))
                .ReturnsAsync(produtoBase);

            // Act
            var result = await _AppServices.GetProdutoById(id);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region [POST]
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PostProduto Returna Produto OK")]
        public async Task PostProduto_ValidProdutoDTO_ReturnsProduto()
        {
            // Arrange
            var produtoDTO = new ProdutoDTO
            {
                IdCategoria = 1,
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                ImagemProduto = "imagem.jpg"
            };

            _Repository.Setup(repo => repo.PostProduto(It.IsAny<Produto>()))
                .ReturnsAsync(new Produto
                {
                    IdProduto = 1,
                    NomeProduto = produtoDTO.NomeProduto,
                    DescricaoProduto = produtoDTO.DescricaoProduto,
                    ValorProduto = produtoDTO.ValorProduto,
                    IdCategoria = produtoDTO.IdCategoria,
                    ImagemProduto = produtoDTO.ImagemProduto
                });

            // Act
            var result = await _AppServices.PostProduto(produtoDTO);

            // Assert
            Assert.Equal(produtoDTO.IdCategoria, result.IdCategoria);
            Assert.Equal(produtoDTO.NomeProduto, result.NomeProduto);
            Assert.Equal(produtoDTO.DescricaoProduto, result.DescricaoProduto);
            Assert.Equal(produtoDTO.ValorProduto, result.ValorProduto);
            Assert.Equal(produtoDTO.ImagemProduto, result.ImagemProduto);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PostProduto Throws PreconditionFailedException WhenInvalidCategoria")]
        public async Task PostProduto_ThrowsPreconditionFailedException_WhenInvalidCategoria()
        {
            // Arrange
            var produtoDTO = new ProdutoDTO
            {
                IdCategoria = 999, // Use um valor inválido para IdCategoria
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                ImagemProduto = "imagem.jpg"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PreconditionFailedException>(() => _AppServices.PostProduto(produtoDTO));
            Assert.Contains("A categoria informada não existe. Operação cancelada.", exception.Message);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PostProduto Throws Valida Exception")]
        public async Task PostProduto_InvalidProdutoDTO_ThrowsValidationException()
        {
            // Arrange
            var produtoDTO = new ProdutoDTO { };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _AppServices.PostProduto(produtoDTO));
        }
        #endregion

        #region [PUT]
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PutProduto UpdatesProduto Successfully")]
        public async Task PutProduto_UpdatesProduto_Successfully()
        {
            // Arrange
            int idProduto = 1; // substitua pelo ID de teste
            ProdutoDTO produtoPut = new()
            {
                IdCategoria = 1,
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                ImagemProduto = "imagem.jpg"
            };
            Produto produto = new()
            {
                IdProduto = idProduto,
                IdCategoria = 1,
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                ImagemProduto = "imagem.jpg"
            };

            _Repository.Setup(repo => repo.GetProdutoById(idProduto))
                .ReturnsAsync(produto);
            _Repository.Setup(repo => repo.UpdateProduto(It.IsAny<Produto>()))
                .Returns(Task.FromResult(1));

            // Act
            await _AppServices.PutProduto(idProduto, produtoPut);

            // Assert
            _Repository.Verify(repo => repo.UpdateProduto(It.IsAny<Produto>()), Times.Once);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PutProduto Throws ResourceNotFoundException WhenProdutoNotFound")]
        public async Task PutProduto_ThrowsResourceNotFoundException_WhenProdutoNotFound()
        {
            // Arrange
            int idProduto = 1; // substitua pelo ID de teste
            ProdutoDTO produtoPut = new() { /* preencha os campos necessários */ };
            Produto produto = null;

            _Repository.Setup(repo => repo.GetProdutoById(idProduto))
                .ReturnsAsync(produto);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _AppServices.PutProduto(idProduto, produtoPut));
        }
        #endregion

        #region [PATCH]
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "PatchProduto UpdatesProduto Successfully")]
        public async Task PatchProduto_UpdatesProduto_Successfully()
        {
            // Arrange
            int idProduto = 1; // substitua pelo ID de teste
            var patchDoc = new JsonPatchDocument<Produto>();
            Produto produto = new()
            {
                IdProduto = idProduto,
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                IdCategoria = 1,
                Categoria = new Categoria
                {
                    IdCategoria = 1,
                    NomeCategoria = "Lanche",
                    Produtos =
                    [
                        new Produto
                        {
                            IdProduto = 1,
                            NomeProduto = "Produto Teste",
                            DescricaoProduto = "Descrição do Produto",
                            ValorProduto = 10.00f,
                            IdCategoria = 1,
                            ImagemProduto = "imagem.jpg",
                            Categoria = new Categoria
                            {
                                IdCategoria = 1,
                                NomeCategoria = "Lanche"
                            }
                        }
                    ]
                },
                ImagemProduto = "imagem.jpg"
            };

            _Repository.Setup(repo => repo.GetProdutoById(idProduto))
                .ReturnsAsync(produto);
            _Repository.Setup(repo => repo.UpdateProduto(It.IsAny<Produto>()))
                .Returns(Task.FromResult(1));

            // Act
            await _AppServices.PatchProduto(idProduto, patchDoc);

            // Assert
            _Repository.Verify(repo => repo.UpdateProduto(It.IsAny<Produto>()));

        }
        #endregion

        #region [DELETE]
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "DeleteProduto ReturnsProduto WhenProdutoIsNotNull")]
        public async Task DeleteProduto_ReturnsProduto_WhenProdutoIsNotNull()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste
            var deletedProduto = new Produto
            {
                IdProduto = id,
                NomeProduto = "Produto Teste",
                DescricaoProduto = "Descrição do Produto",
                ValorProduto = 10.00f,
                IdCategoria = 1,
                ImagemProduto = "imagem.jpg"
            };

            _Repository.Setup(repo => repo.GetProdutoById(id))
                .ReturnsAsync(deletedProduto);

            _Repository.Setup(repo => repo.DeleteProduto(id))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _AppServices.DeleteProduto(id);

            // Assert
            Assert.Equal(id, result.IdProduto);
        }

        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "DeleteProduto Throws KeyNotFoundException WhenProdutoNotFound")]
        public async Task DeleteProduto_ThrowsKeyNotFoundException_WhenProdutoNotFound()
        {
            // Arrange
            int id = 1; // substitua pelo ID de teste

            _Repository.Setup(repo => repo.GetProdutoById(id))
                .ReturnsAsync((Produto)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _AppServices.DeleteProduto(id));
        }

        #endregion

        #region [GET] Categorias
        [Trait("Categoria", "ProdutoService")]
        [Fact(DisplayName = "GetProdutosByIdCategoria ReturnsCategorias")]
        public async Task GetProdutosByIdCategoria_ReturnsCategorias()
        {
            // Arrange
            EnumCategoria idCategoria = EnumCategoria.Lanche; // Substitua pelo valor correto
            List<Categoria> categorias =
            [
                new Categoria { IdCategoria = 1, NomeCategoria = "Lanche" },
                new Categoria { IdCategoria = 2, NomeCategoria = "Bebida" },
                new Categoria { IdCategoria = 3, NomeCategoria = "Sobremesa" }
            ];

            _Repository.Setup(repo => repo.GetProdutosByIdCategoria(idCategoria))
                .ReturnsAsync(categorias);

            // Act
            var result = await _AppServices.GetProdutosByIdCategoria(idCategoria);

            // Assert
            Assert.Equal(categorias, result);
        }
        #endregion

    }
}