using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Tools.Intro
{
    internal class IntroGenerator
    {
        private readonly Random _random = new();

        public void Print()
        {
            System.Console.WriteLine();
            var logoLines = IntroLogoLines.Get;
            logoLines.ForEach(logoLine => logoLine.Print());
            System.Console.WriteLine();
            var lineLength = logoLines.Max(logoLine => logoLine.Content.Length);
            GetMessages().ToList().ForEach(message => message.Print(lineLength));
            System.Console.WriteLine();
        }

        private IEnumerable<IntroMessage> GetMessages()
        {
            var veryImportantMessages = IntroMessages.GetVeryImportant();
            if (veryImportantMessages.Any())
            {
                return veryImportantMessages;
            }

            var importantMessages = IntroMessages.GetImportant();
            return importantMessages.Any()
                ? new[] { PickRandomMessage(importantMessages) }
                : new[] { PickRandomMessage(IntroMessages.GetRegular()) };
        }

        private IntroMessage PickRandomMessage(List<IntroMessage> messages) => messages[_random.Next(0, messages.Count())];
    }
}
