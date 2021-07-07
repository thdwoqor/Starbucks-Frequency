using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starbucks_Frequency
{
    public partial class Form1 : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr Parent, IntPtr Child, string lpszClass, string lpszWindows);

        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;

        string AppPlayerName = "NoxPlayer4";

        static string[] AppPlayerNames = { "NoxPlayer", "NoxPlayer1", "NoxPlayer2", "NoxPlayer3" };

        //녹스플레이어 크기를 저정할 변수
        double full_width = 0;
        double full_height = 0;
        //녹스플레이어 지정한 크기
        double pix_width = 572;
        double pix_height = 1020;
        //찾은 이미지의 위치를 담을 포인트형을 선업합니다.
        static OpenCvSharp.Point minloc, maxloc;
        static Mat FindMat;

        Bitmap gift_c_img = null;
        Bitmap gift_img = null;
        Bitmap lext_img = null;
        Bitmap today_img = null;
        Bitmap near_img = null;
        Bitmap permit_img = null;
        Bitmap near_g_img = null;
        Bitmap min_img = null;
        Bitmap reservation_img = null;
        Bitmap red_img = null;
        Bitmap test_img = null;

        static Bitmap bmp;
        static bool ERR = false;
        static double change_size;

        public Form1()
        {
            InitializeComponent();

            bunifuDropdown1.selectedIndex = 0;

            gift_c_img = new Bitmap(@"img\gift_c_img.PNG");
            gift_img = new Bitmap(@"img\gift_img.PNG");
            lext_img = new Bitmap(@"img\lext_img.PNG");
            today_img = new Bitmap(@"img\today_img.PNG");
            near_img = new Bitmap(@"img\near_img.PNG");
            permit_img = new Bitmap(@"img\permit.PNG");
            near_g_img = new Bitmap(@"img\near_g_img.PNG");
            min_img = new Bitmap(@"img\min.PNG");
            reservation_img = new Bitmap(@"img\reservation.PNG");
            red_img = new Bitmap(@"img\red.png");
            test_img = new Bitmap(@"img\test.png");
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void start()
        {
            bool c = true;
            while (c)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(gift_img);
                    }));
                }
                else
                {
                    getBmp(gift_img);
                }
                Thread.Sleep(100);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        if (searchIMG(bmp, gift_c_img) >= 0.8)
                        {
                            if (searchIMG(bmp, lext_img) >= 0.8)
                            {
                                InClick((int)((maxloc.X + FindMat.Width / 2) * change_size), (int)((maxloc.Y + FindMat.Height / 2) * change_size));
                                c = false;
                            }
                        }
                    }));
                }
                else
                {
                    if (searchIMG(bmp, gift_c_img) >= 0.8)
                    {
                        if (searchIMG(bmp, lext_img) >= 0.8)
                        {
                            InClick((int)((maxloc.X + FindMat.Width / 2) * change_size), (int)((maxloc.Y + FindMat.Height / 2) * change_size));
                            c = false;
                        }
                    }
                }
            }

            bool d = true;

            while (d)
            {
                Thread.Sleep(1000);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(red_img);
                        if(!ERR)
                            d = false;
                    }));
                }
                else
                {
                    getBmp(red_img);
                    if (!ERR)
                        d = false;
                }
            }

            bool e = true;
            while (e)
            {
                Thread.Sleep(200);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(near_img);
                    }));
                }
                else
                {
                    getBmp(near_img);
                }

                Thread.Sleep(100);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(permit_img);
                    }));
                }
                else
                {
                    getBmp(permit_img);
                }

                Thread.Sleep(100);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(reservation_img);
                    }));
                }
                else
                {
                    getBmp(reservation_img);
                }

                
                Thread.Sleep(100);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(near_g_img);
                        if (searchIMG(bmp, near_g_img) >= 0.8)
                        {
                            e = false;
                        }
                    }));
                }
                else
                {
                    getBmp(near_g_img);
                    if (searchIMG(bmp, near_g_img) >= 0.8)
                    {
                         e = false;
                    }
                }
            }


        }

        public void getBmp(Bitmap img)
        {
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //찾은 플레이어를 바탕으로 Graphics 정보를 가져옵니다.
                Graphics Graphicsdata = Graphics.FromHwnd(findwindow);

                //찾은 플레이어 창 크기 및 위치를 가져옵니다. 
                Rectangle rect = Rectangle.Round(Graphicsdata.VisibleClipBounds);

                full_width = rect.Width;
                full_height = rect.Height;

                //플레이어 창 크기 만큼의 비트맵을 선언해줍니다.
                bmp = new Bitmap(rect.Width, rect.Height);

                //비트맵을 바탕으로 그래픽스 함수로 선언해줍니다.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //찾은 플레이어의 크기만큼 화면을 캡쳐합니다.
                    IntPtr hdc = g.GetHdc();
                    PrintWindow(findwindow, hdc, 0x2);
                    g.ReleaseHdc(hdc);
                }

                // pictureBox1 이미지를 표시해줍니다.
                //pictureBox1.Image = bmp;

                System.Drawing.Size resize = new System.Drawing.Size((int)pix_width, (int)pix_height);
                bmp = new Bitmap(bmp, resize);
                change_size = full_width / pix_width;

                if (searchIMG(bmp, img) >= 0.8)
                {
                    //이미지 정중앙 클릭
                    InClick((int)((maxloc.X + FindMat.Width / 2) * change_size), (int)((maxloc.Y + FindMat.Height / 2) * change_size));
                    ERR = false;
                }
                else
                {
                    ERR = true;
                }
            }
        }

        public void getBmp2(Bitmap img)
        {
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //찾은 플레이어를 바탕으로 Graphics 정보를 가져옵니다.
                Graphics Graphicsdata = Graphics.FromHwnd(findwindow);

                //찾은 플레이어 창 크기 및 위치를 가져옵니다. 
                Rectangle rect = Rectangle.Round(Graphicsdata.VisibleClipBounds);

                full_width = rect.Width;
                full_height = rect.Height;

                //플레이어 창 크기 만큼의 비트맵을 선언해줍니다.
                bmp = new Bitmap(rect.Width, rect.Height);

                //비트맵을 바탕으로 그래픽스 함수로 선언해줍니다.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //찾은 플레이어의 크기만큼 화면을 캡쳐합니다.
                    IntPtr hdc = g.GetHdc();
                    PrintWindow(findwindow, hdc, 0x2);
                    g.ReleaseHdc(hdc);
                }

                // pictureBox1 이미지를 표시해줍니다.
                //pictureBox1.Image = bmp;

                if (searchIMG(bmp, img) >= 0.8)
                {
                    //이미지 정중앙 클릭
                    InClick((int)((maxloc.X + FindMat.Width / 2)), (int)((maxloc.Y + FindMat.Height / 2)));
                    ERR = false;
                }
                else
                {
                    ERR = true;
                }
            }
        }

        public double searchIMG(Bitmap screen_img, Bitmap find_img)
        {
            //find_img 크기를 스크린 이미지에 맞게 조절
            Bitmap clone = find_img.Clone(new Rectangle(0, 0, find_img.Width, find_img.Height), PixelFormat.Format32bppArgb);

            Mat ScreenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screen_img);

            FindMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(clone);

            //스크린 이미지에서 FindMat 이미지를 찾아라
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed))
            {
                //찾은 이미지의 유사도를 담을 더블형 최대 최소 값을 선언합니다.
                double minval, maxval = 0;

                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);

                Debug.WriteLine("찾은 이미지의 유사도 : " + maxval);

                return maxval;
            }
        }

        public void InClick(int x, int y)
        {
            //클릭이벤트를 발생시킬 플레이어를 찾습니다.
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //플레이어를 찾았을 경우 클릭이벤트를 발생시킬 핸들을 가져옵니다.
                IntPtr lparam = new IntPtr(x | (y << 16));

                //플레이어 핸들에 클릭 이벤트를 전달합니다.
                SendMessage(findwindow, WM_LBUTTONDOWN, 1, lparam);
                SendMessage(findwindow, WM_LBUTTONUP, 0, lparam);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getBmp(today_img);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            getBmp(test_img);
        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            Debug.WriteLine($"{bunifuDropdown1.selectedValue}");
            AppPlayerName = bunifuDropdown1.selectedValue;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            getBmp2(min_img);

            Thread acceptThread = new Thread(() => start());
            acceptThread.IsBackground = true;   // 부모 종료시 스레드 종료
            acceptThread.Start();
        }
    }
}
