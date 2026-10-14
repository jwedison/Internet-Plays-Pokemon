using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using internetplays.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace internetplays.ViewModels;

public class ChatServiceViewModel : ViewModelBase, IDisposable
{

    internal string _channel;

    public ObservableCollection<MessageViewModel> Messages { get; set; }

    internal readonly IChatLogger? _chatLogger;

    public ChatServiceViewModel(string username, IChatLogger? chatLogger)
    {
        _channel = username;
        _chatLogger = chatLogger;
    }

    public void Dispose()
    {
        _chatLogger?.Dispose();
    }


}
