using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BD20161
{
    [Serializable]
    public partial class Form1 : Form
    {
        private Form2 Form2 = new Form2();
        private Form3 Form3;
        private Form4 Form4 = new Form4();
        private TabControl tabControl1 = new TabControl();
        private TabPage tabPage = new TabPage();
        private TabPage tabPage1 = new TabPage();
        private ComboBox comboBox1 = new ComboBox();
        private TextBox textBox4 = new TextBox();
        private TextBox textBox3 = new TextBox();
        private TextBox textBox2 = new TextBox();
        private TextBox textBox1 = new TextBox();
        private Label label5 = new Label();
        private Label label4 = new Label();
        private Label label3 = new Label();
        private Label label2 = new Label();
        private Label label1 = new Label();
        private pass passport = new pass();
        private string file = @"pasport.cfg";
        private static MySqlConnection con = new MySqlConnection();
        private MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
        private DataGridView tab = new DataGridView();

        //DataGridView dataGridView1 = new DataGridView();
        private List<TabPage> LTabPage = new List<TabPage>();

        private FileStream fs;
        private Tab_zapros zapros = new Tab_zapros();
        private Kabineti kab = new Kabineti();

        private XmlSerializer xmlsr = new XmlSerializer(typeof(pass));

        public Form2 Form21
        {
            get
            {
                return Form2;
            }

            set
            {
                Form2 = value;
            }
        }

        public Form3 Form31
        {
            get
            {
                return Form3;
            }

            set
            {
                Form3 = value;
            }
        }

        public Form4 Form41
        {
            get
            {
                return Form4;
            }

            set
            {
                Form4 = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct pass
        {
            public string IP;
            public UInt32 port;
            public string user;
            public string password;
            public string database;
        }

        private void csb()
        {
            if (File.Exists(file))
            {
                fs = new FileStream(file, FileMode.OpenOrCreate);
                passport = (pass)xmlsr.Deserialize(fs);
                fs.Close();
            }
            if (passport.IP != null)
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
            comboBox1.Items.Clear();
            //List<String> list = new List<string>();
            foreach (DataRow item in GetComments(@"show databases").Rows)
            {
                comboBox1.Items.Add(item.ItemArray[0].ToString());
            }

            //comboBox1.Items.AddRange(list.ToArray());
            try
            {
                comboBox1.SelectedItem = passport.database;
            }
            catch { }
            if (comboBox1.SelectedItem != null)
            {
                mysqlCSB.Database = comboBox1.SelectedItem.ToString();
            }
        }

        public static DataTable GetComments(string queryString)
        {
            DataTable dt = new DataTable();
            //con.ConnectionString = mysqlCSB.ConnectionString;
            try
            {
                con.Open();
                dt.Load(new MySqlCommand(queryString, con).ExecuteReader());
            }
            catch { }
            con.Close();
            return dt;
        }

        public Form1()
        {
            InitializeComponent();
            csb();
            updata();
            Form_create();
            Form21.FormClosing += Form2_FormClosing;
            tabControl.Selected += TabControl_Selected;
            zapros.button.Click += Button_Click;
            kab.data.MouseDoubleClick += Data_MouseDoubleClick;
            kab.menu.ItemClicked += Menu_ItemClicked;
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case ("Добавить"):
                    Form31 = new Form3();
                    Form31.Show();
                    Form31.Activate();
                    Enabled = false;
                    Form31.FormClosing += Form3_FormClosing;
                    break;

                case ("Изменить"):
                    Form31 = new Form3();
                    Form31.Show();
                    Form31.Activate();
                    Form31.doit(kab.data.SelectedRows[0]);
                    Enabled = false;
                    Form31.FormClosing += Form3_FormClosing;
                    break;

                case ("Удалить"):
                    break;

                default:
                    break;
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Enabled = true;
            Form41.FormClosing -= Form4_FormClosing;
            Activate();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Enabled = true;
            Form31.FormClosing -= Form3_FormClosing;
            Activate();
        }

        private void Data_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void Button_Click(object sender, EventArgs e)
        {
            zapros.data.DataSource = GetComments(zapros.textBox.Text);
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            настройка();
        }

        public void настройка()
        {
            Form21.Show();
            Form21.Activate();
            Enabled = false;
        }

        private void updata()
        {
            con.ConnectionString = mysqlCSB.ConnectionString;
            LTabPage = new List<TabPage>();
            foreach (DataRow item in GetComments(@"show tables").Rows)
            {
                LTabPage.Add(new TabPage());
                LTabPage[LTabPage.Count - 1].Text = item.ItemArray[0].ToString();
            }
            tabControl.TabPages.Clear();
            zapros.Parent = tabControl;
            kab.Parent = tabControl;
            foreach (TabPage item in LTabPage)
            {
                tabControl.Controls.Add(item);
                DataGridView data = new DataGridView();
                data.Parent = item;
                data.Dock = DockStyle.Fill;
                data.ReadOnly = true;
            }
        }

        private void Form_create()
        {
            tabControl1.Parent = Form21;
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
                //e.TabPage.Controls.OfType<DataGridView>().FirstOrDefault().DataSource = GetComments(@"select * from " + e.TabPage.Text);
            }
            catch { }
        }

        private void ComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (((ComboBox)sender).Items.Count == 0)
            {
                csb();
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
            ((Form)sender).Hide();
            passport.IP = textBox1.Text;
            passport.port = Convert.ToUInt32(textBox2.Text);
            passport.user = textBox3.Text;
            passport.password = textBox4.Text;
            passport.database = comboBox1.SelectedItem.ToString();
            fs = new FileStream(file, FileMode.Create);
            xmlsr.Serialize(fs, passport);
            fs.Close();
            Enabled = true;
            updata();
        }

        internal static DataTable GetComments()
        {
            throw new NotImplementedException();
        }
    }
}