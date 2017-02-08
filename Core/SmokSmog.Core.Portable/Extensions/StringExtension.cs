using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmokSmog.Extensions
{
    public static class StringExtension
    {
        public static bool Contains(this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            if (value == null)
                return false;

            switch (comparisonType)
            {
                case StringComparison.CurrentCultureIgnoreCase:
                case StringComparison.OrdinalIgnoreCase:
                    return str.ToLower().Contains(value.ToLower());

                case StringComparison.CurrentCulture:
                case StringComparison.Ordinal:
                default:
                    return str.Contains(value);
            }
        }

        public static bool ContainsAll(this string str,
            IEnumerable<string> expressions, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (expressions == null) return false;

            var items = expressions as string[] ?? expressions?.ToArray();

            if (string.IsNullOrWhiteSpace(str) || !items.Any())
                return false;

            foreach (var item in items)
            {
                if (!str.Contains(item, comparisonType)) return false;
            }
            return true;
        }

        public static bool ContainsAny(this string str,
            IEnumerable<string> expressions, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (expressions == null) return false;

            var items = expressions as string[] ?? expressions?.ToArray();

            if (string.IsNullOrWhiteSpace(str) || !items.Any())
                return false;

            foreach (var item in items)
            {
                if (str.Contains(item, comparisonType)) return true;
            }
            return false;
        }

        public static IEnumerable<int> IndexOfAll(this string str,
            string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));

            List<int> indexes = new List<int>();

            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, comparisonType);
                if (index == -1)
                    break;
                indexes.Add(index);
            }

            return indexes;
        }

        public static IDictionary<string, IEnumerable<int>> IndexOfAll(this string str,
            IEnumerable<string> expressions, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            var result = new Dictionary<string, IEnumerable<int>>();

            var items = expressions as string[] ?? expressions.ToArray();

            if (string.IsNullOrWhiteSpace(str) || !items.Any())
                return result;

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                    result[item] = str.IndexOfAll(item, comparisonType).ToList();
            }

            return result;
        }

        public static string RemoveWhiteSpaces(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            return Regex.Replace(str ?? "", "(\\s){2,}", " ");
        }

        public static string ToFirstCharCase(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ToSentenceCase(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            bool IsNewSentense = true;
            var result = new StringBuilder(str.Length);
            foreach (char @char in str)
            {
                if (IsNewSentense && char.IsLetter(@char))
                {
                    result.Append(char.ToUpper(@char));
                    IsNewSentense = false;
                }
                else
                    result.Append(char.ToLower(@char));

                if (@char == '!' || @char == '?' || @char == '.')
                {
                    IsNewSentense = true;
                }
            }

            return result.ToString();
        }

        public static string ToWordsCase(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            bool IsNewWord = true;
            var result = new StringBuilder(str.Length);
            foreach (char @char in str)
            {
                if (IsNewWord && char.IsLetter(@char))
                {
                    result.Append(char.ToUpper(@char));
                    IsNewWord = false;
                }
                else
                    result.Append(char.ToLower(@char));

                if (!char.IsLetter(@char))
                {
                    IsNewWord = true;
                }
            }

            return result.ToString();
        }
    }
}