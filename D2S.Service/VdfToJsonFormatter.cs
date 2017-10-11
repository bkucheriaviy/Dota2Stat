using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace D2S.Service
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
            var commentStart = false;
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
                            sb.Append(c);
                            continue;
                        }
                    case newLine:
                        {
                            return sb.ToString();
                        }
                    case '/':
                        {
                            if (commentStart)
                            {
                                return null;
                            }
                            commentStart = true;
                            sb.Append(c);

                            continue;
                        }
                }
            }

            return sb.ToString();
        }

        public async Task Format(string vdfFilePath, IProgress<int> progress)
        {
            using (var fileStream = File.OpenRead(vdfFilePath))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var jsonTmpFile = Path.ChangeExtension(vdfFilePath, ".tmp");
                    using (var writer = File.AppendText(jsonTmpFile))
                    {
                        string jsonLine = null;
                        if ((jsonLine = this.FormatLine(await reader.ReadLineAsync())) != null)
                        {
                            await writer.WriteLineAsync(jsonLine);
                        }
                    }
                }
            }
        }
    }
}