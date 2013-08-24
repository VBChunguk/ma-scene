using System.Collections.Generic;
using System.IO;

namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 시나리오 스크립트를 해석하는 메서드를 가진 정적 클래스입니다.
    /// </summary>
    public static class ScenarioParser
    {
        /// <summary>
        /// System.IO.Stream으로부터 스크립트를 읽어 해석합니다. 해당 System.IO.Stream은 자동으로 닫힙니다.
        /// </summary>
        /// <param name="stream">스크립트 데이터를 포함한 System.IO.Stream입니다.</param>
        /// <returns>스크립트의 내용을 포함한 Vbc.MA.Scenario.Core.ScenarioCommand 배열입니다.</returns>
        public static ScenarioCommand[] Parse(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return Parse(reader);
        }

        /// <summary>
        /// 문자열으로부터 스크립트를 읽어 해석합니다.
        /// </summary>
        /// <param name="context">스크립트 데이터를 포함한 문자열입니다.</param>
        /// <returns>스크립트의 내용을 포함한 Vbc.MA.Scenario.Core.ScenarioCommand 배열입니다.</returns>
        public static ScenarioCommand[] Parse(string context)
        {
            StringReader reader = new StringReader(context);
            return Parse(reader);
        }

        private static ScenarioCommand[] Parse(TextReader reader)
        {
            List<ScenarioCommand> ret = new List<ScenarioCommand>();
            bool isConv = false;
            string name = string.Empty;
            string context = string.Empty;
            while (true)
            {
                string buf = reader.ReadLine();
                if (buf == null) break;
                if (buf.StartsWith("TI ")) // title
                {
                    if (isConv)
                    {
                        ret.Add(new ConversationCommand(name, context));
                        isConv = false;
                    }
                    string titleRaw = buf.Substring(3);
                    string[] titleSplit = titleRaw.Split(new char[] { ',' }, 2);
                    string title = titleSplit.Length > 1 ? titleSplit[0].Trim() : titleSplit[1].Trim();
                    ret.Add(new TitleCommand(title));
                }
                else if (buf.StartsWith("BG ")) // bg
                {
                    if (isConv)
                    {
                        ret.Add(new ConversationCommand(name, context));
                        isConv = false;
                    }
                }
                else if (buf.StartsWith("MU ")) // music
                {
                    if (isConv)
                    {
                        ret.Add(new ConversationCommand(name, context));
                        isConv = false;
                    }
                }
                else if (buf.StartsWith("C")) // character
                {
                    if (isConv)
                    {
                        ret.Add(new ConversationCommand(name, context));
                        isConv = false;
                    }
                    string args = buf.Substring(1);
                    string[] argsSplit = args.Split(new char[] { ' ' }, 2);
                    if (argsSplit.Length > 1)
                        ret.Add(new CharacterCommand(int.Parse(argsSplit[0]), argsSplit[1]));
                    else
                        ret.Add(new CharacterCommand(int.Parse(argsSplit[0]), null));
                }
                else if (buf.StartsWith("<")) // special
                {
                    if (isConv)
                    {
                        ret.Add(new ConversationCommand(name, context));
                        isConv = false;
                    }
                }
                else // conversation
                {
                    if (isConv) context += buf + "\r\n";
                    else
                    {
                        name = buf;
                        context = string.Empty;
                    }
                    isConv = true;
                }
            }
            reader.Close();
            return ret.ToArray();
        }
    }
}
