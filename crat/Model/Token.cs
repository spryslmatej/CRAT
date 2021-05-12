using System;
using System.Text.RegularExpressions;

namespace CRAT.Model
{
    public class Token
    {
        public string Text { get; }

        public Token(string t)
        {
            var noWhitespaces = Regex.Replace(t, @"\s+", "");
            if (noWhitespaces.Length == 0)
                throw new ArgumentException("Token text cannot be empty.");

            Text = t;
        }

        public override string ToString() { return Text; }
    }
}
