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
    public partial class CreateReservation : Form
    {
        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
        SqlCommand cmd;
        int sec_id;
        public CreateReservation(int id)
        {
            InitializeComponent();
            sec_id = id;
        }

        private void updateList(string query)
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT account_id, account_name, account_type FROM account WHERE account_type = 2 AND (account_name LIKE @query OR account_phone LIKE @query)";
            cmd.Parameters.AddWithValue("@query", query + "%");

            con.Open();
            listBox1.Items.Clear();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(new account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));

            }


            con.Close();
        }

        private void CreateReservation_Load(object sender, EventArgs e)
        {
            updateList("");
            updateSlots();
            dateTimePicker1.MinDate = DateTime.Today;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox1.Text);
        }


        private void updateSlots()
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT reservation_visit_slot FROM reservation WHERE reservation_visit_date = @date";
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            Dictionary<int, string> slots = utils.getSlots();

            while (reader.Read())
            {
                slots.Remove(reader.GetInt32(0));
            }

            comboBox1.Items.Clear();

            foreach (object slot in slots.ToArray())
                comboBox1.Items.Add(slot);

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            con.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateSlots();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //validation
            if (listBox1.SelectedIndex < 0 || listBox1.SelectedIndex > listBox1.Items.Count)
            {
                MessageBox.Show("Please select a patient");
                return;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a slot");
                return;
            }


            //reserve
            int pat_id = ((account)listBox1.SelectedItem).getID();
            int slot = ((KeyValuePair<int, string>) comboBox1.SelectedItem).Key;

            cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO reservation (reservation_secretary_id, reservation_patient_id, reservation_visit_date, reservation_visit_slot, reservation_date) VALUES(@secretary_id, @patient_id, @visit_date, @visit_slot, @date)";
            cmd.Parameters.AddWithValue("@secretary_id", sec_id);
            cmd.Parameters.AddWithValue("@patient_id", pat_id);
            cmd.Parameters.AddWithValue("@visit_date", dateTimePicker1.Value.ToString());
            cmd.Parameters.AddWithValue("@visit_slot", slot);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            con.Open();

            if (cmd.ExecuteNonQuery() > 0)
            {
                //succesfull
                cmd.CommandText = "SELECT reservation_id FROM reservation WHERE reservation_visit_date = @visit_date AND reservation_visit_slot = @visit_slot";
                int res_id = (int)cmd.ExecuteScalar();

                MessageBox.Show("Rservation Complete");
                MessageBox.Show("Reservation ID: " + res_id.ToString());
            }

            else
                MessageBox.Show("Failed to perform reservation");


            con.Close();

            updateSlots();
        }

    }
}
