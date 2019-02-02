using BrightIdeasSoftware;
using Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogParser
{
    public partial class MainForm : Form
    {
        private string filePath;
        private IErrlog errlog;
        public MainForm()
        {
            InitializeComponent();
            filePath = "";
            UpdateStatusStrip();
        }

        void UpdateStatusStrip()
        {
            this.statusLabel.Text = String.Format("{0} items", listView.Items.Count);
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.SelectedObject is ILine line)
            {
                LineLog form = new LineLog(line);
                form.Show(this);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            filePath = fileDialog.FileName;

            Type t = typeof(CppLogLine);
            PropertyInfo[] props = t.GetProperties();
            listView.Columns.AddRange(t.GetProperties().Select(x => x.PropertyType.Equals(typeof(DateTime)) ?
                new OLVColumn(x.Name, x.Name) { Width = 145, ClusteringStrategy = new DateTimeClusteringStrategy(DateTimePortion.Day | DateTimePortion.Month | DateTimePortion.Year, "dd/MM/yyyy") }
                : new OLVColumn(x.Name, x.Name) { Width = 145 }).ToArray());
            CppLogParser parser = new CppLogParser(filePath);
            errlog = parser.Parse();

            listView.SetObjects(errlog.Lines);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string[] txt = txtSearch.Text.Split('|').Select(x => x.Trim()).ToArray();

            TextMatchFilter filter = TextMatchFilter.Contains(listView, txt);
            listView.DefaultRenderer = new HighlightTextRenderer(filter);
            listView.AdditionalFilter = filter;

            IList objects = listView.Objects as IList;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFind.PerformClick();
        }

        private void listView_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            UpdateStatusStrip();
        }
    }
}
