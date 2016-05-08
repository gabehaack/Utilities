using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace GHaack.Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    public class ObjectClonerTests
    {
        [Fact]
        public void Clone()
        {
            var generator = new DataGenerator();
            var testData = new TestData
            {
                Enum = EnumUtility.GetRandomEnumValue<TestEnum>(),
                Int = generator.Int(),
                String = generator.String(10, CharTypes.All),
                Version = new Version(generator.PositiveInt(), generator.Int())
            };

            var clone = testData.CloneObject();
            Equal(testData, clone);
        }

        private static void Equal(TestData expected, TestData actual)
        {
            Assert.Equal(expected.Enum, actual.Enum);
            Assert.Equal(expected.Int, actual.Int);
            Assert.Equal(expected.String, actual.String);
            Assert.Equal(expected.Version, actual.Version);
        }

        private class TestData
        {
            public TestEnum Enum { get; set; }
            public int Int { get; set; }
            public string String { get; set; }
            public Version Version { get; set; }
        }

        private enum TestEnum
        {
            Value1 = 1,
            Value2 = 2,
        }
    }
}
