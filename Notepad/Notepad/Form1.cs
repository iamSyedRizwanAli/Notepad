using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Form1 : Form
    {
        private string box_text = "";
        private string file_name = "Untitled";
        private string sw_name = " - Notepad";
        private bool changed = false;
        private bool external_file_opened = false;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fd.Font;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveWork();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (changed)
                LeavingWithoutSaving();
            else
                this.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = file_name + sw_name;
            richTextBox1.AcceptsTab = true;
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!changed && !external_file_opened)
            {
                changed = true;
                this.Text = file_name + "*" + sw_name;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                DialogResult response = MessageBox.Show("Do you want to save your work?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (response == DialogResult.Yes)
                    SaveWork();
                else if (response == DialogResult.No)
                    openFileDialogCore();
            }
            else
                openFileDialogCore();
        }

        private void openFileDialogCore()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File (.txt)|*.txt";
            openFileDialog.Title = "Open File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                external_file_opened = true;
                StreamReader reader = new StreamReader(openFileDialog.FileName);
                String content = reader.ReadToEnd();
                richTextBox1.Text = content;
                reader.Dispose();
                external_file_opened = false;

                file_name = openFileDialog.SafeFileName;
                this.Text = file_name + sw_name;
            }
        }

        private void SaveWork()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Text File (.txt)|*.txt";
            sfd.Title = "Save work";
            sfd.FileName = file_name;

            box_text = richTextBox1.Text;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                FileStream fstream = File.Create(path);
                StreamWriter writer = new StreamWriter(fstream);
                string[] splittedText = box_text.Split('\n');

                foreach (string str in splittedText)
                    writer.WriteLine(str);

                writer.Dispose();

                file_name = path.Split('\\').Last();
                this.Text = file_name + sw_name;
                changed = false;
            }
        }

        private void LeavingWithoutSaving()
        {
            DialogResult response = MessageBox.Show("Do you want to save your work?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (response == DialogResult.Yes)
            {
                SaveWork();
                this.Dispose();
            }
            else if (response == DialogResult.No)
            {
                this.Dispose();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(changed)
                LeavingWithoutSaving();
        }
    }
}
