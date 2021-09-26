using System;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal class IntroLogoLine
    {
        public const int LineMaxLength = 120;

        public string Text { get; private set; } = String.Empty;
        public ConsoleColor Color { get; private set; }
        public bool EndWithNewLine { get; private set; } = true;

        public IntroLogoLine(string text, ConsoleColor color, bool endWithNewLine = true)
        {
            Text = text.Length > LineMaxLength
                ? text.Substring(0, LineMaxLength)
                : text;
            Color = color;
            EndWithNewLine = endWithNewLine;
        }

        public void Print()
        {
            System.Console.ForegroundColor = Color;

            if (EndWithNewLine)
            {
                System.Console.WriteLine(Text);
            }
            else
            {
                System.Console.Write(Text);
            }

            System.Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
