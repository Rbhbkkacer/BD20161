using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BD20161
{
    public partial class edit_kab_prep : Form
    {
        public Timer _t = new Timer();
        private Label _2 = new Label();
        private bool _new = true;
        private TextBox kabinet = new TextBox();
        private Button ok = new Button();
        private List<CheckBox> tips = new List<CheckBox>();
        private string[] parrant_table;
        private Label[] Lable_array;
        private List<List<object>> mas = new List<List<object>>();
        private List<_item> Lab;
        private List<reference> refer;
        private DataGridViewRow _row = new DataGridViewRow();
        private string main_table;

        public struct _item
        {
            public string name_in_database;
            public string name;
            public string table_name;
            public int n_tip;
        }

        public struct reference
        {
            public string name_in_database;
            public string table_name;
            public string ref_name_in_database;
            public string ref_table_name;
        }

        public edit_kab_prep(string m_t, List<_item> Lables, List<reference> reflo, string[] par)
        {
            InitializeComponent();

            int i = 0;
            Lable_array = new Label[Lables.Count];
            foreach (_item item0 in Lables)
            {
                Lable_array[i] = new Label();
                Lable_array[i].Parent = this;
                Lable_array[i].Text = item0.name;
                Lable_array[i].AutoSize = true;
                Lable_array[i].Location = new Point(10 + i * 110, 10);
                mas.Add(new List<object>());
                mas[i].Clear();
                switch (item0.n_tip)
                {
                    case (0):
                        mas[i].Add(new TextBox());
                        ((TextBox)mas[i][0]).Parent = this;
                        ((TextBox)mas[i][0]).Location = new Point(Lable_array[i].Left, Lable_array[i].Bottom);
                        break;

                    case (1):
                        foreach (DataRow item1 in SQL.GetComments(@"select " + item0.name_in_database + " from " + item0.table_name).Rows)
                        {
                            mas[i].Add(new CheckBox());
                            ((CheckBox)mas[i][mas[i].Count - 1]).Parent = this;
                            ((CheckBox)mas[i][mas[i].Count - 1]).Text = item1.ItemArray[0].ToString();
                            ((CheckBox)mas[i][mas[i].Count - 1]).Location = new Point(Lable_array[i].Left, Lable_array[i].Bottom + 20 * (mas[i].Count - 1));
                        }
                        break;

                    case (2):
                        mas[i].Add(new ComboBox());
                        ((ComboBox)mas[i][0]).Parent = this;
                        ((ComboBox)mas[i][0]).Location = new Point(Lable_array[i].Left, Lable_array[i].Bottom);
                        foreach (DataRow item1 in SQL.GetComments(@"select " + item0.name_in_database + " from " + item0.table_name).Rows)
                        {
                            ((ComboBox)mas[i][0]).Items.Add(item1.ItemArray[0].ToString());
                        }
                        break;

                    default:
                        break;
                }
                i++;
            }

            main_table = m_t;
            Lab = Lables;
            refer = reflo;
            parrant_table = par;

            ok.Parent = this;
            ok.Text = "Ok";
            ok.Click += Ok_Click;
            int max = 0;
            foreach (var item0 in mas)
            {
                try
                {
                    if (((CheckBox)item0[0]) != null)
                    {
                        if (max < item0.Count)
                        {
                            max = item0.Count;
                            ok.Location = new Point((Width - ok.Width) / 2, ((CheckBox)item0[item0.Count - 1]).Bottom);
                        }
                    }
                }
                catch { }
            }

            _t.Tick += _t_Tick;
            _t.Interval = 1000;
            _2.Parent = this;
            _2.AutoSize = true;
            _2.Location = new Point(ok.Right + 10, ok.Top);
        }

        public void doit(DataGridViewRow row)
        {
            /*foreach (DataGridViewCell item in row.Cells)
            {
            }*/
            _row = row;
            for (int i = 0; i < Lab.Count; i++)
            {
                switch (Lab[i].n_tip)
                {
                    case (0):
                        ((TextBox)mas[i][0]).Enabled = false;
                        ((TextBox)mas[i][0]).Text = _row.Cells[i].FormattedValue.ToString();
                        break;

                    case (1):
                        foreach (CheckBox item in mas[i])
                        {
                            if (row.Cells[i].Value.ToString().Contains(item.Text))
                            {
                                item.Checked = true;
                            }
                            else
                            {
                                item.Checked = false;
                            }
                        }
                        break;

                    case (2):
                        foreach (DataRow item in SQL.GetComments("").Rows)
                        {
                            ((ComboBox)mas[i][0]).Items.Add(item.ItemArray[0].ToString());
                        }
                        break;

                    default:
                        break;
                }
            }
            _new = false;
        }

        private void _t_Tick(object sender, EventArgs e)
        {
            _2.Text = "";
            _t.Enabled = false;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            //for (int _i = 1; _i < parrant_table.Length; _i++)
            {
                DataTable dt;
                bool good = false;
                string s = "";
                string com;
                if (_new)
                {
                    com = @"INSERT INTO `" + main_table + "` (`";
                    DataTable td = SQL.GetComments(@"show columns from " + main_table);
                    for (int i = 0; i < td.Rows.Count; i++)
                    {
                        com += td.Rows[i].ItemArray[0].ToString();
                        if (i < (td.Rows.Count - 1))
                        {
                            com += "`, `";
                        }
                        else
                        {
                            com += "`) VALUES ('";
                        }
                    }
                    for (int i = 0; i < Lab.Count; i++)
                    {
                        dt = SQL.GetComments(@"select * from " + parrant_table[i]);
                        switch (Lab[i].n_tip)
                        {
                            case (0):
                                com += ((TextBox)mas[i][0]).Text;
                                break;

                            case (1):
                                s = "";
                                good = false;
                                foreach (CheckBox item in mas[i])
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
                                        com += item.ItemArray[0].ToString();
                                        good = true;
                                        break;
                                    }
                                }
                                if (!good)
                                {
                                    SQL.GetComments(@"INSERT INTO `" + parrant_table[i] + "` (`" + dt.Columns[1].ColumnName + "`) VALUES ('" + s + "')");
                                    Ok_Click(sender, e);
                                }
                                break;

                            default:
                                break;
                        }
                        if (i < (td.Rows.Count - 1))
                        {
                            com += "', '";
                        }
                        else
                        {
                            com += "')";
                        }
                    }
                }
                else
                {
                    com = @"UPDATE `" + main_table + "` SET `";
                    DataTable td = SQL.GetComments(@"show columns from " + main_table);
                    for (int i = 1; i < Lab.Count; i++)
                    {
                        dt = SQL.GetComments(@"select * from " + parrant_table[i]);
                        com += td.Rows[i].ItemArray[0].ToString() + "` = ";
                        switch (Lab[i].n_tip)
                        {
                            case (0):
                                com += ((TextBox)mas[i][0]).Text;
                                break;

                            case (1):
                                s = "";
                                good = false;
                                foreach (CheckBox item in mas[i])
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
                                        com += item.ItemArray[0].ToString();
                                        good = true;
                                        break;
                                    }
                                }
                                if (!good)
                                {
                                    SQL.GetComments(@"INSERT INTO `" + parrant_table[i] + "` (`" + dt.Columns[1].ColumnName + "`) VALUES ('" + s + "')");
                                    Ok_Click(sender, e);
                                }
                                break;

                            default:
                                break;
                        }
                        if (i < (td.Rows.Count - 1))
                        {
                            com += ", `";
                        }
                        else
                        {
                            com += " WHERE `";
                        }
                    }
                    com += main_table + "`.`" + SQL.GetComment(@"show columns from " + main_table + " where `Key` = 'PRI'") + "` = '" + ((TextBox)mas[0][0]).Text + "'";
                }
                if (good)
                {
                    SQL.GetComments(com);
                    _2.Text = "Готово";
                    _t.Enabled = true;
                }
            }
        }
    }
}