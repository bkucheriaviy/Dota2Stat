using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D2S.Service
{
    //quotes = not mandatory present
    //key value = mandatory
    //comments - not mandatory 
    public class LevelState
    {
        public bool Opened { get; set; }
        public bool Key { get; set; }
        public bool Value { get; set; }
        public bool Comment { get; set; }
        
        public static char Open_Brac = '{';
        public static char Closing_Brac = '}';
    }

    public class VdfToJsonFormatter 
    {
        public string Format(string vdfText)
        {
            var sb = new StringBuilder();
            var stack = new Stack<LevelState>();

            stack.Push(new LevelState());

            sb.Append(LevelState.Open_Brac);
            int position = 0;

            int lastCommaPosition = 0;
            foreach (char c in vdfText)
            {
                var state = stack.Peek();
                position++;
                if (state.Comment)
                {
                    if (c != '\n')
                    {
                        continue;
                    }
                    state.Comment = false;
                }

                switch (c)
                {
                    case '"':
                        {
                            if (!state.Opened && !state.Key)
                            {
                                sb.Append(c);
                                state.Opened = true;
                                state.Key = true;

                                continue;
                            }
                            else if (state.Opened && state.Key && !state.Value)
                            {
                                sb.Append(c);
                                sb.Append(':'); 
                                state.Opened = false;

                                continue;
                            }
                            else if (!state.Opened && state.Key && !state.Value)
                            {
                                sb.Append(c);
                                state.Opened = true;
                                state.Value = true;

                                continue;
                            }
                            else if (state.Opened && state.Key && state.Value)
                            {
                                sb.Append(c);
                                sb.Append(',');
                                lastCommaPosition = sb.Length;
                                state.Opened = false;
                                state.Key = false;
                                state.Value = false;

                                continue;
                            }
                            else { throw new InvalidOperationException($"Wrong symbol {c} at position {position}"); }
                        }
                    case '{':
                        {
                            sb.Append(c);
                            stack.Push(new LevelState());
                            continue;
                        }
                    case '}':
                        {
                            sb.Remove(lastCommaPosition - 1, 1);
                            sb.Append(c);
                            stack.Pop();

                            continue;
                        }
                    case '/':
                        {
                            state.Comment |= !state.Opened;
                            continue;
                        }
                    default:
                        {
                            sb.Append(c);
                            break;
                        }
                }
            }

            return sb.ToString();
        }

        public async Task FormatFile (string vdfFilePath, IProgress<int> progress) 
        {
            using (var fileStream = File.OpenRead (vdfFilePath)) 
            {
                using (var reader = new StreamReader (fileStream)) 
                {
                    var jsonTmpFile = Path.ChangeExtension (vdfFilePath, ".tmp");
                    using (var writer = File.AppendText (jsonTmpFile)) 
                    {
                        string jsonLine = null;
                        if ((jsonLine = this.Format (await reader.ReadLineAsync ())) != null) 
                        {
                            await writer.WriteLineAsync (jsonLine);
                        }
                    }
                    var targetFileName = Path.ChangeExtension (jsonTmpFile, ".json");
                    RenameFile (jsonTmpFile, targetFileName);
                    Console.WriteLine ($"{targetFileName} have been created.");
                }
            }
        }

        private void RenameFile(string sourceFile, string targetFile) 
        {
            var maxRetryCount = 5;
            var delayInSeconds = 1;
            var retry = 1;
            while (retry <= maxRetryCount) {
                Console.WriteLine ($"Renaming {sourceFile} to {targetFile}. Attempt {retry} from {maxRetryCount}...");

                try {
                    if (File.Exists(targetFile)) {
                        File.Delete(targetFile);
                    }

                    File.Move (sourceFile, targetFile);
                    break;
                } catch (Exception ex) {
                    Console.WriteLine ($"An error occured during remaming {sourceFile} to {targetFile}.\n{ex}");
                    Thread.Sleep (TimeSpan.FromSeconds (delayInSeconds));
                }
            }
        }
    }
}