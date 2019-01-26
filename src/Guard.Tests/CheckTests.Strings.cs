using System;
using Xunit;

namespace Kraphity.Guard.Tests
{
    public partial class CheckTests
    {
        [Fact]
        public void NotEmpty()
        {
            string arg = "test";
            Check.NotEmpty(arg, nameof(arg));
        }

        [Fact]
        public void NotEmpty_NullValue()
        {
            string arg = null;
            Check.NotEmpty(arg, nameof(arg));
        }

        [Fact]
        public void NotEmpty_EmptyValue_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(() => Check.NotEmpty(arg, nameof(arg)));
        }

        [Fact]
        public void NotEmpty_EmptyValueParamExpression_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(() => Check.NotEmpty(arg, () => arg));
        }

        [Fact]
        public void NotWhitespace()
        {
            string arg = "test";
            Check.NotWhitespace(arg, nameof(arg));
        }

        [Fact]
        public void NotWhitespace_NullValue()
        {
            string arg = null;
            Check.NotWhitespace(arg, nameof(arg));
        }

        [Fact]
        public void NotWhitespace_Empty_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotWhitespace(arg, nameof(arg)));
        }

        [Fact]
        public void NotWhitespace_EmptyParamExpression_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotWhitespace(arg, () => arg));
        }

        [Fact]
        public void NotWhitespace_Whitespace_Throws()
        {
            string arg = "    ";
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotWhitespace(arg, nameof(arg)));
        }

        [Fact]
        public void NotNullOrEmpty()
        {
            string arg = "test";
            Check.NotNullOrEmpty(arg, nameof(arg));
        }

        [Fact]
        public void NotNullOrEmpty_NullValue_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(nameof(arg), () => Check.NotNullOrEmpty(arg, nameof(arg)));
        }

        [Fact]
        public void NotNullOrEmpty_NullValueParamExpression_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(nameof(arg), () => Check.NotNullOrEmpty(arg, () => arg));
        }

        [Fact]
        public void NotNullOrEmpty_EmptyValue_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotNullOrEmpty(arg, nameof(arg)));
        }

        [Fact]
        public void NotNullOrWhitespace()
        {
            string arg = "test";
            Check.NotNullOrWhitespace(arg, nameof(arg));
        }

        [Fact]
        public void NotNullOrWhitespace_NullValue_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(nameof(arg), () => Check.NotNullOrWhitespace(arg, nameof(arg)));
        }

        [Fact]
        public void NotNullOrWhitespace_NullValueParamExpression_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(nameof(arg), () => Check.NotNullOrWhitespace(arg, () => arg));
        }

        [Fact]
        public void NotNullOrWhitespace_EmptyValue_Throws()
        {
            string arg = string.Empty;
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotNullOrWhitespace(arg, nameof(arg)));
        }

        [Fact]
        public void NotNullOrWhitespace_WhitspaceValue_Throws()
        {
            string arg = "   ";
            Assert.Throws<ArgumentException>(nameof(arg), () => Check.NotNullOrWhitespace(arg, nameof(arg)));
        }
    }
}
