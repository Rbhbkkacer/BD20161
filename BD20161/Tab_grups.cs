using System;
using System.Drawing;
using System.Windows.Forms;

namespace BD20161
{
    internal class Tab_grups : TabPage
    {
        private DataGridView data = new DataGridView();
        private ContextMenuStrip menu = new ContextMenuStrip();
        public Form Form = new Form();
        private TextBox Text = new TextBox();
        private Button Ok = new Button();

        public Tab_grups()
        {
            data.Parent = this;
            data.Dock = DockStyle.Fill;
            data.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            data.MultiSelect = false;
            data.AutoSize = true;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            data.MouseClick += DataMouseClick;
            data.CellMouseClick += DataCellMouseClick;
            ParentChanged += TabParentChanged;
            menu.Items.Add("Добавить");
            menu.Items.Add("Удалить");
            menu.ItemClicked += MenuItemClicked;
        }

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //Form3 = new edit_kab_prep(main_table, arr, refer, par);
            switch (e.ClickedItem.Text)
            {
                case ("Удалить"):
                    foreach (DataGridViewRow item in data.SelectedRows)
                    {
                        SQL.GetComments(@"DELETE FROM `groups` WHERE `groups`.`Id` = '" + item.Cells[0].Value.ToString() + "'");
                    }
                    TabParentChanged(sender, new EventArgs());
                    break;

                default:

                    Text.Parent = Form;
                    Text.Location = new Point(10, 10);
                    Text.TabIndex = 2;
                    Text.KeyPress += Text_KeyPress;

                    Ok.Parent = Form;
                    Ok.AutoSize = true;
                    Ok.Location = new Point(10, 34);
                    Ok.TabIndex = 5;
                    Ok.Text = "Ок";
                    Ok.Click += OkClick;

                    Form.Size = new Size(100, 100);
                    Form.Show();
                    Form.Activate();
                    Parent.Parent.Enabled = false;
                    break;
            }
        }

        private void Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                OkClick(Ok, new EventArgs());
            }
        }

        private void OkClick(object sender, EventArgs e)
        {
            SQL.GetComments("INSERT INTO `groups` (`Id`) VALUES ('" + Text.Text + "')");
            ((sender as Button).Parent as Form).Close();
            TabParentChanged(sender, e);
        }

        private void DataCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
            ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
        }

        private void DataMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menu.Show((DataGridView)sender, new Point(e.X, e.Y));
        }

        private void TabParentChanged(object sender, EventArgs e)
        {
            data.DataSource = SQL.GetComments("select * from groups");
        }
    }
}