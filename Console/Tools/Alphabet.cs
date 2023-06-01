namespace ConsoleProject.Tools
{
    /// <summary>
    /// Represents the alphabet used for grid coordinates.
    /// </summary>
    internal abstract class Alphabet
    {
        private static readonly char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// Returns the index of the specified letter in the alphabet.
        /// </summary>
        /// <param name="letter">The letter to find the index of.</param>
        /// <returns>The index of the letter in the alphabet.</returns>
        /// <exception cref="Exception">Thrown when the letter is not found in the alphabet.</exception>
        public static int IndexOf(char letter)
        {
            int index = Array.IndexOf(alphabet, char.ToUpper(letter));

            if (index == -1)
            {
                throw new Exception("Letter is not in alphabet");
            }

            return index;
        }

        /// <summary>
        /// Returns an array of characters from the alphabet up to the specified count.
        /// </summary>
        /// <param name="count">The number of characters to take from the alphabet.</param>
        /// <returns>An array of characters from the alphabet.</returns>
        /// <exception cref="Exception">Thrown when the count is out of range.</exception>
        public static char[] Take(int count)
        {
            if (count < 0 || count > alphabet.Length)
            {
                throw new Exception("Count is out of alphabet");
            }

            return alphabet.Take(count).ToArray();
        }
    }
}
