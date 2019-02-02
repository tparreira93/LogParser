using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Parser
{
    public class CppLogLine : ILine, INotifyPropertyChanged
    {
        public CppLogLine(string module, string client, string machineUser, string login, int level, string function, string database, DateTime date, int processID)
        {
            Module = module;
            Client = client;
            MachineUser = machineUser;
            Login = login;
            Level = level;
            Function = function;
            Database = database;
            Date = date;
            ProcessID = processID;
        }

        public DateTime Date { get; set; }
        public string Module { get; set; }
        public string Login { get; set; }
        public string MachineUser { get; set; }
        public int ProcessID { get; set; }
        public string Function { get; set; }
        public string Text { get; set; }
        public string Client { get; set; }
        public string Database { get; set; }
        public int Level { get; set; }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
