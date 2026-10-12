using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TwitchLib.Client.Models;
using WindowsInput;
using WindowsInput.Native;

namespace internetplays.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    internal async Task HandleCommand(MessageViewModel message)
    {
        if (Regex.IsMatch(message.Message, @"^([a|b]|start|select|up|down|left|right|l|r)\*?[0-9]?$", RegexOptions.IgnoreCase))
        {
            Console.WriteLine($"Valid command received: {message.Message}");

            if (message.Message.Contains("*"))
            {
                var parts = message.Message.Split('*');
                var command = parts[0].ToLower();
                if (int.TryParse(parts[1], out int repeatCount))
                {
                    for (int i = 0; i < repeatCount; i++)
                    {
                        await HandleKeyPress(command);
                    }
                }
            }
            else await HandleKeyPress(message.Message);
        }
    }

    private async Task HandleKeyPress(string command)
    {
        var keypress = new InputSimulator();
        switch (command.ToLower())
        {
            case "a":
                keypress.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                break;
            case "b":
                keypress.Keyboard.KeyDown(VirtualKeyCode.VK_B);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.VK_B);
                break;
            case "start":
                keypress.Keyboard.KeyDown(VirtualKeyCode.RETURN);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.RETURN);
                break;
            case "select":
                keypress.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                break;
            case "up":
                keypress.Keyboard.KeyDown(VirtualKeyCode.UP);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.UP);
                break;
            case "down":
                keypress.Keyboard.KeyDown(VirtualKeyCode.DOWN);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.DOWN);
                break;
            case "left":
                keypress.Keyboard.KeyDown(VirtualKeyCode.LEFT);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.LEFT);   
                break;
            case "right":
                keypress.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
                break;
            case "l":
                keypress.Keyboard.KeyDown(VirtualKeyCode.VK_L);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.VK_L);
                break;
            case "r":
                keypress.Keyboard.KeyDown(VirtualKeyCode.VK_R);
                await Task.Delay(250); // Simulate key press duration
                keypress.Keyboard.KeyUp(VirtualKeyCode.VK_R);
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                return;
        }
        await Task.Delay(100); // Delay between key presses
    }
}
