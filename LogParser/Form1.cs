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
        private CppLogParser parser;
        public MainForm()
        {
            InitializeComponent();
            filePath = "";
            UpdateStatusStrip();
            comboBoxFilterType.SelectedIndex = 0;
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
            this.Text = string.Format("Log");
            listView.ClearObjects();
            listView.Columns.Clear();
        }

        private void OpenFile(string file)
        {
            toolProgress.Visible = true;
            menuStrip.Enabled = false;
            btnCancel.Visible = true;
            filePath = file;
            this.Text = string.Format("Log [{0}]", filePath);

            Type t = typeof(CppLogLine);
            PropertyInfo[] props = t.GetProperties();
            listView.Columns.AddRange(t.GetProperties().Select(x => x.PropertyType.Equals(typeof(DateTime)) ?
                new OLVColumn(x.Name, x.Name) { Width = 145, ClusteringStrategy = new DateTimeClusteringStrategy(DateTimePortion.Day | DateTimePortion.Month | DateTimePortion.Year, "dd/MM/yyyy") }
                : new OLVColumn(x.Name, x.Name) { Width = 145 }).ToArray());
            parser = new CppLogParser(filePath);
            parser.ProgressChanged += worker_ProgressChanged;
            parser.RunWorkerCompleted += worker_end;
            parser.RunWorkerAsync();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            OpenFile(fileDialog.FileName);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolProgress.Value = e.ProgressPercentage;
        }

        private void worker_end(object sender, RunWorkerCompletedEventArgs e)
        {
            errlog = parser.Result;
            if (errlog != null)
                listView.SetObjects(errlog.Lines);
            toolProgress.Value = 100;
            toolProgress.Visible = false;
            btnCancel.Visible = false;
            menuStrip.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }
        private void Find()
        {
            string[] txt = txtSearch.Text.Split('|').Select(x => x.Trim()).ToArray();
            TextMatchFilter filter = null;

            switch (comboBoxFilterType.SelectedIndex)
            {
                case 0:
                default:
                    filter = TextMatchFilter.Contains(listView, txt);
                    break;
                case 1:
                    filter = TextMatchFilter.Prefix(listView, txt);
                    break;
                case 2:
                    filter = TextMatchFilter.Regex(listView, txt);
                    break;
            }
            listView.DefaultRenderer = new HighlightTextRenderer(filter);
            listView.AdditionalFilter = filter;

            IList objects = listView.Objects as IList;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void listView_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            UpdateStatusStrip();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            parser.CancelAsync();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (parser != null && parser.IsBusy)
                parser.CancelAsync();
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1)
                OpenFile(files[0]);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
