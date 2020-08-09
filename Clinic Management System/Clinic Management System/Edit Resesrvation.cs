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
    public partial class Edit_Resesrvation : Form
    {
        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
        SqlCommand cmd;

        public Edit_Resesrvation(reservation res)
        {
            InitializeComponent();

            

            dateTimePicker1.Value = res.visit_date;

           
            textBox1.Text = res.id.ToString();
            textBox2.Text = res.patient.ToString();
            textBox3.Text = res.secretary.ToString();
            textBox4.Text = res.date.ToString();


            dateTimePicker1.MinDate = DateTime.Today;

            updateCombo(res.slot);


        }

        private void updateCombo(int visit_slot)
        {
            Dictionary<int, string> slots = utils.getSlots();


            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT reservation_visit_slot FROM reservation WHERE reservation_visit_date = @date AND reservation_id <> @id";
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@id", textBox1.Text);
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                slots.Remove(reader.GetInt32(0));
            }
            comboBox1.Items.Clear();

            foreach (KeyValuePair<int, string> slot in slots)
            {
                comboBox1.Items.Add(slot);
                if (slot.Key == visit_slot)
                    comboBox1.SelectedItem = slot;
            }
            con.Close();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE reservation SET reservation_visit_date = @visit_date, reservation_visit_slot = @visit_slot WHERE reservation_id = @id";
            cmd.Parameters.AddWithValue("@id", textBox1.Text);
            cmd.Parameters.AddWithValue("@visit_date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@visit_slot", ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key);

            con.Open();

            if (cmd.ExecuteNonQuery() > 0)
                MessageBox.Show("Reservation Updated");
            else
                MessageBox.Show("Failed updating reservation");


            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM reservation WHERE reservation_id = @id";
            cmd.Parameters.AddWithValue("@id", textBox1.Text);

            con.Open();
            if (cmd.ExecuteNonQuery() > 0)
                MessageBox.Show("Reservation Deleted");
            else
                MessageBox.Show("Failed to delete reservation");


            con.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateCombo(1);
        }
    }
}
