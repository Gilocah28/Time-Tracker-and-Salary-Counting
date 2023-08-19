using PMTTS_Feb_10_2023.Class_Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using Timer = System.Windows.Forms.Timer;
using PMTTS_Feb_10_2023.Properties;
using Microsoft.VisualBasic.ApplicationServices;

namespace PMTTS_Feb_10_2023.Pages
{
    public partial class Admin_tracker : Form
    {
        MyConnection db = new MyConnection();
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True");
        SqlConnection con1 = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True");
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True");
        Stopwatch stopwatch;
        int rate1 = 60;
        int timeleft1;
        private Timer idleTimer;
        public Admin_tracker()
        {
            InitializeComponent();
            InitializeIdleTimer();
        }
        private void InitializeIdleTimer()
        {
            idleTimer = new Timer();
            idleTimer.Interval = 1000;
            idleTimer.Tick += new EventHandler(IdleTimer_Tick);
        }
        private void IdleTimer_Tick(object sender, EventArgs e)
        {
            const int idleTime = 10;
            if (IdleTimeDetector.GetIdleTime() > idleTime)
            {
                notifyIcon1.BalloonTipTitle = "Alert";
                notifyIcon1.BalloonTipText = "NO MOVEMENT DETECT ON YOUR COMPUTER. THE TRACKER HAS BEEN AN AUTOMATIC PAUSED THE TRACKER";
                notifyIcon1.Icon = SystemIcons.Information;
                notifyIcon1.ShowBalloonTip(10000);
                endAll();
            }
        }
        private void Admin_tracker_Load(object sender, EventArgs e)
        {
            label9.Text = Settings.Default.minS;
            timeleft1 = int.Parse(label9.Text);
            timer5.Enabled = true;
            try
            {
                id.Text = LoginUser.Employee_ID.ToString();
                string db = "select * from Employee_Details where Employee_ID = '" + id.Text + "'";
                SqlCommand cmd = new SqlCommand(db, con);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string f = lblfname.Text = dr["First_name"].ToString();
                        string a = lbllname.Text = dr["Last_name"].ToString();
                        lblout.Text = f + " " + a;
                        lblfname.Enabled = false;
                        lbllname.Enabled = false;
                        lblemp.Text = dr["Employee_Position"].ToString();
                        lblrate.Text = dr["Hour_rate"].ToString();
                        label14.Text = dr["Hour_rate"].ToString();
                        byte[] img = (byte[])(dr[9]);
                        if (img == null)
                            employee_images = null;
                        else
                        {
                            MemoryStream ms = new MemoryStream(img);
                            employee_images.Image = Image.FromStream(ms);
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                id.Text = LoginUser.Employee_ID.ToString();
                string db = "select * from Logs2 where Employee_ID = '" + id.Text + "' AND Login_Time = '" + DateTime.Now.ToString("hh:mm tt") + "' ";
                SqlCommand cmd = new SqlCommand(db, con);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        lbltimer.Text = dr["Daily_time"].ToString();
                        lbltotal.Text = dr["DailySalary_Total"].ToString();
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            timer1.Start();
            label5.Text = DateTime.Now.ToString("hh:mm tt");
            stopwatch = new Stopwatch();
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            idleTimer.Start();
            stopwatch.Start();
            btnstart.Hide();
            btnpause.Show();
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;
            timer6.Enabled = true;
            timer7.Enabled = true;
            timer5.Enabled = false;
            label9.Text = Settings.Default.minS;
            timeleft1 = int.Parse(label9.Text);
            guna2ShadowPanel3.ShadowColor = Color.Green;
            guna2ShadowPanel1.ShadowColor = Color.Green;
            guna2ShadowPanel2.ShadowColor = Color.Green;
            guna2ShadowPanel4.ShadowColor = Color.Green;
        }
        private void btnpause_Click(object sender, EventArgs e)
        {
            endAll();
        }
        public void endAll()
        {
            idleTimer.Stop();
            stopwatch.Stop();
            btnpause.Hide();
            btnstart.Show();
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer6.Enabled = false;
            timer7.Enabled = false;
            guna2ShadowPanel3.ShadowColor = Color.IndianRed;
            guna2ShadowPanel1.ShadowColor = Color.IndianRed;
            guna2ShadowPanel2.ShadowColor = Color.IndianRed;
            guna2ShadowPanel4.ShadowColor = Color.IndianRed;
            timer5.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString();
            lbltime.Text = DateTime.Now.ToLongTimeString();
        }
        int timeleft = 10;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timeleft > 0)
            {
                timeleft = timeleft - 1;
                label6.Text = timeleft + "";
            }
            if (label6.Text == "0")
            {
                timeleft = 10;
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("UPDATE Logs2 SET Employee_ID = @EI, Login_Date = @LD,Login_Time = @LT, Daily_time = @DT, DailySalary_Total = @DST WHERE Employee_ID = @EI AND Login_Time = @LT", con1);
                cmd1.Parameters.AddWithValue("@EI", int.Parse(id.Text));
                cmd1.Parameters.AddWithValue("@LD", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd1.Parameters.AddWithValue("@LT", label5.Text);
                cmd1.Parameters.AddWithValue("@DT", lbltimer.Text);
                cmd1.Parameters.AddWithValue("@DST", float.Parse(lbltotal.Text));
                cmd1.ExecuteNonQuery();
                con1.Close();
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (rate1 > 0)
            {
                rate1 = rate1 - 1;
                label7.Text = rate1 + "";
            }

            if (label7.Text == "0")
            {
                rate1 = 60;
            }
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (timeleft1 > 0)
            {
                timeleft1 = timeleft1 - 1;
                label9.Text = timeleft1 + "";
            }
            if (label9.Text == "0")
            {
                try
                {
                    label9.Text = Settings.Default.minS;
                    timeleft1 = int.Parse(label9.Text);
                    Bitmap bm = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                    Rectangle cr = Screen.AllScreens[0].Bounds;
                    Graphics g = Graphics.FromImage(bm as Image);
                    g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                    SImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    SImage.Image = bm;
                    Image img = SImage.Image;
                    byte[] arr;
                    ImageConverter converter = new ImageConverter();
                    arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand("INSERT INTO Screenwork (Employee_ID,Login_Date,Login_Time,Screen_Shot) Values (@Employee_ID,@Login_Date,@Login_Time,@SC)", conn);
                    cmd1.Parameters.AddWithValue("@Employee_ID", int.Parse(id.Text));
                    cmd1.Parameters.AddWithValue("@Login_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd1.Parameters.AddWithValue("@Login_Time", lbltime.Text);
                    cmd1.Parameters.AddWithValue("@SC", arr);
                    cmd1.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        int timeleft2 = 120;
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (timeleft2 > 0)
            {
                timeleft2 = timeleft2 - 1;
                label10.Text = timeleft2 + "";
            }
            if (label10.Text == "0")
            {
                timeleft2 = 120;
                notifyIcon2.BalloonTipTitle = "Alert";
                notifyIcon2.BalloonTipText = "HAVE YOU BEGUN WORKING? YOU FORGOT TO START THE TIME TRACKER";
                notifyIcon2.Icon = SystemIcons.Information;
                notifyIcon2.ShowBalloonTip(10000);
            }
        }
        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e)
        {
        }
        string imageUrl = null;
        private void SImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageUrl = ofd.FileName;
                    SImage.Image = Image.FromFile(imageUrl);
                }
            }
        }
        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pages.Admin_Control c = new Pages.Admin_Control();
            c.Show();
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("DO YOU WANT TO LOG OUT?", "MESSAGE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("UPDATE Logs2 SET Employee_ID = @EI, Login_Date = @LD,Login_Time = @LT, Daily_time = @DT, DailySalary_Total = @DST, Logout_time = @out WHERE Employee_ID = @EI AND Login_Time = @LT", con1);
                cmd1.Parameters.AddWithValue("@EI", int.Parse(id.Text));
                cmd1.Parameters.AddWithValue("@LD", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd1.Parameters.AddWithValue("@LT", label5.Text);
                cmd1.Parameters.AddWithValue("@DT", lbltimer.Text);
                cmd1.Parameters.AddWithValue("@DST", float.Parse(lbltotal.Text));
                cmd1.Parameters.AddWithValue("@out", DateTime.Now.ToString("hh:mm tt"));
                cmd1.ExecuteNonQuery();
                con1.Close();
                Login l = new Login();
                l.Show();
                this.Hide();
                endAll();
                timer5.Enabled = false;
                label9.Text = Settings.Default.minS;
                timeleft1 = int.Parse(label9.Text);
            }
        }
        private void lblDate_Click(object sender, EventArgs e)
        {
        }
        int timeleft3 = 60;
        private void timer6_Tick(object sender, EventArgs e)
        {
            if (timeleft3 > 0)
            {
                timeleft3 = timeleft3 - 1;
                label11.Text = timeleft3 + "";
            }
            if (label11.Text == "0")
            {
                timeleft3 = 60;
                float num1 = float.Parse(lblrate.Text);
                float num2 = float.Parse(label4.Text);
                float num3 = float.Parse(lblresult.Text);
                float num4 = float.Parse(label12.Text);
                float result1 = 0;
                float result2 = 0;
                result1 = num1 / num2;
                lblresult.Text = result1.ToString();
                result2 = result1 * num4;
                lbltotal.Text = result2.ToString();
            }
        }
        int timeleft7 = 00;
        private void timer7_Tick(object sender, EventArgs e)
        {
            timeleft7 = timeleft7 + 1;
            label12.Text = timeleft7 + "";
        }
        private void timer8_Tick(object sender, EventArgs e)
        {
            try
            {
                lbltimer.Text = string.Format("{0:hh\\:mm\\:ss}", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Admin_tracker_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void Admin_tracker_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void Admin_tracker_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettingsAdmin l = new frmSettingsAdmin();
            l.Show();
        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void leaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void learnMoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtu.be/DuJcVRmUbl4");
        }
    }
    public static class IdleTimeDetector
    {
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public static int GetIdleTime()
        {
            int idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                uint systemTick = (uint)Environment.TickCount;
                uint idleTick = systemTick - lastInputTick;
                idleTime = (int)(idleTick / 1000);
            }
            return idleTime;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
