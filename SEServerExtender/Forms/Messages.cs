using SEServerExtender.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEServerExtender.Forms
{
    public partial class Messages : Form
    {

        public SEServerExtender SESE { get; set; }
        private int? EditIndex { get; set; }

        private DataTable dt;

        public Messages()
        {
            InitializeComponent();
        }

        public Messages(SEServerExtender sese)
        {
            InitializeComponent();
            SESE = sese;
        }

        private void AddGridColumns(DataTable dt)
        {
            dt.Columns.Add(new DataColumn("Message", typeof(string)));
        }

        private void LoadMessages(DataTable dt)
        {
            dt.Clear();
            var messages = Settings.Default.Chat_Auto_Messages;
            if (messages == null)
            {
                Settings.Default.Chat_Auto_Messages = new StringCollection();
                Settings.Default.Save();
                messages = Settings.Default.Chat_Auto_Messages;
            }
            foreach (var i in messages)
            {
                dt.Rows.Add(i);
            }
        }

        private void Messages_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            AddGridColumns(dt);
            LoadMessages(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Add")
            {
                Settings.Default.Chat_Auto_Messages.Add(textBox1.Text);
                Settings.Default.Save();
                textBox1.Text = "";
                LoadMessages(dt);
            }
            else
            {
                if(EditIndex != null) {
                    Settings.Default.Chat_Auto_Messages[(int) EditIndex] = textBox1.Text;
                    Settings.Default.Save();
                    textBox1.Text = "";
                    button1.Text = "Add";
                    EditIndex = null;
                    LoadMessages(dt);
                }
                button2.Visible = false;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                EditIndex = e.RowIndex;
                textBox1.Text = dataGridView1.Rows[(int) EditIndex].Cells[0].Value.ToString();
                button1.Text = "Save";
                button2.Visible = true;
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.Default.Chat_Auto_Messages.RemoveAt((int) EditIndex);
                Settings.Default.Save();
                textBox1.Text = "";
                button1.Text = "Add";
                button2.Visible = false;
                EditIndex = null;
                LoadMessages(dt);
            }
            catch (Exception) { }
        }

        private void Messages_FormClosing(object sender, FormClosingEventArgs e)
        {
            SESE.m_server.threadAutoMessageServer.ReloadMessages();
        }
    }
}
