using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    class CppLogLine : ILine
    {
        public CppLogLine(string machineUser, string login, int level, string function, string database, DateTime date)
        {
            MachineUser = machineUser;
            Login = login;
            Level = level;
            Function = function;
            Database = database;
            Date = date;
        }

        public string Text { get; set; }
        public string MachineUser { get; set; }
        public string Login { get; set; }
        public int Level { get; set; }
        public string Function { get; set; }
        public string Database { get; set; }
        public DateTime Date { get; set; }
    }
}
