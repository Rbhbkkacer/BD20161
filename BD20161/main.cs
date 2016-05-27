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
    public partial class main : Form
    {
        private AboutBox1 About = new AboutBox1();
        private ComboBox SelectDatabase = new ComboBox();
        private Form ConfigForm = new Form();
        private string ConfigFile = @"pasport.cfg";
        private FileStream fs;
        private Tab_grups grups = new Tab_grups();

        private Tab_kab_prep kab = new Tab_kab_prep(@"select a.ID as '№', b.tips as 'Тип' from kabinet as a,tipkabineta_tip as b where a.tip_kabineta=b.ID ORDER BY a.ID", "kabinet", new string[] { "tip_predmeta", "tipkabineta_tip" }, new List<edit_kab_prep._item>()
        {
            new edit_kab_prep._item() { name = "№", name_in_database = "ID", n_tip = 0, table_name = "kabinet" },
            new edit_kab_prep._item() { name = "Тип пар", name_in_database = "tip", n_tip = 1, table_name = "tip_predmeta" }
        }, new List<edit_kab_prep.reference>()
        {
            new edit_kab_prep.reference() {name_in_database="tip_kabineta",ref_name_in_database="Id",ref_table_name="tipkabineta_tip",table_name="kabinet" }
        })
        { Text = "Кабинеты" };

        private Tab_kab_prep prepod = new Tab_kab_prep(@"select a.Prepod as 'ФИО', b.predmets as 'Предметы', c.tips as 'Тип' from prepodovateli as a, predmets_group as b, tipkabineta_tip as c where a.predmrts=b.ID && a.ID_Tips=c.ID ORDER BY a.Prepod", "prepodovateli", new string[] { "predmet", "predmets_group", "tipkabineta_tip" }, new List<edit_kab_prep._item>()
        {
            new edit_kab_prep._item() { name = "ФИО", name_in_database = "Prepod", n_tip = 0, table_name = "prepodovateli" },
            new edit_kab_prep._item() { name = "Предметы", name_in_database = "predmet", n_tip = 1, table_name = "predmet" },
            new edit_kab_prep._item() { name = "Тип пар", name_in_database = "tip", n_tip = 1, table_name = "tip_predmeta" }
        }, new List<edit_kab_prep.reference>()
        {
            new edit_kab_prep.reference() {name_in_database="predmrts",ref_name_in_database="Id",ref_table_name="predmets_group",table_name="prepodovateli" },
            new edit_kab_prep.reference() {name_in_database="ID_tips",ref_name_in_database="Id",ref_table_name="tipkabineta_tip",table_name="prepodovateli" }
        })
        { Text = "Преподаавтели" };

        private Label LabelUrl = new Label();
        private Label LabelPort = new Label();
        private Label LabelUserName = new Label();
        private Label LabelPassword = new Label();
        private Label LabelDatabaseName = new Label();
        private Button Ok = new Button();

        private List<TabPage> LTabPage = new List<TabPage>();

        private MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
        private pass ConnectionConfig = new pass();
        private DataGridView tab = new DataGridView();
        private TabControl tabControl1 = new TabControl();
        private TabPage tabPage = new TabPage();
        private TabPage tabPage1 = new TabPage();
        private TextBox Url = new TextBox();
        private TextBox Port = new TextBox();
        private TextBox UserName = new TextBox();
        private TextBox Password = new TextBox();
        private XmlSerializer xmlsr = new XmlSerializer(typeof(pass));

        //private Tab_zapros zapros = new Tab_zapros();
        private TabPage tabr1 = new TabPage();

        private Tab_rasp[] r1 = new Tab_rasp[4];

        public main()
        {
            InitializeComponent();
            csb();
            updata();
            Form_create();
            ConfigForm.FormClosing += FormClosing;
            About.FormClosing += FormClosing;
            //zapros.button.Click += Button_Click;
            grups.Form.FormClosing += FormClosing;
            About.okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ((sender as Button).Parent.Parent as Form).Close();
        }

        private void настройка()
        {
            ConfigForm.Show();
            ConfigForm.Activate();
            Enabled = false;
        }

        /*private void Button_Click(object sender, EventArgs e)
        {
            zapros.data.DataSource = SQL.GetComments(zapros.textBox.Text);
        }*/

        private void ComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (((ComboBox)sender).Items.Count == 0)
            {
                csb();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectDatabase.SelectedItem != null)
            {
                mysqlCSB.Database = SelectDatabase.SelectedItem.ToString();
            }
        }

        private void csb()
        {
            if (File.Exists(ConfigFile))
            {
                fs = new FileStream(ConfigFile, FileMode.OpenOrCreate);
                ConnectionConfig = (pass)xmlsr.Deserialize(fs);
                fs.Close();
            }
            if (ConnectionConfig.IP != null)
            {
                Url.Text = ConnectionConfig.IP;
                Port.Text = ConnectionConfig.port.ToString();
                UserName.Text = ConnectionConfig.user;
                Password.Text = ConnectionConfig.password;
            }
            mysqlCSB.Server = Url.Text;
            if (Port.Text != "")
            {
                mysqlCSB.Port = Convert.ToUInt32(Port.Text);
            }
            mysqlCSB.UserID = UserName.Text;
            mysqlCSB.Password = Password.Text;

            SQL.con.ConnectionString = mysqlCSB.ConnectionString;
            SelectDatabase.Items.Clear();
            foreach (DataRow item in SQL.GetComments(@"show databases").Rows)
            {
                SelectDatabase.Items.Add(item.ItemArray[0].ToString());
            }

            try
            {
                SelectDatabase.SelectedItem = ConnectionConfig.database;
            }
            catch { }
            if (SelectDatabase.SelectedItem != null)
            {
                mysqlCSB.Database = SelectDatabase.SelectedItem.ToString();
            }
        }

        private void Form_create()
        {
            tabControl1.Parent = ConfigForm;
            tabControl1.Controls.Add(tabPage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.SelectedIndex = 0;

            tabPage.Parent = tabControl1;
            tabPage.Text = "Сеть";
            tabPage.UseVisualStyleBackColor = true;

            SelectDatabase.Parent = tabPage;
            SelectDatabase.Location = new Point(108, 135);
            SelectDatabase.MouseClick += ComboBox1_MouseClick;
            SelectDatabase.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            SelectDatabase.TabIndex = 4;

            Password.Parent = tabPage;
            Password.Location = new Point(108, 109);
            Password.TabIndex = 3;

            UserName.Parent = tabPage;
            UserName.Location = new Point(108, 83);
            UserName.TabIndex = 2;

            Port.Parent = tabPage;
            Port.Location = new Point(108, 57);
            Port.TabIndex = 1;
            Port.KeyPress += TextBox2_KeyPress;

            Url.Parent = tabPage;
            Url.Location = new Point(108, 31);
            Url.TabIndex = 0;

            LabelDatabaseName.Parent = tabPage;
            LabelDatabaseName.AutoSize = true;
            LabelDatabaseName.Location = new Point(9, 138);
            LabelDatabaseName.Text = "Имя базы";

            LabelPassword.Parent = tabPage;
            LabelPassword.AutoSize = true;
            LabelPassword.Location = new Point(9, 112);
            LabelPassword.Text = "Пароль";

            LabelUserName.Parent = tabPage;
            LabelUserName.AutoSize = true;
            LabelUserName.Location = new Point(9, 86);
            LabelUserName.Text = "Логин";

            LabelPort.Parent = tabPage;
            LabelPort.AutoSize = true;
            LabelPort.Location = new Point(9, 60);
            LabelPort.Text = "Порт";

            LabelUrl.Parent = tabPage;
            LabelUrl.AutoSize = true;
            LabelUrl.Location = new Point(9, 34);
            LabelUrl.Text = "Адрес";

            Ok.Parent = tabPage;
            Ok.AutoSize = true;
            Ok.Location = new Point(9, 164);
            Ok.TabIndex = 5;
            Ok.Text = "Ок";
            Ok.Click += Ok_Click;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            ((sender as Button).Parent.Parent.Parent as Form).Close();
            ConnectionConfig.IP = Url.Text;
            ConnectionConfig.port = Convert.ToUInt32(Port.Text);
            ConnectionConfig.user = UserName.Text;
            ConnectionConfig.password = Password.Text;
            ConnectionConfig.database = SelectDatabase.SelectedItem.ToString();
            fs = new FileStream(ConfigFile, FileMode.Create);
            xmlsr.Serialize(fs, ConnectionConfig);
            fs.Close();
            updata();
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            Activate();
            e.Cancel = true;
            ((Form)sender).Hide();
            Enabled = true;
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        public void updata()
        {
            SQL.con.ConnectionString = mysqlCSB.ConnectionString;
            tabControl.TabPages.Clear();
            //zapros.Parent = tabControl;
            grups.Parent = tabControl;
            kab.Parent = tabControl;
            prepod.Parent = tabControl;
            for (int i = 0; i < r1.Length; i++)
            {
                if (r1[i] != null)
                {
                    r1[i].Dispose();
                }
                r1[i] = new Tab_rasp(i + 1) { Parent = tabControl, Text = (i + 1).ToString() + " курс" };
            }
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            настройка();
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

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About.Show();
            About.Activate();
            Enabled = false;
        }
    }
}