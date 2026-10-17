using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using internetplays.ViewModels;

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

        private static readonly Dictionary<string, string> RedSpeciesMap = new()
        {
            { "01", "Rhydon" },
            { "02", "Kangaskhan" },
            { "03", "Nidoran♂" },
            { "04", "Clefairy" },
            { "05", "Spearow" },
            { "06", "Voltorb" },
            { "07", "Nidoking" },
            { "08", "Slowbro" },
            { "09", "Ivysaur" },
            { "0a", "Exeggutor" },
            { "0b", "Lickitung" },
            { "0c", "Exeggcute" },
            { "0d", "Grimer" },
            { "0e", "Gengar" },
            { "0f", "Nidoran♀" },
            { "10", "Nidoqueen" },
            { "11", "Cubone" },
            { "12", "Rhyhorn" },
            { "13", "Lapras" },
            { "14", "Arcanine" },
            { "15", "Mew" },
            { "16", "Gyarados" },
            { "17", "Shellder" },
            { "18", "Tentacool" },
            { "19", "Gastly" },
            { "1a", "Scyther" },
            { "1b", "Staryu" },
            { "1c", "Blastoise" },
            { "1d", "Pinsir" },
            { "1e", "Tangela" },
            { "21", "Growlithe" },
            { "22", "Onix" },
            { "23", "Fearow" },
            { "24", "Pidgey" },
            { "25", "Slowpoke" },
            { "26", "Kadabra" },
            { "27", "Graveler" },
            { "28", "Chansey" },
            { "29", "Machoke" },
            { "2a", "Mr. Mime" },
            { "2b", "Hitmonlee" },
            { "2c", "Hitmonchan" },
            { "2d", "Arbok" },
            { "2e", "Parasect" },
            { "2f", "Psyduck" },
            { "30", "Drowzee" },
            { "31", "Golem" },
            { "33", "Magmar" },
            { "35", "Electabuzz" },
            { "36", "Magneton" },
            { "37", "Koffing" },
            { "39", "Mankey" },
            { "3a", "Seel" },
            { "3b", "Diglett" },
            { "3c", "Tauros" },
            { "40", "Farfetch'd" },
            { "41", "Venonat" },
            { "42", "Dragonite" },
            { "46", "Doduo" },
            { "47", "Poliwag" },
            { "48", "Jynx" },
            { "49", "Moltres" },
            { "4a", "Articuno" },
            { "4b", "Zapdos" },
            { "4c", "Ditto" },
            { "4d", "Meowth" },
            { "4e", "Krabby" },
            { "52", "Vulpix" },
            { "53", "Ninetales" },
            { "54", "Pikachu" },
            { "55", "Raichu" },
            { "58", "Dratini" },
            { "59", "Dragonair" },
            { "5a", "Kabuto" },
            { "5b", "Kabutops" },
            { "5c", "Horsea" },
            { "5d", "Seadra" },
            { "60", "Sandshrew" },
            { "61", "Sandslash" },
            { "62", "Omanyte" },
            { "63", "Omastar" },
            { "64", "Jigglypuff" },
            { "65", "Wigglytuff" },
            { "66", "Eevee" },
            { "67", "Flareon" },
            { "68", "Jolteon" },
            { "69", "Vaporeon" },
            { "6a", "Machop" },
            { "6b", "Zubat" },
            { "6c", "Ekans" },
            { "6d", "Paras" },
            { "6e", "Poliwhirl" },
            { "6f", "Poliwrath" },
            { "70", "Weedle" },
            { "71", "Kakuna" },
            { "72", "Beedrill" },
            { "74", "Dodrio" },
            { "75", "Primeape" },
            { "76", "Dugtrio" },
            { "77", "Venomoth" },
            { "78", "Dewgong" },
            { "7e", "Machamp" },
            { "80", "Golduck" },
            { "81", "Hypno" },
            { "82", "Golbat" },
            { "83", "Mewtwo" },
            { "84", "Snorlax" },
            { "85", "Magikarp" },
            { "88", "Muk" },
            { "8a", "Kingler" },
            { "8b", "Cloyster" },
            { "8d", "Electrode" },
            { "8e", "Clefable" },
            { "8f", "Weezing" },
            { "90", "Persian" },
            { "91", "Marowak" },
            { "93", "Haunter" },
            { "94", "Abra" },
            { "95", "Alakazam" },
            { "96", "Pidgeotto" },
            { "97", "Pidgeot" },
            { "98", "Starmie" },
            { "99", "Bulbasaur" },
            { "9a", "Venusaur" },
            { "9b", "Tentacruel" },
            { "9d", "Goldeen" },
            { "9e", "Seaking" },
            { "a3", "Ponyta" },
            { "a4", "Rapidash" },
            { "a5", "Rattata" },
            { "a6", "Raticate" },
            { "a7", "Nidorino" },
            { "a8", "Nidorina" },
            { "a9", "Geodude" },
            { "aa", "Porygon" },
            { "ab", "Aerodactyl" },
            { "b0", "Charmander" },
            { "b1", "Squirtle" },
            { "b2", "Charmeleon" },
            { "b3", "Wartortle" },
            { "b4", "Charizard" },
            { "b9", "Oddish" },
            { "ba", "Gloom" },
            { "bb", "Vileplume" },
            { "bc", "Bellsprout" },
            { "bd", "Weepinbell" },
            { "be", "Victreebel" }
        };

        private static readonly Dictionary<string, string> RedCharacterMap = new()
        {
            { "7f", " " },
            { "80", "A" },
            { "81", "B" },
            { "82", "C" },
            { "83", "D" },
            { "84", "E" },
            { "85", "F" },
            { "86", "G" },
            { "87", "H" },
            { "88", "I" },
            { "89", "J" },
            { "8a", "K" },
            { "8b", "L" },
            { "8c", "M" },
            { "8d", "N" },
            { "8e", "O" },
            { "8f", "P" },
            { "90", "Q" },
            { "91", "R" },
            { "92", "S" },
            { "93", "T" },
            { "94", "U" },
            { "95", "V" },
            { "96", "W" },
            { "97", "X" },
            { "98", "Y" },
            { "99", "Z" },
            { "9a", "(" },
            { "9b", ")" },
            { "9c", ":" },
            { "9d", ";" },
            { "9e", "[" },
            { "9f", "]" },
            { "a0", "a" },
            { "a1", "b" },
            { "a2", "c" },
            { "a3", "d" },
            { "a4", "e" },
            { "a5", "f" },
            { "a6", "g" },
            { "a7", "h" },
            { "a8", "i" },
            { "a9", "j" },
            { "aa", "k" },
            { "ab", "l" },
            { "ac", "m" },
            { "ad", "n" },
            { "ae", "o" },
            { "af", "p" },
            { "b0", "q" },
            { "b1", "r" },
            { "b2", "s" },
            { "b3", "t" },
            { "b4", "u" },
            { "b5", "v" },
            { "b6", "w" },
            { "b7", "x" },
            { "b8", "y" },
            { "b9", "z" },
            { "e1", "PK" },
            { "e2", "MN" },
            { "e3", "-" },
            { "e6", "?" },
            { "e7", "!" },
            { "e8", "." },
            { "ef", "♂" },
            { "f1", "×" },
            { "f2", "." },
            { "f3", "/" },
            { "f4", "," },
            { "f5", "♀" }
        };

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

        public async Task<List<PokemonViewModel>> GetPartyRed()
        {
            var party = new List<PokemonViewModel>();
            try
            {
                var partyLengthRequest = new HttpRequestMessage(HttpMethod.Get, "/core/readrange?address=0xD163&length=1");
                var partyLength = await _client.SendAsync(partyLengthRequest);
                partyLength.EnsureSuccessStatusCode();
                var partyString = await partyLength.Content.ReadAsStringAsync();
                var partyInt = Int32.Parse(partyString);

                for (int i = 1; i <= partyInt; i++)
                {
                    var requestAddress = 163 + i;
                    var pokemonRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xD{requestAddress}&length=1");
                    var pokemonResponse = await _client.SendAsync(pokemonRequest);
                    pokemonResponse.EnsureSuccessStatusCode();
                    var pokemonString = await pokemonResponse.Content.ReadAsStringAsync();
                    var species = RedSpeciesMap.ContainsKey(pokemonString) ? RedSpeciesMap[pokemonString] : "Unknown";
                    var nickname = await GetName(i);
                    if (!string.Equals(species, "Unknown")) party.Add(new PokemonViewModel(species, nickname));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get party: {e.Message}");
            }

            return party;
        }

        private async Task<string> GetName(int partyIndex)
        {
            var requestAddress = partyIndex switch
            {
                1 => "0xD2B5",
                2 => "0xD2C0",
                3 => "0xD2CB",
                4 => "0xD2D6",
                5 => "0xD2E1",
                6 => "0xD2EC"
            };

            try
            {
                var nickRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address={requestAddress}&length=11");
                var nickResponse = await _client.SendAsync(nickRequest);
                nickResponse.EnsureSuccessStatusCode();
                var nickname = (await nickResponse.Content.ReadAsStringAsync()).Split(",");

                string nicknameString = "";

                foreach (var charId in nickname)
                {
                    if (charId == "50") 
                    {
                        break;
                    }

                    nicknameString += RedCharacterMap.ContainsKey(charId) ? RedCharacterMap[charId] : " ";
                }
                return nicknameString;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get nickname: {e.Message}");
                return null;
            }
        }
    }
}
