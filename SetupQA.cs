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

namespace Automated_MCQ_Grading_UI
{
    public partial class frmSetupQA : Form
    {
        string[] ans = { "A", "B", "C", "D", "E" };
        TextBox[] publ;
        int nApub, nQpub;
        public frmSetupQA()
        {
            InitializeComponent();
        }

        private void btnAddQAn_Click(object sender, EventArgs e)
        {
            try
            {
                int nQ = 0, nA = 0, y = 130;
                if (cmbBxQuestionN.Text != "Select..." && cmbBxAnswersN.Text != "Select...")
                {
                    nQ = Convert.ToInt32(cmbBxQuestionN.Text);
                    nA = Convert.ToInt32(cmbBxAnswersN.Text);
                    nApub = nA;
                    nQpub = nQ;
                    int[] ansIndex = new int[nA];

                    TextBox[] txtBxQA = new TextBox[nQ];
                    if (nA > 0 && nQ > 0)
                    {
                        string rc = nQ.ToString() + "," + nA.ToString();
                        PathAssist ob = new PathAssist();
                        ob.sendArray(rc, "rcCount.txt");

                        Label[] label = new Label[nQ];
                        for (int i = 0; i < txtBxQA.Length; i++)
                        {
                            txtBxQA[i] = new TextBox();
                            label[i] = new Label();

                            txtBxQA[i].Location = new System.Drawing.Point(300, y);
                            label[i].Location = new Point(225, y);

                            txtBxQA[i].Size = new Size(100, 50);
                            txtBxQA[i].Name = "txtbx" + i.ToString();
                            txtBxQA[i].Text = "";
                            txtBxQA[i].Anchor = AnchorStyles.None;

                            label[i].Text = "Question " + (i + 1) + " :";
                            label[i].BackColor = Color.Transparent;
                            label[i].ForeColor = Color.White;
                            label[i].Anchor = AnchorStyles.None;

                            this.Controls.Add(txtBxQA[i]);
                            this.Controls.Add(label[i]);
                            this.ResumeLayout(false);
                            y += 30;
                            this.Refresh();
                        }
                        publ = txtBxQA;
                    }
                    else { }
                }
                else
                {
                    MessageBox.Show("Please select valid values!", "Error");
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

        public int getAnsIndex(string x, int n)
        {
            for(int i = 0; i < n; i++)
            {
                if (x.ToUpper() == ans[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnConfirmSetup_Click(object sender, EventArgs e)
        {
            try
            {
                int[] ansIndex = new int[nQpub];
                for (int i = 0; i < nQpub; i++)
                {
                    int idx = getAnsIndex(publ[i].Text, nApub);
                    if (idx >= 0)
                    {
                        ansIndex[i] = idx;
                    }
                    else
                    {
                        MessageBox.Show(("Invalid answer range in : Question " + (i + 1)), "Error");
                    }
                }

                string finalAns = "";
                for (int i = 0; i < nQpub; i++)
                {

                    if (i == nQpub - 1)
                    {
                        finalAns += ansIndex[i].ToString();
                    }
                    else
                    {
                        finalAns += ansIndex[i].ToString() + "\n";
                    }
                }

                PathAssist ob = new PathAssist();
                ob.sendArray(finalAns, "ansIndex.txt");
                this.Close();
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
