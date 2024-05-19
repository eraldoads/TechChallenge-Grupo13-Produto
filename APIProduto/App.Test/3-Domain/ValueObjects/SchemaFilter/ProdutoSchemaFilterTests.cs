using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Domain.ValueObjects.Tests
{
    public class ProdutoSchemaFilterTests
    {
        private readonly ProdutoSchemaFilter _ProdutoSchemaFilter;

        public ProdutoSchemaFilterTests()
        {
            _ProdutoSchemaFilter = new ProdutoSchemaFilter();
        }

        [Trait("Categoria", "SchemaFilter")]
        [Fact(DisplayName = "Aplicar Define Exemplo para Schema de Produto")]
        public void Aplicar_DefineExemploParaSchemaProduto()
        {
            // Arrange
            var schema = new OpenApiSchema();
            var context = new SchemaFilterContext(typeof(Produto), null, null);

            // Act
            _ProdutoSchemaFilter.Apply(schema, context);

            // Assert
            Assert.NotNull(schema.Example);
        }

        [Trait("Categoria", "SchemaFilter")]
        [Fact(DisplayName = "Aplicar Define Exemplo para Schema de ProdutoDTO")]
        public void Aplicar_DefineExemploParaSchemaProdutoDTO()
        {
            // Arrange
            var schema = new OpenApiSchema();
            var context = new SchemaFilterContext(typeof(ProdutoDTO), null, null);

            // Act
            _ProdutoSchemaFilter.Apply(schema, context);

            // Assert
            Assert.NotNull(schema.Example);
        }

    }
}
