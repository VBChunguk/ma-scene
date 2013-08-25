using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Vbc.MA.Scenario
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "(SD 카드)/Android/data/com.square_enix.million_kr/files/save/ 디렉터리를 골라 주세요.";
            string[] roots = Directory.GetLogicalDrives();
            foreach (string root in roots)
            {
                string tpath = Path.Combine(root, @"Android\data\com.square_enix.million_kr\files\save");
                if (Directory.Exists(tpath))
                {
                    fbd.SelectedPath = tpath;
                    break;
                }
            }
            while (true)
            {
                if (fbd.ShowDialog() == DialogResult.Cancel) return;
                Utilities.FileManager.BasePath = fbd.SelectedPath;
                if (!Utilities.FileManager.IsValidPath)
                {
                    MessageBox.Show("올바르지 않은 경로입니다.");
                }
                break;
            }

            Application.Run(new UI.DisplayForm());
        }
    }
}
