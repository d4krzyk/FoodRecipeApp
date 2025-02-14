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
        public string idMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string name { get; set; }

        [JsonPropertyName("strCategory")]
        public string category { get; set; }

        [JsonPropertyName("strArea")]
        public string area { get; set; }

        [JsonPropertyName("strInstructions")]
        public string instructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string imageUrl { get; set; }

        [JsonPropertyName("strYoutube")]
        public string youtubeUrl { get; set; }

        [JsonPropertyName("strSource")]
        public string sourceUrl { get; set; }
    }
}
