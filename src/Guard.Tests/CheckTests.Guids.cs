using System;
using Xunit;

namespace Kraphity.Guard.Tests
{
    public partial class CheckTests
    {
        [Fact]
        public void NotEmpty_Guid()
        {
            var guid = Guid.NewGuid();

            Check.NotEmpty(guid, nameof(guid));
        }

        [Fact]
        public void NotEmpty_EmptyGuid_Throws()
        {
            var guid = Guid.Empty;
            Assert.Throws<ArgumentException>(() => Check.NotEmpty(guid, nameof(guid)));
        }

        [Fact]
        public void NotEmpty_EmptyGuidParamExpression_Throws()
        {
            var guid = Guid.Empty;
            Assert.Throws<ArgumentException>(() => Check.NotEmpty(guid, () => guid));
        }
    }
}
