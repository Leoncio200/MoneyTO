using Newtonsoft.Json.Linq;
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
        private readonly HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<decimal?> ObtenerTipoCambio(string monedaBase, string monedaDestino)
        {
            try
            {
                // API de ejemplo que devuelve tipos de cambio
                string url = $"https://open.er-api.com/v6/latest/{monedaBase.Substring(monedaBase.Length -4, 3)}";

                var response = await _client.GetStringAsync(url);
                var json = JObject.Parse(response);

                // Obtener el tipo de cambio para la moneda destino
                var tipoCambio = json["rates"]?[monedaDestino.Substring(monedaDestino.Length - 4, 3)]?.Value<decimal>();
                return tipoCambio;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el tipo de cambio: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Monedas>> ObtenerMonedasFormato()
        {
            try
            {
                string url = "https://openexchangerates.org/api/currencies.json"; // URL del endpoint de monedas
                var response = await _client.GetStringAsync(url);
                var json = JObject.Parse(response);

                var monedas = new List<Monedas>();
                int id = 1;

                foreach (var item in json)
                {
                    monedas.Add(new Monedas
                    {
                        Id = id++,
                        Nombre = $"{item.Value} ({item.Key})",
                        ActivoDivisa = true
                    });
                }

                return monedas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las monedas: {ex.Message}");
                return new List<Monedas>(); // Devuelve lista vacía en caso de error
            }
        }

    }
}

