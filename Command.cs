﻿using Discord.WebSocket;

namespace TheRemembererDiscordBot
{
    public class Command
    {
        public virtual string? CommandName() => null;
        public virtual string? CommandDescription() => null;
        public virtual object? CommandAction(SocketMessage inputMessage, List<object> args) => null;
    }
}