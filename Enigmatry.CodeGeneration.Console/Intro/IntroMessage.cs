using System;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal class IntroMessage
    {
        private const string TrimSuffix = "...";

        public string Text { get; private set; } = String.Empty;
        public IntroMessageType Type { get; private set; } = IntroMessageType.Regular;
        public Func<bool> Condition { get; private set; } = null;
        public ConsoleColor ForegroundColor { get; private set; } = ConsoleColor.Magenta;

        public IntroMessage(string text)
        {
            Text = $" {text}";
        }

        public bool SatisfiesCondition => Condition == null || Condition.Invoke();

        public void Print(int maxLength)
        {
            System.Console.ForegroundColor = ForegroundColor;

            if (Text.Length > maxLength)
            {
                System.Console.WriteLine($"{Text.Substring(0, maxLength - TrimSuffix.Length)}{TrimSuffix}");
            }
            else if (Text.Length < maxLength)
            {
                System.Console.WriteLine(String.Format($"{{0, {maxLength}}}", Text));
            }
            else
            {
                System.Console.WriteLine(Text);
            }

            System.Console.ForegroundColor = ConsoleColor.Gray;
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
    }
}
