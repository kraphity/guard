using System;
using Xunit;

namespace Kraphity.Guard.Tests
{
    public partial class CheckTests
    {
        [Fact]
        public void NotNull()
        {
            string arg = "test";
            Check.NotNull(arg, () => arg);
        }

        [Fact]
        public void NotNull_NullValue_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(nameof(arg), () => Check.NotNull(arg, () => arg));
        }
        
        [Fact]
        public void If()
        {
            Check.If(true, () => new ArgumentException());
        }
        
        [Fact]
        public void If_InvalidCondition_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Check.If(false, () => new InvalidOperationException()));
        }
    }
}
