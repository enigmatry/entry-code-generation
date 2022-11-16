using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Tools.Intro
{
    internal static class IntroLogoLines
    {
        public static List<IntroLogoLine> Get =>
            new Dictionary<string, List<IntroLogoLine>>()
            {
                {
                    IntroLogoNames.Italic1,
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@"   ______          __        ______                           __            ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"  / ____/___  ____/ /__     / ____/__  ____  ___  _________ _/ /_____  _____", ConsoleColor.Yellow),
                        new IntroLogoLine(@" / /   / __ \/ __  / _ \   / / __/ _ \/ __ \/ _ \/ ___/ __ `/ __/ __ \/ ___/", ConsoleColor.Yellow),
                        new IntroLogoLine(@" \____/\____/\__,_/\___/   \____/\___/_/ /_/\___/_/   \__,_/\__/\____/_/     ", ConsoleColor.Yellow),
                    }
                },
                {
                    IntroLogoNames.Speed1,
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" _________     _________          _________                              _____              ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" __  ____/___________  /____      __  ____/_____________________________ __  /______________", ConsoleColor.Yellow),
                        new IntroLogoLine(@" _  /    _  __ \  __  /_  _ \     _  / __ _  _ \_  __ \  _ \_  ___/  __ `/  __/  __ \_  ___/", ConsoleColor.Yellow),
                        new IntroLogoLine(@" / /___  / /_/ / /_/ / /  __/     / /_/ / /  __/  / / /  __/  /   / /_/ // /_ / /_/ /  /    ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" \____/  \____/\__,_/  \___/      \____/  \___//_/ /_/\___//_/    \__,_/ \__/ \____//_/     ", ConsoleColor.Yellow),
                    }
                },
                {
                    IntroLogoNames.Isometric1,
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ ░█░ █▀▄ ░█░  ", ConsoleColor.DarkCyan, false),
                        new IntroLogoLine(@" THE WEEKEND EDITION ").WithColors(ConsoleColor.White, ConsoleColor.DarkGreen),
                        new IntroLogoLine(@"                  __                                                            __                   ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"                 /\ \                                                          /\ \__                ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"   ___    ___    \_\ \     __          __      __    ___      __   _ __    __  \ \ ,_\   ___   _ __  ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"  /'___\ / __`\  /'_` \  /'__`\      /'_ `\  /'__`\/' _ `\  /'__`\/\`'__\/'__`\ \ \ \/  / __`\/\`'__\").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@" /\ \__//\ \L\ \/\ \L\ \/\  __/     /\ \L\ \/\  __//\ \/\ \/\  __/\ \ \//\ \L\.\_\ \ \_/\ \L\ \ \ \/ ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@" \ \____\ \____/\ \___,_\ \____\    \ \____ \ \____\ \_\ \_\ \____\\ \_\\ \__/.\_\\ \__\ \____/\ \_\ ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"  \/____/\/___/  \/__,_ /\/____/     \/___L\ \/____/\/_/\/_/\/____/ \/_/ \/__/\/_/ \/__/\/___/  \/_/ ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"                                       /\____/                                                       ").WithColors(ConsoleColor.Green),
                        new IntroLogoLine(@"                                       \_/__/                                                        ").WithColors(ConsoleColor.Green),
                    }
                },
                {
                    IntroLogoNames.Bloodbath1,
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ ░█░ █▀▄ ░█░  ", ConsoleColor.DarkCyan, false),
                        new IntroLogoLine(@" FRIDAY THE 13th EDITION ").WithColors(ConsoleColor.White, ConsoleColor.DarkRed),
                        new IntroLogoLine(@"", ConsoleColor.Red),
                        new IntroLogoLine(@"  ▄████▄   ▒█████  ▓█████▄ ▓█████      ▄████ ▓█████  ███▄    █ ▓█████  ██▀███   ▄▄▄     ▄▄▄█████▓ ▒█████   ██▀███  ", ConsoleColor.Red),
                        new IntroLogoLine(@" ▒██▀ ▀█  ▒██▒  ██▒▒██▀ ██▌▓█   ▀     ██▒ ▀█▒▓█   ▀  ██ ▀█   █ ▓█   ▀ ▓██ ▒ ██▒▒████▄   ▓  ██▒ ▓▒▒██▒  ██▒▓██ ▒ ██▒", ConsoleColor.Red),
                        new IntroLogoLine(@" ▒▓█    ▄ ▒██░  ██▒░██   █▌▒███      ▒██░▄▄▄░▒███   ▓██  ▀█ ██▒▒███   ▓██ ░▄█ ▒▒██  ▀█▄ ▒ ▓██░ ▒░▒██░  ██▒▓██ ░▄█ ▒", ConsoleColor.Red),
                        new IntroLogoLine(@" ▒▓▓▄ ▄██▒▒██   ██░░▓█▄   ▌▒▓█  ▄    ░▓█  ██▓▒▓█  ▄ ▓██▒  ▐▌██▒▒▓█  ▄ ▒██▀▀█▄  ░██▄▄▄▄██░ ▓██▓ ░ ▒██   ██░▒██▀▀█▄  ", ConsoleColor.Red),
                        new IntroLogoLine(@" ▒ ▓███▀ ░░ ████▓▒░░▒████▓ ░▒████▒   ░▒▓███▀▒░▒████▒▒██░   ▓██░░▒████▒░██▓ ▒██▒ ▓█   ▓██▒ ▒██▒ ░ ░ ████▓▒░░██▓ ▒██▒", ConsoleColor.Red),
                        new IntroLogoLine(@" ░ ░▒ ▒  ░░ ▒░▒░▒░  ▒▒▓  ▒ ░░ ▒░ ░    ░▒   ▒ ░░ ▒░ ░░ ▒░   ▒ ▒ ░░ ▒░ ░░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ▒ ░░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░", ConsoleColor.Red),
                        new IntroLogoLine(@"   ░  ▒     ░ ▒ ▒░  ░ ▒  ▒  ░ ░  ░     ░   ░  ░ ░  ░░ ░░   ░ ▒░ ░ ░  ░  ░▒ ░ ▒░  ▒   ▒▒ ░   ░      ░ ▒ ▒░   ░▒ ░ ▒░", ConsoleColor.Red),
                        new IntroLogoLine(@" ░        ░ ░ ░ ▒   ░ ░  ░    ░      ░ ░   ░    ░      ░   ░ ░    ░     ░░   ░   ░   ▒    ░      ░ ░ ░ ▒    ░░   ░ ", ConsoleColor.Red),
                        new IntroLogoLine(@" ░ ░          ░ ░     ░       ░  ░         ░    ░  ░         ░    ░  ░   ░           ░  ░            ░ ░     ░     ", ConsoleColor.Red),
                        new IntroLogoLine(@" ░                  ░                                                                                              ", ConsoleColor.Red),
                    }
                },
            }[GetLogoName(DateTime.Now)];

        private static string GetLogoName(DateTime dateTime)
        {
            if (dateTime.IsFriday13th())
            {
                return IntroLogoNames.Bloodbath1;
            }
            if (dateTime.IsWeekend())
            {
                return IntroLogoNames.Isometric1;
            }
            if (dateTime.IsMorning())
            {
                return IntroLogoNames.Speed1;
            }
            return IntroLogoNames.Italic1;
        }
    }
}
