namespace ConsoleProject.Tools
{
    internal abstract class Alphabet
    {
        private static readonly char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static int IndexOf(char letter)
        {
            int index = Array.IndexOf(alphabet, char.ToUpper(letter));

            if (index == -1)
            {
                throw new Exception("Letter is not in alphabet");
            }

            return index;
        }

        public static char LetterOf(int index)
        {
            if (index < 0 || index > alphabet.Length - 1)
            {
                throw new Exception("Index is out of alphabet");
            }

            return alphabet[index];
        }

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
