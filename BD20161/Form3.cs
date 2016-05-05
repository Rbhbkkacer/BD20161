using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BD20161
{
    public partial class Form3 : Form
    {
        public static string oldid;
        private bool _new = true;
        private Timer _t = new Timer();
        private TextBox kabinet = new TextBox();
        private List<CheckBox> tips = new List<CheckBox>();
        private Button ok = new Button();
        private Label _0 = new Label();
        private Label _1 = new Label();
        private Label _2 = new Label();

        public Form3()
        {
            InitializeComponent();
            Activated += Form3_Activated;
            _0.Parent = this;
            _0.Text = "№";
            _0.AutoSize = true;
            _0.Location = new Point(10, 10);
            kabinet.Parent = this;
            kabinet.Location = new Point(_0.Left, _0.Bottom);
            _1.Parent = this;
            _1.Text = "Тип пары";
            _1.AutoSize = true;
            _1.Location = new Point(kabinet.Right + 10, _0.Top);
            ok.Parent = this;
            ok.Text = "Ok";
            ok.Click += Ok_Click;
            _t.Tick += _t_Tick;
            _t.Interval = 1000;
            _2.Parent = this;
            _2.AutoSize = true;
        }

        public void doit(DataGridViewRow row)
        {
            /*foreach (DataGridViewCell item in row.Cells)
            {
            }*/
            kabinet.Text = row.Cells[0].Value.ToString();
            foreach (CheckBox item in tips)
            {
                if (row.Cells[1].Value.ToString().Contains(item.Text))
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }
            oldid = kabinet.Text;
            _new = false;
        }

        private void _t_Tick(object sender, EventArgs e)
        {
            _2.Text = "";
            _t.Enabled = false;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            DataTable dt;
            bool good = false;
            dt = Form1.GetComments(@"select * from tipkabineta_tip");
            string s = "";
            foreach (CheckBox item in tips)
            {
                if (item.Checked)
                {
                    s += s == "" ? "" : ", ";
                    s += item.Text;
                }
            }
            foreach (DataRow item in dt.Rows)
            {
                if (s == item.ItemArray[1].ToString())
                {
                    if (_new)
                    {
                        Form1.GetComments(@"INSERT INTO `kabinet` (`Id`, `tip_kabineta`) VALUES ('" + kabinet.Text + "', '" + item.ItemArray[0].ToString() + "')");
                    }
                    else
                    {
                        Form1.GetComments(@"UPDATE `kabinet` SET `Id` = '" + kabinet.Text + "', `tip_kabineta` = '" + item.ItemArray[0].ToString() + "' WHERE `kabinet`.`Id` = " + kabinet.Text);
                    }
                    good = true;
                    break;
                }
            }
            if (!good)
            {
                Form1.GetComments(@"INSERT INTO `tipkabineta_tip` (`" + dt.Columns[1].ColumnName + "`) VALUES ('" + s + "')");
                Ok_Click(sender, e);
            }
            _2.Text = "Готово";
            _t.Enabled = true;
        }

        private void Form3_Activated(object sender, EventArgs e)
        {
            if (tips.Count == 0)
            {
                foreach (DataRow item in Form1.GetComments(@"select tip from tip_predmeta").Rows)
                {
                    tips.Add(new CheckBox());
                    tips[tips.Count - 1].Parent = this;
                    tips[tips.Count - 1].Text = item.ItemArray[0].ToString();
                    tips[tips.Count - 1].Location = new Point(_1.Left, _1.Bottom + 20 * (tips.Count - 1));
                }
                ok.Location = new Point((Width - ok.Width) / 2, tips[tips.Count - 1].Bottom);
                _2.Location = new Point(ok.Right + 10, ok.Top);
                Height = ok.Bottom + 40;
            }
        }
    }
}