using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace AppLibrary
{
    public class Recipe
    {
        [JsonPropertyName("idMeal")]
        public string IdMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string Name { get; set; }

        [JsonPropertyName("strCategory")]
        public string Category { get; set; }

        [JsonPropertyName("strArea")]
        public string Area { get; set; }

        [JsonPropertyName("strInstructions")]
        public string Instructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("strYoutube")]
        public string YoutubeUrl { get; set; }

        [JsonPropertyName("strSource")]
        public string SourceUrl { get; set; }
    }
}
