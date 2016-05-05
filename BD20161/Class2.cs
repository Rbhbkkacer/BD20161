using System;
using System.Data;
using System.Windows.Forms;

namespace BD20161
{
    public class Kabineti : TabPage
    {
        private DataTable dt;
        public DataGridView data = new DataGridView();
        public ContextMenuStrip menu = new ContextMenuStrip();

        public Kabineti()
        {
            data.Parent = this;
            data.Dock = DockStyle.Fill;
            data.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //data.ContextMenuStrip = menu;
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
            //if (e.Button == MouseButtons.Right)
            {
                ((DataGridView)sender).ClearSelection();
                ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
            }
        }

        private void Data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu.Show((DataGridView)sender, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void Kabineti_ParentChanged(object sender, EventArgs e)
        {
            dt = Form1.GetComments(@"select tip from tip_predmeta");
            data.DataSource = Form1.GetComments(@"select a.ID as '№', b.tips as 'Тип' from kabinet as a,tipkabineta_tip as b where a.tip_kabineta=b.ID ORDER BY a.ID");
        }
    }
}