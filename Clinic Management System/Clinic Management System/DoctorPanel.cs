using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic_Management_System
{
    public partial class DoctorPanel : Form
    {
        int acc_id;
        public DoctorPanel(int id)
        {
            InitializeComponent();
            acc_id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            EditProfile edp = new EditProfile(acc_id);
            edp.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ViewReservations vr = new ViewReservations(acc_id);
            vr.ShowDialog();
            Show();
        }

        private void DoctorPanel_Load(object sender, EventArgs e)
        {
            
        }
    }
}
