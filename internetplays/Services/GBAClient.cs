using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace internetplays.Services
{
    public class GBAClient
    {
        private HttpClient _client;

        public GBAClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:5000/");

            Console.WriteLine("Connected to mGBA-http.");
        }

        public async Task PressKey(string key)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"mgba-http/button/add?button={key}");
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                await Task.Delay(100);
                request = new HttpRequestMessage(HttpMethod.Post, $"mgba-http/button/clear?button={key}");
                response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                await Task.Delay(250);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to send or remove key: {e.Message}");
            }
        }

        public async Task<String> GetGame()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/core/getgametitle");
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var game = await response.Content.ReadAsStringAsync();
                return game;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get game: {e.Message}");
                return null;
            }
        }

        public async Task SaveState(int slot)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"/core/savestateslot?slot={slot}");
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to save state: {e.Message}");
            }
        }
    }
}
