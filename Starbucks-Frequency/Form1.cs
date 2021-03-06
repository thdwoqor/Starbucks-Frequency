using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
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

        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;

        string AppPlayerName = "NoxPlayer2";

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
        Bitmap gift_c_img2 = null;
        Bitmap gift_img2 = null;
        Bitmap gift_c_img3 = null;
        Bitmap gift_img3 = null;

        Bitmap bt_start = null;
        Bitmap bt_stop = null;

        Bitmap lext_img = null;
        Bitmap today_img = null;
        Bitmap near_img = null;
        Bitmap permit_img = null;
        Bitmap near_g_img = null;
        Bitmap min_img = null;
        Bitmap reservation_img = null;
        Bitmap red_img = null;
        //Bitmap test_img = null;

        Bitmap bt1_img = null;
        Bitmap bt2_img = null;
        Bitmap bt3_img = null;
        Bitmap bt4_img = null;
        Bitmap bt5_img = null;
        Bitmap bt6_img = null;
        

        static Bitmap bmp;
        static bool ERR = false;
        static double change_size;

        public Form1()
        {
            InitializeComponent();

            gift_c_img2 = new Bitmap(@"img\gift_c_img2.PNG");
            gift_img2 = new Bitmap(@"img\gift_img2.PNG");
            gift_c_img3 = new Bitmap(@"img\gift_c_img3.PNG");
            gift_img3 = new Bitmap(@"img\gift_img3.PNG");

            lext_img = new Bitmap(@"img\lext_img.PNG");
            today_img = new Bitmap(@"img\today_img.PNG");
            near_img = new Bitmap(@"img\near_img.PNG");
            permit_img = new Bitmap(@"img\permit.PNG");
            near_g_img = new Bitmap(@"img\near_g_img.PNG");
            min_img = new Bitmap(@"img\min.PNG");
            reservation_img = new Bitmap(@"img\reservation.PNG");
            red_img = new Bitmap(@"img\red.png");
            //test_img = new Bitmap(@"img\test.png");

            bt1_img = new Bitmap(@"btimg\bt1.PNG");
            bt2_img = new Bitmap(@"btimg\bt2.PNG");
            bt3_img = new Bitmap(@"btimg\bt3.png");
            bt4_img = new Bitmap(@"btimg\bt4.png");
            bt5_img = new Bitmap(@"btimg\bt5.png");
            bt6_img = new Bitmap(@"btimg\bt6.png");

            bt_start = new Bitmap(@"btimg\start.png");
            bt_stop = new Bitmap(@"btimg\stop.png");

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        static bool c, d, f;
        public void start()
        {
            c = true;
            if (bt == 1)
            {
                gift_img = gift_img2;
                gift_c_img = gift_c_img2;
            }else if(bt == 2)
            {
                gift_img = gift_img3;
                gift_c_img = gift_c_img3;
            }

            while (c)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(gift_img,1);
                    }));
                }
                else
                {
                    getBmp(gift_img,1);
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

            d = true;

            while (d)
            {
                Thread.Sleep(1000);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(red_img,1);
                        if(!ERR)
                            d = false;
                    }));
                }
                else
                {
                    getBmp(red_img,1);
                    if (!ERR)
                        d = false;
                }
            }

            f = true;
            while (f)
            {

                Thread.Sleep(200);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(near_img,1);
                    }));
                }
                else
                {
                    getBmp(near_img,1);
                }
                int near_x = (int)((maxloc.X + FindMat.Width / 2 + 120) * change_size);
                int near_y = (int)((maxloc.Y + FindMat.Height / 2 + 45) * change_size);
                
                Thread.Sleep(200);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(permit_img,1);
                    }));
                }
                else
                {
                    getBmp(permit_img,1);
                }

                InClick(near_x, near_y);

                Thread.Sleep(200);
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        getBmp(near_g_img,1);
                        if (searchIMG(bmp, near_g_img) >= 0.8)
                        {
                            f = false;
                        }
                    }));
                }
                else
                {
                    getBmp(near_g_img,1);
                    if (searchIMG(bmp, near_g_img) >= 0.8)
                    {
                         f = false;
                    }
                }
            }


        }

        #region 이미지 클릭

        public void getBmp(Bitmap img,int modified)
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

                if (modified == 1)
                {
                    System.Drawing.Size resize = new System.Drawing.Size((int)pix_width, (int)pix_height);
                    bmp = new Bitmap(bmp, resize);
                    change_size = full_width / pix_width;
                }

                if (searchIMG(bmp, img) >= 0.75)
                {
                    //이미지 정중앙 클릭
                    if(modified==1)
                        InClick((int)((maxloc.X + FindMat.Width / 2) * change_size), (int)((maxloc.Y + FindMat.Height / 2) * change_size));
                    else
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

                //Debug.WriteLine("찾은 이미지의 유사도 : " + maxval);

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

        #endregion

        #region 마우스 위로 드래그

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        public void NoxDrag(int X, int Y, int to_X, int to_Y)
        {
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            Y -= 30;
            to_Y -= 30;
            PostMessage(findwindow, WM_LBUTTONDOWN, 1, new IntPtr(Y * 0x10000 + X));
            PostMessage(findwindow, WM_LBUTTONDOWN, 1, new IntPtr(to_Y * 0x10000 + to_X));
            PostMessage(findwindow, WM_LBUTTONUP, 0, new IntPtr(to_Y * 0x10000 + to_X));
        }

        #endregion

        #region 폼 이동

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region 버튼 클릭

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>

        public static int bt = 1;

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            if (bt != 1)
            {
                bunifuImageButton5.Image = bt5_img;
                bunifuImageButton4.Image = bt4_img;
                bunifuImageButton3.Image = bt1_img;
                bt = 1;
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (bt != 0)
            {
                bunifuImageButton5.Image = bt5_img;
                bunifuImageButton4.Image = bt3_img;
                bunifuImageButton3.Image = bt2_img;
                bt = 0;
            }
        }
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            if (bt != 2)
            {
                bunifuImageButton5.Image = bt6_img;
                bunifuImageButton4.Image = bt3_img;
                bunifuImageButton3.Image = bt1_img;
                bt = 2;
            }
        }

        #endregion

        #region 현재 실행중인 프로세서 출력

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWMAXIMIZED = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            IntPtr handle = IntPtr.Zero;
            uint pid = 0;
            Process ps = null;

            handle = GetForegroundWindow();        // 활성화 윈도우
            GetWindowThreadProcessId(handle, out pid); // 핸들로 프로세스아이디 얻어옴
            ps = Process.GetProcessById((int)pid); // 프로세스아이디로 프로세스 검색

            if (ps.MainWindowTitle.Contains("Nox"))
            {
                bunifuCustomLabel1.Text = ps.MainWindowTitle;
                AppPlayerName = bunifuCustomLabel1.Text;
            }
        }
        #endregion

        #region 실행

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        static bool enable=false;
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Thread acceptThread = new Thread(() => start());
            if (!enable)
            {
                getBmp(min_img, 0); //<<

                if (bt != 0)
                    NoxDrag(bmp.Width / 2, bmp.Height / 2, bmp.Width / 2, 0);

                Thread.Sleep(200);

                timer1.Enabled = false;

                acceptThread.IsBackground = true;   // 부모 종료시 스레드 종료
                acceptThread.Start();
                enable = true;
                bunifuImageButton1.Image = bt_stop;
            }
            else
            {
                c= false;
                d= false;
                f= false;

                acceptThread.Interrupt();
                acceptThread.Abort();

                enable = false;
                bunifuImageButton1.Image = bt_start;
                timer1.Enabled = true;
            }
        }

        #endregion
    }
}
