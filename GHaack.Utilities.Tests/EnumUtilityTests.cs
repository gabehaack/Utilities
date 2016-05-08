using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace GHaack.Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    public class EnumUtilityTests
    {
        [Fact]
        public void ParseString()
        {
            var testEnum = EnumUtility.Parse<TestEnum>("Value1");
            Assert.Equal(TestEnum.Value1, testEnum);
        }

        [Fact]
        public void ParseBadString()
        {
            var testEnum = EnumUtility.Parse<TestEnum>("BadValue");
            Assert.Equal(0, (int)testEnum);
        }

        [Fact]
        public void ParseInt()
        {
            var testEnum = EnumUtility.Parse<TestEnum>(1);
            Assert.Equal(TestEnum.Value1, testEnum);
        }

        [Fact]
        public void ParseBadInt()
        {
            var testEnum = EnumUtility.Parse<TestEnum>(1000);
            Assert.Equal(0, (int)testEnum);
        }

        [Fact]
        public void GetRandomEnum()
        {
            var testEnum = EnumUtility.GetRandomEnumValue<TestEnum>();
            var enumValues = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToList();
            Assert.Contains(testEnum, enumValues);
        }

        [Fact]
        public void GetRandomEnum_BlackList()
        {
            var testEnum = EnumUtility.GetRandomEnumValue(new List<TestEnum> { TestEnum.Value1 });
            Assert.Equal(TestEnum.Value2, testEnum);
        }

        [Fact]
        public void GetRandomEnum_FullBlackList()
        {
            var exception =
                Assert.Throws<Exception>(
                    () => EnumUtility.GetRandomEnumValue(new List<TestEnum> { TestEnum.Value1, TestEnum.Value2 }));
            Assert.Equal("No possible return values.", exception.Message);
        }

        private enum TestEnum
        {
            Value1 = 1,
            Value2 = 2,
        }
    }
}
