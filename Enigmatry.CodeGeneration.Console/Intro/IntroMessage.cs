using System;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal class IntroMessage
    {
        private const string TrimSuffix = "...";
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.Gray;
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public string Text { get; private set; } = String.Empty;
        public IntroMessageType Type { get; private set; } = IntroMessageType.Regular;
        public Func<bool> Condition { get; private set; } = null;
        public ConsoleColor ForegroundColor { get; private set; } = ConsoleColor.Magenta;
        public ConsoleColor BackgroundColor { get; private set; } = DefaultBackgroundColor;

        public IntroMessage(string text)
        {
            Text = $" {text}";
        }

        public bool SatisfiesCondition => Condition == null || Condition.Invoke();

        public void Print(int maxLength)
        {
            System.Console.ForegroundColor = ForegroundColor;
            System.Console.BackgroundColor = BackgroundColor;

            if (Text.Length > maxLength)
            {
                System.Console.WriteLine($"{Text.Substring(0, maxLength - TrimSuffix.Length)}{TrimSuffix}");
            }
            else if (Text.Length < maxLength)
            {
                System.Console.BackgroundColor = DefaultBackgroundColor;
                System.Console.Write(new string(' ', maxLength - Text.Length));
                System.Console.BackgroundColor = BackgroundColor;
                System.Console.WriteLine(Text);
            }
            else
            {
                System.Console.WriteLine(Text);
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
    }
}
