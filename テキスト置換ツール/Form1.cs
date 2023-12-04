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

namespace テキスト置換ツール
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            var listWords = new List<(string, string)>();
            var words = tbSource.Text.Split(new string[] {Environment.NewLine },StringSplitOptions.None);
            foreach(var word in words)
            {
                var split = word.Split('\t');
                if (split.Length != 2) continue;
                listWords.Add((split[0],split[1]));
            }

            var files = Directory.GetFiles(tbDirectory.Text);
            var new_directory = tbDirectory.Text + "\\" + "置換後";
            if (!Directory.Exists(new_directory))
            {
                Directory.CreateDirectory(new_directory);
            }
            foreach (var file in files)
            {
                var fileinfo = new FileInfo(file);
                using (var sw = new StreamWriter(new_directory + "\\" + fileinfo.Name, true, Encoding.GetEncoding("Shift_Jis")))
                using (var sr = new StreamReader(file, Encoding.GetEncoding("Shift_Jis")))
                {
                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine();
                        foreach (var word in listWords)
                        {
                            line = line.Replace(word.Item1, word.Item2);
                        }
                        sw.WriteLine(line);
                    }
                }
            }
            MessageBox.Show("Done");
        }
    }
}
