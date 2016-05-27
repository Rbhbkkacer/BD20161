using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BD20161
{
    public class Tab_kab_prep : TabPage
    {
        private DataGridView data = new DataGridView();
        private ContextMenuStrip menu = new ContextMenuStrip();
        private edit_kab_prep Form3;
        private string zapros;
        private string main_table;
        private List<edit_kab_prep._item> arr;
        private List<edit_kab_prep.reference> refer;
        private string[] par;

        public Tab_kab_prep(string zap, string m_t, string[] parrent, List<edit_kab_prep._item> z, List<edit_kab_prep.reference> x)
        {
            par = parrent;
            refer = x;
            arr = z;
            zapros = zap;
            main_table = m_t;
            data.Parent = this;
            data.Dock = DockStyle.Fill;
            data.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            data.MultiSelect = false;
            data.AutoSize = true;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            data.MouseClick += Data_MouseClick;
            data.CellMouseClick += Data_CellMouseClick;
            ParentChanged += Kabineti_ParentChanged;
            menu.Items.Add("Добавить");
            menu.Items.Add("Изменить");
            menu.Items.Add("Удалить");
            menu.ItemClicked += Menu_ItemClicked;
        }

        private void Data_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
            ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
        }

        private void Data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menu.Show((DataGridView)sender, new System.Drawing.Point(e.X, e.Y));
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parent.Parent.Enabled = true;
            Form3.FormClosing -= Form3_FormClosing;
            (Parent.Parent as Form).Activate();
            Kabineti_ParentChanged(sender, new EventArgs());
        }

        private void Kabineti_ParentChanged(object sender, EventArgs e)
        {
            data.DataSource = SQL.GetComments(zapros);
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form3 = new edit_kab_prep(main_table, arr, refer, par);
            switch (e.ClickedItem.Text)
            {
                case ("Изменить"):
                    if (data.SelectedRows.Count != 0)
                        foreach (DataGridViewCell item in data.SelectedRows[0].Cells)
                            if (item.FormattedValue.ToString() != "")
                            {
                                Form3.doit(data.SelectedRows[0]);
                                break;
                            }
                    goto default;

                case ("Удалить"):
                    foreach (DataGridViewRow item in data.SelectedRows)
                    {
                        SQL.GetComments(@"DELETE FROM `" + main_table + "` WHERE `" + main_table + "`.`" + SQL.GetComment(@"show columns from " + main_table + " where `Key` = 'PRI'") + "` = '" + item.Cells[0].Value.ToString() + "'");
                    }
                    Kabineti_ParentChanged(sender, new EventArgs());
                    break;

                default:
                    Form3.Show();
                    Form3.Activate();
                    Parent.Parent.Enabled = false;
                    Form3.FormClosing += Form3_FormClosing;
                    Form3._t.Tick += Kabineti_ParentChanged;
                    break;
            }
        }
    }
}