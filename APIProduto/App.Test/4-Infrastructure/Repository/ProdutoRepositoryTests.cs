using App.Test._4_Infrastructure.Context;
using Data.Repository;
using Domain.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data.Tests.Repository
{
    public class ProdutoRepositoryTests
    {
        private readonly MySQLContextTests _context;
        private readonly ProdutoRepository _repository;

        public ProdutoRepositoryTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySQLContextTests>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "TestDatabase");
            var options = optionsBuilder.Options;

            _context = new MySQLContextTests(options);
            _repository = new ProdutoRepository(_context);
        }

        #region [GetProdutos]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "GetProdutos_DeveRetornarTodosProdutos")]
        public async Task GetProdutos_DeveRetornarTodosProdutos()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();

            var produtos = new List<Produto>
            {
                new() {
                    IdProduto = 305,
                    NomeProduto = "Produto 305",
                    DescricaoProduto = "Descrição do Produto 305",
                    ValorProduto = 10.00f,
                    IdCategoria = 305,
                    Categoria = new Categoria
                    {
                        IdCategoria = 305,
                        NomeCategoria = "Categoria Teste 305"
                    },
                    ImagemProduto = "imagem305.jpg"
                },
                new() {
                    IdProduto = 405,
                    NomeProduto = "Produto 405",
                    DescricaoProduto = "Descrição do Produto 405",
                    ValorProduto = 10.00f,
                    IdCategoria = 405,
                    Categoria = new Categoria
                    {
                        IdCategoria = 405,
                        NomeCategoria = "Categoria Teste 405"
                    },
                    ImagemProduto = "imagem405.jpg"
                }
            };

            _context.Produto.AddRange(produtos);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProdutos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(produtos.OrderBy(p => p.IdProduto), result.OrderBy(p => p.IdProduto));
        }

        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "GetProdutoById_DeveRetornarProdutoPorId")]
        public async Task GetProdutoById_DeveRetornarProdutoPorId()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();
            var produto = new Produto
            {
                IdProduto = 102,
                NomeProduto = "Produto 102",
                DescricaoProduto = "Descrição do Produto 102",
                ValorProduto = 1.00f,
                IdCategoria = 102,
                Categoria = new Categoria
                {
                    IdCategoria = 102,
                    NomeCategoria = "Categoria Teste"
                },
                ImagemProduto = "imagem1.jpg"
            };

            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            // Act
            var produtoRetornado = await _repository.GetProdutoById(102);

            // Assert
            Assert.NotNull(produtoRetornado);
            Assert.Equal("Produto 102", produtoRetornado.NomeProduto);
        }
        #endregion

        #region [POST]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "Post_DeveInserirProduto")]
        public async Task Post_DeveInserirProduto()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();
            var produto = new Produto
            {
                IdProduto = 1,
                NomeProduto = "Produto 1",
                DescricaoProduto = "Descrição do Produto 1",
                ValorProduto = 1.00f,
                IdCategoria = 1,
                Categoria = new Categoria
                {
                    IdCategoria = 1,
                    NomeCategoria = "Categoria Teste"
                },
                ImagemProduto = "imagem1.jpg"
            };

            // Act
            await _repository.PostProduto(produto);

            // Assert
            var produtoRetornado = await _repository.GetProdutoById(1);
            Assert.NotNull(produtoRetornado);
            Assert.Equal("Produto 1", produtoRetornado.NomeProduto);
        }
        #endregion

        #region [PUT e PATCH]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "Put_DeveAtualizarProduto")]
        public async Task Put_DeveAtualizarProduto()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();

            var produto = new Produto
            {
                IdProduto = 100,
                NomeProduto = "Produto 100",
                DescricaoProduto = "Descrição do Produto 100",
                ValorProduto = 1.00f,
                IdCategoria = 100,
                Categoria = new Categoria
                {
                    IdCategoria = 100,
                    NomeCategoria = "Categoria Teste"
                },
                ImagemProduto = "imagem1.jpg"
            };

            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            var produtoUpdate = new Produto
            {
                IdProduto = 100,
                NomeProduto = "Produto 100 Atualizado",
                DescricaoProduto = "Descrição do Produto 100 Atualizado",
                ValorProduto = 2.00f,
                IdCategoria = 100,
                Categoria = new Categoria
                {
                    IdCategoria = 100,
                    NomeCategoria = "Categoria Teste"
                },
                ImagemProduto = "imagem1.jpg"
            };

            // Act
            _context.Entry(produto).State = EntityState.Detached; // Detach the existing entity
            await _repository.UpdateProduto(produtoUpdate);

            // Assert
            var produtoRetornado = await _repository.GetProdutoById(100);
            Assert.NotNull(produtoRetornado);
            Assert.Equal("Produto 100 Atualizado", produtoRetornado.NomeProduto);
        }
        #endregion

        #region [DELETE]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "Delete_DeveExcluirProduto")]
        public async Task Delete_DeveExcluirProduto()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();

            var produto = new Produto
            {
                IdProduto = 111,
                NomeProduto = "Produto 111",
                DescricaoProduto = "Descrição do Produto 111",
                ValorProduto = 1.00f,
                IdCategoria = 111,
                Categoria = new Categoria
                {
                    IdCategoria = 111,
                    NomeCategoria = "Categoria Teste"
                },
                ImagemProduto = "imagem1.jpg"
            };

            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteProduto(111);

            // Assert
            var produtoRetornado = await _repository.GetProdutoById(111);
            Assert.Null(produtoRetornado);
        }
        #endregion

        #region [GetProdutosByIdCategoria]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "GetProdutosByIdCategoria_DeveRetornarProdutosPorCategoria")]
        public async Task GetProdutosByIdCategoria_DeveRetornarProdutosPorCategoria()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            await _context.SaveChangesAsync();

            var categoria = new Categoria
            {
                IdCategoria = 2,
                NomeCategoria = "Categoria Teste 101",
                Produtos = [
                                new() {
                                    IdProduto = 101,
                                    NomeProduto = "Produto 101",
                                    DescricaoProduto = "Descrição do Produto 101",
                                    ValorProduto = 1.00f,
                                    IdCategoria = 2,
                                    ImagemProduto = "imagem1.jpg"
                                },
                                new() {
                                    IdProduto = 102,
                                    NomeProduto = "Produto 102",
                                    DescricaoProduto = "Descrição do Produto 102",
                                    ValorProduto = 1.00f,
                                    IdCategoria = 2,
                                    ImagemProduto = "imagem1.jpg"
                                }
                            ]
            };

            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProdutosByIdCategoria(EnumCategoria.Acompanhamento);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(2, result.First().Produtos.Count);
        }
        #endregion


        #region [Dispose]
        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "Dispose_DeveDescartarContexto")]
        public void Dispose_DeveDescartarContexto()
        {
            // Arrange
            _context.Produto.RemoveRange(_context.Produto);
            _context.SaveChangesAsync();

            var produto = new Produto
            {
                IdProduto = 101,
                NomeProduto = "Produto 101",
                DescricaoProduto = "Descrição do Produto 101",
                ValorProduto = 1.00f,
                IdCategoria = 101,
                Categoria = new Categoria
                {
                    IdCategoria = 101,
                    NomeCategoria = "Categoria Teste 101"
                },
                ImagemProduto = "imagem1.jpg"
            };

            _context.Produto.Add(produto);
            _context.SaveChanges();

            // Act
            _repository.Dispose();

            // Assert
            Assert.NotNull(_context);
        }

        [Trait("Categoria", "ProdutoRepository")]
        [Fact(DisplayName = "Dispose")]
        public void Dispose()
        {
            //act
            _repository.Dispose();
            Assert.NotNull(_context);
        }
        #endregion

    }
}