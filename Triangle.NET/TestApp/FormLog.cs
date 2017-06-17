using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TriangleNet;
using TriangleNet.Logging;

namespace MeshExplorer
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }

        public void AddItem(string message, bool warning)
        {
            var log = Log.Instance;

            if (warning)
            {
                log.Warning(message, "Mesh Explorer");
            }
            else
            {
                log.Info(message);
            }
        }

        public void UpdateItems()
        {
            listLog.Items.Clear();

            var log = Log.Instance;

            foreach (var item in log.Data)
            {
                listLog.Items.Add(CreateListViewItem(item));
            }
        }

        private ListViewItem CreateListViewItem(LogItem item)
        {
            ListViewItem lvi = new ListViewItem(new string[] { item.Message, item.Info });

            if (item.Level == LogLevel.Error)
            {
                lvi.ForeColor = Color.DarkRed;
            }
            else if (item.Level == LogLevel.Warning)
            {
                lvi.ForeColor = Color.Peru;
            }
            else
            {
                lvi.ForeColor = Color.Black;
            }

            lvi.UseItemStyleForSubItems = true;

            return lvi;
        }

        private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void listLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                if (ModifierKeys == Keys.Control)
                {
                    var selection = listLog.SelectedItems;

                    if (selection != null && selection.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var item in selection)
                        {
                            GetRowText(sb, item);
                        }

                        Clipboard.SetText(sb.ToString());
                    }
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (ModifierKeys == Keys.Control)
                {
                    listLog.Items.Clear();
                    Log.Instance.Clear();
                }
            }
        }

        private void listLog_DoubleClick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in listLog.SelectedItems)
            {
                GetRowText(sb, item);
            }

            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());
            }
        }

        private void GetRowText(StringBuilder sb, object item)
        {
            var row = item as ListViewItem;

            if (row != null)
            {
                foreach (var col in row.SubItems)
                {
                    var lvi = col as ListViewItem.ListViewSubItem;

                    if (lvi != null)
                    {
                        sb.Append(lvi.Text);
                        sb.Append("; ");
                    }
                }
            }
        }
    }
}
