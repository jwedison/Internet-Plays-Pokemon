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

        private static readonly Dictionary<int, string> SpeciesMap = new()
        {
            {1, "Bulbasaur"},
            {2, "Ivysaur"},
            {3, "Venusaur"},
            {4, "Charmander"},
            {5, "Charmeleon"},
            {6, "Charizard"},
            {7, "Squirtle"},
            {8, "Wartortle"},
            {9, "Blastoise"},
            {10, "Caterpie"},
            {11, "Metapod"},
            {12, "Butterfree"},
            {13, "Weedle"},
            {14, "Kakuna"},
            {15, "Beedrill"},
            {16, "Pidgey"},
            {17, "Pidgeotto"},
            {18, "Pidgeot"},
            {19, "Rattata"},
            {20, "Raticate"},
            {21, "Spearow"},
            {22, "Fearow"},
            {23, "Ekans"},
            {24, "Arbok"},
            {25, "Pikachu"},
            {26, "Raichu"},
            {27, "Sandshrew"},
            {28, "Sandslash"},
            {29, "Nidoran♀"},
            {30, "Nidorina"},
            {31, "Nidoqueen"},
            {32, "Nidoran♂"},
            {33, "Nidorino"},
            {34, "Nidoking"},
            {35, "Clefairy"},
            {36, "Clefable"},
            {37, "Vulpix"},
            {38, "Ninetales"},
            {39, "Jigglypuff"},
            {40, "Wigglytuff"},
            {41, "Zubat" },
            {42, "Golbat"},
            {43, "Oddish"},
            {44, "Gloom"},
            {45, "Vileplume"},
            {46, "Paras"},
            {47, "Parasect"},
            {48, "Venonat"},
            {49, "Venomoth"},
            {50, "Diglett"},
            {51, "Dugtrio"},
            {52, "Meowth"},
            {53, "Persian"},
            {54, "Psyduck"},
            {55, "Golduck"},
            {56, "Mankey"},
            {57, "Primeape"},
            {58, "Growlithe"},
            {59, "Arcanine"},
            {60, "Poliwag"},
            {61, "Poliwhirl"},
            {62, "Poliwrath"},
            {63, "Abra"},
            {64, "Kadabra"},
            {65, "Alakazam"},
            {66, "Machop"},
            {67, "Machoke"},
            {68, "Machamp"},
            {69, "Bellsprout"},
            {70, "Weepinbell"},
            {71, "Victreebel"},
            {72, "Tentacool"},
            {73, "Tentacruel"},
            {74, "Geodude"},
            {75, "Graveler"},
            {76, "Golem"},
            {77, "Ponyta"},
            {78, "Rapidash"},
            {79, "Slowpoke"},
            {80, "Slowbro"},
            {81, "Magnemite" },
            {82, "Magneton"},
            {83, "Farfetch'd"},
            {84, "Doduo"},
            {85, "Dodrio"},
            {86, "Seel"},
            {87, "Dewgong"},
            {88, "Grimer"},
            {89, "Muk"},
            {90, "Shellder"},
            {91, "Cloyster"},
            {92, "Gastly"},
            {93, "Haunter"},
            {94, "Gengar"},
            {95, "Onix"},
            {96, "Drowzee"},
            {97, "Hypno"},
            {98, "Krabby"},
            {99, "Kingler"},
            {100, "Voltorb"},
            {101, "Electrode"},
            {102, "Exeggcute"},
            {103, "Exeggutor"},
            {104, "Cubone"},
            {105, "Marowak"},
            {106, "Hitmonlee"},
            {107, "Hitmonchan"},
            {108, "Lickitung"},
            {109, "Koffing"},
            {110, "Weezing"},
            {111, "Rhyhorn"},
            {112, "Rhydon"},
            {113, "Chansey"},
            {114, "Tangela"},
            {115, "Kangaskhan"},
            {116, "Horsea"},
            {117, "Seadra"},
            {118, "Goldeen"},
            {119, "Seaking"},
            {120, "Staryu"},
            {121, "Starmie"},
            {122, "Mr. Mime"},
            {123, "Scyther"},
            {124, "Jynx"},
            {125, "Electabuzz"},
            {126, "Magmar"},
            {127, "Pinsir"},
            {128, "Tauros"},
            {129, "Magikarp"},
            {130, "Gyarados"},
            {131, "Lapras"},
            {132, "Ditto"},
            {133, "Eevee"},
            {134, "Vaporeon"},
            {135, "Jolteon"},
            {136, "Flareon"},
            {137, "Porygon"},
            {138, "Omanyte"},
            {139, "Omastar"},
            {140, "Kabuto"},
            {141, "Kabutops"},
            {142, "Aerodactyl"},
            {143, "Snorlax"},
            {144, "Articuno"},
            {145, "Zapdos"},
            {146, "Moltres"},
            {147, "Dratini"},
            {148, "Dragonair"},
            {149, "Dragonite"},
            {150, "Mewtwo"},
            {151, "Mew"},
            {152, "Chikorita" },
            {153, "Bayleef"},
            {154, "Meganium"},
            {155, "Cyndaquil"},
            {156, "Quilava"},
            {157, "Typhlosion"},
            {158, "Totodile"},
            {159, "Croconaw"},
            {160, "Feraligatr"},
            {161, "Sentret"},
            {162, "Furret"},
            {163, "Hoothoot"},
            {164, "Noctowl"},
            {165, "Ledyba"},
            {166, "Ledian"},
            {167, "Spinarak"},
            {168, "Ariados"},
            {169, "Crobat"},
            {170, "Chinchou"},
            {171, "Lanturn"},
            {172, "Pichu"},
            {173, "Cleffa"},
            {174, "Igglybuff"},
            {175, "Togepi"},
            {176, "Togetic"},
            {177, "Natu"},
            {178, "Xatu"},
            {179, "Mareep"},
            {180, "Flaaffy"},
            {181, "Ampharos"},
            {182, "Bellossom"},
            {183, "Marill"},
            {184, "Azumarill"},
            {185, "Sudowoodo"},
            {186, "Politoed"},
            {187, "Hoppip" },
            {188, "Skiploom"},
            {189, "Jumpluff"},
            {190, "Aipom"},
            {191, "Sunkern"},
            {192, "Sunflora"},
            {193, "Yanma"},
            {194, "Wooper"},
            {195, "Quagsire"},
            {196, "Espeon"},
            {197, "Umbreon"},
            {198, "Murkrow"},
            {199, "Slowking"},
            {200, "Misdreavus"},
            {201, "Unown"},
            {202, "Wobbuffet"},
            {203, "Girafarig"},
            {204, "Pineco"},
            {205, "Forretress"},
            {206, "Dunsparce"},
            {207, "Gligar" },
            {208, "Steelix"},
            {209, "Snubbull"},
            {210, "Granbull"},
            {211, "Qwilfish"},
            {212, "Scizor"},
            {213, "Shuckle"},
            {214, "Heracross"},
            {215, "Sneasel"},
            {216, "Teddiursa"},
            {217, "Ursaring"},
            {218, "Slugma"},
            {219, "Magcargo"},
            {220, "Swinub"},
            {221, "Piloswine"},
            {222, "Corsola"},
            {223, "Remoraid"},
            {224, "Octillery" },
            {225, "Delibird"},
            {226, "Mantine"},
            {227, "Skarmory"},
            {228, "Houndour"},
            {229, "Houndoom"},
            {230, "Kingdra"},
            {231, "Phanpy"},
            {232, "Donphan"},
            {233, "Porygon2"},
            {234, "Stantler"},
            {235, "Smeargle"},
            {236, "Tyrogue"},
            {237, "Hitmontop"},
            {238, "Smoochum"},
            {239, "Elekid" },
            {240, "Magby"},
            {241, "Miltank"},
            {242, "Blissey"},
            {243, "Raikou"},
            {244, "Entei"},
            {245, "Suicune"},
            {246, "Larvitar"},
            {247, "Pupitar"},
            {248, "Tyranitar"},
            {249, "Lugia"},
            {250, "Ho-oh"},
            {251, "Celebi"}
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
                    var nickname = await GetNameRed(i);
                    if (!string.Equals(species, "Unknown")) party.Add(new PokemonViewModel(species, nickname, "Red"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get party: {e.Message}");
            }

            return party;
        }

        public async Task<List<PokemonViewModel>> GetPartyGold()
        {
            var party = new List<PokemonViewModel>();
            try
            {
                var partyLengthRequest = new HttpRequestMessage(HttpMethod.Get, "/core/readrange?address=0xDA22&length=1");
                var partyLength = await _client.SendAsync(partyLengthRequest);
                partyLength.EnsureSuccessStatusCode();
                var partyString = await partyLength.Content.ReadAsStringAsync();
                var partyInt = Int32.Parse(partyString);

                for (int i = 1; i <= partyInt; i++)
                {
                    var requestAddress = 22 + i;
                    var pokemonRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xDA{requestAddress}&length=1");
                    var pokemonResponse = await _client.SendAsync(pokemonRequest);
                    pokemonResponse.EnsureSuccessStatusCode();
                    var pokemonString = await pokemonResponse.Content.ReadAsStringAsync();
                    var pokeIndex = int.Parse(pokemonString, System.Globalization.NumberStyles.HexNumber);
                    var species = SpeciesMap.ContainsKey(pokeIndex) ? SpeciesMap[pokeIndex] : "Unknown";
                    var nickname = await GetNameGold(i);
                    if (!string.Equals(species, "Unknown")) party.Add(new PokemonViewModel(species, nickname, "Gold"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get party: {e.Message}");
            }

            return party;
        }

        private async Task<string> GetNameRed(int partyIndex)
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

                return GetTextFromBytes(nickname);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get nickname: {e.Message}");
                return null;
            }
        }

        private async Task<string> GetNameGold(int partyIndex)
        {
            var requestAddress = partyIndex switch
            {
                1 => "0xDB8C",
                2 => "0xDB97",
                3 => "0xDBA2",
                4 => "0xDBAD",
                5 => "0xDBB8",
                6 => "0xDBC3"
            };
            try
            {
                var nickRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address={requestAddress}&length=11");
                var nickResponse = await _client.SendAsync(nickRequest);
                nickResponse.EnsureSuccessStatusCode();
                var nickname = (await nickResponse.Content.ReadAsStringAsync()).Split(",");
                return GetTextFromBytes(nickname);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get nickname: {e.Message}");
                return null;
            }
        }

        internal async Task<String> GetPlayerNameRed()
        {
            try
            {
                var nameRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xD158&length=11");
                var nameResponse = await _client.SendAsync(nameRequest);
                nameResponse.EnsureSuccessStatusCode();

                var name = (await nameResponse.Content.ReadAsStringAsync()).Split(",");

                return GetTextFromBytes(name);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get name: {e.Message}");
                return null;
            }
        }

        internal async Task<String> GetPlayerNameGold()
        {
            try
            {
                var nameRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xD1A3&length=11");
                var nameResponse = await _client.SendAsync(nameRequest);
                nameResponse.EnsureSuccessStatusCode();

                var name = (await nameResponse.Content.ReadAsStringAsync()).Split(",");

                return GetTextFromBytes(name);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get name: {e.Message}");
                return null;
            }
        }

        public async Task<char[]> GetBadgesRed()
        {
            var badgeRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xD356&length=1");
            var badgeResponse = await _client.SendAsync(badgeRequest);
            badgeResponse.EnsureSuccessStatusCode();
            string badgeString = await badgeResponse.Content.ReadAsStringAsync();

            var badgeBinary = Convert.ToString(Convert.ToInt32(badgeString, 16), 2);
            while (badgeBinary.Length < 8)
            {
                badgeBinary = "0" + badgeBinary;
            }
            var hasBadge = badgeBinary.ToCharArray();

            return hasBadge;
        }

        public async Task<char[]> GetBadgesGold()
        {
            var badgeRequest = new HttpRequestMessage(HttpMethod.Get, $"/core/readrange?address=0xD57C&length=1");
            var badgeResponse = await _client.SendAsync(badgeRequest);
            badgeResponse.EnsureSuccessStatusCode();
            string badgeString = await badgeResponse.Content.ReadAsStringAsync();

            var badgeBinary = Convert.ToString(Convert.ToInt32(badgeString, 16), 2);
            while (badgeBinary.Length < 8)
            {
                badgeBinary = "0" + badgeBinary;
            }
            var hasBadge = badgeBinary.ToCharArray();

            return hasBadge;
        }

        private string GetTextFromBytes(string[] bytes)
        {
            string text = "";
            foreach (var charId in bytes)
            {
                if (charId == "50")
                {
                    break;
                }

                text += RedCharacterMap.ContainsKey(charId) ? RedCharacterMap[charId] : " ";
            }
            return text;
        }
    }
}
