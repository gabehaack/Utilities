using System;
using System.Linq;
using System.Text;

namespace GHaack.Utilities
{
    public class DataGenerator
    {
        #region Fields/Constants

        public const string UpperAlphabeticChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string LowerAlphabeticChars = "abcdefghijklmnopqrstuvwxyz";
        public const string NumericChars = "0123456789";
        public const string SpecialChars = "!@#$%^&*()_+={}[];:'/\\,\".<>~`?";
        public const string AllChars =
            UpperAlphabeticChars + LowerAlphabeticChars + NumericChars + SpecialChars;
        public const string SqlUnsafeChars = "[";
        public const string HtmlUnsafeChars = "<";
        public const string SeleniumUnsafeChars = HtmlUnsafeChars + "!(&";
        public static readonly string UnsafeChars =
            SqlUnsafeChars + HtmlUnsafeChars + SeleniumUnsafeChars;

        private readonly Random _random;

        public DataGenerator()
        {
            _random = new Random();
        }

        public DataGenerator(Random random)
        {
            _random = random ?? new Random();
        }

        #endregion

        #region Numbers

        /// <summary>
        /// Returns a random double no less than 0 and less than 1.
        /// </summary>
        /// <returns>A random double no less than 0 and less than 1.</returns>
        public double Double()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Returns a random double no less than 0 and less than the specified maximum value.
        /// </summary>
        /// <param name="maxValue">The maximum returnable value.</param>
        /// <returns>A random double no less than 0 and less than the specified maximum value.</returns>
        public double Double(int maxValue)
        {
            return _random.NextDouble() * maxValue;
        }

        /// <summary>
        /// Returns a random integer within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public int Int(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        /// <returns>A nonnegative 32-bit signed integer less than maxValue.</returns>
        public int Int(int maxValue)
        {
            return _random.Next(maxValue);
        }

        /// <summary>
        /// Returns an index integer for the specified array length.
        /// </summary>
        /// <param name="count">The number of values in the array.</param>
        /// <returns>A nonnegative 32-bit signed integer less than count; that is, the range of return values does not include count.</returns>
        public int Index(int count)
        {
            return _random.Next(count);
        }

        /// <summary>
        /// Returns a nonnegative random integer.
        /// </summary>
        /// <returns>A nonnegative 32-bit signed integer.</returns>
        public int Int()
        {
            return _random.Next();
        }

        /// <summary>
        /// Returns a positive ( > 0) random integer.
        /// </summary>
        /// <returns>A positive ( > 0) 32-bit signed integer.</returns>
        public int PositiveInt()
        {
            return _random.Next(1, Int32.MaxValue);
        }

        /// <summary>
        /// Returns a random positive integer.
        /// </summary>
        /// <param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal to 1.</param>
        /// <returns>A positive 32-bit signed integer.</returns>
        public int PositiveInt(int maxValue)
        {
            if (maxValue < 1) throw new ArgumentOutOfRangeException("maxValue", maxValue, "Maximum value must be greater than 0 to produce a positive integer.");
            return _random.Next(maxValue - 1) + 1;
        }

        /// <summary>
        /// Returns a random length integer within a specified range. Minimum value returned is 1 regardless of range inputs.
        /// </summary>
        /// <param name="minLength">The inclusive lower bound of the random number returned. Must be positive.</param>
        /// <param name="maxLength">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A random length integer within a specified range. Minimum value returned is 1 regardless of range inputs.</returns>
        public int Length(int minLength, int maxLength)
        {
            if (minLength < 1) { minLength = 1; }
            return _random.Next(minLength, maxLength);
        }

        #endregion

        #region Strings

        /// <summary>
        /// Returns a random string within the specified length range from the specified characters.
        /// </summary>
        /// <param name="minLength">Minimum length of the string returned.</param>
        /// <param name="maxLength">Maximum length of the string returned.</param>
        /// <param name="charTypes">Character types to include in the string returned.</param>
        /// <param name="strict">If true, at least one character of each type specified in charTypes must be included in the string returned.</param>
        /// <returns>A random string bound by the specified parameters.</returns>
        public string String(int minLength, int maxLength, CharTypes charTypes = CharTypes.Alphabetic, bool strict = false)
        {
            // Determine length
            int length = Length(minLength, maxLength);

            // Determine which characters to use
            int minChars = 0;
            var charsBuilder = new StringBuilder();
            if (charTypes.HasFlag(CharTypes.UpperAlphabetic))
            {
                charsBuilder.Append(UpperAlphabeticChars);
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.LowerAlphabetic))
            {
                charsBuilder.Append(LowerAlphabeticChars);
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.Numeric))
            {
                charsBuilder.Append(NumericChars);
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.Special))
            {
                charsBuilder.Append(SpecialChars);
                minChars++;
            }

            // If too many requirements and not enough string...
            if (strict && length < minChars)
            {
                throw new ArgumentOutOfRangeException("minLength", minLength, System.String.Format("Minimum length must be at least {0} for each of the required character types to be included.", minChars));
            }
            string charsToInclude = charsBuilder.ToString();

            if (!strict) return String(length, charsToInclude);

            // If strict, need to make sure one of each character type is included
            var stringBuilder = new StringBuilder();
            if (charTypes.HasFlag(CharTypes.UpperAlphabetic))
            {
                stringBuilder.Append(String(1, UpperAlphabeticChars));
            }
            if (charTypes.HasFlag(CharTypes.LowerAlphabetic))
            {
                stringBuilder.Append(String(1, LowerAlphabeticChars));
            }
            if (charTypes.HasFlag(CharTypes.Numeric))
            {
                stringBuilder.Append(String(1, NumericChars));
            }
            if (charTypes.HasFlag(CharTypes.Special))
            {
                stringBuilder.Append(String(1, SpecialChars));
            }

            // Then scramble the string so it's truly random
            stringBuilder.Append(String(length - minChars, charsToInclude));
            var scrambledChars = stringBuilder.ToString().ToCharArray().OrderBy(c => _random.NextDouble()).ToArray();
            return new string(scrambledChars);
        }

        /// <summary>
        /// Returns a random string shorter than the specified maximum length from the specified characters.
        /// </summary>
        /// <param name="maxLength">Maximum length of the string returned.</param>
        /// <param name="charTypes">Character types to include in the string returned.</param>
        /// <param name="strict">If true, at least one character of each type specified in charTypes must be included in the string returned.</param>
        /// <returns>A random string bound by the specified parameters.</returns>
        public string String(int maxLength, CharTypes charTypes, bool strict = false)
        {
            int minChars = 0;
            if (charTypes.HasFlag(CharTypes.UpperAlphabetic))
            {
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.LowerAlphabetic))
            {
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.Numeric))
            {
                minChars++;
            }
            if (charTypes.HasFlag(CharTypes.Special))
            {
                minChars++;
            }
            int min = minChars == 0 ? 1 : minChars;
            int max = maxLength <= min ? min : maxLength;
            return String(min, max, charTypes, strict);
        }

        /// <summary>
        /// Returns a random string of the specified length from the specified characters.
        /// </summary>
        /// <param name="length">Length of the string returned.</param>
        /// <param name="chars">Character values that may be included in the string returned.</param>
        /// <returns>A random string of the specified length from the specified characters.</returns>
        public string String(int length, string chars)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(
                    chars[_random.Next(chars.Length)]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns a random string of the specified length from all SQL-safe characters.
        /// </summary>
        /// <param name="length">Length of the string returned.</param>
        /// <returns>A random string of the specified length from all SQL-safe characters.</returns>
        public string SqlSafeString(int length)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in AllChars.Where(c => !SqlUnsafeChars.Contains(c)))
            {
                stringBuilder.Append(c);
            }
            return String(length, stringBuilder.ToString());
        }

        /// <summary>
        /// Returns a random string of the specified length from all HTML-safe characters.
        /// </summary>
        /// <param name="length">Length of the string returned.</param>
        /// <returns>A random string of the specified length from all HTML-safe characters.</returns>
        public string HtmlSafeString(int length)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in AllChars.Where(c => !HtmlUnsafeChars.Contains(c)))
            {
                stringBuilder.Append(c);
            }
            return String(length, stringBuilder.ToString());
        }

        /// <summary>
        /// Returns a random string of the specified length from all Selenium-safe characters.
        /// </summary>
        /// <param name="length">Length of the string returned.</param>
        /// <returns>A random string of the specified length from all Selenium-safe characters.</returns>
        public string SeleniumSafeString(int length)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in AllChars.Where(c => !SeleniumUnsafeChars.Contains(c)))
            {
                stringBuilder.Append(c);
            }
            return String(length, stringBuilder.ToString());
        }

        /// <summary>
        /// Returns a random string of the specified length from all safe characters.
        /// </summary>
        /// <param name="length">Length of the string returned.</param>
        /// <returns>A random string of the specified length from all safe characters.</returns>
        public string SafeString(int length)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in AllChars.Where(c => !UnsafeChars.Contains(c)))
            {
                stringBuilder.Append(c);
            }
            return String(length, stringBuilder.ToString());
        }

        #endregion

        #region Other

        /// <summary>
        /// Returns a random bool with 50/50 probability.
        /// </summary>
        /// <returns>A random bool with 50/50 probability.</returns>
        public bool Bool()
        {
            return _random.NextDouble() >= 0.5;
        }

        #endregion
    }

    /// <summary>
    /// Character types to include in a string
    /// </summary>
    [Flags]
    public enum CharTypes
    {
        // Base character types
        UpperAlphabetic = 1,
        LowerAlphabetic = 2,
        Numeric = 4,
        Special = 8,

        // Combination character types
        Alphabetic = UpperAlphabetic | LowerAlphabetic,
        Alphanumeric = Alphabetic | Numeric,
        All = Alphanumeric | Special,
    }
}
