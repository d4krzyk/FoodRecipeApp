using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MealsLibrary1
{
    [DataContract]
    public class Recipe
    {
        [JsonPropertyName("idMeal")]
        [DataMember]
        public string idMeal { get; set; }

        [JsonPropertyName("strMeal")]
        [DataMember]
        public string name { get; set; }

        [JsonPropertyName("strCategory")]
        [DataMember]
        public string category { get; set; }

        [JsonPropertyName("strArea")]
        [DataMember]
        public string area { get; set; }

        [JsonPropertyName("strInstructions")]
        [DataMember]
        public string instructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        [DataMember]
        public string imageUrl { get; set; }

        [JsonPropertyName("strYoutube")]
        [DataMember]
        public string youtubeUrl { get; set; }

        [JsonPropertyName("strSource")]
        [DataMember]
        public string sourceUrl { get; set; }
    }
}
