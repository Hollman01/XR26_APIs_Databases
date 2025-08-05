using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data using async/await
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            // Check if the API key is properly configured
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in the StreamingAssets folder.");
                return null;
            }

            // Construct the full request URL
            string apiKey = ApiConfig.OpenWeatherMapApiKey;
            string url = $"{baseUrl}?q={city}&appid={apiKey}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // Send the web request and wait asynchronously for the result
                await request.SendWebRequest();

                

                // Handle the result of the web request
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        try
                        {
                            // Parse JSON response into WeatherData object
                            string json = request.downloadHandler.text;
                            Debug.Log($"Raw JSON: {json}");

                            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(json);
                            return data;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Failed to parse weather data: {ex.Message}");
                            return null;
                        }

                    case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"Request error: {request.error}");
                        return null;
                    case UnityWebRequest.Result.ProtocolError: 
                    Debug.LogError($"Request error: {request.error}");
                        return null;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Request error: {request.error}");
                        return null;

                    default:
                        Debug.LogError("Unknown request error");
                        return null;
                }
            }
        }

        /// <summary>
        /// Example usage on start: fetch weather for London
        /// </summary>
        private async void Start()
        {
            var weatherData = await GetWeatherDataAsync("London");

            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
    }
}
