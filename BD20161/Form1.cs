using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml;

namespace BD20161
{
    [Serializable]
    public partial class Form1 : Form
    {
        Form Form2 = new Form2();
        TabControl tabControl1 = new TabControl();
        TabPage tabPage = new TabPage();
        TabPage tabPage1 = new TabPage();
        ComboBox comboBox1 = new ComboBox();
        TextBox textBox4 = new TextBox();
        TextBox textBox3 = new TextBox();
        TextBox textBox2 = new TextBox();
        TextBox textBox1 = new TextBox();
        Label label5 = new Label();
        Label label4 = new Label();
        Label label3 = new Label();
        Label label2 = new Label();
        Label label1 = new Label();
        pass passport = new pass();
        string file = @"pasport.cfg";
        MySqlConnection con = new MySqlConnection();
        MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
        DataGridView tab = new DataGridView();
        //DataGridView dataGridView1 = new DataGridView();
        List<TabPage> LTabPage = new List<TabPage>();

        [StructLayout(LayoutKind.Sequential)]
        public struct pass
        {
            public string IP;
            public UInt32 port;
            public string user;
            public string password;
            public string database;
        }

        void csb(bool b = false)
        {
            
            if (File.Exists(file))
            {
                try
                {
                    XmlSerializer xmlsr = new XmlSerializer(typeof(pass));
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                        passport = (pass)xmlsr.Deserialize(fs);
                }
                catch (Exception)
                {
                }
                
            }
            if (passport.IP!=null)
            {
                textBox1.Text = passport.IP;
                textBox2.Text = passport.port.ToString();
                textBox3.Text = passport.user;
                textBox4.Text = passport.password;
            }
            mysqlCSB.Server = textBox1.Text;
            if (textBox2.Text != "")
            {
                mysqlCSB.Port = Convert.ToUInt32(textBox2.Text);
            }
            mysqlCSB.UserID = textBox3.Text;
            mysqlCSB.Password = textBox4.Text;
            

            con.ConnectionString = mysqlCSB.ConnectionString;
            try
            {
                DataTable dt = new DataTable();
                con.Open();
                dt.Load(new MySqlCommand(@"show databases;", con).ExecuteReader());
                con.Close();
                List<String> list = new List<string>();
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item.ItemArray[0].ToString());
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(list.ToArray());
            }
            catch (Exception ex)
            {
                if (b)
                {
                    Close();
                }
                MessageBox.Show(ex.Message);
            }
            comboBox1.SelectedItem = passport.database;
            if (comboBox1.SelectedItem != null)
            {
                mysqlCSB.Database = comboBox1.SelectedItem.ToString();
            }
        }

        DataTable GetComments(string queryString = @"show databases;")
        {
            DataTable dt = new DataTable();
            con.ConnectionString = mysqlCSB.ConnectionString;
            MySqlCommand com = new MySqlCommand(queryString, con);
            try
            {
                con.Open();
                dt.Load(com.ExecuteReader());
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public Form1()
        {
            InitializeComponent();
            csb();
            updata();
            Form_create();
            Form2.FormClosing += Form2_FormClosing;
            tabControl.Selected += TabControl_Selected;
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            настройка();
        }

        public void настройка()
        {
            Form2.Show();
            Form2.Activate();
            Enabled = false;
        }

        private void updata()
        {
            con.ConnectionString = mysqlCSB.ConnectionString;
            try
            {
                DataTable dt = new DataTable();
                con.Open();
                dt.Load(new MySqlCommand(@"show tables", con).ExecuteReader());
                con.Close();
                List<String> list = new List<string>();
                int i = 0;
                LTabPage.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    LTabPage.Add(new TabPage());
                    LTabPage[i].Name = "tabPage" + i.ToString();
                    LTabPage[i].Text = item.ItemArray[0].ToString();

                    i++;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            tabControl.TabPages.Clear();
            foreach (var item in LTabPage)
            {
                tabControl.Controls.Add(item);
                DataGridView data = new DataGridView();
                data.Name = "dataGridView" + item.Text;
                data.Parent = item;
                data.Dock = DockStyle.Fill;
                data.ReadOnly = true;
            }
            if (tabControl.TabCount!=0)
            {
                TabControl_Selected(tabControl, null);
            }
        }

        private void Form_create()
        {
            
            
            tabControl1.Parent = Form2;
            tabControl1.Controls.Add(tabPage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.SelectedIndex = 0;

            tabPage.Parent = tabControl1;
            tabPage.Text = "Сеть";
            tabPage.UseVisualStyleBackColor = true;

            comboBox1.Parent = tabPage;
            comboBox1.Location = new Point(108, 135);
            comboBox1.MouseClick += ComboBox1_MouseClick;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            textBox4.Parent = tabPage;
            textBox4.Location = new Point(108, 109);
            textBox4.TabIndex = 8;

            textBox3.Parent = tabPage;
            textBox3.Location = new Point(108, 83);
            textBox3.TabIndex = 7;

            textBox2.Parent = tabPage;
            textBox2.Location = new Point(108, 57);
            textBox2.TabIndex = 6;
            textBox2.KeyPress += TextBox2_KeyPress;

            textBox1.Parent = tabPage;
            textBox1.Location = new Point(108, 31);
            textBox1.TabIndex = 5;

            label5.Parent = tabPage;
            label5.AutoSize = true;
            label5.Location = new Point(9, 138);
            label5.TabIndex = 4;
            label5.Text = "Имя базы";

            label4.Parent = tabPage;
            label4.AutoSize = true;
            label4.Location = new Point(9, 112);
            label4.TabIndex = 3;
            label4.Text = "Пароль";

            label3.Parent = tabPage;
            label3.AutoSize = true;
            label3.Location = new Point(9, 86);
            label3.TabIndex = 2;
            label3.Text = "Логин";

            label2.Parent = tabPage;
            label2.AutoSize = true;
            label2.Location = new Point(9, 60);
            label2.TabIndex = 1;
            label2.Text = "Порт";

            label1.Parent = tabPage;
            label1.AutoSize = true;
            label1.Location = new Point(9, 34);
            label1.TabIndex = 0;
            label1.Text = "Адрес";
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                mysqlCSB.Database = comboBox1.SelectedItem.ToString();
            }
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                TabPage t1 = (TabPage)((TabControl)sender).Controls["tabPage" + ((TabControl)sender).SelectedIndex];
                ((DataGridView)t1.Controls["dataGridView" + t1.Text]).DataSource = GetComments(@"select * from " + t1.Text);
            }
            catch { }
        }

        private void ComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Items.Count == 0)
            {
                csb(true);
            }
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Activate();
            e.Cancel = true;
            Form f = (Form)sender;
            f.Hide();
            passport.IP = textBox1.Text;
            passport.port = Convert.ToUInt32(textBox2.Text);
            passport.user = textBox3.Text;
            passport.password = textBox4.Text;
            if (comboBox1.SelectedItem != null)
            {
                passport.database = comboBox1.SelectedItem.ToString();
            }
            else
            {
                Close();
            }
            XmlSerializer xmlsr = new XmlSerializer(typeof(pass));
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate)) xmlsr.Serialize(fs, passport);
            Enabled = true;
            TabControl_Selected(tabControl, null);
            //GetComments();
            updata();
        }

    }
}
