using System;
using System.Collections.Generic;
using Xunit;

namespace Kraphity.Guard.Tests
{
    public partial class CheckTests
    {
        [Fact]
        public void NotEmpty_Array()
        {
            var list = new[] { 1, 2, 3 };
            Check.NotEmpty(list, nameof(list));
        }

        [Fact]
        public void NotEmpty_EmptyArray_Throws()
        {
            var list = new int[0];
            Assert.Throws<ArgumentException>(nameof(list), () => Check.NotEmpty(list, nameof(list)));
        }

        [Fact]
        public void NotEmpty_EmptyArrayParamExpression_Throws()
        {
            var list = new int[0];
            Assert.Throws<ArgumentException>(nameof(list), () => Check.NotEmpty(list, () => list));
        }

        [Fact]
        public void NotEmpty_List()
        {
            var list = new List<int> { 1, 2, 3 };
            Check.NotEmpty(list, () => list);
        }

        [Fact]
        public void NotEmpty_EmptyList_Throws()
        {
            var list = new List<int>();
            Assert.Throws<ArgumentException>(nameof(list), () => Check.NotEmpty(list, nameof(list)));
        }

        [Fact]
        public void NotEmpty_EmptyListParamExpression_Throws()
        {
            var list = new List<int>();
            Assert.Throws<ArgumentException>(nameof(list), () => Check.NotEmpty(list, () => list));
        }
    }
}
