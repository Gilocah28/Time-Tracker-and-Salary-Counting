using PMTTS_Feb_10_2023.Class_Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PMTTS_Feb_10_2023
{
    public partial class Login : Form


    {
        Thread th;
        
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True");
        SqlConnection conn1;
        
        ConnectionDB db = new ConnectionDB();
        DataSet ds= new DataSet();
        public Login()
        {
            InitializeComponent();
            conn1 = new SqlConnection(db.GetConnection());
        }
        public void openmdi(object obj)
        {
            Application.Run(new Pages.Admin_tracker());
        }
        public void openmdi1(object obj)
        {
            Application.Run(new user_Tracker1());
        }
        private void Login_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblcorrect.Text ="";
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            loginEnter();
        }
        public void loginEnter()
        {
            string uname = username.Text.Trim();
            string upass = password.Text.Trim();

            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(upass))
            {
                lblcorrect.Text = "Please enter username and password";
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Employee_Details WHERE Username=@uname COLLATE SQL_Latin1_General_CP1_CS_AS AND Password=@upass COLLATE SQL_Latin1_General_CP1_CS_AS", conn))
                    {
                        cmd.Parameters.AddWithValue("@uname", uname);
                        cmd.Parameters.AddWithValue("@upass", upass);
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.HasRows)
                            {
                                rd.Read();

                                if (rd[8].ToString() == "Admin")
                                {
                                    LoginUser.Employee_ID = Convert.ToInt32(rd["Employee_ID"]);
                                    rd.Close();

                                    using (SqlCommand cmd1 = new SqlCommand("insert into Logs2 (Employee_ID,Login_Date,Login_Time,Daily_time,DailySalary_Total) values (@EmployeeID,@LoginDate,@LoginTime,@DailyTime,@DailySalaryTotal)", conn))
                                    {
                                        cmd1.Parameters.AddWithValue("@EmployeeID", LoginUser.Employee_ID);
                                        cmd1.Parameters.AddWithValue("@LoginDate", DateTime.Now.Date);
                                        cmd1.Parameters.AddWithValue("@LoginTime", DateTime.Now.ToString("hh:mm tt"));
                                        cmd1.Parameters.AddWithValue("@DailyTime", lbltimer.Text);
                                        cmd1.Parameters.AddWithValue("@DailySalaryTotal", lbltotal.Text);

                                        cmd1.ExecuteNonQuery();
                                    }

                                    this.Close();
                                    th = new Thread(openmdi);
                                    th.TrySetApartmentState(ApartmentState.STA);
                                    th.Start();
                                }
                                else if (rd[8].ToString() == "User")
                                {
                                    LoginUser.Employee_ID = Convert.ToInt32(rd["Employee_ID"]);
                                    rd.Close();
                                    using (SqlCommand cmd1 = new SqlCommand("insert into Logs2 (Employee_ID,Login_Date,Login_Time,Daily_time,DailySalary_Total) values (@EmployeeID,@LoginDate,@LoginTime,@DailyTime,@DailySalaryTotal)", conn))
                                    {
                                        cmd1.Parameters.AddWithValue("@EmployeeID", LoginUser.Employee_ID);
                                        cmd1.Parameters.AddWithValue("@LoginDate", DateTime.Now.Date);
                                        cmd1.Parameters.AddWithValue("@LoginTime", DateTime.Now.ToString("hh:mm tt"));
                                        cmd1.Parameters.AddWithValue("@DailyTime", lbltimer.Text);
                                        cmd1.Parameters.AddWithValue("@DailySalaryTotal", lbltotal.Text);
                                        cmd1.ExecuteNonQuery();
                                    }
                                    this.Close();
                                    th = new Thread(openmdi1);
                                    th.TrySetApartmentState(ApartmentState.STA);
                                    th.Start();
                                }
                            }
                            else
                            {
                                lblcorrect.Text = "INCORRECT USERNAME OR PASSWORD!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Username_Enter(object sender, EventArgs e)
        {
            if (username.Text == "Username")
            {
                username.Text = "";
                username.ForeColor = Color.Black;
            }
        }
        private void Username_Leave(object sender, EventArgs e)
        {
            if (username.Text == "")
            {
                username.Text = "Username";
                username.ForeColor = Color.Black;
            }
        }
        private void password_Enter(object sender, EventArgs e)
        {
            if (password.Text == "Password")
            {
                password.Text = "";
                password.ForeColor = Color.Black;
            }
        }
        private void password_Leave(object sender, EventArgs e)
        {
            if (password.Text == "")
            {
                password.Text = "Password";
                password.ForeColor = Color.Black;
            }
        }
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                password.UseSystemPasswordChar = false;
            }
            else
            {
                password.UseSystemPasswordChar = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private Label lbltimer;
        private Label lbltotal;
        private Label lblcorrect;
        private void password_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginEnter();
            }
        }
        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginEnter();
            }
        }
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private IContainer components;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl2;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2DragControl2 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2CirclePictureBox2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2CheckBox1 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.password = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2GradientButton1 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbltimer = new System.Windows.Forms.Label();
            this.lbltotal = new System.Windows.Forms.Label();
            this.lblcorrect = new System.Windows.Forms.Label();
            this.guna2GradientButton2 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).BeginInit();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 15;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.TargetControl = this;
            // 
            // guna2DragControl2
            // 
            this.guna2DragControl2.TargetControl = this.guna2Panel1;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(241)))));
            this.guna2Panel1.Controls.Add(this.guna2CirclePictureBox2);
            this.guna2Panel1.Controls.Add(this.guna2ControlBox1);
            this.guna2Panel1.Controls.Add(this.guna2ControlBox2);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.Parent = this.guna2Panel1;
            this.guna2Panel1.Size = new System.Drawing.Size(338, 24);
            this.guna2Panel1.TabIndex = 15;
            // 
            // guna2CirclePictureBox2
            // 
            this.guna2CirclePictureBox2.Image = global::PMTTS_Feb_10_2023.Properties.Resources.reload_media2_1_e1628075381997;
            this.guna2CirclePictureBox2.Location = new System.Drawing.Point(5, 0);
            this.guna2CirclePictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.guna2CirclePictureBox2.Name = "guna2CirclePictureBox2";
            this.guna2CirclePictureBox2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox2.ShadowDecoration.Parent = this.guna2CirclePictureBox2;
            this.guna2CirclePictureBox2.Size = new System.Drawing.Size(22, 24);
            this.guna2CirclePictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2CirclePictureBox2.TabIndex = 14;
            this.guna2CirclePictureBox2.TabStop = false;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BorderColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.BorderRadius = 5;
            this.guna2ControlBox1.CustomIconSize = 5F;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.HoverState.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.Black;
            this.guna2ControlBox1.Location = new System.Drawing.Point(310, -1);
            this.guna2ControlBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.ShadowDecoration.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.Size = new System.Drawing.Size(28, 24);
            this.guna2ControlBox1.TabIndex = 14;
            this.guna2ControlBox1.Click += new System.EventHandler(this.guna2ControlBox1_Click);
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox2.BorderColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.BorderRadius = 5;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox2.CustomIconSize = 5F;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.HoverState.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.Black;
            this.guna2ControlBox2.Location = new System.Drawing.Point(284, -1);
            this.guna2ControlBox2.Margin = new System.Windows.Forms.Padding(2);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.ShadowDecoration.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.Size = new System.Drawing.Size(28, 24);
            this.guna2ControlBox2.TabIndex = 2;
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.guna2Panel2);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(36, 145);
            this.guna2ShadowPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.Radius = 15;
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(93)))), ((int)(((byte)(95)))));
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(266, 269);
            this.guna2ShadowPanel1.TabIndex = 12;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.guna2CheckBox1);
            this.guna2Panel2.Controls.Add(this.label3);
            this.guna2Panel2.Controls.Add(this.username);
            this.guna2Panel2.Controls.Add(this.label2);
            this.guna2Panel2.Controls.Add(this.password);
            this.guna2Panel2.Controls.Add(this.guna2GradientButton1);
            this.guna2Panel2.Location = new System.Drawing.Point(4, 6);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(2);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.ShadowDecoration.Parent = this.guna2Panel2;
            this.guna2Panel2.Size = new System.Drawing.Size(258, 259);
            this.guna2Panel2.TabIndex = 10;
            // 
            // guna2CheckBox1
            // 
            this.guna2CheckBox1.AutoSize = true;
            this.guna2CheckBox1.CheckedState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2CheckBox1.CheckedState.BorderRadius = 2;
            this.guna2CheckBox1.CheckedState.BorderThickness = 0;
            this.guna2CheckBox1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.guna2CheckBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2CheckBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2CheckBox1.Location = new System.Drawing.Point(11, 159);
            this.guna2CheckBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2CheckBox1.Name = "guna2CheckBox1";
            this.guna2CheckBox1.Size = new System.Drawing.Size(126, 23);
            this.guna2CheckBox1.TabIndex = 14;
            this.guna2CheckBox1.Text = "Show Password";
            this.guna2CheckBox1.UncheckedState.BorderColor = System.Drawing.Color.LightGray;
            this.guna2CheckBox1.UncheckedState.BorderRadius = 2;
            this.guna2CheckBox1.UncheckedState.BorderThickness = 0;
            this.guna2CheckBox1.UncheckedState.FillColor = System.Drawing.Color.LightGray;
            this.guna2CheckBox1.UseVisualStyleBackColor = true;
            this.guna2CheckBox1.CheckedChanged += new System.EventHandler(this.guna2CheckBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(93)))), ((int)(((byte)(95)))));
            this.label3.Location = new System.Drawing.Point(8, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // username
            // 
            this.username.BorderColor = System.Drawing.Color.Gray;
            this.username.BorderRadius = 10;
            this.username.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.username.DefaultText = "Username";
            this.username.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.username.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.username.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.username.DisabledState.Parent = this.username;
            this.username.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.username.FillColor = System.Drawing.Color.LightGray;
            this.username.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.username.FocusedState.Parent = this.username;
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.username.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.username.HoverState.Parent = this.username;
            this.username.Location = new System.Drawing.Point(3, 37);
            this.username.Name = "username";
            this.username.PasswordChar = '\0';
            this.username.PlaceholderText = "";
            this.username.SelectedText = "";
            this.username.SelectionStart = 8;
            this.username.ShadowDecoration.Parent = this.username;
            this.username.Size = new System.Drawing.Size(249, 36);
            this.username.TabIndex = 1;
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            this.username.Enter += new System.EventHandler(this.Username_Enter);
            this.username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_KeyDown);
            this.username.Leave += new System.EventHandler(this.Username_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(93)))), ((int)(((byte)(95)))));
            this.label2.Location = new System.Drawing.Point(8, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "Username";
            // 
            // password
            // 
            this.password.BorderColor = System.Drawing.Color.Gray;
            this.password.BorderRadius = 10;
            this.password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.password.DefaultText = "Password";
            this.password.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.password.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.password.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.password.DisabledState.Parent = this.password;
            this.password.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.password.FillColor = System.Drawing.Color.LightGray;
            this.password.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.password.FocusedState.Parent = this.password;
            this.password.ForeColor = System.Drawing.Color.Black;
            this.password.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.password.HoverState.Parent = this.password;
            this.password.Location = new System.Drawing.Point(3, 111);
            this.password.Name = "password";
            this.password.PasswordChar = '\0';
            this.password.PlaceholderText = "";
            this.password.SelectedText = "";
            this.password.SelectionStart = 8;
            this.password.ShadowDecoration.Parent = this.password;
            this.password.Size = new System.Drawing.Size(249, 36);
            this.password.TabIndex = 2;
            this.password.Tag = "";
            this.password.UseSystemPasswordChar = true;
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged_1);
            this.password.Enter += new System.EventHandler(this.password_Enter);
            this.password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.password_KeyDown);
            this.password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.password_KeyPress);
            this.password.Leave += new System.EventHandler(this.password_Leave);
            this.password.MouseEnter += new System.EventHandler(this.password_Enter);
            // 
            // guna2GradientButton1
            // 
            this.guna2GradientButton1.BorderRadius = 15;
            this.guna2GradientButton1.CheckedState.Parent = this.guna2GradientButton1;
            this.guna2GradientButton1.CustomImages.Parent = this.guna2GradientButton1;
            this.guna2GradientButton1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(93)))), ((int)(((byte)(95)))));
            this.guna2GradientButton1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.guna2GradientButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GradientButton1.ForeColor = System.Drawing.Color.White;
            this.guna2GradientButton1.HoverState.Parent = this.guna2GradientButton1;
            this.guna2GradientButton1.Location = new System.Drawing.Point(18, 199);
            this.guna2GradientButton1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2GradientButton1.Name = "guna2GradientButton1";
            this.guna2GradientButton1.ShadowDecoration.Parent = this.guna2GradientButton1;
            this.guna2GradientButton1.Size = new System.Drawing.Size(219, 36);
            this.guna2GradientButton1.TabIndex = 3;
            this.guna2GradientButton1.Text = "Login";
            this.guna2GradientButton1.Click += new System.EventHandler(this.guna2GradientButton1_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.Image = global::PMTTS_Feb_10_2023.Properties.Resources.reload_media2_1_e1628075381997;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(125, 29);
            this.guna2CirclePictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.ShadowDecoration.Parent = this.guna2CirclePictureBox1;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(87, 76);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2CirclePictureBox1.TabIndex = 14;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbltimer
            // 
            this.lbltimer.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltimer.Location = new System.Drawing.Point(139, 63);
            this.lbltimer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbltimer.Name = "lbltimer";
            this.lbltimer.Size = new System.Drawing.Size(52, 19);
            this.lbltimer.TabIndex = 16;
            this.lbltimer.Text = "00:00:00";
            this.lbltimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbltotal
            // 
            this.lbltotal.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltotal.Location = new System.Drawing.Point(156, 66);
            this.lbltotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbltotal.Name = "lbltotal";
            this.lbltotal.Size = new System.Drawing.Size(35, 16);
            this.lbltotal.TabIndex = 79;
            this.lbltotal.Text = "000";
            this.lbltotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblcorrect
            // 
            this.lblcorrect.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcorrect.ForeColor = System.Drawing.Color.Red;
            this.lblcorrect.Location = new System.Drawing.Point(36, 115);
            this.lblcorrect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblcorrect.Name = "lblcorrect";
            this.lblcorrect.Size = new System.Drawing.Size(266, 19);
            this.lblcorrect.TabIndex = 80;
            this.lblcorrect.Text = "INCORRENT USERNAME OR PASSWORD!";
            this.lblcorrect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2GradientButton2
            // 
            this.guna2GradientButton2.BorderRadius = 15;
            this.guna2GradientButton2.CheckedState.Parent = this.guna2GradientButton2;
            this.guna2GradientButton2.CustomImages.Parent = this.guna2GradientButton2;
            this.guna2GradientButton2.FillColor = System.Drawing.Color.Transparent;
            this.guna2GradientButton2.FillColor2 = System.Drawing.Color.Transparent;
            this.guna2GradientButton2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GradientButton2.ForeColor = System.Drawing.Color.Black;
            this.guna2GradientButton2.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(93)))), ((int)(((byte)(95)))));
            this.guna2GradientButton2.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.guna2GradientButton2.HoverState.Parent = this.guna2GradientButton2;
            this.guna2GradientButton2.Location = new System.Drawing.Point(92, 431);
            this.guna2GradientButton2.Margin = new System.Windows.Forms.Padding(2);
            this.guna2GradientButton2.Name = "guna2GradientButton2";
            this.guna2GradientButton2.ShadowDecoration.Parent = this.guna2GradientButton2;
            this.guna2GradientButton2.Size = new System.Drawing.Size(154, 36);
            this.guna2GradientButton2.TabIndex = 15;
            this.guna2GradientButton2.Text = "LEARN MORE";
            this.guna2GradientButton2.Click += new System.EventHandler(this.guna2GradientButton2_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(338, 488);
            this.Controls.Add(this.guna2GradientButton2);
            this.Controls.Add(this.lblcorrect);
            this.Controls.Add(this.guna2CirclePictureBox1);
            this.Controls.Add(this.lbltotal);
            this.Controls.Add(this.lbltimer);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Login_Load);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).EndInit();
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBox1;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox username;
        private Label label2;
        private Guna.UI2.WinForms.Guna2TextBox password;
        private Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private System.Windows.Forms.Timer timer1;

        private void password_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton2;

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtu.be/DuJcVRmUbl4");
        }



        //public SqlConnection Contest { get; set; } = new Connection();
    }
}
