using System;

namespace Enigmatry.Entry.CodeGeneration.Tools.Intro
{
    internal class IntroLogoLine : IntroContent
    {
        private const int MaxContentLength = 120;

        public bool EndWithNewLine { get; private set; } = true;

        public IntroLogoLine(string content)
        {
            Content = content.Length > MaxContentLength ? content[..MaxContentLength] : content;
        }

        public IntroLogoLine(string text, ConsoleColor color, bool endWithNewLine = true) : this(text)
        {
            ForegroundColor = color;
            EndWithNewLine = endWithNewLine;
        }

        public void Print()
        {
            System.Console.ForegroundColor = ForegroundColor;
            System.Console.BackgroundColor = BackgroundColor;

            if (EndWithNewLine)
            {
                System.Console.WriteLine(Content);
            }
            else
            {
                System.Console.Write(Content);
            }

            System.Console.ForegroundColor = DefaultForegroundColor;
            System.Console.BackgroundColor = DefaultBackgroundColor;
        }

        public IntroLogoLine WithEndWithNewLine(bool endWithNewLine)
        {
            EndWithNewLine = endWithNewLine;
            return this;
        }

        public IntroLogoLine WithForegroundColor(ConsoleColor color)
        {
            ForegroundColor = color;
            return this;
        }

        public IntroLogoLine WithBackgroundColor(ConsoleColor color)
        {
            BackgroundColor = color;
            return this;
        }

        public IntroLogoLine WithColors(ConsoleColor foregroundColor) =>
            WithForegroundColor(foregroundColor);
    }
}
