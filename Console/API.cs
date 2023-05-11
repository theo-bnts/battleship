using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ConsoleProject
{
    public class Attributes
    {
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public BoatAttributes[] bateaux { get; set; }
    }

    public class BoatAttributes
    {
        public int taille { get; set; }
    }

    internal class API
    {
        private static HttpClient client = new();

        private const string apiUrl = "https://api-lprgi.natono.biz/api/GetConfig";
        private const string apiKeyHeader = "x-functions-key";
        private const string apiValueHeader = "lprgi_api_key_2023";

        private int width;
        private int height;
        private List<int> boatSizes = new ();

        public int Width {
            get => width;
        }

        public int Height {
            get => height;
        }

        public List<int> BoatSizes
        {
            get => boatSizes;
        }

        public async Task GetDataAsync()
        {
            client.DefaultRequestHeaders.Add(apiKeyHeader, apiValueHeader);

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                Attributes serializedResponse = JsonSerializer.Deserialize<Attributes>(stringResponse);
                this.width = serializedResponse.nbLignes;
                this.height = serializedResponse.nbColonnes;
                this.boatSizes = serializedResponse.bateaux.Select(boat => boat.taille).ToList();
            }
            else
            {
                throw new Exception($"Error {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
    }
}