using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SmokSmog.Extensions;

namespace SmokSmog.Core.Common.Tests.Extensions
{
    [TestClass]
    public class StringExtensionTests
    {
        private static readonly string _loremIpsumLong =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit,
sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        private static readonly string _loremIpsumShort = "Lorem ipsum dolor sit amet";

        [TestMethod]
        public void Contains_NullStringArgumentReturnFalse()
        {
            // act
            Assert.IsFalse(StringExtension.Contains(null, _loremIpsumShort));
        }

        [TestMethod]
        public void Contains_NullValueReturnFalse()
        {
            // act
            Assert.IsFalse(StringExtension.Contains(_loremIpsumShort, null));
        }

        [TestMethod]
        public void Contains_ValidInputReturnTrue()
        {
            // act
            Assert.IsTrue(StringExtension.Contains(_loremIpsumShort, "ipsum"));
        }

        [TestMethod]
        public void ContainsAll_NullStringArgumentReturnFalse()
        {
            string[] exp = { "dolor", "ipsum" };

            // act
            Assert.IsFalse(StringExtension.ContainsAll(null, exp));
        }

        [TestMethod]
        public void ContainsAll_NullValueReturnFalse()
        {
            // act
            Assert.IsFalse(StringExtension.ContainsAll(_loremIpsumShort, null));
        }

        [TestMethod]
        public void ContainsAll_OneMissingWordReturnFalse()
        {
            string[] exp = { "dolor", "ipsum", "missing" };

            // act
            Assert.IsFalse(StringExtension.ContainsAll(_loremIpsumShort, exp));
        }

        [TestMethod]
        public void ContainsAll_ValidInputReturnTrue()
        {
            string[] exp = { "dolor", "ipsum" };

            // act
            Assert.IsTrue(StringExtension.ContainsAll(_loremIpsumShort, exp));
        }

        [TestMethod]
        public void ContainsAny_NullStringArgumentReturnFalse()
        {
            string[] exp = { "dolor", "ipsum" };

            // act
            Assert.IsFalse(StringExtension.ContainsAny(null, exp));
        }

        [TestMethod]
        public void ContainsAny_NullValueReturnFalse()
        {
            // act
            Assert.IsFalse(StringExtension.ContainsAny(_loremIpsumShort, null));
        }

        [TestMethod]
        public void ContainsAny_OneMissingWordReturnTrue()
        {
            string[] exp = { "dolor", "ipsum", "missing" };

            // act
            Assert.IsTrue(StringExtension.ContainsAny(_loremIpsumShort, exp));
        }

        [TestMethod]
        public void ContainsAny_ValidInputReturnTrue()
        {
            string[] exp = { "dolor", "ipsum" };

            // act
            Assert.IsTrue(StringExtension.ContainsAny(_loremIpsumShort, exp));
        }

        [TestMethod]
        public void IndexOfAll_GetIndexOfAllTestWords()
        {
            // act
            var result = StringExtension.IndexOfAll(_loremIpsumLong, " in ");
            int count = result.Count();
            Assert.IsTrue(count == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfAll_NullStringThrowException()
        {
            // act

            string str = null;
            str.IndexOfAll(_loremIpsumShort);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IndexOfAll_NullValueThrowException()
        {
            string value = null;

            // act
            StringExtension.IndexOfAll(_loremIpsumShort, value, StringComparison.Ordinal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveWhiteSpaces_NullStringThrowException()
        {
            // act
            StringExtension.RemoveWhiteSpaces(null);
        }

        [TestMethod]
        public void RemoveWhiteSpaces_ValidInputReturnTrue()
        {
            string str = "lorem    ipsum";

            // act
            Assert.IsTrue("lorem ipsum" == StringExtension.RemoveWhiteSpaces(str));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToFirstCharCase_NullStringThrowException()
        {
            // act
            StringExtension.ToFirstCharCase(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToSentenceCase_NullStringThrowException()
        {
            // act
            StringExtension.ToSentenceCase(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToWordsCase_NullStringThrowException()
        {
            // act
            StringExtension.ToWordsCase(null);
        }
    }
}