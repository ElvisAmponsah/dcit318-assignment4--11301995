using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class AppointmentForm : Form
    {
        private string connectionString;

        public AppointmentForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
            LoadDoctors();
            LoadPatients();
        }

        private void LoadDoctors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DoctorID, FullName FROM Doctors WHERE Availability = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxDoctors.Items.Add(new DoctorItem
                                {
                                    DoctorID = reader.GetInt32(0),
                                    FullName = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatients()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT PatientID, FullName FROM Patients ORDER BY FullName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxPatients.Items.Add(new PatientItem
                                {
                                    PatientID = reader.GetInt32(0),
                                    FullName = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                BookAppointment();
            }
        }

        private bool ValidateForm()
        {
            if (comboBoxDoctors.SelectedItem == null)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBoxPatients.SelectedItem == null)
            {
                MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dateTimePickerAppointment.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BookAppointment()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES (@DoctorID, @PatientID, @AppointmentDate, @Notes)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var doctorItem = (DoctorItem)comboBoxDoctors.SelectedItem;
                        var patientItem = (PatientItem)comboBoxPatients.SelectedItem;

                        command.Parameters.AddWithValue("@DoctorID", doctorItem.DoctorID);
                        command.Parameters.AddWithValue("@PatientID", patientItem.PatientID);
                        command.Parameters.AddWithValue("@AppointmentDate", dateTimePickerAppointment.Value);
                        command.Parameters.AddWithValue("@Notes", textBoxNotes.Text);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Appointment booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error booking appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            comboBoxDoctors.SelectedIndex = -1;
            comboBoxPatients.SelectedIndex = -1;
            dateTimePickerAppointment.Value = DateTime.Now.AddDays(1);
            textBoxNotes.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private class DoctorItem
        {
            public int DoctorID { get; set; }
            public string FullName { get; set; }
            public override string ToString() => FullName;
        }

        private class PatientItem
        {
            public int PatientID { get; set; }
            public string FullName { get; set; }
            public override string ToString() => FullName;
        }
    }
}

