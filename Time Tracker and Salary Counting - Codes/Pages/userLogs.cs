using PMTTS_Feb_10_2023.Class_Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PMTTS_Feb_10_2023.Pages
{
    public partial class userLogs : Form
    {
        SqlConnection conn1;
        SqlCommand cmd;
        ConnectionDB db = new ConnectionDB();
        SqlDataReader dr;
        public userLogs()
        {
            InitializeComponent();
            conn1 = new SqlConnection(db.GetConnection());
            id.Text = user_Tracker1.txt1;
        }
        private void userLogs_Load(object sender, EventArgs e)
        {
            string f = lblfname.Text;
            string a = lbllname.Text;
            lblout.Text = f + " " + a;
            readerData1();
            timer1.Start();  
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            LoadRecords();
            logsGrid.Show();
            ImagesGrid.Hide();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ImagesGrid.Show();
        }
        public void readerData1()
        {
            try
            {
                string db = "select * from Employee_Details where Employee_ID = '" + id.Text + "'";
                SqlCommand cmd = new SqlCommand(db, conn1);
                conn1.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string f = lblfname.Text = dr["First_name"].ToString();
                        string a = lbllname.Text = dr["Last_name"].ToString();
                        lblout.Text = f + " " + a;
                        lblposition.Text = dr["Employee_Position"].ToString();
                        byte[] img = (byte[])(dr[9]);
                        if (img == null)
                            picturecircle = null;
                        else
                        {
                            MemoryStream ms = new MemoryStream(img);
                            picturecircle.Image = Image.FromStream(ms);
                        }
                    }
                    conn1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void LoadRecords()
        {
            try
            {
                logsGrid.Rows.Clear();
                conn1.Open();
                cmd = new SqlCommand("SELECT * FROM Logs2 WHERE Employee_ID LIKE @id + '%' ORDER BY Employee_ID ASC", conn1);
                cmd.Parameters.AddWithValue("@id", id.Text);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    logsGrid.Rows.Add(dr["ID"].ToString(), dr["Employee_ID"].ToString(), dr["Login_Date"], dr["Login_Time"].ToString(), dr["Daily_time"].ToString(), dr["Logout_time"].ToString(), dr["DailySalary_Total"].ToString(), dr["WeeklyTime_Total"].ToString());
                    logsGrid.Sort(logsGrid.Columns[0], ListSortDirection.Descending);
                }
                dr.Close();
                conn1.Close();
                logsGrid.Columns[2].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
            catch (Exception ex)
            {
                conn1.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                ImagesGrid.Rows.Clear();
                conn1.Open();
                cmd = new SqlCommand("SELECT * FROM Screenwork WHERE Employee_ID LIKE @id + '%' ORDER BY Employee_ID ASC", conn1);
                cmd.Parameters.AddWithValue("@id", id.Text);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ImagesGrid.Rows.Add(dr["ID"].ToString(), dr["Employee_ID"].ToString(), dr["Login_Date"], dr["Login_Time"].ToString(), dr["Screen_Shot"]);
                    ImagesGrid.Sort(ImagesGrid.Columns[0], ListSortDirection.Descending);
                }
                dr.Close();
                conn1.Close();
            }
            catch (Exception ex)
            {
                conn1.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        Byte[] ImageByteArray;
        public static string txt1 = "";
        private void ImagesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = ImagesGrid.Columns[e.ColumnIndex].Name;
            if (colName == "colDel")
            {
                if (MessageBox.Show("DO YOU WANT TO DELETE THIS PICTURE?", "MESSAGE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn1.Open();
                    cmd = new SqlCommand("DELETE FROM Screenwork WHERE ID = '" + ImagesGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn1);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    MessageBox.Show("SCREENSHOT HAS BEEN SUCCESSFULLY DELETED.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
            else if (colName == "Screen_Shot")
            {
                Pages.Uss f2 = new Pages.Uss();
                byte[] ImageArray = (byte[])ImagesGrid.CurrentRow.Cells[4].Value;
                ImageByteArray = ImageArray;
                f2.SSimage.Image = Image.FromStream(new MemoryStream(ImageArray));
                f2.lbldate.Text = ImagesGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                f2.time.Text = ImagesGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                f2.ShowDialog();
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
        private void id_Click(object sender, EventArgs e)
        {

        }
        int rate1 = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rate1 > 0)
            {
                rate1 = rate1 - 1;
                label4.Text = rate1 + "";

            }

            if (label4.Text == "0")
            {
                rate1 = 3;
                LoadRecords();
                timer1.Stop();

            }
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            renderT.Text = "000:00:00";
            renderS.Text = "000.00";
            LoadRecords();
        }
        private void btnrender_Click_1(object sender, EventArgs e)
        {
            try
            {
                ImagesGrid.Rows.Clear();
                conn1.Open();
                cmd = new SqlCommand("SELECT * FROM Screenwork WHERE Employee_ID LIKE @id + '%' AND Login_Date BETWEEN @startDate AND @endDate ORDER BY Employee_ID ASC", conn1);
                cmd.Parameters.AddWithValue("@id", id.Text);
                cmd.Parameters.AddWithValue("@startDate", dateTimePicker_Start.Value.Date);
                cmd.Parameters.AddWithValue("@endDate", dateTimePicker_End.Value.Date);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ImagesGrid.Rows.Add(dr["ID"].ToString(), dr["Employee_ID"].ToString(), dr["Login_Date"], dr["Login_Time"].ToString(), dr["Screen_Shot"]);
                    ImagesGrid.Sort(ImagesGrid.Columns[0], ListSortDirection.Descending);
                }
                dr.Close();
                conn1.Close();
                logsGrid.Rows.Clear();
                conn1.Open();
                cmd = new SqlCommand("SELECT * FROM Logs2 WHERE Employee_ID = @id AND Login_Date BETWEEN @startDate AND @endDate ORDER BY Login_Date ASC", conn1);
                cmd.Parameters.AddWithValue("@id", id.Text);
                cmd.Parameters.AddWithValue("@startDate", dateTimePicker_Start.Value.Date);
                cmd.Parameters.AddWithValue("@endDate", dateTimePicker_End.Value.Date);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    logsGrid.Rows.Add(dr["ID"].ToString(), dr["Employee_ID"].ToString(), dr["Login_Date"], dr["Login_Time"].ToString(), dr["Daily_time"].ToString(), dr["Logout_time"].ToString(), dr["DailySalary_Total"].ToString(), dr["WeeklyTime_Total"].ToString());
                    logsGrid.Sort(logsGrid.Columns[0], ListSortDirection.Descending);
                }
                dr.Close();
                conn1.Close();
                renderS.Text = "0";
                for (int i = 0; i < logsGrid.Rows.Count; i++)
                {
                    renderS.Text = Convert.ToString(float.Parse(renderS.Text) + float.Parse(logsGrid.Rows[i].Cells[6].Value.ToString()));
                }
                TimeSpan sum;
                TimeSpan total = TimeSpan.Parse("000:00:00");
                for (int i = 0; i < logsGrid.Rows.Count; i++)
                {
                    string time = Convert.ToString(logsGrid.Rows[i].Cells[4].Value);
                    sum = TimeSpan.Parse(time);
                    total = total.Add(sum);
                }
                renderT.Text = total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
