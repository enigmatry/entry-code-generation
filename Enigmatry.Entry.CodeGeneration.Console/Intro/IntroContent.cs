using System;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal abstract class IntroContent
    {
        protected const ConsoleColor DefaultForegroundColor = ConsoleColor.Gray;
        protected const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public string Content { get; protected set; } = String.Empty;
        public ConsoleColor ForegroundColor { get; protected set; } = DefaultForegroundColor;
        public ConsoleColor BackgroundColor { get; protected set; } = DefaultBackgroundColor;
    }
}
