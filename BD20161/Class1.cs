using System.Windows.Forms;

namespace BD20161
{
    public class Tab_zapros : TabPage
    {
        public TextBox textBox = new TextBox();
        public Button button = new Button();
        public DataGridView data = new DataGridView();
        public SplitContainer split = new SplitContainer();

        public Tab_zapros()
        {
            Text = "Запрос";
            split.Parent = this;
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Horizontal;
            split.SplitterMoved += Split_SplitterMoved;
            split.IsSplitterFixed = false;
            textBox.Parent = split.Panel1;
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Top;
            button.Parent = split.Panel1;
            button.Text = "Отправить";
            button.Dock = DockStyle.Bottom;
            data.Parent = split.Panel2;
            data.Dock = DockStyle.Fill;
            data.ReadOnly = true;
        }

        private void Split_SplitterMoved(object sender, SplitterEventArgs e)
        {
            textBox.Height = split.Panel1.Height - button.Height;
            button.Top = textBox.Height;
        }
    }
}