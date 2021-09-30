﻿using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal static class IntroMessages
    {
        public static List<IntroMessage> Get =>
            new()
            {
                // Regular:
                new IntroMessage("Hello world!"),
                new IntroMessage("Nice day, isn't it?"),
                new IntroMessage("You look handsome ;)"),
                new IntroMessage("Woo-hoo, let's generate some code."),
                new IntroMessage("The Dude abides."),
                new IntroMessage("[MESSAGE_GOES_HERE]"),
                new IntroMessage(@"4@$#F\#9%5Zx~!@$55^9HX"),
                new IntroMessage("What! Who allowed that?"),
                new IntroMessage("Your password is incorrect"),
                new IntroMessage("Please allow me to introduce myself ..."),
                new IntroMessage("Are you sure you are not missing any semicolons?"),
                new IntroMessage("Ah, the great outdoors ..."),
                new IntroMessage("Burek is with meat!"),
                new IntroMessage("\"Fear is the mind-killer\""),
                new IntroMessage("Are you SOLID?"),
                new IntroMessage("Busy, busy, busy ..."),
                new IntroMessage("... beep ... beep ... beep ..."),
                new IntroMessage("Are you a robot?"),
                new IntroMessage("3,14159265358979323846264338327950288 ..."),
                new IntroMessage("Fancy a cup of coffee?"),
                new IntroMessage("Preparing exceptions ..."),
                new IntroMessage("Wash your hands!"),
                new IntroMessage("[ \"hip\", \"hip\" ]"),
                new IntroMessage("Bugs everywhere"),
                new IntroMessage("Be right back"),
                new IntroMessage("Give me a break"),
                new IntroMessage("42"),
                new IntroMessage("Hello sexy 3:)"),
                new IntroMessage("Sup yo!"),
                new IntroMessage("Hello ... I was just thinking about you ;)"),
                new IntroMessage("I love my job! How about you?"),
                new IntroMessage("I love this ... :D"),
                new IntroMessage("\"Work. Work never changes.\""),
                new IntroMessage("\"O Captain, my Captain!\""),
                new IntroMessage("Let's play throw and catch. I'll throw."),
                new IntroMessage("\"If you want to win you mustn't lose\""),
                new IntroMessage("It's perfect day"),
                new IntroMessage("Update me regularly!"),
                new IntroMessage("if (true) { Console.Write(\"I LOVE YOU!\"); }"),
                new IntroMessage("Let's have a meeting"),
                new IntroMessage("Pushing to the limits"),
                new IntroMessage("Push me, pull me"),
                new IntroMessage("Consolidate my nuggets"),
                new IntroMessage("OK, OK, hold your horses, I'll start in a second"),
                new IntroMessage("You want me to generate THAT?"),
                new IntroMessage("I can see sharp"),
                new IntroMessage("01101000 01100101 01101100 01101100 01101111!"),
                new IntroMessage("Running infinite loops ..."),
                new IntroMessage("Booooring"),
                new IntroMessage("Are you ready to rumble?"),
                new IntroMessage("Generating code since 6. April 2021."),
                new IntroMessage("\"Winamp, it really whips the llama's ass!\""),
                new IntroMessage("Log your hours every day!"),
                new IntroMessage("YOLO"),
                new IntroMessage("Follow the white rabbit.")
                    .WithForegroundColor(ConsoleColor.Green),
                new IntroMessage("Good morning! Did you sleep well?")
                    .WithCondition(() => DateTime.Now.IsMorning()),
                new IntroMessage("Don't forget to eat your breakfast")
                    .WithCondition(() => DateTime.Now.IsMorning()),
                new IntroMessage("Nothing beats late night coding")
                    .WithCondition(() => DateTime.Now.IsEvening()),
                new IntroMessage("I see, you are a late bird :-/")
                    .WithCondition(() => DateTime.Now.IsEvening()),
                new IntroMessage("... ZZZZZZZZZ ... ZZZ ... ZZZZZZZZZZZZZZZZ ...")
                    .WithCondition(() => DateTime.Now.IsNight()),
                new IntroMessage("Counting sheep ...")
                    .WithCondition(() => DateTime.Now.IsNight()),
                new IntroMessage("Take it easy, it's Friday.")
                    .WithCondition(() => DateTime.Now.IsFriday()),
                new IntroMessage("Is it happy hour yet?")
                    .WithCondition(() => DateTime.Now.IsFriday()),
                new IntroMessage("It's Friday, I'm in love ...")
                    .WithCondition(() => DateTime.Now.IsFriday()),
                new IntroMessage("The weekend is getting closer")
                    .WithCondition(() => DateTime.Now.IsFriday()),
                new IntroMessage("BLUE MONDAY :'(")
                    .WithCondition(() => DateTime.Now.IsMonday())
                    .WithForegroundColor(ConsoleColor.Cyan),
                new IntroMessage("I don't care if Monday's blue ...")
                    .WithCondition(() => DateTime.Now.IsMonday())
                    .WithForegroundColor(ConsoleColor.Cyan),
                new IntroMessage("I hate Mondays")
                    .WithCondition(() => DateTime.Now.IsMonday()),
                new IntroMessage("P A Y D A Y ! ! !")
                    .WithCondition(() => DateTime.Now.IsFirstDayOfMonth()),
                // Special cases:
                new IntroMessage("It's the weekend and you should not be working >:(")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsWeekend()),
                new IntroMessage("There are better ways to spend your weekends")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsWeekend()),
                new IntroMessage("Go out, have fun. It's the weekend!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsWeekend()),
                new IntroMessage("STOP WORKING!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsWeekend()),
                new IntroMessage("Ah, the best time of the day, lunch time!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("So, what shell we order, Thai, Chinese, Mexican ... PIZZA?")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Woo-hoo, lunch time!!!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("It's Friday the 13th, stay out of the second floors!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("It's Friday the 13th, run in zig-zag!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("Lock your doors")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                // Important dates:
                new IntroMessage("Happy New Year's Eve!!!")
                    .WithType(IntroMessageType.Important)
                    .WithCondition(() => DateTime.Now.IsNewYearsEve()),
                new IntroMessage("Happy birthday Alan Turing!")
                    .WithType(IntroMessageType.Important)
                    .WithCondition(() => new DateTime(1912, 6, 23).IsBirthDay()),
                new IntroMessage("Happy birthday Miljan & Vladan :D")
                    .WithType(IntroMessageType.Important)
                    .WithCondition(() => new DateTime(1985, 9, 12).IsBirthDay()),
            };
    }
}