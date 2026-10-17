using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace internetplays.ViewModels;

public partial class PokemonViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private Bitmap _image;

    public PokemonViewModel(string name, string nickname)
    {
        Name = nickname;
        var assetUri = new System.Uri("avares://internetplays/Assets/Pokemon/" + name + ".png");
        Image = new Bitmap(AssetLoader.Open(assetUri));
    }
}
