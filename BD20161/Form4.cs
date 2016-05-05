using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BD20161
{
    public partial class Form4 : Form
    {
        private List<TextBox> ltext = new List<TextBox>();
        private string pri;

        public void doit(DataGridViewRow row)
        {
            pri = Form1.GetComments(@"show columns from kabinet where `Key` = 'PRI'").Rows[0].ItemArray[0].ToString();

            foreach (DataGridViewCell item in row.Cells)
            {
                Label _0 = new Label();
                TextBox _1 = new TextBox();
                _0.Parent = this;
                _0.AutoSize = true;
                _0.Location = new Point(10, 10 + ltext.Count * 30);
                _0.Text = item.OwningColumn.Name;
                _1.Text = item.FormattedValue.ToString();
                ltext.Add(_1);
                ltext[ltext.Count - 1].Parent = this;
                ltext[ltext.Count - 1].Location = new Point(_0.Right, _0.Top);
            }
        }

        public Form4()
        {
            InitializeComponent();
        }
    }
}

//show columns from kabinet where `Key` = "PRI";
//UPDATE `kabinet` SET `Id` = '20', `tip_kabineta` = '5' WHERE `kabinet`.`Id` = 201