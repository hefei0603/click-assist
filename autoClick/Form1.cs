using System;
using System.Windows.Forms;
using static autoClick.WinApi;

namespace autoClick
{
    public partial class Form1 : Form
    {
        Click click = new Click();

        protected override void WndProc(ref Message m)//监视Windows消息  
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case HOTKEY_ID_F9:
                            click.clickScheduler(textBox1.Text, label5);
                            break;
                        case HOTKEY_ID_F10: click.addPoint(dataGridView1); break;
                    }
                    break;

            }
            base.WndProc(ref m);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, HOTKEY_ID_F9, 0, Keys.F9);
            RegisterHotKey(this.Handle, HOTKEY_ID_F10, 0, Keys.F10);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_F9);
            UnregisterHotKey(this.Handle, HOTKEY_ID_F10);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // this.Visible = false; //这个也可以
                this.Hide();
                this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            click.clickScheduler(textBox1.Text, label5);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Dispose();
            System.Environment.Exit(0);//这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
