using System;
using System.Reflection.Metadata;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal class IntroMessage : IntroContent
    {
        private const string ContentPrefix = " ";
        private const string ContnetSufix = "...";

        public IntroMessageType Type { get; private set; } = IntroMessageType.Regular;
        public Func<bool> Condition { get; private set; } = null;
        public bool SatisfiesCondition => Condition == null || Condition.Invoke();

        public IntroMessage(string content)
        {
            Content = content;
            ForegroundColor = ConsoleColor.Magenta;
        }

        public void Print(int maxLineLength)
        {
            System.Console.ForegroundColor = ForegroundColor;
            System.Console.BackgroundColor = BackgroundColor;

            var totalContentLength = Content.Length + ContentPrefix.Length;
            if (totalContentLength > maxLineLength)
            {
                var trimmedContnetLength = maxLineLength - ContnetSufix.Length - ContentPrefix.Length;
                System.Console.BackgroundColor = DefaultBackgroundColor;
                System.Console.Write(ContentPrefix);
                System.Console.BackgroundColor = BackgroundColor;
                System.Console.WriteLine($"{Content[..(trimmedContnetLength)]}{ContnetSufix}");
            }
            else if (totalContentLength < maxLineLength)
            {
                System.Console.BackgroundColor = DefaultBackgroundColor;
                System.Console.Write(new string(' ', maxLineLength - Content.Length));
                System.Console.BackgroundColor = BackgroundColor;
                System.Console.WriteLine(Content);
            }
            else
            {
                System.Console.BackgroundColor = DefaultBackgroundColor;
                System.Console.Write(ContentPrefix);
                System.Console.BackgroundColor = BackgroundColor;
                System.Console.WriteLine(Content);
            }

            System.Console.ForegroundColor = DefaultForegroundColor;
            System.Console.BackgroundColor = DefaultBackgroundColor;
        }

        public IntroMessage WithType(IntroMessageType type)
        {
            Type = type;
            return this;
        }

        public IntroMessage WithCondition(Func<bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IntroMessage WithForegroundColor(ConsoleColor color)
        {
            ForegroundColor = color;
            return this;
        }

        public IntroMessage WithBackgroundColor(ConsoleColor color)
        {
            BackgroundColor = color;
            return this;
        }

        public IntroMessage WithColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black) =>
            WithForegroundColor(foregroundColor).WithBackgroundColor(backgroundColor);
    }
}
