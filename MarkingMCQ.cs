using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Automated_MCQ_Grading_UI
{
    public partial class frmMarkingMCQ : Form
    {
        int fileCount = 0;
        string[] pathListGlob;
        public frmMarkingMCQ()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectImages_Click(object sender, EventArgs e)
        {
            try
            {
                lblPaths.ForeColor = Color.White;
                lblPaths.BackColor = Color.Transparent;
                lblPaths.Anchor = AnchorStyles.None;
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;)| *.jpg; *.jpeg; *.gif; *.bmp;";
                open.Multiselect = true;
                string[] pathList = new string[open.FileNames.Length];
                if (open.ShowDialog() == DialogResult.OK)
                {
                    fileCount += open.FileNames.Length;
                    grpBoxFileNames.Text = fileCount.ToString() + " File(s) selected";
                    pathList = open.FileNames;
                }

                foreach (string path in pathList)
                {
                    lblPaths.Text += (path + "\n\n");
                }

                pathListGlob = pathList;
            }
            catch(Exception ex)
            {
                if (ex.ToString().Contains("UnauthorizedAccessException"))
                {
                    exceptionThrow("Task Aborted!\nUnauthorizedAccessException\nTry Running Your Program as an Administrator!");
                }
                else
                {
                    exceptionThrow(ex.Message);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                PathAssist ob = new PathAssist();
                var venv = ob.MainPath() + "venv\\Scripts\\python.exe";

                // Create process info
                var psi = new ProcessStartInfo();
                psi.FileName = venv;

                // Provide scripts and args
                var script = ob.MainPath() + "MCQ_Grading.py";
                psi.Arguments = $"\"{script}\"";

                // Process configuration
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                // Starting the process and reading errors and results
                var process = Process.Start(psi);
                var errors = process.StandardError.ReadToEnd();
                var results = process.StandardOutput.ReadToEnd();

                Console.WriteLine("Errors : \n{0}", errors);
                Console.WriteLine();
                Console.WriteLine("Results : \n{0}", results);
                Console.WriteLine();


                DialogResult res = MessageBox.Show("Answer Sheet Marking Finished!\nClick OK to Find Results Sheet.", "Done", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    var destDir = ob.MainPath() + "Results Sheet";
                    Process.Start("explorer.exe", destDir);
                }
            }
            catch(Exception ex)
            {
                if (ex.ToString().Contains("UnauthorizedAccessException"))
                {
                    exceptionThrow("Task Aborted!\nUnauthorizedAccessException\nTry Running Your Program as an Administrator!");
                }
                else
                {
                    exceptionThrow(ex.Message);
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PathAssist pName = new PathAssist();
                string dirPath = pName.MainPath() + "Answer Sheet Images";
                var dest = dirPath;

                foreach (string path in pathListGlob)
                {
                    File.Copy(path, Path.Combine(dest, Path.GetFileName(path)), true);
                }
                MessageBox.Show("Images saved to " + dest, "Done");
            }
            catch(Exception ex)
            {
                if (ex.ToString().Contains("UnauthorizedAccessException"))
                {
                    exceptionThrow("Task Aborted!\nUnauthorizedAccessException\nTry Running Your Program as an Administrator!");
                }
                else
                {
                    exceptionThrow(ex.Message);
                }
            }
        }

        private void btnImageLoc_Click(object sender, EventArgs e)
        {
            try
            {
                PathAssist ob = new PathAssist();
                var destDir = ob.MainPath() + "Answer Sheet Images";
                Process.Start("explorer.exe", destDir);
            }
            catch(Exception ex)
            {
                if (ex.ToString().Contains("UnauthorizedAccessException"))
                {
                    exceptionThrow("Task Aborted!\nUnauthorizedAccessException\nTry Running Your Program as an Administrator!");
                }
                else
                {
                    exceptionThrow(ex.Message);
                }
            }
        }

        private void exceptionThrow(string ex)
        {
            MessageBox.Show(ex, "Warning");
        }
    }
}
