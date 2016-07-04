using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenCVWPFLib;

namespace ShowImageWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenCVMatCLI m_openCvMatCLI;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            // ファイルを開くダイアログ
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG|*.jpg|BMP|*.bmp";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                // 既に読み込まれていたら解放する
                if (m_openCvMatCLI != null)
                {
                    m_openCvMatCLI = null;
                }
                m_openCvMatCLI = new OpenCVMatCLI();
                m_openCvMatCLI.load(filename);
                image.Source = m_openCvMatCLI.Image;
            }
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
