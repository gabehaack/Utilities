using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace GHaack.Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    public class DataGeneratorTests
    {
        #region String Methods

        private const int MaxStringLength = 256;

        [Fact]
        public void StringLength()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next(2, MaxStringLength);
            int min = random.Next(max);

            string result = dataGenerator.String(min, max);
            Assert.InRange(result.Length, min, max);
        }

        [Fact]
        public void StringLength2()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next(2, MaxStringLength);

            string result = dataGenerator.String(max, CharTypes.All);
            Assert.InRange(result.Length, 1, max);
        }

        [Fact]
        public void StringLength3()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int length = random.Next(2, MaxStringLength);

            string result = dataGenerator.String(length, DataGenerator.AllChars);
            Assert.Equal(length, result.Length);
        }

        [Fact]
        public void StringLengthNegative()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next(2, MaxStringLength);
            int min = -random.Next(max);

            string result = dataGenerator.String(min, max);
            Assert.InRange(result.Length, 1, max);
        }

        [Fact]
        public void StringTooShort()
        {
            var dataGenerator = new DataGenerator();
            var allChars = CharTypes.All;
            int tooFew = 3;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => dataGenerator.String(tooFew, tooFew, allChars, true));
            Assert.Equal(String.Format("Minimum length must be at least 4 for each of the required character types to be included.\r\nParameter name: minLength\r\nActual value was {0}.", tooFew), exception.Message);
        }

        [Fact]
        public void StringContent()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next(MaxStringLength);

            string result = dataGenerator.String(max, CharTypes.All, true);
            bool upper = false;
            bool lower = false;
            bool numeric = false;
            bool special = false;
            foreach (var c in result)
            {
                if (upper && lower && numeric && special)
                    break;
                string ch = c.ToString(CultureInfo.InvariantCulture);
                if (!upper)
                    upper = DataGenerator.UpperAlphabeticChars.Contains(ch);
                if (!lower)
                    lower = DataGenerator.LowerAlphabeticChars.Contains(ch);
                if (!numeric)
                    numeric = DataGenerator.NumericChars.Contains(ch);
                if (!special)
                    special = DataGenerator.SpecialChars.Contains(ch);
            }

            Assert.True(upper);
            Assert.True(lower);
            Assert.True(numeric);
            Assert.True(special);
        }

        [Fact]
        public void SqlSafeString()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int length = random.Next(2, MaxStringLength);

            string result = dataGenerator.SqlSafeString(length);
            foreach (var c in DataGenerator.SqlUnsafeChars)
            {
                string ch = c.ToString(CultureInfo.InvariantCulture);
                Assert.DoesNotContain(result, ch);
            }
        }

        [Fact]
        public void HtmlSafeString()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int length = random.Next(2, MaxStringLength);

            string result = dataGenerator.HtmlSafeString(length);
            foreach (var c in DataGenerator.HtmlUnsafeChars)
            {
                string ch = c.ToString(CultureInfo.InvariantCulture);
                Assert.DoesNotContain(result, ch);
            }
        }

        [Fact]
        public void SeleniumSafeString()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int length = random.Next(2, MaxStringLength);

            string result = dataGenerator.SeleniumSafeString(length);
            foreach (var c in DataGenerator.SeleniumUnsafeChars)
            {
                string ch = c.ToString(CultureInfo.InvariantCulture);
                Assert.DoesNotContain(result, ch);
            }
        }

        [Fact]
        public void SafeString()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int length = random.Next(2, MaxStringLength);

            string result = dataGenerator.SafeString(length);
            foreach (var c in DataGenerator.UnsafeChars)
            {
                string ch = c.ToString(CultureInfo.InvariantCulture);
                Assert.DoesNotContain(result, ch);
            }
        }

        #endregion

        #region Number Methods

        [Fact]
        public void DoubleRange()
        {
            var dataGenerator = new DataGenerator();

            double result = dataGenerator.Double();
            Assert.InRange(result, 0, 1);
        }

        [Fact]
        public void DoubleRangeMax()
        {
            var dataGenerator = new DataGenerator();
            int max = Int32.MaxValue;

            double result = dataGenerator.Double(max);
            Assert.InRange(result, 0, max);
        }

        [Fact]
        public void IntMinMaxRange()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next();
            int min = random.Next(max);

            int result = dataGenerator.Int(min, max);
            Assert.InRange(result, min, max);
        }

        [Fact]
        public void IntMaxRange()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int max = random.Next();

            int result = dataGenerator.Int(max);
            Assert.InRange(result, 0, max);
        }

        [Fact]
        public void IntRangeMax()
        {
            var dataGenerator = new DataGenerator();
            int max = Int32.MaxValue;
            int min = 0;

            int result = dataGenerator.Int();
            Assert.InRange(result, min, max);
        }

        [Fact]
        public void IndexRange()
        {
            var dataGenerator = new DataGenerator();
            var random = new Random();
            int count = random.Next();

            int result = dataGenerator.Index(count);
            Assert.InRange(result, 0, count);
        }

        [Fact]
        public void PositiveInt()
        {
            var dataGenerator = new DataGenerator();

            int positive = dataGenerator.PositiveInt();
            Assert.NotEqual(0, positive);
            Assert.True(positive > 0);
        }

        [Fact]
        public void PositiveIntOne()
        {
            var dataGenerator = new DataGenerator();

            int positive = dataGenerator.PositiveInt(1);
            Assert.Equal(1, positive);
        }

        [Fact]
        public void PositiveIntTwo()
        {
            var dataGenerator = new DataGenerator();

            int positive = dataGenerator.PositiveInt(2);
            Assert.NotEqual(0, positive);
            Assert.True(positive > 0);
        }

        [Fact]
        public void PositiveIntZeroInput()
        {
            var dataGenerator = new DataGenerator();

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => dataGenerator.PositiveInt(0));
            Assert.Equal("Maximum value must be greater than 0 to produce a positive integer.\r\nParameter name: maxValue\r\nActual value was 0.", exception.Message);
        }

        #endregion
    }
}
