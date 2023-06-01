using System.Text.Json;

namespace ConsoleProject.Tools
{
    /// <summary>
    /// Represents the top-level attributes received from the API.
    /// </summary>
    public class TopLevelAttributes
    {
        /// <summary>
        /// The number of rows in the grid.
        /// </summary>
        public int nbLignes { get; set; }

        /// <summary>
        /// The number of columns in the grid.
        /// </summary>
        public int nbColonnes { get; set; }

        /// <summary>
        /// The attributes of the boats.
        /// </summary>
        public BoatAttributes[]? bateaux { get; set; }
    }

    /// <summary>
    /// Represents the attributes of a boat.
    /// </summary>
    public class BoatAttributes
    {
        public int taille { get; set; }
    }

    /// <summary>
    /// Provides methods to fetch data from an API.
    /// </summary>
    internal class API
    {
        private static readonly HttpClient client = new ();

        private static readonly string apiUrl = "https://api-lprgi.natono.biz/api/GetConfig";
        private static readonly string apiKeyHeader = "x-functions-key";
        private static readonly string apiValueHeader = "lprgi_api_key_2023";

        private int width;
        private int height;
        private List<int> boatSizes = new ();

        /// <summary>
        /// The width of the grid.
        /// </summary>
        public int Width
        {
            get => width;
        }

        /// <summary>
        /// The height of the grid.
        /// </summary>
        public int Height
        {
            get => height;
        }

        /// <summary>
        /// The sizes of the boats.
        /// </summary>
        public List<int> BoatSizes
        {
            get => boatSizes;
        }

        /// <summary>
        /// Fetches data from the API.
        /// </summary>
        /// <exception cref="Exception">Thrown when an error occurs during the API request or the data is invalid.</exception>
        public async Task FetchData()
        {
            client.DefaultRequestHeaders.Add(apiKeyHeader, apiValueHeader);

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error {response.StatusCode} : {response.ReasonPhrase}");
            }

            string stringResponse = await response.Content.ReadAsStringAsync();

            TopLevelAttributes? serializedResponse = JsonSerializer.Deserialize<TopLevelAttributes>(stringResponse);

            if (serializedResponse == null || serializedResponse.bateaux == null)
            {
                throw new Exception("Some values were not found");
            }

            width = serializedResponse.nbLignes;
            height = serializedResponse.nbColonnes;
            boatSizes = serializedResponse.bateaux.Select(boat => boat.taille).ToList();
        }
    }
}