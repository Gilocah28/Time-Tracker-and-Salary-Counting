using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using PMTTS_Feb_10_2023.Class_Connection;
using System.IO;
using System.Text.RegularExpressions;
using Guna.UI2.WinForms;

namespace PMTTS_Feb_10_2023.Pages
{
    public partial class Add_Form : Form
    {
        SqlConnection conn1;
        SqlCommand cmd;
        ConnectionDB db = new ConnectionDB();
        string imageUrl = null;
        Pages.Admin_Control f;
        Image DefaultImage;
        public Add_Form(Admin_Control f)
        {
            InitializeComponent();
            conn1 = new SqlConnection(db.GetConnection());
            this.f = f;
            DefaultImage = employee_images.Image;
            posrate();
        }
        private void SImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageUrl = ofd.FileName;
                    employee_images.Image = Image.FromFile(imageUrl);
                }
            }
        }
        private void clear()
        {
            id.Text = "00000000";
            txtfname.Clear();
            txtmname.Clear();
            txtlname.Clear();
            txtuname.Clear();
            txtpass.Clear();
            txtrate.Clear();
            employee_images.Image = DefaultImage;
        }

        public void posrate()
        {
            List<val> ft = new List<val>();
            ft.Add(new val() { rate = 112.00, position = "ASSISTANT HUMAN RESOURCE" });
            ft.Add(new val() { rate = 100.00, position = "COINLIST ADMINISTRATOR" });
            ft.Add(new val() { rate = 87.40, position = "COSTUMER SUPPORT" });
            ft.Add(new val() { rate = 87.40, position = "EMAIL MARKETER" });
            ft.Add(new val() { rate = 100.00, position = "FACEBOOK ADMINISTRATOR" });
            ft.Add(new val() { rate = 100.00, position = "GRAPHICS DESIGNER" });
            ft.Add(new val() { rate = 150.00, position = "HUMAN RESOURCE" });
            ft.Add(new val() { rate = 120.00, position = "SOCIAL MEDIA MANAGER" });
            ft.Add(new val() { rate = 100.00, position = "TEAM LEADER" });
            ft.Add(new val() { rate = 100.00, position = "TIKTOK ADMINISTRATOR" });
            ft.Add(new val() { rate = 100.00, position = "VIDEO EDITOR" });
            ft.Add(new val() { rate = 100.00, position = "VIRTUAL ASSISTANT" });
            ft.Add(new val() { rate = 100.00, position = "COSTUMER SUPPORT" });
            ft.Sort((x, y) => y.rate.CompareTo(x.rate));
            txtposition.DataSource = ft;
            txtposition.DisplayMember = "position";
        }
        private void txtposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            val ft1 = txtposition.SelectedItem as val;
            txtrate.Text = Convert.ToString(ft1.rate);
        }



        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtfname.Text == "" || txtmname.Text == "" || txtlname.Text == "" || txtuname.Text == "" || txttype.Text == "" || txtposition.Text == "" || txtrate.Text == "")
                {
                    MessageBox.Show("REQUIRED MISSING FIELD!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtpass.Text.Length < 8 || txtpass.Text.Length > 15)
                {
                    MessageBox.Show("Password must be between 8 and 15 characters long.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var input = txtpass.Text;
                if (input == "")
                {
                    MessageBox.Show("Password should not be empty");
                    return;
                }
                var HasNumber = new Regex(@"[0-9]+");
                var HasUpperChar = new Regex(@"[A-Z]+");
                var Lowercase = new Regex(@"[a-z]+");
                if (!HasNumber.IsMatch(input))
                {
                    MessageBox.Show("Password should contain at least one numeric value.");
                    return;
                }
                else if (!HasUpperChar.IsMatch(input))
                {
                    MessageBox.Show("Password should contain at least Upper case letter.");
                    return;
                }
                else if (!Lowercase.IsMatch(input))
                {
                    MessageBox.Show("Password should contain at least Lower case letter.");
                    return;
                }
                else
                {
                    Image img = employee_images.Image;
                    byte[] arr;
                    ImageConverter converter = new ImageConverter();
                    arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                    conn1.Open();
                    cmd = new SqlCommand("INSERT INTO Employee_Details (First_name,Middle_name,Last_name,Username,Password,Employee_Position,Hour_rate,Accountype,Employee_Image,photourl,date1) VALUES (@First_name,@Middle_name,@Last_name,@Username,@Password,@Employee_Position,@Hour_rate,@Accountype,@Employee_Image,@photourl,@date1)", conn1);
                    cmd.Parameters.AddWithValue("@First_name", txtfname.Text);
                    cmd.Parameters.AddWithValue("@Middle_name", txtmname.Text);
                    cmd.Parameters.AddWithValue("@Last_name", txtlname.Text);
                    cmd.Parameters.AddWithValue("@Username", txtuname.Text);
                    cmd.Parameters.AddWithValue("@Password", txtpass.Text);
                    cmd.Parameters.AddWithValue("@Employee_Position", txtposition.Text);
                    cmd.Parameters.AddWithValue("@Hour_rate", txtrate.Text);
                    cmd.Parameters.AddWithValue("@Accountype", txttype.Text);
                    cmd.Parameters.AddWithValue("@Employee_Image", arr);
                    cmd.Parameters.AddWithValue("@photourl", imageUrl);
                    cmd.Parameters.AddWithValue("@date1", DateTime.Now.Date);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    MessageBox.Show("NEW EMPLOYEE HAS BEEN SUCCESSFULLY ADDED.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.LoadRecords();
                    clear();
                }
            }
            catch (Exception ex)
            {
                conn1.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


           
        }
        private void employee_images_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        imageUrl = ofd.FileName;
                        employee_images.Image = Image.FromFile(imageUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                conn1.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Add_Form_Load(object sender, EventArgs e)
        {
            txtrate.Enabled = false;
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
               
                    if (txtfname.Text == "" || txtmname.Text == "" || txtlname.Text == "" || txtuname.Text == "" || txttype.Text == "" || txtposition.Text == "" || txtrate.Text == "")
                    {
                        MessageBox.Show("REQUIRED MISSING FIELD!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    if (txtpass.Text.Length < 8 || txtpass.Text.Length > 15)
                    {
                        MessageBox.Show("Password must be between 8 and 15 characters long.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    var input = txtpass.Text;
                    if (input == "")
                    {
                        MessageBox.Show("Password should not be empty");
                        return;
                    }
                    var HasNumber = new Regex(@"[0-9]+");
                    var HasUpperChar = new Regex(@"[A-Z]+");
                    var Lowercase = new Regex(@"[a-z]+");
                    if (!HasNumber.IsMatch(input))
                    {
                        MessageBox.Show("Password should contain at least one numeric value.");
                        return;
                    }
                    else if (!HasUpperChar.IsMatch(input))
                    {
                        MessageBox.Show("Password should contain at least Upper case letter.");
                        return;
                    }
                    else if (!Lowercase.IsMatch(input))
                    {
                        MessageBox.Show("Password should contain at least Lower case letter.");
                        return;
                    }
                    else
                    {

                        Image img1 = employee_images.Image;
                        byte[] arr = (byte[])new ImageConverter().ConvertTo(img1, typeof(byte[]));
                        conn1.Open();
                        cmd = new SqlCommand("UPDATE Employee_Details SET First_name = @First_name, Middle_name = @Middle_name, Last_name = @Last_name, Username = @Username, Password = @Password, Employee_Position = @Employee_Position, Hour_rate = @Hour_rate, Accountype = @Accountype, Employee_Image = @Employee_Image WHERE Employee_ID = @Employee_ID", conn1);
                        cmd.Parameters.AddWithValue("@Employee_ID", id.Text);
                        cmd.Parameters.AddWithValue("@First_name", txtfname.Text);
                        cmd.Parameters.AddWithValue("@Middle_name", txtmname.Text);
                        cmd.Parameters.AddWithValue("@Last_name", txtlname.Text);
                        cmd.Parameters.AddWithValue("@Username", txtuname.Text);
                        cmd.Parameters.AddWithValue("@Password", txtpass.Text);
                        cmd.Parameters.AddWithValue("@Employee_Position", txtposition.Text);
                        cmd.Parameters.AddWithValue("@Hour_rate", txtrate.Text);
                        cmd.Parameters.AddWithValue("@Accountype", txttype.Text);
                        cmd.Parameters.AddWithValue("@Employee_Image", arr);
                        cmd.ExecuteNonQuery();
                        conn1.Close();
                        MessageBox.Show("THE DETAILS HAS BEEN SUCCESSFULLY UPDATED.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        f.LoadRecords();
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                conn1.Close();
            }
        }
        byte[] ConvertImageToBytes(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public Image ConvertByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                return Image.FromStream(ms);
            }
        }

        private void guna2CheckBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                txtpass.UseSystemPasswordChar = false;
            }
            else
            {
                txtpass.UseSystemPasswordChar = true;
            }

        }
    }
}
