using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BD20161
{
    public partial class edit_rasp : Form
    {
        private ComboBox Combo1 = new ComboBox();
        private ComboBox Combo2 = new ComboBox();
        private ComboBox Combo3 = new ComboBox();
        private ComboBox Combo4 = new ComboBox();
        private string Predmet;
        private string Teacher;
        private string Cabinet;
        private string Tip_par;
        private Button ok = new Button() { Text = "Ok" };
        private FakeDataGridViewCell cell;

        public edit_rasp(FakeDataGridViewCell sender)
        {
            InitializeComponent();
            cell = sender;
            Predmet = cell.Predmet;
            Teacher = cell.Teacher;
            Cabinet = cell.Cabinet;
            Tip_par = cell.Tip_par;
            Combo1.Parent = this;
            Combo1.Location = new Point(10, 10);
            Combo1.SelectedIndexChanged += Combo1_SelectedIndexChanged;
            Combo2.Parent = this;
            Combo2.Location = new Point(10, Combo1.Bottom + 10);
            Combo2.SelectedIndexChanged += Combo2_SelectedIndexChanged;
            Combo3.Parent = this;
            Combo3.Location = new Point(10, Combo2.Bottom + 10);
            Combo3.SelectedIndexChanged += Combo3_SelectedIndexChanged;
            Combo4.Parent = this;
            Combo4.Location = new Point(10, Combo3.Bottom + 10);
            foreach (DataRow item in SQL.GetComments(@"select * from predmet").Rows)
            {
                Combo1.Items.Add(item.ItemArray[0].ToString());
            }
            Combo1.SelectedItem = cell.Predmet;
            ok.Parent = this;
            ok.Location = new Point(10, Combo4.Bottom + 10);
            ok.Click += Ok_Click;
            //cell.OwningRow.HeaderCell;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            cell.Predmet = Combo1.SelectedItem.ToString();
            cell.Teacher = Combo2.SelectedItem.ToString();
            cell.Tip_par = Combo3.SelectedItem.ToString();
            cell.Cabinet = Combo4.SelectedItem.ToString();
            cell.Value = cell.Predmet + ", " + cell.Cabinet;
            string IdPara = SQL.GetComment("select Id from para where Predmet='" + Combo1.SelectedItem + "' && Prepod='" + Combo2.SelectedItem + "' && Tip='" + Combo3.SelectedItem + "' && Kabinet='" + Combo4.SelectedItem + "'");
            if (IdPara == "")
            {
                SQL.GetComments("INSERT INTO `para` (`Prepod`,`Predmet`,`Tip`,`Kabinet`) VALUES('" + Combo2.SelectedItem + "','" + Combo1.SelectedItem + "','" + Combo3.SelectedItem + "','" + Combo4.SelectedItem + "')");
                Ok_Click(sender, e);
                return;
            }
            //cell.OwningRow.HeaderCell==new DateTime(2016, 2, _0 + 1).ToString("dddd");
            //int dow = Convert.ToInt32(SQL.GetComment("select dow from main where dow = '" + (cell.OwningColumn.Index + 1) + "'  time='" + (cell.OwningRow.Index + 1) + "'"));
            if (SQL.GetComment("select dow from main where dow = '" + (cell.OwningColumn.Index + 1) + "' && time='" + (cell.OwningRow.Index + 1) + "' && Gruppa='" + (cell.DataGridView.Parent as TabPage).Text + "'") == "")
            {
                SQL.GetComments("INSERT INTO `main` (`dow`,`time`,`Gruppa`,`Para`) VALUES('" + (cell.OwningColumn.Index + 1) + "','" + (cell.OwningRow.Index + 1) + "','" + (cell.DataGridView.Parent as TabPage).Text + "','" + IdPara + "')");
                Ok_Click(sender, e);
                return;
            }
            else
            {
                //int dow = Convert.ToInt32(SQL.GetComment("select dow from main where dow = '" + (cell.OwningColumn.Index + 1) + "' && time='" + (cell.OwningRow.Index + 1) + "'"));
                SQL.GetComments("UPDATE `main` SET Para='" + IdPara + "' where dow='" + (cell.OwningColumn.Index + 1) + "' && time='" + (cell.OwningRow.Index + 1) + "' && Gruppa='" + (cell.DataGridView.Parent as TabPage).Text + "'");
            }
            ((sender as Button).Parent as Form).Close();
        }

        private void Combo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combo2.Items.Clear();
            //Combo2.Text = "";
            foreach (DataRow item in SQL.GetComments(@"select prepod from prepodovateli where (select predmets_group.Id from predmets_group where predmets_group.predmets like '%" + Combo1.SelectedItem + "%' && predmets_group.Id=prepodovateli.predmrts)=predmrts").Rows)
            {
                Combo2.Items.Add(item.ItemArray[0].ToString());
            }
            Combo2.SelectedItem = cell.Teacher;
        }

        private void Combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combo3.Items.Clear();
            //Combo3.Text = "";
            foreach (DataRow item in SQL.GetComments(@"select tip from tip_predmeta as a, prepodovateli as b, tipkabineta_tip as c where c.Id=b.Id_Tips && b.Prepod='" + Combo2.SelectedItem + "' && c.tips like CONCAT('%', tip, '%')").Rows)
            {
                Combo3.Items.Add(item.ItemArray[0].ToString());
            }
            Combo3.SelectedItem = cell.Tip_par;
        }

        private void Combo3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combo4.Items.Clear();
            //Combo4.Text = "";
            foreach (DataRow item in SQL.GetComments(@"select a.Id from kabinet as a, tipkabineta_tip as b where b.Id=a.tip_kabineta && b.tips like '%" + Combo3.SelectedItem + "%'").Rows)
            {
                Combo4.Items.Add(item.ItemArray[0].ToString());
            }
            Combo4.SelectedItem = cell.Cabinet;
        }
    }
}