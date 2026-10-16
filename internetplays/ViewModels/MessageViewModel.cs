using System;
using System.ComponentModel;
using System.Threading;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace internetplays.ViewModels;

public partial class MessageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private string _usernameColor;

    [ObservableProperty]
    private bool _isAdmin;

    public MessageViewModel(string username, string message, string color = "#FFFFFF", bool isAdmin = false)
    {
        Username = username;
        Message = message;
        UsernameColor = color;
        IsAdmin = isAdmin;
    }

    public override string ToString()
    {
        return $"{Username}: {Message}";
    }
}
