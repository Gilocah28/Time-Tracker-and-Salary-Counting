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
    public partial class Screenshot : Form
    {
        Pages.Logs ff;
        public Screenshot(Logs ff)
        {
            InitializeComponent();
            this.ff = ff;
        }
        private void Screenshot_Load(object sender, EventArgs e)
        {
        }
    }
}
