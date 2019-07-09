using Parser.Core;
using System;
using System.Windows.Forms;

namespace Parser
{
    public partial class FormMain : Form
    {
        ParserWorker parser;

        public FormMain()
        {
            InitializeComponent();

            parser = new ParserWorker(new Core.Parser());

            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNewData += Parser_OnNewData;
        }

        private void Parser_OnNewData(object arg1, string arg2)
        {
            ListTitles.Items.Add(arg2);
        }

        private void Parser_OnCompleted(object obj)
        {
            MessageBox.Show("All works done!");
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            parser.Start();
        }

        private void ButtonAbort_Click(object sender, EventArgs e)
        {
            parser.Abort();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

    }
}
