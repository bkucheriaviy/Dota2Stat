using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D2S.Service.Tests
{
    public class VdfToJsonFormatter
    {
        private const char quoute = '"';
        private const char tabChar = '\t';
        private const char newLine = '\n';

        private const char esc = '\u001B';

        public string FormatLine(string vdfLine)
        {
            var openQuote = false;

            var sb = new StringBuilder();

            foreach (char c in vdfLine)
            {
                switch (c)
                {
                    case '"':
                        {
                            if (!openQuote)
                            {
                                openQuote = true;
                                sb.Append(c);
                            }
                    
                            sb.Append(c);
                            sb.Append(':');
                            openQuote = false;
                            continue;
                        }
                    case tabChar:
                        {
                            continue;
                        }
                }
            }

            return sb.ToString();
        }

        internal object Format(string v)
        {
            throw new NotImplementedException();
        }
    }
}