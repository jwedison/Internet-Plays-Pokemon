using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace internetplays.ViewModels;

public partial class TrainerViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private GameSelectViewModel _game;

    public ObservableCollection<PokemonViewModel> Party { get; } = new();

    [ObservableProperty] private Bitmap _playerImage;

    [ObservableProperty] private Bitmap _badge1;
    [ObservableProperty] private Bitmap _badge2;
    [ObservableProperty] private Bitmap _badge3;
    [ObservableProperty] private Bitmap _badge4;
    [ObservableProperty] private Bitmap _badge5;
    [ObservableProperty] private Bitmap _badge6;
    [ObservableProperty] private Bitmap _badge7;
    [ObservableProperty] private Bitmap _badge8;

    public bool HasBadge1;
    public bool HasBadge2;
    public bool HasBadge3;
    public bool HasBadge4;
    public bool HasBadge5;
    public bool HasBadge6;
    public bool HasBadge7;
    public bool HasBadge8;

    private string _region;

    public TrainerViewModel(string name, GameSelectViewModel game)
    {
        Name = name;
        Game = game;
        SetGame();
    }

    private void SetGame()
    {
        switch (Game.GameName)
        {
            case "Pokémon Red":
            case "Pokémon Blue":
            case "Pokémon Yellow":
            case "Pokémon FireRed":
            case "Pokémon LeafGreen":
                _region = "Kanto";
                break;
            case "Pokémon Gold":
            case "Pokémon Silver":
            case "Pokémon Crystal":
                _region = "Johto";
                break;
        }
        UpdateBadgeImages();

        var assetUri = new System.Uri($"avares://internetplays/Assets/Trainers/{_region}.png");
        PlayerImage = new Bitmap(AssetLoader.Open(assetUri));
    }
    
    public void UpdateBadgeImages()
    {
        var assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge1 ? "1.png" : "NOT_1.png"));
        Badge1 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge2 ? "2.png" : "NOT_2.png"));
        Badge2 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge3 ? "3.png" : "NOT_3.png"));
        Badge3 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge4 ? "4.png" : "NOT_4.png"));
        Badge4 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge5 ? "5.png" : "NOT_5.png"));
        Badge5 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge6 ? "6.png" : "NOT_6.png"));
        Badge6 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge7 ? "7.png" : "NOT_7.png"));
        Badge7 = new Bitmap(AssetLoader.Open(assetUri));

        assetUri = new System.Uri($"avares://internetplays/Assets/Badges/{_region}/" + (HasBadge8 ? "8.png" : "NOT_8.png"));
        Badge8 = new Bitmap(AssetLoader.Open(assetUri));
    }
    partial void OnGameChanged(GameSelectViewModel value) => SetGame();
}
