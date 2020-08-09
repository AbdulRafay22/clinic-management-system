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
    public partial class EditProfile : Form
    {
        int account_id;
        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);

        public EditProfile(int account_id)
        {
            InitializeComponent();
            this.account_id = account_id;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please Enter Name");
                return;
            }


            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE account set account_name = @name, account_dob = @dob, account_note = @notes, account_phone = @phone WHERE account_id = @account_id";
            cmd.Parameters.AddWithValue("@name", textBox3.Text);
            cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Value.ToString());
            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
            cmd.Parameters.AddWithValue("@notes", textBox6.Text);
            cmd.Parameters.AddWithValue("@account_id", account_id);

            con.Open();
            if (cmd.ExecuteNonQuery() > 0)
                MessageBox.Show("Account Updated");
            else
                MessageBox.Show("Account not updated");

            con.Close();
        }

        
        private void EditProfile_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT user_username, account_name, account_dob, account_phone, account_type, account_note, account_creation_date FROM [user], account WHERE account_user_id = user_id AND account_id = @account_id";
            cmd.Parameters.AddWithValue("@account_id", account_id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                textBox1.Text = account_id.ToString();
                textBox2.Text = reader.GetValue(0).ToString();
                textBox3.Text = reader.GetValue(1).ToString();

                try
                {
                    dateTimePicker1.Value = DateTime.Parse(reader.GetValue(2).ToString());
                }
                catch (Exception)
                {

                }
                textBox4.Text = reader.GetValue(3).ToString();
                if (reader.GetInt32(4) == 0)
                    textBox5.Text = "Secretary";
                
                else if (reader.GetInt32(4) == 1)
                    textBox5.Text = "Doctor";
                else if (reader.GetInt32(4) == 2)
                    textBox5.Text = "Patient";

                textBox6.Text = reader.GetValue(5).ToString();
                textBox7.Text = reader.GetValue(6).ToString();
            }

            con.Close();

        }
    }
}
