using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using internetplays.Services;
using SkiaSharp;

namespace internetplays.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ChatServiceViewModel _chatService;

    public ObservableCollection<GameSelectViewModel> Games { get; set; } = new();
    //[ObservableProperty]
    //private GameSelectViewModel _selectedGame;

    [ObservableProperty]
    private SettingsOptionsViewModel _settingsOptions = new();

    [ObservableProperty]
    private bool _settingsOpen = false;

    private GBAClient _gbaClient;

    public ObservableCollection<String> ChatServices { get; set; } = new() { "Twitch", "YouTube" };

    [ObservableProperty]
    private TrainerViewModel _trainer;

    private static readonly string SettingsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "InternetPlays");

    private static readonly string SettingsPath = Path.Combine(
        SettingsDir,
        "settings.json");

    private IChatLogger _chatLogger;

    private PeriodicTimer _timer;

    public MainViewModel(IChatLogger chatLogger, GBAClient gbaClient)
    {
        _chatLogger = chatLogger;
        _gbaClient = gbaClient;
        Games.Add(new("Pokémon Red", "PKMNRED.png", "GB"));
        Games.Add(new("Pokémon Blue", "PKMNBLUE.png", "GB"));
        Games.Add(new("Pokémon Yellow", "PKMNYELLOW.png", "GBC"));
        Games.Add(new("Pokémon Gold", "PKMNGOLD.png", "GBC"));
        Games.Add(new("Pokémon Silver", "PKMNSILVER.png", "GBC"));
        Games.Add(new("Pokémon Crystal", "PKMNCRYSTAL.png", "GBC"));
        Games.Add(new("Pokémon Ruby", "PKMNRUBY.png", "GBA"));
        Games.Add(new("Pokémon Sapphire", "PKMNSAPPHIRE.png", "GBA"));
        Games.Add(new("Pokémon Emerald", "PKMNEMERALD.jpg", "GBA"));
        Games.Add(new("Pokémon FireRed", "PKMNFIRERED.png", "GBA"));
        Games.Add(new("Pokémon LeafGreen", "PKMNLEAFGREEN.png", "GBA"));

        LoadSettings();
        if (SettingsOptions.Username != null)
        {
            if (SettingsOptions.SelectedService == "Twitch")
            {
                ChatService = new TwitchViewModel(SettingsOptions.Username, _chatLogger, _gbaClient);
            }
            else if (SettingsOptions.SelectedService == "YouTube")
            {
                ChatService = new YoutubeViewModel(SettingsOptions.Username, _chatLogger, _gbaClient);
            }
        }
    }

    [RelayCommand]
    public async Task LaunchPrograms()
    {
        if (SettingsOptions.mGBAPath == null || SettingsOptions.mGBAHttpPath == null) return;

        try
        {
            Process.Start(SettingsOptions.mGBAPath);
            Process.Start(SettingsOptions.mGBAHttpPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to launch programs: " + ex);
        }

        await HandleTimer();
    }

    private async Task HandleTimer()
    {
        if (_timer is null) _timer = new(TimeSpan.FromMinutes(1));
        while (await _timer.WaitForNextTickAsync())
        {
            switch (Trainer.Game.GameName)
            {
                case "Pokémon Red":
                case "Pokémon Blue":
                    await GetTrainerDataRB();
                    break;
                case "Pokémon Gold":
                case "Pokémon Silver":
                    await GetTrainerDataGS();
                    break;
                //case "Pokémon Crystal":
                //    await GetTrainerDataCrystal();
                //    break;

            }
        }
    }

    private async Task GetTrainerDataRB()
    {
        var party = await _gbaClient.GetPartyRed();
        var playerName = await _gbaClient.GetPlayerNameRed();
        var badges = await _gbaClient.GetBadgesRed();

        SetTrainerData(playerName, party, badges);
    }

    private async Task GetTrainerDataGS()
    {
        var party = await _gbaClient.GetPartyGold();
        var playerName = await _gbaClient.GetPlayerNameGold();
        var badges = await _gbaClient.GetBadgesGold();

        SetTrainerData(playerName, party, badges);
    }

    private void SetTrainerData(string playerName, List<PokemonViewModel> party, char[] badges)
    {
        Trainer.Name = playerName;
        Trainer.Party.Clear();
        foreach (var pokemon in party)
        {
            Trainer.Party.Add(pokemon);
        }
        Trainer.HasBadge1 = badges[7] == '1';
        Trainer.HasBadge2 = badges[6] == '1';
        Trainer.HasBadge3 = badges[5] == '1';
        Trainer.HasBadge4 = badges[4] == '1';
        Trainer.HasBadge5 = badges[3] == '1';
        Trainer.HasBadge6 = badges[2] == '1';
        Trainer.HasBadge7 = badges[1] == '1';
        Trainer.HasBadge8 = badges[0] == '1';

        Trainer.UpdateBadgeImages();
    }

    [RelayCommand]
    public void OpenSettings()
    {
        SettingsOpen = true;
    }

    [RelayCommand]
    public void CloseSettings()
    {
        SettingsOpen = false;
    }

    [RelayCommand]
    public void SaveSettings()
    {
        if (SettingsOptions.SelectedService == "Twitch")
        {
            ChatService.Dispose();
            ChatService = new TwitchViewModel(SettingsOptions.Username, _chatLogger, _gbaClient);
        }
        else if (SettingsOptions.SelectedService == "YouTube")
        {
            ChatService.Dispose();
            ChatService = new YoutubeViewModel(SettingsOptions.Username, _chatLogger, _gbaClient);
        }
        SettingsOptions.SelectedGame = Trainer.Game.GameName; 
        WriteSettings();
    }

    private void LoadSettings()
    {
        if (!File.Exists(SettingsPath))
        {
            SettingsOptions = new();
            SettingsOptions.SelectedGame = Games[0].GameName;
            Trainer = new TrainerViewModel("", Games[0]);
            SettingsOptions.SelectedService = "Twitch";
            SettingsOptions.Username = "";
        } 
        else
        {
            var json = File.ReadAllText(SettingsPath);
            try
            {
                SettingsOptions = JsonSerializer.Deserialize<SettingsOptionsViewModel>(json);
                Trainer = new TrainerViewModel("", Games.FirstOrDefault(game => game.GameName == SettingsOptions.SelectedGame));
            }
            catch (Exception ex)
            {
                SettingsOptions = new();
                SettingsOptions.SelectedGame = Games[0].GameName;
                Trainer = new TrainerViewModel("", Games[0]);
                SettingsOptions.SelectedService = "Twitch";
                SettingsOptions.Username = "";
            }

        }
    }

    private void WriteSettings()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        try
        {
            var settingsJson = JsonSerializer.Serialize(SettingsOptions, options);

            if (!string.IsNullOrEmpty(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }

            File.WriteAllText(SettingsPath, settingsJson);
            Console.WriteLine($"Settings saved to {SettingsPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save settings: " + ex);
        }
    }
}
