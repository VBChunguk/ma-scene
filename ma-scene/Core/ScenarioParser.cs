﻿using System.Collections.Generic;
using System.Drawing;
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
                    string titleRaw = buf.Substring(3);
                    string[] titleSplit = titleRaw.Split(new char[] { ',' }, 2);
                    string title = titleSplit.Length > 1 ? titleSplit[1].Trim() : titleSplit[0].Trim();
                    ret.Add(new TitleCommand(title));
                }
                else if (buf.StartsWith("BG ")) // bg
                {
                    string idRaw = buf.Substring(3);
                    if (string.IsNullOrWhiteSpace(idRaw)) ret.Add(new BackgroundCommand());
                    else
                    {
                        int id;
                        if (int.TryParse(idRaw, out id))
                            ret.Add(new BackgroundCommand(id));
                        else
                        {
                            if (idRaw.StartsWith("color="))
                            {
                                string colors = idRaw.Substring(6);
                                string[] colorValues = colors.Split(',');
                                ret.Add(new BackgroundCommand(Color.FromArgb(
                                    int.Parse(colorValues[0]), int.Parse(colorValues[1]),
                                    int.Parse(colorValues[2]), int.Parse(colorValues[3])
                                    )));
                            }
                        }
                    }
                }
                else if (buf.StartsWith("VO ")) // voice
                {
                    string id = buf.Substring(3);
                    ret.Add(new VoiceCommand(id));
                }
                else if (buf.StartsWith("C")) // character
                {
                    string args = buf.Substring(1);
                    string[] argsSplit = args.Split(new char[] { ' ' }, 2, System.StringSplitOptions.RemoveEmptyEntries);
                    if (argsSplit.Length > 1)
                        ret.Add(new CharacterCommand(int.Parse(argsSplit[0]), argsSplit[1]));
                    else
                        ret.Add(new CharacterCommand(int.Parse(argsSplit[0]), null));
                }
                else if (char.IsUpper(buf, 0) || char.IsSymbol(buf, 0)) // unknown
                {
                    ret.Add(new UnknownCommand(buf));
                }
                else // conversation
                {
                    if (isConv)
                    {
                        context += buf + "\r\n";
                        if (buf.EndsWith("」"))
                        {
                            ret.Add(new ConversationCommand(name, context));
                            isConv = false;
                        }
                    }
                    else
                    {
                        name = buf;
                        context = string.Empty;
                        isConv = true;
                    }
                }
            }
            if (isConv)
            {
                ret.Add(new ConversationCommand(name, context));
                isConv = false;
            }
            reader.Close();
            return ret.ToArray();
        }
    }
}
