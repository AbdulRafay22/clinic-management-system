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
    public partial class AdminPanel : Form
    {

        public AdminPanel()
        {
            InitializeComponent();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }


        private void updateList(string query)
        {
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "SELECT account_id, account_name, account_type FROM account WHERE account_type in (0, 1) and (account_name LIKE @query OR account_phone LIKE @query) ORDER BY account_type";
            cmd.Parameters.AddWithValue("@query" , query + "%");

            SqlDataReader reader = cmd.ExecuteReader();

            listBox1.Items.Clear();
            while (reader.Read())
                listBox1.Items.Add(new account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));


            con.Close();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            updateList("");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox4.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int account_id;
            try
            {
                account_id = ((account)listBox1.SelectedItem).getID();

            }
            catch (Exception)
            {
                return;
            }

            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT user_username, account_name, account_dob, account_phone, account_type, account_note, account_creation_date FROM [user], account WHERE user_id = account_user_id AND account_id = @id";
            cmd.Parameters.AddWithValue("@id", account_id);
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                textBox5.Text = account_id.ToString();
                textBox6.Text = reader.GetValue(0).ToString();
                textBox7.Text = reader.GetValue(1).ToString();
                textBox8.Text = reader.GetValue(2).ToString();
                textBox9.Text = reader.GetValue(3).ToString();

                if (reader.GetInt32(4) == 0)
                    textBox10.Text = "Secretary";
                else
                    textBox10.Text = "Doctor";

                textBox11.Text = reader.GetValue(5).ToString();
                textBox12.Text = reader.GetValue(6).ToString();
            }

            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!validateInputs())
            {
                MessageBox.Show("Please check the inputs fields");
                return;
            }
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO [user] (user_username, user_password) VALUES(@username, @password)";
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            con.Open();
            if (cmd.ExecuteNonQuery() > 0)
            {
                //create the record
                cmd.CommandText = "SELECT user_id FROM [user] WHERE user_username = @username";
                int user_id = (int) cmd.ExecuteScalar();

                cmd.CommandText = "INSERT INTO account (account_user_id, account_name, account_dob, account_phone, account_type, account_note, account_creation_date) VALUES (@user_id, @name, @dob, @phone, @type, @notes, @date)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@name", textBox3.Text);
                cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@phone", textBox14.Text.ToString());
                cmd.Parameters.AddWithValue("@type", comboBox1.SelectedIndex);
                cmd.Parameters.AddWithValue("@notes", textBox4.Text);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    //account is created
                    MessageBox.Show("Account created succesfully");
                }

                else
                {
                    MessageBox.Show("Error craeting account");
                }


            }

            else
            {
                MessageBox.Show("Error craeting user");
            }


            con.Close();

            updateList("");
        }

        private bool validateInputs()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                return false;

            if (comboBox1.SelectedIndex < 0)
                return false;

            return true;
        }


        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            Char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
                MessageBox.Show("Please enter numbers only");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
                return;
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM [user] WHERE user_username = @username";
            cmd.Parameters.AddWithValue("@username", textBox6.Text);
            con.Open();
            if (cmd.ExecuteNonQuery() > 0)
                MessageBox.Show("Account deleted");
            else
                MessageBox.Show("Account not deleted");



            con.Close();
            updateList("");
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
        }
    }
}
