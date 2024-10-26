using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyTakeOver.Models
{
    public class RestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = "16ea9582518d82e32fcba625e8771869d09b836d5c274bf8338f73c7546f59ad";
        public RestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetDataAsync()
        {
            var response = await _httpClient.GetAsync($"https://open.er-api.com/v6/latest/MXN");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error en la solicitud: {response.StatusCode}");
            }
        }

        
    }
}

