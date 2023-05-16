using System.Text.Json;

namespace ConsoleProject.Tools
{
    public class TopLevelAttributes
    {
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public BoatAttributes[]? bateaux { get; set; }
    }

    public class BoatAttributes
    {
        public int taille { get; set; }
    }

    internal class API
    {
        private static HttpClient client = new ();

        private const string apiUrl = "https://api-lprgi.natono.biz/api/GetConfig";
        private const string apiKeyHeader = "x-functions-key";
        private const string apiValueHeader = "lprgi_api_key_2023";

        private int width;
        private int height;
        private List<int> boatSizes = new ();

        public int Width
        {
            get => width;
        }

        public int Height
        {
            get => height;
        }

        public List<int> BoatSizes
        {
            get => boatSizes;
        }

        public async Task GetData()
        {
            client.DefaultRequestHeaders.Add(apiKeyHeader, apiValueHeader);

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                TopLevelAttributes? serializedResponse = JsonSerializer.Deserialize<TopLevelAttributes>(stringResponse);

                if (serializedResponse == null || serializedResponse.bateaux == null)
                {
                    throw new Exception("Some values was not found");
                }

                width = serializedResponse.nbLignes;
                height = serializedResponse.nbColonnes;
                boatSizes = serializedResponse.bateaux.Select(boat => boat.taille).ToList();
            }
            else
            {
                throw new Exception($"Error {response.StatusCode} : {response.ReasonPhrase}");
            }
        }
    }
}