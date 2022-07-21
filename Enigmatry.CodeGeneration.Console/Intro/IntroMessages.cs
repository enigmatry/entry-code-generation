using System;
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
                new IntroMessage("Less is more!"),
                new IntroMessage("Keep it simple"),
                new IntroMessage("\"Bring out the Gimp\""),
                new IntroMessage("[MESSAGE_GOES_HERE]"),
                new IntroMessage(@"4@$#F\#9%5Zx~!@$55^9HX"),
                new IntroMessage("What! Who allowed that?"),
                new IntroMessage("Your password is incorrect"),
                new IntroMessage("Don't call me, I'll call you"),
                new IntroMessage("Break the rules!"),
                new IntroMessage("Think outside the box"),
                new IntroMessage("Please allow me to introduce myself ..."),
                new IntroMessage("Are you sure you are not missing any semicolons?"),
                new IntroMessage("Ah, the great outdoors ..."),
                new IntroMessage("Burek is with meat!"),
                new IntroMessage("\"Fear is the mind-killer\""),
                new IntroMessage("Are you SOLID?"),
                new IntroMessage("Please enter your 2FA code: * * * *"),
                new IntroMessage("(1923589348923) rows affected"),
                new IntroMessage("\"Opinions are like assholes — Everyone’s got one\""),
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
                new IntroMessage("It's a perfect day"),
                new IntroMessage("Update me regularly!"),
                new IntroMessage("if (true) { Console.Write(\"I LOVE YOU!\"); }"),
                new IntroMessage("Let's have a meeting"),
                new IntroMessage("Pushing to the limits"),
                new IntroMessage("Push me, pull me"),
                new IntroMessage("Consolidate my nuggets"),
                new IntroMessage("It's overkill"),
                new IntroMessage("Regularly exercise"),
                new IntroMessage("OK, OK, hold your horses, I'll start in a second"),
                new IntroMessage("You want me to generate THAT?"),
                new IntroMessage("I can see sharp"),
                new IntroMessage("01101000 01100101 01101100 01101100 01101111!"),
                new IntroMessage("#NOPRESSURE"),
                new IntroMessage("You are my favorite developer ;)"),
                new IntroMessage("Running infinite loops ..."),
                new IntroMessage("Booooring"),
                new IntroMessage("Are you ready to rumble?"),
                new IntroMessage("Generating code since 6. April 2021."),
                new IntroMessage("\"Winamp, it really whips the llama's ass!\""),
                new IntroMessage("Log your hours every day!"),
                new IntroMessage("YOLO"),
                new IntroMessage("I like you very much"),
                new IntroMessage("The clock is ticking ..."),
                new IntroMessage("Totally awesome"),
                new IntroMessage("This will take a while"),
                new IntroMessage("You made a mistake!"),
                new IntroMessage("Press play"),
                new IntroMessage("The cake is a lie."),
                new IntroMessage("Abort the mission!"),
                new IntroMessage("I am GOAT"),
                new IntroMessage("I am GOAT"),
                new IntroMessage("Go fish!")
                    .WithForegroundColor(ConsoleColor.Blue),
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
                new IntroMessage("Every Friday is Good Firday")
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
                new IntroMessage("Go vegan")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithForegroundColor(ConsoleColor.Green)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Don't get fat!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Woo-hoo, lunch time!!!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Me so hungry")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Feasting")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsLunchTime()),
                new IntroMessage("Stay out of the second floors!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("Run in zig-zag!")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("Lock your doors")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("Bring your shootgun")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                new IntroMessage("Be quiet")
                    .WithType(IntroMessageType.SpecialCase)
                    .WithCondition(() => DateTime.Now.IsFriday13th())
                    .WithForegroundColor(ConsoleColor.Red),
                // Important dates:
                new IntroMessage("Happy New Year's Eve!!!")
                    .WithType(IntroMessageType.Important)
                    .WithForegroundColor(ConsoleColor.Red)
                    .WithCondition(() => DateTime.Now.IsNewYearsEve()),
                new IntroMessage("It is the first day of the year. Make a new year's resolution ;)")
                    .WithType(IntroMessageType.Important)
                    .WithForegroundColor(ConsoleColor.Yellow)
                    .WithCondition(() => DateTime.Now.IsFirstDayOfYear()),
                new IntroMessage("Happy birthday Alan Turing!")
                    .WithType(IntroMessageType.Important)
                    .WithCondition(() => new DateTime(1912, 6, 23).IsBirthDay()),
                new IntroMessage("Happy birthday Miljan & Vladan :D")
                    .WithType(IntroMessageType.Important)
                    .WithCondition(() => new DateTime(1985, 9, 12).IsBirthDay()),
            };
    }
}
