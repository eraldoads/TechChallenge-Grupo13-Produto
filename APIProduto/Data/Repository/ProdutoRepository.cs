using Data.Context;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

namespace Data.Repository
{
    /// <summary>
    /// Inicializa uma nova instância do repositório de produtos com o contexto fornecido.
    /// </summary>
    /// <param name="context">O contexto MySQL para o repositório de produtos.</param>
    public class ProdutoRepository(MySQLContext context) : IProdutoRepository
    {
        private readonly MySQLContext _context = context;

        /// <summary>
        /// Obtém todos os produtos do contexto do banco de dados.
        /// </summary>
        /// <returns>Uma lista de todos os produtos no contexto.</returns>
        public async Task<List<Produto>> GetProdutos()
        {
            return await _context.Produto.Include(p => p.Categoria).ToListAsync();

        }

        /// <summary>
        /// Obtém um produto pelo ID no contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do produto a ser recuperado.</param>
        /// <returns>O produto correspondente ao ID fornecido.</returns>
        public async Task<Produto?> GetProdutoById(int id)
        {

            var produto = await _context.Produto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProduto == id);

            return produto;
        }

        /// <summary>
        /// Adiciona um novo produto ao contexto do banco de dados.
        /// </summary>
        /// <param name="produto">O produto a ser adicionado ao contexto.</param>
        /// <returns>O produto recém-adicionado.</returns>
        public async Task<Produto> PostProduto(Produto produto)
        {
            if (_context.Produto is not null)
            {
                _context.Produto.Add(produto);
                await _context.SaveChangesAsync();
            }

            return produto;
        }

        /// <summary>
        /// Atualiza um produto existente no contexto do banco de dados.
        /// </summary>
        /// <param name="produto">O produto a ser atualizado no contexto.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> UpdateProduto(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Exclui um produto com o ID fornecido do contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do produto a ser excluído.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id) ?? throw new KeyNotFoundException($"O produto com o ID {id} não foi encontrado.");

            _context.Produto.Remove(produto);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtém uma lista de produtos por id de categoria usando uma consulta SQL.
        /// </summary>
        /// <param name="idCategoria">O id da categoria dos produtos desejados.</param>
        /// <returns>Uma lista de objetos Produto com os dados dos produtos da categoria especificada.</returns>
        /// <exception cref="MySqlException">Se ocorrer um erro ao executar a consulta SQL.</exception>
        public async Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            List<Categoria> categorias = [];

            if (_context.Categoria is not null)
            {
                categorias = await _context.Categoria
                    .Where(c => c.IdCategoria == (int)idCategoria.Value)
                    .Include(c => c.Produtos)
                    .ToListAsync();
            }

            return categorias;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }

    }
}
