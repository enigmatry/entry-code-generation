using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal class IntroGenerator
    {
        private readonly Random _random = new();
        private readonly List<IntroLogoLine> _logoLines = IntroLogoLines.Get;
        private readonly List<IntroMessage> _messages = IntroMessages.Get;

        public void Print()
        {
            System.Console.WriteLine();
            _logoLines.ForEach(x => x.Print());
            System.Console.WriteLine();
            GetMessages().ToList().ForEach(x => x.Print(_logoLines.Select(x => x.Text.Length).Max()));
            System.Console.WriteLine();
        }

        private IEnumerable<IntroMessage> GetMessages()
        {
            var messages = _messages.Where(x => x.SatisfiesCondition).ToList();

            if (!messages.Any())
            {
                return messages;
            }

            return messages.Any(x => x.Type == IntroMessageType.Important)
                ? messages.Where(x => x.Type == IntroMessageType.Important)
                : messages.Any(x => x.Type == IntroMessageType.SpecialCase)
                    ? new[] { PickRandomMessage(messages.Where(x => x.Type == IntroMessageType.SpecialCase)) }
                    : new[] { PickRandomMessage(messages.Where(x => x.Type == IntroMessageType.Regular)) };
        }

        private IntroMessage PickRandomMessage(IEnumerable<IntroMessage> messages) =>
            messages.ToList()[_random.Next(0, messages.Count())];
    }
}
