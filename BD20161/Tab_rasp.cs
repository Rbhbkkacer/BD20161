using System;
using System.Data;
using System.Windows.Forms;

namespace BD20161
{
    internal class Tab_rasp : TabPage
    {
        public TabControl control = new TabControl() { Dock = DockStyle.Fill };
        public grouptab[] _0;

        public Tab_rasp(int curse)
        {
            control.Parent = this;
            int count = Convert.ToInt32(SQL.GetComment("select count(*) from groups where id like 'Б0" + (curse * 2 - 1).ToString() + "%'"));
            _0 = new grouptab[count];
            for (int i = 0; i < _0.Length; i++)
            {
                _0[i] = new grouptab(SQL.GetComments("select id from groups where id like 'Б0" + (curse * 2 - 1).ToString() + "%'").Rows[i].ItemArray[0].ToString()) { Parent = control };
            }
        }
    }

    internal class grouptab : TabPage
    {
        public edit_rasp f;
        public DataGridView data = new DataGridView() { Dock = DockStyle.Fill };

        public grouptab(string grup)
        {
            Text = grup;
            data.ColumnCount = 7;
            data.RowCount = 6;
            for (int _0 = 0; _0 < data.ColumnCount; _0++)
            {
                data.Columns[_0].HeaderText = new DateTime(2016, 2, _0 + 1).ToString("dddd");
                for (int _1 = 0; _1 < data.RowCount; _1++)
                {
                    if (data.Rows[_1].HeaderCell.Value == null)
                    {
                        data.Rows[_1].HeaderCell.Value = DateTime.Parse(SQL.GetComment("select Time from time LIMIT 1 OFFSET " + _1.ToString())).ToShortTimeString();
                    }
                    data.Rows[_1].Cells[_0] = new FakeDataGridViewCell();
                    data.Rows[_1].Cells[_0].ReadOnly = true;
                    string Para = SQL.GetComment("select Para from main where dow='" + (_0 + 1) + "' && time='" + (_1 + 1) + "' && Gruppa='" + Text + "'");
                    if (Para != "")
                    {
                        DataTable dt = SQL.GetComments("select * from para where Id='" + Para + "'");
                        using (FakeDataGridViewCell d = (FakeDataGridViewCell)data.Rows[_1].Cells[_0])
                        {
                            d.Predmet = dt.Rows[0].ItemArray[2].ToString();
                            d.Teacher = dt.Rows[0].ItemArray[1].ToString();
                            d.Tip_par = dt.Rows[0].ItemArray[3].ToString();
                            d.Cabinet = dt.Rows[0].ItemArray[4].ToString();
                            d.Value = d.Predmet + ", " + d.Cabinet;
                        }
                    }
                }
            }
            data.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            data.Parent = this;
            data.CellDoubleClick += Data_CellDoubleClick;
        }

        private void Data_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            f = new edit_rasp((sender as DataGridView).SelectedCells[0] as FakeDataGridViewCell);
            Parent.Parent.Parent.Parent.Enabled = false;
            f.Show();
            f.FormClosed += F_FormClosed;
        }

        private void F_FormClosed(object sender, FormClosedEventArgs e)
        {
            Parent.Parent.Parent.Parent.Enabled = true;
            f.Dispose();
        }
    }

    public class FakeDataGridViewCell : DataGridViewTextBoxCell
    {
        public string Predmet = "";
        public string Teacher = "";
        public string Tip_par = "";
        public string Cabinet = "";

        public FakeDataGridViewCell()
        {
            if (Value == null)
            {
                Value = "";
            }
        }
    }
}