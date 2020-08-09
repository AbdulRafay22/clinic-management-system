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

namespace Clinic_Management_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = Clinic_Management_System.Properties.Resources.connectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT user_id From [user] WHERE user_username = @username AND user_password = @password";
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);

            con.Open();
            var result = cmd.ExecuteScalar();
            con.Close();

            if (result != null)
            {
                if (textBox1.Text == "admin")
                {
                    //adminPanel
                    Hide();
                    AdminPanel adp = new AdminPanel();
                    adp.ShowDialog();
                    Show();
                }
                else
                {
                    con.Open();
                    cmd.CommandText = "SELECT account_id, account_type FROM account WHERE account_user_id =@user_id";
                    cmd.Parameters.AddWithValue("@user_id", result.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        int acc_id = reader.GetInt32(0);
                        int acc_type = reader.GetInt32(1);

                        con.Close();

                        if (acc_type == 0)
                        {
                            //secretary panel
                            Hide();
                            SecretaryPanel scp = new SecretaryPanel(acc_id);
                            scp.ShowDialog();
                            Show();
                        }
                        else if (acc_type == 1)
                        {
                            // doctor panel
                            Hide();
                            DoctorPanel drp = new DoctorPanel(acc_id);
                            drp.ShowDialog();
                            Show();
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Authentication Failed");
            }


        }
    }
}
