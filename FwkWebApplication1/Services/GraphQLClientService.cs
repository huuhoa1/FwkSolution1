using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FwkWebApplication1.Services
{
    /// <summary>
    /// Service for handling GraphQL API calls
    /// </summary>
    public class GraphQLClientService
    {
        private readonly string _graphqlEndpoint;
        private readonly HttpClient _httpClient;

        public GraphQLClientService(string graphqlEndpoint = "http://localhost:4000")
        {
            _graphqlEndpoint = graphqlEndpoint;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Execute a GraphQL query
        /// </summary>
        public async Task<T> QueryAsync<T>(string query, Dictionary<string, object> variables = null) where T : class
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = query,
                    Variables = variables
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(_graphqlEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var graphqlResponse = JsonConvert.DeserializeObject<GraphQLResponse<T>>(responseContent);

                if (graphqlResponse?.Data != null)
                {
                    return graphqlResponse.Data;
                }

                throw new Exception($"GraphQL Error: {string.Join(", ", graphqlResponse?.Errors ?? new List<GraphQLError>())}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to connect to GraphQL endpoint: {_graphqlEndpoint}", ex);
            }
        }

        /// <summary>
        /// Execute a GraphQL mutation
        /// </summary>
        public async Task<T> MutateAsync<T>(string mutation, Dictionary<string, object> variables = null) where T : class
        {
            return await QueryAsync<T>(mutation, variables);
        }

        /// <summary>
        /// Get generic data from GraphQL
        /// </summary>
        public async Task<dynamic> GetDataAsync(string query)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = query
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(_graphqlEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"GraphQL request failed: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// GraphQL Request format
    /// </summary>
    public class GraphQLRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, object> Variables { get; set; }
    }

    /// <summary>
    /// GraphQL Response wrapper
    /// </summary>
    public class GraphQLResponse<T> where T : class
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<GraphQLError> Errors { get; set; }
    }

    /// <summary>
    /// GraphQL Error object
    /// </summary>
    public class GraphQLError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("extensions")]
        public Dictionary<string, object> Extensions { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
