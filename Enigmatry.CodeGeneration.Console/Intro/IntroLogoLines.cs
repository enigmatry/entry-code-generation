using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal static class IntroLogoLines
    {
        public static List<IntroLogoLine> Get =>
            new Dictionary<string, List<IntroLogoLine>>()
            {
                {
                    "default_1",
                     new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@"   _____          _         _____                           _             ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"  / ____|        | |       / ____|                         | |            ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" | |     ___   __| | ___  | |  __  ___ _ __   ___ _ __ __ _| |_ ___  _ __ ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" | |    / _ \ / _` |/ _ \ | | |_ |/ _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|", ConsoleColor.Yellow),
                        new IntroLogoLine(@" | |___| (_) | (_| |  __/ | |__| |  __/ | | |  __/ | | (_| | || (_) | |   ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"  \_____\___/ \__,_|\___|  \_____|\___|_| |_|\___|_|  \__,_|\__\___/|_|   ", ConsoleColor.Yellow),
                    }
                },
                {
                    "default_2",
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@"    ___          _          ___                          _             ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"   / __\___   __| | ___    / _ \___ _ __   ___ _ __ __ _| |_ ___  _ __ ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"  / /  / _ \ / _` |/ _ \  / /_\/ _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|", ConsoleColor.Yellow),
                        new IntroLogoLine(@" / /__| (_) | (_| |  __/ / /_\\  __/ | | |  __/ | | (_| | || (_) | |   ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" \____/\___/ \__,_|\___| \____/\___|_| |_|\___|_|  \__,_|\__\___/|_|   ", ConsoleColor.Yellow),
                    }
                },
                {
                    "italic_1",
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@"    ______          __        ______                           __            ", ConsoleColor.Yellow),
                        new IntroLogoLine(@"   / ____/___  ____/ /__     / ____/__  ____  ___  _________ _/ /_____  _____", ConsoleColor.Yellow),
                        new IntroLogoLine(@"  / /   / __ \/ __  / _ \   / / __/ _ \/ __ \/ _ \/ ___/ __ `/ __/ __ \/ ___/", ConsoleColor.Yellow),
                        new IntroLogoLine(@" / /___/ /_/ / /_/ /  __/  / /_/ /  __/ / / /  __/ /  / /_/ / /_/ /_/ / /    ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" \____/\____/\__,_/\___/   \____/\___/_/ /_/\___/_/   \__,_/\__/\____/_/     ", ConsoleColor.Yellow),
                    }
                },
                {
                    "speed_1",
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" _________     _________          _________                              _____              ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" __  ____/___________  /____      __  ____/_____________________________ __  /______________", ConsoleColor.Yellow),
                        new IntroLogoLine(@" _  /    _  __ \  __  /_  _ \     _  / __ _  _ \_  __ \  _ \_  ___/  __ `/  __/  __ \_  ___/", ConsoleColor.Yellow),
                        new IntroLogoLine(@" / /___  / /_/ / /_/ / /  __/     / /_/ / /  __/  / / /  __/  /   / /_/ // /_ / /_/ /  /    ", ConsoleColor.Yellow),
                        new IntroLogoLine(@" \____/  \____/\__,_/  \___/      \____/  \___//_/ /_/\___//_/    \__,_/ \__/ \____//_/     ", ConsoleColor.Yellow),
                    }
                },
                {
                    "bloodbath_1",
                    new List<IntroLogoLine>()
                    {
                        new IntroLogoLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█", ConsoleColor.DarkCyan),
                        new IntroLogoLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░                                                      ", ConsoleColor.DarkCyan, false),
                        new IntroLogoLine(@"Friday the 13th edition", ConsoleColor.Red),
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
            }[DateTime.Now.IsFriday13th() ? "bloodbath_1" : "italic_1"];
    }
}
