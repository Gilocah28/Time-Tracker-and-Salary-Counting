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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMTTS_Feb_10_2023.Pages
{
    public partial class accountpass : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True");
        SqlCommand cmd;
        ConnectionDB db = new ConnectionDB();
        string imageUrl = null;
        public accountpass()
        {
            InitializeComponent();
            id.Text = user_Tracker1.txt1;
        }
        private void accountpass_Load(object sender, EventArgs e)
        {
            readerData();
            txtposition.Enabled= false;
            txtrate.Enabled= false;
            txttype.Enabled= false;  
        }

        private void readerData()
        {
            try
            {
                string db = "select * from Employee_Details where Employee_ID = '" + id.Text + "'";
                SqlCommand cmd = new SqlCommand(db, con);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtfname.Text = dr["First_name"].ToString();
                        txtmname.Text = dr["Middle_name"].ToString();
                        txtlname.Text = dr["Last_name"].ToString();
                        txtuname.Text = dr["Username"].ToString();
                        txtpass.Text = dr["Password"].ToString();
                        txtposition.Text = dr["Employee_Position"].ToString();
                        txtrate.Text = dr["Hour_rate"].ToString();
                        txttype.Text = dr["Accountype"].ToString();
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
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("WANT TO UPDATE THIS RECORD?", "MESSAGE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (txtfname.Text == "" || txtmname.Text == "" || txtlname.Text == "" || txtuname.Text == "" || txtpass.Text == "" || txttype.Text == "" || txtposition.Text == "" || txtrate.Text == "")
                    {
                        MessageBox.Show("REQUIRED MISSING FIELD!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        byte[] arr;
                        ImageConverter converter = new ImageConverter();
                        arr = (byte[])converter.ConvertTo(img1, typeof(byte[]));
                        con.Open();
                        cmd = new SqlCommand("UPDATE Employee_Details SET First_name = @First_name, Middle_name = @Middle_name, Last_name = @Last_name, Username = @Username, Password = @Password, Employee_Position = @Employee_Position, Hour_rate = @Hour_rate, Accountype = @Accountype, Employee_Image = @Employee_Image WHERE Employee_ID = @Employee_ID", con);
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
                        con.Close();
                        MessageBox.Show("THE DETAILS HAS BEEN SUCCESSFULLY UPDATED, YOU NEED TO LOG OUT TO APPLY CHANGES", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
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
        private void employee_images_Click_1(object sender, EventArgs e)
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
