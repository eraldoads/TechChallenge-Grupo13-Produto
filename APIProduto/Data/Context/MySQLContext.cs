using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class MySQLContext(DbContextOptions options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration = configuration;

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringMysql = _configuration.GetConnectionString("ConnectionMysql");
            optionsBuilder.UseMySql(connectionStringMysql, ServerVersion.AutoDetect(connectionStringMysql), builder => builder.MigrationsAssembly("APIProduto"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das entidades do modelo, incluindo chaves primárias, chaves estrangeiras e outros relacionamentos.
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<ProdutoLista>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Categoria>().HasKey(c => c.IdCategoria);

            modelBuilder.Entity<Produto>()
                        .HasOne(p => p.Categoria)
                        .WithMany(c => c.Produtos)
                        .HasForeignKey(p => p.IdCategoria);

            modelBuilder.Entity<Produto>()
                        .Navigation(p => p.Categoria)
                        .UsePropertyAccessMode(PropertyAccessMode.Property);
        }

        public DbSet<Produto>? Produto { get; set; }
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DbSet<ProdutoLista>? ProdutoLista { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
    }
}
