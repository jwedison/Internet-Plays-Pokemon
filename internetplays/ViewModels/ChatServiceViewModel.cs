using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Headers;
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

    internal IChatLogger? _chatLogger;

    internal GBAClient _gbaClient;

    public ChatServiceViewModel(string username, IChatLogger? chatLogger, GBAClient gbaClient)
    {
        _channel = username;
        _chatLogger = chatLogger;
        _gbaClient = gbaClient;
    }

    public void Dispose()
    {
        _chatLogger = null;
    }

    internal async Task HandleCommand(MessageViewModel message)
    {
        var msg = message.Message.Trim().ToLower();
        if (msg[0] == '!')
        {
            await HandleAdminCommand(message);
            return;
        }
        if (Regex.IsMatch(msg, @"^([a|b]|start|select|up|down|left|right|l|r)\*?[0-9]?$", RegexOptions.IgnoreCase))
        {
            Console.WriteLine($"Valid command received: {message.Message}");

            if (message.Message.Contains("*"))
            {
                var parts = message.Message.Split('*');
                var command = CapitalizeFirstLetter(parts[0]);
                if (int.TryParse(parts[1], out int repeatCount))
                {
                    for (int i = 0; i < repeatCount; i++)
                    {
                        await _gbaClient.PressKey(command);
                    }
                }
            }
            
            else
            {
                var command = CapitalizeFirstLetter(msg);
                await _gbaClient.PressKey(command);
            }
        }
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (input.Length <= 1)
        {
            return input.ToUpper();
        }
        else
        {
            return input.Substring(0, 1).ToUpper() + input.Substring(1);
        }
    }

    private async Task HandleAdminCommand(MessageViewModel message)
    {
        var msg = message.Message.Trim().ToLower();
        Console.WriteLine($"Admin command received: {msg}");

        if (!message.IsAdmin) return;

        if (Regex.IsMatch(msg, @"^!savestate\d$"))
        {
            await _gbaClient.SaveState(int.Parse(msg[10].ToString()));
        }
    }


}
