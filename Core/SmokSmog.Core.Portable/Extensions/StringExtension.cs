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
            if (string.IsNullOrWhiteSpace(str) || expressions == null || expressions.Count() == 0)
                return false;

            foreach (var item in expressions)
            {
                if (!str.Contains(item, comparisonType)) return false;
            }
            return true;
        }

        public static bool ContainsAny(this string str,
            IEnumerable<string> expressions, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(str) || expressions == null || expressions.Count() == 0)
                return false;

            foreach (var item in expressions)
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

            if (string.IsNullOrWhiteSpace(str) || expressions.Count() == 0)
                return result;

            foreach (var item in expressions)
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
            for (int i = 0; i < str.Length; i++)
            {
                if (IsNewSentense && char.IsLetter(str[i]))
                {
                    result.Append(char.ToUpper(str[i]));
                    IsNewSentense = false;
                }
                else
                    result.Append(char.ToLower(str[i]));

                if (str[i] == '!' || str[i] == '?' || str[i] == '.')
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
            for (int i = 0; i < str.Length; i++)
            {
                if (IsNewWord && char.IsLetter(str[i]))
                {
                    result.Append(char.ToUpper(str[i]));
                    IsNewWord = false;
                }
                else
                    result.Append(char.ToLower(str[i]));

                if (!char.IsLetter(str[i]))
                {
                    IsNewWord = true;
                }
            }

            return result.ToString();
        }
    }
}