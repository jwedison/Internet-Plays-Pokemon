using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace internetplays.ViewModels;

public class ChatServiceViewModel : ViewModelBase
{

    internal string _channel;

    public ObservableCollection<MessageViewModel> Messages { get; set; }

    public ChatServiceViewModel(string username)
    {
        _channel = username;
    }


}
