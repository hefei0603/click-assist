using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace autoClick : WinApi
{
    class Click
    {
        public const string UNENABLE = "未开启";
        public const string ENABLE = "已开启";
        public const string NUMBER_PATTERN = @"^[0-9]*[1-9][0-9]*$"; // 判断是否为整数
        public const string CLICKER_HEROES = "Clicker Heroes";

        // 点击点列表
        List<Point> pointList = new List<Point>();

        // 是否开始连点
        bool isStart = false;

        // 点击间隔
        int clickInterval;

        /***
         * 点击线程任务
         * 
         */
        public void clickScheduler(string interval, Label label)
        {
            if (!Regex.IsMatch(interval, NUMBER_PATTERN))
            {
                return;
            }
            clickInterval = int.Parse(interval);

            Thread t = new Thread(doClick);
            t.Start();//调用主处理程序  
            label.Text = isStart ? ENABLE : UNENABLE; // 切换显示文案
        }

        /**
         * 点击主体
         */
        public void doClick()
        {
            // 若已经开始则停止,否则就开始
            isStart = !isStart;
            
            if (!isStart)
            {
                return;
            }

            int time = clickInterval;
            IntPtr hwnd = getHandle();
            Point point = getCurrPoint();
            while (isStart)
            {
                // mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                clickMouse(hwnd, point.X, point.Y);

                Thread.Sleep(time);
            }
        }

        /**
         * 获取句柄
         */
        public IntPtr getHandle()
        {
            // 【1】找到窗口
            IntPtr hwnd = FindWindow(null, CLICKER_HEROES); // CLICKER_HEROES
            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("没有找到对应的窗口");
            }

            return hwnd;
        }

        /**
          * 获取窗口右上角坐标
          */
        public Point getWindowPoint()
        {
            IntPtr hwnd = getHandle();

            // 【2】获取窗口当前坐标
            Rect rect = new Rect();
            GetWindowRect(hwnd, out rect);

            return new Point(rect.Left, rect.Top);
        }

        /**
         * 获取当卡相对点
         */
        public Point getCurrPoint()
        {
            // 当前窗口的位置
            Point windowPoint = getWindowPoint();

            // 获取相对需点击窗口的相对位置
            Point point = new Point();
            point.X = Control.MousePosition.X - windowPoint.X;
            point.Y = Control.MousePosition.Y - windowPoint.Y;

            return point;
        }

        /**
         * 添加点击点
         */
        public void addPoint(DataGridView dataGridView)
        {
            Point point = new Point();

            pointList.Add(point);

            clickMouse(getHandle(), point.X, point.Y);

            // 渲染至界面
            int index = dataGridView.Rows.Add();
            dataGridView.Rows[index].Cells[0].Value = point.X;
            dataGridView.Rows[index].Cells[1].Value = point.Y;
        }

        /**
         * 鼠标左键点击效果
         */
        public void clickMouse(IntPtr h, int x, int y)
        {
            PostMessage(h, WM_LBUTTONDOWN, MK_LBUTTON, MakeLParam(x, y));
            PostMessage(h, WM_LBUTTONUP, MK_LBUTTON, MakeLParam(x, y));
        }
    }
}
