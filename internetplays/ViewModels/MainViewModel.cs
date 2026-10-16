using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using internetplays.Services;

namespace internetplays.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ChatServiceViewModel _chatService;

    public ObservableCollection<GameSelectViewModel> Games { get; set; } = new();
    [ObservableProperty]
    private GameSelectViewModel _selectedGame;

    [ObservableProperty]
    private SettingsOptionsViewModel _settingsOptions = new();

    [ObservableProperty]
    private bool _settingsOpen = false;

    private GBAClient _gbaClient;

    public ObservableCollection<String> ChatServices { get; set; } = new() { "Twitch", "YouTube" };

    private static readonly string SettingsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "InternetPlays");

    private static readonly string SettingsPath = Path.Combine(
        SettingsDir,
        "settings.json");

    private IChatLogger _chatLogger;


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
    public void LaunchPrograms()
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
        SettingsOptions.SelectedGame = SelectedGame.GameName; 
        WriteSettings();
    }

    private void LoadSettings()
    {
        if (!File.Exists(SettingsPath))
        {
            SettingsOptions = new();
            SettingsOptions.SelectedGame = Games[0].GameName;
            SelectedGame = Games[0];
            SettingsOptions.SelectedService = "Twitch";
            SettingsOptions.Username = "";
        } 
        else
        {
            var json = File.ReadAllText(SettingsPath);
            try
            {
                SettingsOptions = JsonSerializer.Deserialize<SettingsOptionsViewModel>(json);
                SelectedGame = Games.FirstOrDefault(game => game.GameName == SettingsOptions.SelectedGame);
            }
            catch (Exception ex)
            {
                SettingsOptions = new();
                SettingsOptions.SelectedGame = Games[0].GameName;
                SelectedGame = Games[0];
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
