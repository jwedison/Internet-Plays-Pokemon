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

public partial class TwitchViewModel : ChatServiceViewModel
{
    [ObservableProperty]

    ILoggerFactory _loggerFactory;

    TwitchClient _chatClient;

    [ObservableProperty]
    private bool _isConnected = false;

    public TwitchViewModel(string username, IChatLogger? chatLogger, GBAClient gbaClient) : base(username, chatLogger, gbaClient)
    {
        Messages = new();

        _loggerFactory = new LoggerFactory();
        ConnectionCredentials credentials = new();

        
        _chatClient = new TwitchClient(loggerFactory: _loggerFactory);

        _chatClient.Initialize(credentials);

        _chatClient.OnConnected += Client_OnConnected;
        _chatClient.OnMessageReceived += Client_OnMessageReceived;

        _chatClient.ConnectAsync();
    }

    private async Task Client_OnConnected(object? sender, OnConnectedEventArgs e)
    {
        try
        {
            await _chatClient.JoinChannelAsync(_channel);
            Console.WriteLine($"Successfully connected to Twitch chat as {_channel}");
            IsConnected = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to Twitch chat: {ex.Message}");
        }
    }

    private async Task Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
    {
        var message = e.ChatMessage;
        var msg = new MessageViewModel(message.Username, message.Message, message.HexColor, message.IsBroadcaster);

        Messages.Add(msg);
        _chatLogger?.LogMessage(msg.ToString());

        await HandleCommand(msg);
    }

}
