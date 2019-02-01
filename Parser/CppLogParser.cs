using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Parser
{
    public class CppLogParser : IParser
    {
        public string FileName { get; private set; }
        private Regex LogHeader = new Regex(@"(DATA=(\d{2}/\d{2}/\d{4}))?( )*(HORA=(\d{2}:\d{2}:\d{2}))?( )*(MODULE=(\w+))?( )*(DB=(\w+))?( )*(CLIENT=(\w+))?( *)(NETUSER=([^\s]+))?( )*(LOGIN=([^\s]+))?( )*(LEVEL=([^\s]+))?( )*(FUNÇÃO=([^\s]+))?");

        public CppLogParser(string fileName)
        {
            FileName = fileName;
        }

        public ErrlogCpp Parse()
        {
            IList<ILine> lines = new List<ILine>();
            ErrlogCpp errlog = new ErrlogCpp(lines);
            if (!File.Exists(FileName))
                return errlog;

            IEnumerable<string> file = File.ReadAllLines(FileName);
            CppLogLine line = null;
            StringBuilder text = new StringBuilder();
            foreach (var item in file)
            {
                Match match = LogHeader.Match(item);
                if (match.Success)
                {
                    if (text.Length != 0)
                        line.Text = text.ToString();

                    lines.Add(line);

                    string data = match.Groups[1].Value;
                    string hora = match.Groups[4].Value;
                    string modulo = match.Groups[8].Value;
                    string db = match.Groups[11].Value;
                    string client = match.Groups[14].Value;
                    string netuser = match.Groups[17].Value;
                    string login = match.Groups[20].Value;
                    string level = match.Groups[23].Value;
                    string funcao = match.Groups[26].Value;

                    TimeSpan horaParsed;
                    DateTime.TryParse(data, out DateTime dataParsed);
                    if (TimeSpan.TryParse(hora, out horaParsed))
                        dataParsed = dataParsed.Add(horaParsed);
                    int.TryParse(level, out int levelParsed);

                    line = new CppLogLine(netuser, login, levelParsed, funcao, db, dataParsed);

                }
                else
                    text.Append(item);
            }

            if (line != null)
                lines.Add(line);
            
            return errlog;
        }
    }
}
