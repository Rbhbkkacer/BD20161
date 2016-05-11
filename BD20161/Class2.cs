using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BD20161
{
    public class Kabineti : TabPage
    {
        public DataGridView data = new DataGridView();
        public ContextMenuStrip menu = new ContextMenuStrip();
        private Form3 Form3;
        private string zapros;
        private string main_table;
        private List<Form3._item> arr;
        private List<Form3.reference> refer;
        private string[] par;

        public Kabineti(string zap, string m_t, string[] parrent, List<Form3._item> z, List<Form3.reference> x)
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
            {
                menu.Show((DataGridView)sender, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Enabled = true;
            Form3.FormClosing -= Form3_FormClosing;
            (this.Parent.Parent as Form).Activate();
            Kabineti_ParentChanged(sender, new EventArgs());
        }

        private void Kabineti_ParentChanged(object sender, EventArgs e)
        {
            data.DataSource = Form1.GetComments(zapros);
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case ("Добавить"):
                    Form3 = new Form3(main_table, arr, refer, par);
                    Form3.Show();
                    Form3.Activate();
                    Enabled = false;
                    Form3.FormClosing += Form3_FormClosing;
                    Form3._t.Tick += Kabineti_ParentChanged;
                    break;

                case ("Изменить"):
                    Form3 = new Form3(main_table, arr, refer, par);
                    Form3.Show();
                    Form3.Activate();
                    Form3.doit(data.SelectedRows[0]);
                    Enabled = false;
                    Form3.FormClosing += Form3_FormClosing;
                    Form3._t.Tick += Kabineti_ParentChanged;
                    break;

                case ("Удалить"):
                    Form1.GetComments(@"DELETE FROM `" + main_table + "` WHERE `" + main_table + "`.`" + Form1.GetComments(@"show columns from " + main_table + " where `Key` = 'PRI'").Rows[0].ItemArray[0].ToString() + "` = '" + data.SelectedRows[0].Cells[0].Value.ToString() + "'");
                    Kabineti_ParentChanged(sender, new EventArgs());
                    break;

                default:
                    break;
            }
        }
    }
}