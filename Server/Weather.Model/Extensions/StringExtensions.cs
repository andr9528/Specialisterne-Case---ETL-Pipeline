namespace Weather.Model.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var builder = new System.Text.StringBuilder(input.Length + 5);

            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];

                if (char.IsUpper(current))
                {
                    if (ShouldInsertUnderscore(input, i))
                    {
                        builder.Append('_');
                    }

                    builder.Append(char.ToLowerInvariant(current));
                }
                else
                {
                    builder.Append(current);
                }
            }

            return builder.ToString();
        }

        private static bool ShouldInsertUnderscore(string input, int index)
        {
            if (!HasPreviousCharacter(index))
                return false;

            var previous = input[index - 1];
            var current = input[index];
            var next = HasNextCharacter(input, index) ? input[index + 1] : '\0';

            return IsStartOfNewWord(previous) || IsEndOfUppercaseSequence(previous, current, next, index, input.Length);
        }

        private static bool HasPreviousCharacter(int index) => index > 0;

        private static bool HasNextCharacter(string input, int index) => index < input.Length - 1;

        private static bool IsStartOfNewWord(char previous)
        {
            return char.IsLower(previous) || char.IsDigit(previous);
        }

        private static bool IsEndOfUppercaseSequence(char previous, char current, char next, int index, int length)
        {
            return char.IsUpper(previous) && index < length - 1 && char.IsLower(next);
        }
    }
}