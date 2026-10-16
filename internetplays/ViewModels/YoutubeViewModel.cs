using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using internetplays.Services;
using Microsoft.Extensions.Logging;
using YTLiveChat.Contracts;
using YTLiveChat.Contracts.Models;
using YTLiveChat.Contracts.Services;
using YTLiveChat.Services;

namespace internetplays.ViewModels;

public partial class YoutubeViewModel : ChatServiceViewModel
{
    [ObservableProperty]
    private IYTLiveChat _chatClient;


    public YoutubeViewModel(string username, IChatLogger? chatLogger, GBAClient gbaClient) : base(username, chatLogger, gbaClient)
    {
        Messages = new();

        var options = new YTLiveChatOptions
        {
            RequestFrequency = 1000
        };

        var http = new HttpClient();
        http.BaseAddress ??= new Uri("https://www.youtube.com");

        var client = new YTHttpClient(http);
        _chatClient = new YTLiveChat.Services.YTLiveChat(options, client);

        _chatClient.ChatReceived += (_, e) => Client_OnMessageReceived(e.ChatItem);

        _chatClient.Start(handle: "@" + username);
    }


    private async Task Client_OnMessageReceived(ChatItem message)
    {
        if (message != null && !string.IsNullOrWhiteSpace(message.Message.ToString()))
        {
            var username = message.Author.Name;
            var messageText = GetMessagePlainText(message.Message);

            var textColor = message.IsModerator ? "#5474cc" : message.IsOwner ? "#fcd330" : "#FFFFFF";
            var msg = new MessageViewModel(username, messageText, textColor, message.IsModerator);

            _chatLogger?.LogMessage(msg.ToString());
            Messages.Add(msg);
            await HandleCommand(msg);
        }
    }

    private string GetMessagePlainText(MessagePart[] message)
    {
        string fullMessage = "";
        foreach (MessagePart part in message) 
        {
            if (part is TextPart text)
            {
                fullMessage += text.Text;
            } 
            else if (part is EmojiPart emoji)
            {
                fullMessage += emoji.EmojiText;
            }
        }
        return fullMessage;
    }
}
