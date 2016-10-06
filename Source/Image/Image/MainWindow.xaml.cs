using System;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
namespace test
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {   
        //画像を処理したかどうかのフラグ
        public class Flag
        {
            public int original;
            public int gray;
            public int blur;
            public int Threshold;
            public int Canny;
            public int polygon;
            public void Flags()
            {
                original = 0;
                gray = 0;
                blur = 0;
                Threshold = 0;
                Canny = 0;
                polygon = 0;
            }            
        }

        //----------------------------------------------------------------------------------------------------------
        // 宣言部
        //----------------------------------------------------------------------------------------------------------

        List<System.Drawing.Point[]> polygonList = new List<System.Drawing.Point[]>(); //多角形の頂点集合のリスト
        UInt16 peace_minimum_interval_px;  //ピース間の最小距離 ピクセル単位
        double length_pxmm;  //長さの単位 px/mm
        Flag flag = new Flag();
        Mat OutImage = new Mat();
        Mat OriginalImage = new Mat();
        string FileName;
        string OutPutFileName = "";
        int FileCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        //開くボタンが押されたとき
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            // ファイルを開くダイアログ
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG|*.jpg|BMP|*.bmp|PNG|*.png|イメージファイル|*.jpg;*.bmp;*.png";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    FileName = dlg.FileName;
                    Path.Content = FileName;
                    CvInvoke.DestroyAllWindows(); //ウインドウをすべて消す
                    OriginalImage = new Mat(); //画像の変数の削除
                    OutImage = new Mat(); //画像の変数の削除
                    flag.Flags(); //フラグの削除
                    polygonList = new List<System.Drawing.Point[]>(); //ポリゴンリストの削除

                    OriginalImage = load(FileName); //読み込み
                    length_pxmm = OriginalImage.Height / double.Parse(image_size_mm.Text); // 画像の高さを約150mmと仮定
                    peace_minimum_interval_px = (UInt16)(length_pxmm * double.Parse(peace_min_interval.Text)); //ピース間の最小距離をpx単位で

                    Imgshow("Original", OriginalImage); //オリジナルを表示
                    OutImage = Gray(OriginalImage); //グレースケールに変換する
                    Imgshow("OutPut", OutImage);
                    message.Text = "Load->";

                    flag.original = 1;
                    flag.gray = 1;
                    polygonList.Clear();

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error: 画像が開けません " + ex.Message);
                    message.Text = "Error: 画像が開けません " + ex.Message;
                }
            }
        }

        //リセットボタンが押されたとき
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            CvInvoke.DestroyAllWindows(); //ウインドウをすべて消す
            OriginalImage = new Mat(); //画像の変数の削除
            OutImage = new Mat(); //画像の変数の削除
            flag.Flags(); //フラグの削除
            polygonList = new List<System.Drawing.Point[]>(); //ポリゴンリストの削除

            OriginalImage = load(FileName); //読み込み

            length_pxmm = OriginalImage.Height / double.Parse(image_size_mm.Text); // 画像の高さを約150mmと仮定
            peace_minimum_interval_px = (UInt16)(length_pxmm * double.Parse(peace_min_interval.Text)); //ピース間の最小距離をpx単位で

            Imgshow("Original", OriginalImage); //オリジナルを表示
            OutImage = Gray(OriginalImage); //グレースケールに変換する
            Imgshow("OutPut", OutImage);
            message.Text = "Load->";

            flag.original = 1;
            flag.gray = 1;
            polygonList.Clear();
        }

        //グレースケールのボタンが押されたとき
        private void Gray_button(object sender, RoutedEventArgs e)
        {
            if(flag.original != 0)
            {
                OutImage = Gray(OriginalImage); 
                Imgshow("OutPut", OutImage);
                message.Text = "Gray->";
                flag.gray = 1;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";

            }
        }

        //二値化のボタンが押されたとき
        private void Thresholded_Click(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                if (flag.gray != 0)
                {
                    OutImage = Threshold(OutImage);
                    Imgshow("OutPut", OutImage);
                    message.Text += "Threshold->";
                    flag.Threshold = 1;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error: グレースケールになっていません ");
                    message.Text = "Error: グレースケールになっていません ";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        //Cannyエッジフィルターのボタンが押されたとき
        private void Canny(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                if (flag.gray != 0)
                {
                    OutImage = Canny(OutImage);
                    Imgshow("OutPut", OutImage);
                    message.Text += "Canny->";
                    flag.Canny = 1;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error: グレースケールになっていません ");
                    message.Text = "Error: グレースケールになっていません ";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        //ガウシアンのボタンが押されたとき
        private void gauss_filter_Click(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                OutImage = gauss(OutImage);
                Imgshow("OutPut", OutImage);
                message.Text += "Gauss->";
                flag.blur = 1;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        
        //多角形検出ボタンが押されたとき
        private void polygondetection_Click(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                if (flag.Canny != 0)
                {
                    if(flag.polygon != 0)
                    {
                        polygonList = new List<System.Drawing.Point[]>(); //ポリゴンリストの削除

                    }
                    //多角形をpolygonListに入れる
                    find_polygon(OutImage);
                    //反時計回りになるように並べ替え
                    sort_polygon();
                    //重複する多角形を削除（互いに1cm未満の距離に重心がある）
                    check_nearest_polygon();
                    //画像で表示
                    display_polygon();

                    flag.polygon = 1;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error: Cannyエッジフィルターをかけてください ");
                    message.Text = "Error: Cannyエッジフィルターをかけてください ";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        private void list_save_Click(object sender, RoutedEventArgs e)
        {
            // ファイルを開くダイアログ
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Title = "ファイルを保存";
            dlg.FileName = "Polygon.txt";
            dlg.Filter = "テキストファイル|*.txt";
            if (dlg.ShowDialog() == true)
            {
                save_polygonlist(dlg.FileName);
                message.Text = dlg.FileName + "に書き出しました";
            }
            else
            {
                message.Text = "キャンセルされました";
            }

        }

        //膨張ボタンが押されたとき
        private void dilate_Click(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                CvInvoke.Dilate(OutImage, OutImage, new Mat(), new System.Drawing.Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));
                Imgshow("OutPut", OutImage);
                message.Text += "Dilate->";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        //収縮ボタンが押されたとき
        private void erode_Click(object sender, RoutedEventArgs e)
        {
            if (flag.original != 0)
            {
                CvInvoke.Erode(OutImage, OutImage, new Mat(), new System.Drawing.Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));
                Imgshow("OutPut", OutImage);
                message.Text += "Erode->";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: 画像を読み込んでいません ");
                message.Text = "Error: 画像を読み込んでいません ";
            }
        }

        //枠の出力のボタンが押されたとき
        private void Frame_Out_Click(object sender, RoutedEventArgs e)
        {            
            // ファイルを開くダイアログ
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Title = "ファイルを保存";
            dlg.FileName = "Result.pro27.txt";
            dlg.Filter = "プロコンファイル|*.pro27.txt";
            if (dlg.ShowDialog() == true)
            {
                Frame_OutPut(dlg.FileName);
                message.Text = dlg.FileName + "に書き出しました";
                OutPutFileName = dlg.FileName;
            }
            else
            {
                message.Text = "キャンセルされました";
            }
        }

        //ピースの出力のボタンが押されたとき
        private void Piece_Out_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Piece_OutPut(OutPutFileName);
                message.Text = OutPutFileName + "に書き出しました";
            }
            catch
            {
                message.Text = "ファイルを開けません";
            }
        }

        //----------------------------------------------------------------------------------------------------------
        // 画像処理
        //----------------------------------------------------------------------------------------------------------

        //グレースケール
        Mat Gray(Mat img)
        {
            Mat dst = new Mat();
            CvInvoke.CvtColor(img, dst, ColorConversion.Bgr2Gray);
            return dst;
        }

        //二値化
        Mat Threshold(Mat img)
        {
            Mat dst = new Mat();
            CvInvoke.Threshold(img, dst, Threshold_1.Value, Threshold_2.Value, ThresholdType.Binary);
            return dst;
        }

        //Cannyエッジフィルター
        Mat Canny(Mat img)
        {
            Mat dst = new Mat();
            CvInvoke.Canny(img, dst, CannyEdge_1.Value, CannyEdge_2.Value);
            return dst;
        }

        //ガウシアンぼけフィルター
        Mat gauss(Mat img)
        {
            Mat dst = new Mat();
            CvInvoke.GaussianBlur(img, dst, new System.Drawing.Size(3, 3), 1, 0, BorderType.Constant);　　//Sizeは3,5,7,...   1はSizeに応じて　数字が大きいとよくボケる
            return dst;
        }


        //----------------------------------------------------------------------------------------------------------
        // 多角形検出
        //----------------------------------------------------------------------------------------------------------

        //srcから多角形見つけてpolygonListに入れる
        private void find_polygon(Mat src)
        {
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint()) // c++の標準のvector of VectorOfPoint // pointの2次元配列
            {
                CvInvoke.FindContours(src, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                //cannyEdges像からcoutnours（座標点の2次元配列）を生成。ChangeApproxMethodは、ChainCode=チェインコード（多角形でない）
                //ChainApproxNone=全ての点をチェインコードから点に変換する。ChainApproxSimple=水平と垂直と直交の線分を圧縮。すなわち最後の点のみを残す。
                //ChainApproxTc89Kcos=Teh-Chinチェイン近似アルゴリズムの一種。

                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        // 近似曲線をapproxContourに求める。誤差は全長の5%許容、閉曲線=true
                        // ArcLength contourの周囲長を求める。閉曲線=true
                        CvInvoke.ApproxPolyDP(contour, approxContour, 15, true);
                        //許容誤差2px
                        if (CvInvoke.ContourArea(approxContour, false) > length_pxmm * length_pxmm * 100) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size >= 3) //3角形以上
                            {
                                System.Drawing.Point[] pts = approxContour.ToArray(); //多角形の頂点の配列
                                try
                                {
                                    polygonList.Add(adjust_invalid_points(pts).ToArray()); //新しい多角形をリストに追加
                                }
                                catch
                                {
                                    //void
                                }
                            }
                        }
                        
                        //このあたりに点と点の最小距離は peace_minimum_interval_px 以上離れているという制約条件が要りそう　あるいは　GUIで手で省く
                    }
                }
            }
        }

        //polygonlist
        List<System.Drawing.Point> adjust_invalid_points(System.Drawing.Point[] pts)
        {
            List<System.Drawing.Point> newpts = new List<System.Drawing.Point>();
            int i = 0;
            for (i = 0; i < pts.Length; i++)
            {
                if (!(pts[i].X < 10 || pts[i].X > OutImage.Rows - 10 || pts[i].Y < 10 || pts[i].Y > OutImage.Cols - 10))  //周辺は省く
                {
                    newpts.Add(new System.Drawing.Point(pts[i].X, pts[i].Y));
                }
            }

            if (newpts.Count < 3)
                return null;
            return newpts;
        }

        //多角形の重心が1cm未満を削除
        private void check_nearest_polygon()
        {
            int i, j, k;
            int[] centerx = new int[polygonList.Count];
            int[] centery = new int[polygonList.Count];
            int[] FLAG = new int[polygonList.Count];

            for (i = 0; i < polygonList.Count; i++)
            {
                for (j = 0; j < polygonList[i].Length; j++)
                {
                    centerx[i] += polygonList[i][j].X;
                    centery[i] += polygonList[i][j].Y;
                }
                centerx[i] /= polygonList[i].Length;
                centery[i] /= polygonList[i].Length;
            }
            k = 0;
            for (i = 0; i < polygonList.Count; i++)
            {
                for (j = 1; j < polygonList.Count; j++)
                {
                    double disntace = Math.Sqrt((centerx[i] - centerx[j]) * (centerx[i] - centerx[j]) + (centery[i] - centery[j]) * (centery[i] - centery[j]));
                    if (disntace < length_pxmm * 10)
                    {
                        FLAG[k] = j;
                    }
                }
                k++;
            }
            for (k = 0; k < polygonList.Count; k++)
            {
                polygonList.RemoveAt(FLAG[k]);
            }
        }

        //多角形の頂点座標を反時計回りになるように並べ替える
        private void sort_polygon()
        {
            System.Drawing.Point[] temp = new System.Drawing.Point[100];
            int i, j;
            for (i = 0; i < polygonList.Count; i++)
            {
                if (is_counter_clockwise_polygon(polygonList[i])) //時計回りの場合
                {
                    Array.Reverse(polygonList[i]); //反時計回りにする
                }
            }
        }

        //多角形の頂点が時計回りか反時計回りかを調べる
        private bool is_counter_clockwise_polygon(System.Drawing.Point[] point)
        {
            bool counterclockwise = new bool();//角度が増える方向は counterclockwise=true;(反時計回り)
            VectorOfPoint contour = new VectorOfPoint(point);
            double area = CvInvoke.ContourArea(contour, true); //面積は時計回りの多角形で負、反時計回りで正の値を取る
            if (area < 0)
            {
                counterclockwise = true; //時計回りの場合
            }
            else
            {
                counterclockwise = false; //反時計回りの場合
            }
            return counterclockwise;
        }

        //多角形を画像で表示
        private void display_polygon()
        {
            Image<Bgr, Byte> polyImage = new Image<Bgr, Byte>(OutImage.Size);
            LineSegment2D line = new LineSegment2D();
            System.Drawing.Point[] point = new System.Drawing.Point[500];
            int i, j;
            for (i = 0; i < polygonList.Count; i++)
            {
                for (j = 0; j < polygonList[i].Length - 1; j++)
                {
                    point = polygonList[i];
                    line.P1 = point[j];
                    line.P2 = point[j + 1];
                    polyImage.Draw(line, new Bgr(Color.White), 2);
                }
                point = polygonList[i];
                line.P1 = point[j];
                line.P2 = point[0];
                polyImage.Draw(line, new Bgr(Color.White), 2);
            }
            Imgshow("polygon image", polyImage);
        }

        //最も大きい多角形を取り出す
        private int select_largest_polygon()
        {
            double area, maxarea = 0;
            int maxi = 0;

            for (int i = 0; i < polygonList.Count; i++)
            {
                using (VectorOfPoint contour = new VectorOfPoint(polygonList[i]))
                {
                    area = CvInvoke.ContourArea(contour, false);
                }
                if (area > maxarea)
                {
                    maxarea = area;//面積が最大
                    maxi = i; //最大値を取るi
                }
            }
            return maxi;
        }

        //----------------------------------------------------------------------------------------------------------
        // 出力
        //----------------------------------------------------------------------------------------------------------

        //枠の出力
        private void Frame_OutPut(string filename)
        {
            string Out = "";
            for (int ji = 0; ji < 2; ji++)
            {
                Out += "60";
                Out += Environment.NewLine;
                for (int ij = 0; ij < 60; ij++)
                {
                    Out += ij.ToString();
                    if (ij != 59)
                    {
                        Out += " ";
                    }
                }
                Out += Environment.NewLine;
            }
            Out += Frame_Number.Text + Environment.NewLine;
            int i, j;
            int maxi = select_largest_polygon();
            for (i = 0; i < polygonList.Count; i++)
            {
                if (i != maxi)
                {
                    Out += polygonList[i].Length.ToString() + Environment.NewLine;
                    for (j = 0; j < polygonList[i].Length; j++)
                    {
                        Out += (polygonList[i][j].X / length_pxmm).ToString() + " " + (polygonList[i][j].Y / length_pxmm).ToString() + System.Environment.NewLine;
                    }
                }
            }
            System.IO.StreamWriter textfile = new System.IO.StreamWriter(filename);
            textfile.Write(Out);
            textfile.Close();
        }

        //ピースの出力
        private void Piece_OutPut(string filename)
        {
            string Out = "";
            if(FileCount == 0)
                Out += Piece_Number.Text + Environment.NewLine;
            int i, j;
            for (i = 0; i < polygonList.Count; i++)
            {
                Out += polygonList[i].Length.ToString() + Environment.NewLine;
                for (j = 0; j < polygonList[i].Length; j++)
                {
                    Out += (polygonList[i][j].X / length_pxmm).ToString() + " " + (polygonList[i][j].Y / length_pxmm).ToString() + System.Environment.NewLine;
                }
            }
            System.IO.StreamWriter textfile = new System.IO.StreamWriter(filename, true);
            textfile.Write(Out);
            textfile.Close();
            FileCount++;
        }
        
        //多角形の頂点座標をファイルに書き出す
        void save_polygonlist(string outputfilename)
        {
            System.IO.StreamWriter textfile = new System.IO.StreamWriter(outputfilename);
            int i, j;
            string outputtext = "";

            for (i = 0; i < polygonList.Count; i++)
            {
                for (j = 0; j < polygonList[i].Length; j++)
                {
                    outputtext += polygonList[i][j].X.ToString() + "," + polygonList[i][j].Y.ToString() + System.Environment.NewLine;
                }
                outputtext += "=,=" + System.Environment.NewLine;
            }
            textfile.Write(outputtext);
            textfile.Close();

        }

        //----------------------------------------------------------------------------------------------------------
        // 共通
        //----------------------------------------------------------------------------------------------------------

        //画像の読み込み
        Mat load(string filename)
        {
            return CvInvoke.Imread(filename, LoadImageType.Color);
        }

        //画像を縮小して表示(Mat)
        void Imgshow(string Name, Mat img)
        {
            Mat ResizedImg = new Mat();
            CvInvoke.Resize(img, ResizedImg, ResizedImg.Size, double.Parse(magnification.Text)/100, double.Parse(magnification.Text) / 100);
            CvInvoke.Imshow(Name, ResizedImg);
        }

        //画像を縮小して表示(Image)
        void Imgshow(string Name, Image<Bgr, Byte> img)
        {
            Image<Bgr, Byte> ResizedImg = img.Resize(double.Parse(magnification.Text) / 100, Inter.Cubic);
            CvInvoke.Imshow(Name, ResizedImg);
        }
    }
}
