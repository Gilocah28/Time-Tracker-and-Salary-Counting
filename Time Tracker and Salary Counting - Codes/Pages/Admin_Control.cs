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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PMTTS_Feb_10_2023.Pages
{
    public partial class Admin_Control : Form
    {
        SqlConnection conn1;
        SqlCommand cmd;
        ConnectionDB db = new ConnectionDB();
        SqlDataReader dr;
        public Admin_Control()
        {
            InitializeComponent();
            conn1 = new SqlConnection(db.GetConnection());
            LoadRecords();
        }
        public void LoadRecords()
        {
            try
            {
                employeeGrid.Rows.Clear();
                conn1.Open();
                cmd = new SqlCommand("SELECT * FROM Employee_Details WHERE CONCAT(First_name,Middle_name,Last_name,Employee_Position,Accountype) LIKE '%" + txtfsearch.Text+"%'", conn1);
                dr = cmd.ExecuteReader();


                while (dr.Read())
            {
                employeeGrid.Rows.Add(dr["Employee_ID"].ToString(), dr["Employee_Image"], dr["First_name"].ToString(), dr["Middle_name"].ToString(), dr["Last_name"].ToString(), dr["Username"].ToString(), dr["Password"].ToString(), dr["Employee_Position"].ToString(), dr["Hour_rate"].ToString(), dr["Accountype"].ToString(), dr["date1"]);
                employeeGrid.Sort(employeeGrid.Columns[0], ListSortDirection.Descending);
            }
                dr.Close();
                conn1.Close();
                employeeGrid.Columns[10].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
            catch (Exception ex)
            {
                conn1.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Admin_Control_Load(object sender, EventArgs e)
        {
            employeeGrid.Sort(employeeGrid.Columns[0], ListSortDirection.Descending);
        }
        private void addnew_Click(object sender, EventArgs e)
        {
            Pages.Add_Form f = new Pages.Add_Form(this);
            f.btnsave.Enabled = true;
            f.btnupdate.Enabled = false;
            f.label1.Hide();
            f.id.Hide();
            f.ShowDialog();
        }
        Byte[] ImageByteArray;
        private void employeeGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = employeeGrid.Columns[e.ColumnIndex].Name;
           
            if (colName == "colEdit")
            {
                Pages.Add_Form f = new Pages.Add_Form(this);
                f.id.Text = employeeGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                byte[] ImageArray = (byte[])employeeGrid.CurrentRow.Cells[1].Value;
                ImageByteArray = ImageArray;
                f.employee_images.Image = Image.FromStream(new MemoryStream(ImageArray));
                f.txtfname.Text = employeeGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.txtmname.Text = employeeGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                f.txtlname.Text = employeeGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.txtuname.Text = employeeGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                f.txtpass.Text = employeeGrid.Rows[e.RowIndex].Cells[6].Value.ToString();
                f.txtposition.Text = employeeGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.txtrate.Text = employeeGrid.Rows[e.RowIndex].Cells[8].Value.ToString();
                f.txttype.Text = employeeGrid.Rows[e.RowIndex].Cells[9].Value.ToString();
                f.btnsave.Enabled = false;
                f.btnupdate.Enabled = true;
                f.ShowDialog();
            }
            else if (colName == "colDel")
            {
                if (MessageBox.Show("DO YOU WANT TO DELETE THIS RECORD? IF YES, ALL RECORD HAS BEEN REMOVE! ", "MESSAGE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn1.Open();
                    cmd = new SqlCommand("DELETE FROM Employee_Details WHERE Employee_ID = '" + employeeGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn1);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    conn1.Open();
                    cmd = new SqlCommand("DELETE FROM Logs2 WHERE Employee_ID = '" + employeeGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn1);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    conn1.Open();
                    cmd = new SqlCommand("DELETE FROM Screenwork WHERE Employee_ID = '" + employeeGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn1);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    MessageBox.Show("RECORD HAS BEEN SUCCESSFULLY DELETED.","MESSAGE", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
            else if (colName == "colDetails")
            {
                Pages.Logs f = new Pages.Logs(this);
                f.id.Text = employeeGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                byte[] ImageArray = (byte[])employeeGrid.CurrentRow.Cells[1].Value;
                ImageByteArray = ImageArray;
                f.picturecircle.Image = Image.FromStream(new MemoryStream(ImageArray));
                f.lblfname.Text = employeeGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.lbllname.Text = employeeGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.lblposition.Text = employeeGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.ShowDialog();
            }
        }
        byte[] ConvertImageToBytes(Image img) 
        { 
            using(MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public Image ConvertByteArrayToImage(byte[] bytes)
        {
            using(MemoryStream ms = new MemoryStream()) 
            {
                return Image.FromStream(ms);
            }
        }
        private void txtfsearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }
    }
}
