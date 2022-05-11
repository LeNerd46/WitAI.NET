using Newtonsoft.Json;
using RestSharp;
using WitAI.NET.Entities;
using WitAI.NET.Intents;

namespace WitAI.NET
{
    public class WitClient
    {
        private const string baseUrl = "https://api.wit.ai";
        private const string defaultVersion = "20220511"; // YYYY-MM-DD

        internal static string Token { get; private set; }

        private RestClient Rest { get; set; }

        /// <summary>
        /// Creates a new Wit client
        /// </summary>
        /// <param name="token">Your server access token</param>
        public WitClient(string token)
        {
            Token = token;
            Rest = new RestClient(baseUrl);
            
            Rest.AddDefaultHeader("Authorization", $"Bearer {Token}");
            Rest.AddDefaultHeader("Content-Type", "application/json");
            Rest.AddDefaultHeader("Accept", "application/json");

            Rest.AddDefaultParameter("v", defaultVersion, ParameterType.QueryString);
        }

        /// <summary>
        /// Sends a message to your Wit app
        /// </summary>
        /// <param name="query">What the user says</param>
        /// <returns>The JSON</returns>
        public string Message(string query)
        {
            RestRequest request = new RestRequest("message", Method.Get);
            request.AddQueryParameter("q", query);

            var result = Rest.Execute(request);

            return result.Content;
        }

        /// <summary>
        /// Returns the list of all intents for the app
        /// </summary>
        /// <returns>A list of all intents</returns>
        public async Task<IEnumerable<Intent>> GetAllIntents()
        {
            RestRequest request = new RestRequest("intents", Method.Get);

            RestResponse result = await Rest.ExecuteAsync(request);

            var intents = JsonConvert.DeserializeObject<IEnumerable<Intent>>(result.Content);

            foreach (var intent in intents)
            {
                if (intent.Name.StartsWith("wit$"))
                {
                    intent.Name = intent.Name.Substring(4);
                    intent.IsCustom = false;
                }
                else
                    intent.IsCustom = true;
            }

            return intents;
        }

        /// <summary>
        /// Returns the list of the top detected locales for the text message
        /// </summary>
        /// <param name="query">User's query, between 0 and 280 characters</param>
        /// <returns>A list of all languages detected in the query</returns>
        public async Task<Locale> GetLocale(string query)
        {
            RestRequest request = new RestRequest("language");
            request.AddQueryParameter("q", query);

            RestResponse response = await Rest.ExecuteAsync(request, Method.Get);

            return JsonConvert.DeserializeObject<Locale>(response.Content);
        }

        /// <summary>
        /// Returns the list of the top detected locales for the text message
        /// </summary>
        /// <param name="query">User's query, between 0 and 280 characters</param>
        /// <param name="max">The maximum number of top detected locales you want to get back. Can not be more than 8</param>
        /// <returns>A list of all languages detected in the query</returns>
        public async Task<Locale> GetLocale(string query, int max)
        {
            RestRequest request = new RestRequest("language");
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("n", max);

            RestResponse response = await Rest.ExecuteAsync(request, Method.Get);

            return JsonConvert.DeserializeObject<Locale>(response.Content);
        }

        /// <summary>
        /// Creates a new intent with the given attributes
        /// </summary>
        /// <param name="name">Name for the intent</param>
        public async Task<Intent> CreateIntent(string name)
        {
            RestRequest request = new RestRequest("intents", Method.Post);
            request.AddBody("{\"name\": \"" + name.ToLower().Replace(' ', '_') + "\"}", "application/json");

            RestResponse response = await Rest.ExecuteAsync(request, Method.Post);

            return JsonConvert.DeserializeObject<Intent>(response.Content);
        }

        /// <summary>
        /// Returns all available information about an intent
        /// </summary>
        /// <param name="intent">Name of the intent</param>
        /// <returns>All available information about an intent</returns>
        public async Task<Intent> GetIntent(string intent)
        {
            RestRequest request = new RestRequest($"intents/{intent}");
            RestResponse response = await Rest.ExecuteAsync(request, Method.Get);

            return JsonConvert.DeserializeObject<Intent>(response.Content);
        }

        /// <summary>
        /// Permanently deletes the intent
        /// </summary>
        /// <param name="intent"></param>
        public async Task DeleteIntent(string intent)
        {
            RestRequest request = new RestRequest($"intents/{intent}");
            RestResponse response = await Rest.ExecuteAsync(request, Method.Delete);
        }

        /// <summary>
        /// Returns the list of all entities for the app
        /// </summary>
        /// <returns>The list of all entities for the app</returns>
        public async Task<IEnumerable<Entity>> GetAllEntities()
        {
            RestRequest request = new RestRequest("entities");
            RestResponse response = await Rest.ExecuteAsync(request, Method.Get);

            var thing = JsonConvert.DeserializeObject<List<Entity>>(response.Content);

            foreach (var entity in thing)
            {
                if (entity.Name.StartsWith("wit$"))
                {
                    entity.Name = entity.Name.Substring(4);
                    entity.IsCustom = false;
                }
                else
                    entity.IsCustom = true;
            }

            return thing;
        }

        /// <summary>
        /// Creates a new entity with the given attributes
        /// </summary>
        /// <param name="name">Name for the entity. For built-in entities, use the wit$ prefix</param>
        /// <param name="roles">List of roles you want to create for the entity. A default role will always be created</param>
        /// <param name="lookups">For custom entities, list of lookup strategies</param>
        /// <param name="keywords">For keywords entities, list of keywords and synonyms</param>
        /// <returns>The entity that you created</returns>
        public async Task<Entity> CreateEntity(string name, string[] roles, string[] lookups = null, object[] keywords = null)
        {
            RestRequest request = new RestRequest("entities");

            EntityPost entity = null;

            if (lookups == null && keywords == null)
                entity = new EntityPost(name, roles);
            else if (lookups != null && keywords == null)
                entity = new EntityPost(name, roles, lookups);
            else if (lookups == null && keywords != null)
                entity = new EntityPost(name, roles, keywords: keywords);
            else
                entity = new EntityPost(name, roles, lookups, keywords);

            request.AddJsonBody(entity);

            RestResponse response = await Rest.ExecuteAsync(request, Method.Post);

            return JsonConvert.DeserializeObject<Entity>(response.Content);
        }
    }
}
