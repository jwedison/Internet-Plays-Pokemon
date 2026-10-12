using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace internetplays.ViewModels;

public partial class GameSelectViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private Bitmap _image;

    [ObservableProperty]
    private string _console;

    public GameSelectViewModel(string name, string path, string console)
    {
        GameName = name;
        var assetUri = new System.Uri("avares://internetplays/Assets/GameImages/" + path);
        Image = new Bitmap(AssetLoader.Open(assetUri));
        _console = console;
    }
}
