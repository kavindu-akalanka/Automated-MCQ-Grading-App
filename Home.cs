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

namespace Automated_MCQ_Grading_UI
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            PathAssist pathClass = new PathAssist();
            var imgPath = pathClass.MainPath() + "Answer Sheet Images";
            try
            {
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
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

        private void btnSetupQA_Click(object sender, EventArgs e)
        {
            frmSetupQA ob = (frmSetupQA)Application.OpenForms["frmSetupQA"];
            if (ob != null)
            {
                ob.Show();
            }
            else
            {
                ob = new frmSetupQA();
                ob.Show();
            }

        }

        
        private void btnStartMarking_Click(object sender, EventArgs e)
        {
            frmMarkingMCQ ob = (frmMarkingMCQ)Application.OpenForms["frmMarkingMCQ"];
            if (ob != null)
            {
                ob.Show();
            }
            else
            {
                ob = new frmMarkingMCQ();
                ob.Show();
            }
        }

        private void btnClearOldMarks_Click(object sender, EventArgs e)
        {
            try
            {
                PathAssist obj = new PathAssist();
                var csvDir = obj.MainPath() + "Results Sheet\\Grades.csv";
                File.WriteAllBytes(csvDir, new byte[] { 0 });
                File.WriteAllText(csvDir, "Registration_Number,Marks");

                MessageBox.Show("Records Cleared!", "Done");
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

        private void btnClearAnsSheets_Click(object sender, EventArgs e)
        {
            try
            {
                PathAssist ob = new PathAssist();
                string path = ob.MainPath() + "Answer Sheet Images";
                string[] filePaths = Directory.GetFiles(path);
                if (filePaths.Length > 0)
                {
                    foreach (string filePath in filePaths)
                    {
                        File.Delete(filePath);
                    }
                    MessageBox.Show("Files Cleared", "Done");
                }
                else
                {
                    MessageBox.Show("Folder is already empty!", "Alert");
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

        private void btnOpenGradingCSV_Click_1(object sender, EventArgs e)
        {
            try
            {
                PathAssist ob = new PathAssist();

                var destDir = ob.MainPath() + "Results Sheet";
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

        private void frmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
