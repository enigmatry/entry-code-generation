namespace Enigmatry.CodeGeneration.Console.Intro
{
    /// <summary>
    /// Used to determine priority of message printing, in case when multiple messages have printing condition fulfilled.
    /// </summary>
    internal enum IntroMessageType
    {
        /// <summary>
        /// Lowest printing priority. Gets overriden by all other messages.
        /// Single random Regular message is printed if multiple fullfill printing condition.
        /// </summary>
        Regular = 0,
        /// <summary>
        /// Medium printing priority. Overrides Regular messages, but gets overriden by VeryImportant messages.
        /// Single random Important message is printed if multiple fullfill printing condition.
        /// </summary>
        Important = 1,
        /// <summary>
        /// Highest printing priority. Overrides all other messages.
        /// All VeryImportant messages are printed if multipley fullfill printing condition.
        /// </summary>
        VeryImportant = 2
    }
}
