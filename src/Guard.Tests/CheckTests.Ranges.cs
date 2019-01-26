using System;
using Xunit;

namespace Kraphity.Guard.Tests
{
    public partial class CheckTests
    {
        [Fact]
        public void InRange()
        {
            int i = 5;
            Check.InRange(i >= 5, nameof(i), i);
        }

        [Fact]
        public void InRange_InvalidValue_Throws()
        {
            int i = 5;
            Assert.Throws<ArgumentOutOfRangeException>(nameof(i), () => Check.InRange(i > 5, nameof(i), i));
        }

        [Fact]
        public void InRange_InvalidValueParamExpression_Throws()
        {
            int i = 5;
            Assert.Throws<ArgumentOutOfRangeException>(nameof(i), () => Check.InRange(i > 5, () => i, i));
        }

        [Fact]
        public void InRange_InvalidValidCustomMsg_Throws()
        {
            var msg = "test";
            int i = 5;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(nameof(i), () => Check.InRange(i > 5, nameof(i), i, msg));
            Assert.Contains(msg, ex.Message);
        }
    }
}
