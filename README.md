# Internet Plays Pokémon

Internet Plays Pokémon is a .NET application allowing users to play Pokémon games through a livestream chat. Built with C# and Avalonia to be run on Windows, Linux, and MacOS, it includes a graphical chat interface to be included on stream.

## Installation

Download the latest release from the [Releases page](https://github.com/jwedison/Internet-Plays-Pokemon/releases/latest) and the latest copy of [mGBA](https://mgba.io/) and [mGBA-http](https://github.com/nikouu/mGBA-http/releases/latest). Currently, the application is only available for Windows. Linux and MacOS builds are planned for future releases.

## Usage

Open the settings menu (Ctrl + ,) to configure the application. Select the streaming platform you are using (Twitch, YouTube currently supported) and enter your username, and copy the path to both mGBA and mGBA-http's .exe files (including the file name). then click Save and Launch mGBA-http.

In mGBA, click Tools > Scripting to open the Scripting window. Load `mGBASocketServer.lua`.

The application will display all messages from the chat in a graphical interface. Users can type commands in the chat to control the game, such as "up", "down", "left", "right", "A", "B", etc. The application will press the corresponding buttons in the game based on the commands received from the chat.

## Credits
The latest release of Internet Plays Pokémon would not be possible without the help of the following people:
- [@nikouu](https://github.com/nikouu) for creating mGBA-http, which allows for the communication between the game and the application.