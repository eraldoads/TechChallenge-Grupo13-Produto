using Application.Interfaces;
using Data.Context;
using Data.Repository;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;

namespace APIProduto
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    // Cria um builder de aplica��o web com os argumentos passados.
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria um builder de aplica��o web com os argumentos passados.
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona servi�os ao cont�iner.
            //// Adiciona um servi�o do tipo MySQLContext ao objeto builder.Services.
            builder.Services.AddDbContext<MySQLContext>();

            // Adiciona os servi�os de controllers ao builder.
            builder.Services.AddControllers(options =>
            {
                // Insere um formato de entrada personalizado para o JsonPatch.
                options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
            });

            // Adiciona os servi�os espec�ficos ao cont�iner.
            builder.Services.AddScoped<IProdutoService, ProdutoService>();

            // Adiciona os reposit�rios espec�ficos ao cont�iner.
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Adiciona o suporte ao NewtonsoftJson aos controllers.
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Configura as op��es de rota para usar URLs e query strings em min�sculo.
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            // Configura os servi�os relacionados aos controladores.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AjustaDataHoraLocal));
            }).AddNewtonsoftJson(options =>
            {
                // Usa a formata��o padr�o (PascalCase) para as propriedades.
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });

            // Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var licenseUrl = Environment.GetEnvironmentVariable("LicenseUrl");
            // Configura��o do SwaggerGen para gerar a documenta��o da Web API.
            builder.Services.AddSwaggerGen(
                c =>
                {
                    // Habilita o uso de anota��es (como [SwaggerOperation]) para melhorar a documenta��o.
                    c.EnableAnnotations();
                    // Define a vers�o da documenta��o Swagger como "v1".
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Tech Challenge - Grupo 13 - Fase IV",
                        Description = "Documenta��o dos endpoints da API sobre o uso de microsservi�os de produtos.",
                        Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
                        License = new OpenApiLicense() { Name = "MIT License", Url = licenseUrl != null ? new Uri(licenseUrl) : null },
                        Version = "1.0.11"
                    });

                    // Habilita o uso para registrar o SchemaFilter.
                    c.SchemaFilter<ProdutoSchemaFilter>();
                }
            );

            var app = builder.Build();

            // Adiciona o middleware de codifica��o para garantir a codifica��o correta em todas as respostas.
            app.Use((context, next) =>
            {
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.Headers.ContentEncoding = "utf-8";
                context.Response.Headers.ContentLanguage = CultureInfo.CurrentCulture.Name;
                return next();
            });

            // Configura a pipeline de solicita��o HTTP.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.DefaultModelRendering(ModelRendering.Example);
                    /// c.DefaultModelExpandDepth(-1);
                    /// c.DefaultModelsExpandDepth(-1);
                    /// c.DocExpansion(DocExpansion.None);
                    /// c.DisplayRequestDuration();
                    c.DisplayOperationId();
                    c.EnableDeepLinking();
                    c.EnableFilter();
                    c.ShowExtensions();
                    c.EnableValidator();
                    /// c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                });

                app.UseReDoc(c =>
                {
                    c.DocumentTitle = "REDOC API Documentation";
                    c.SpecUrl = "/swagger/v1/swagger.json";
                });
            }

            // Adiciona o middleware de autoriza��o � pipeline de solicita��o HTTP.
            // Este middleware � respons�vel por garantir que o usu�rio esteja autorizado a acessar os recursos solicitados.
            app.UseAuthorization();

            // Adiciona o middleware de roteamento de controladores � pipeline de solicita��o HTTP.
            // Este middleware � respons�vel por rotear as solicita��es HTTP para os controladores apropriados.
            app.MapControllers();

            // Inicia a execu��o da aplica��o.
            // Este m�todo bloqueia o thread chamado e aguarda at� que a aplica��o seja encerrada.
            app.Run();
        }
    }
}