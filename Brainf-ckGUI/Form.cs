using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Brainf_ckGUI
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The name and version of this software
        /// </summary>
        const string about = "Brainf-ck GUI v1.0";
        /// <summary>
        /// If the text is changed
        /// </summary>
        static bool isChanged = false;
        /// <summary>
        /// Path to interpreter
        /// </summary>
        static string intPath =
            Application.StartupPath + "\\Brainf-ckInterpreter.exe";
        /// <summary>
        /// Path to temporary file
        /// </summary>
        static string tempPath = Application.StartupPath + "\\temp.bf";

        /// <summary>
        /// Set title when form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            this.Text = about;
            this.Form_Resize(sender, e);
        }

        /// <summary>
        /// Resize rich textbox when the form is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Resize(object sender, EventArgs e)
        {
            richTextBox.Size = new Size(
                this.Size.Width - 41,
                this.Size.Height - 80
            );
        }

        /// <summary>
        /// Check if script is saved then exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            // TODO
            if (
                isChanged &&
                MessageBox.Show(
                    "You have not saved your work, do you want " +
                    "to exit anyway? ",
                    about,
                    MessageBoxButtons.YesNo
                ) == DialogResult.No
            )
            {
                e.Cancel = true;
            }
            */
        }

        /// <summary>
        /// Open file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Start interpreter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(intPath))
            {
                try
                {
                    // Save temporary script
                    File.WriteAllText(tempPath, richTextBox.Text);
                    // Start interpreter
                    Process p = new Process();
                    p.StartInfo.FileName = intPath;
                    p.StartInfo.Arguments = "/f \"" + tempPath + "\"";
                    p.Start();
                }
                catch
                {
                    MessageBox.Show(
                        "Failed to start the interpreter, please try again. ",
                        about
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Cannot find the interpreter, please try to reinstall " +
                    "this software. ",
                    about
                );
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(about, about);
        }

        /// <summary>
        /// Rich textbox content changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;
        }
    }
}
