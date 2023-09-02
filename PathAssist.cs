using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automated_MCQ_Grading_UI
{
    public class PathAssist
    {
        public string MainPath()
        {
            //string filePath = Directory.GetCurrentDirectory();
            //string[] fullPath = filePath.Split('\\');
            //int n = fullPath.Length;
            //string ans = "";
            //for (int i = 0; i < n - 4; i++)
            //{
            //    ans += fullPath[i] + '\\';
            //}
            return Directory.GetCurrentDirectory() + '\\';
        }

        public void sendArray(string text, string destTxt)
        {
            try
            {
                var path = MainPath() + destTxt;
                File.WriteAllText(path, text);
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
