using PMTTS_Feb_10_2023.Class_Connection;
using PMTTS_Feb_10_2023.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMTTS_Feb_10_2023.Pages
{
    public partial class frmSettingsUser : Form
    {
        public frmSettingsUser()
        {
            InitializeComponent();
            valuedate();
        }
        public void valuedate()
        {
            List<val> ft = new List<val>();
            ft.Add(new val() { minutes = 600, name = "10 Minutes" });
            ft.Add(new val() { minutes = 540, name = "9 Minutes" });
            ft.Add(new val() { minutes = 480, name = "8 Minutes" });
            ft.Add(new val() { minutes = 420, name = "7 Minutes" });
            ft.Add(new val() { minutes = 360, name = "6 Minutes" });
            ft.Add(new val() { minutes = 300, name = "5 Minutes" });
            guna2ComboBox1.DataSource = ft;
            guna2ComboBox1.DisplayMember = "Name";
        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            val ft1 = guna2ComboBox1.SelectedItem as val;
            label2.Text = Convert.ToString(ft1.minutes);
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Settings.Default.nameS_U = guna2ComboBox1.Text;
            Settings.Default.minS_U = label2.Text;
            Settings.Default.Save();
            MessageBox.Show("YOU NEED TO LOGOUT TO APPLY THE SCREENSHOT MINUTES CHANGES.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void frmSettingsUser_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.Text = Settings.Default.nameS_U;
            label2.Text = Settings.Default.minS_U;
        }
    }
}
