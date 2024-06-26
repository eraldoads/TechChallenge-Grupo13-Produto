﻿using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Categoria
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        [JsonProperty("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("nomeCategoria")]
        public string? NomeCategoria { get; set; }

        [JsonProperty("produtos")]
        public List<Produto>? Produtos { get; set; }
    }
}