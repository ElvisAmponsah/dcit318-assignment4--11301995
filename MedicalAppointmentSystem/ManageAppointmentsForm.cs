using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class ManageAppointmentsForm : Form
    {
        private string connectionString;
        private DataSet appointmentsDataSet;

        public ManageAppointmentsForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
            appointmentsDataSet = new DataSet();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT a.AppointmentID, d.FullName AS DoctorName, p.FullName AS PatientName, 
                                   a.AppointmentDate, a.Notes 
                                   FROM Appointments a 
                                   INNER JOIN Doctors d ON a.DoctorID = d.DoctorID 
                                   INNER JOIN Patients p ON a.PatientID = p.PatientID 
                                   ORDER BY a.AppointmentDate";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        appointmentsDataSet.Clear();
                        adapter.Fill(appointmentsDataSet, "Appointments");
                        dataGridViewAppointments.DataSource = appointmentsDataSet.Tables["Appointments"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count > 0)
            {
                UpdateAppointment();
            }
            else
            {
                MessageBox.Show("Please select an appointment to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateAppointment()
        {
            try
            {
                DataRow selectedRow = ((DataRowView)dataGridViewAppointments.SelectedRows[0].DataBoundItem).Row;
                int appointmentId = Convert.ToInt32(selectedRow["AppointmentID"]);
                DateTime newDate = dateTimePickerNewDate.Value;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Appointments SET AppointmentDate = @AppointmentDate WHERE AppointmentID = @AppointmentID";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentDate", newDate);
                        command.Parameters.AddWithValue("@AppointmentID", appointmentId);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAppointments();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteAppointment();
                }
            }
            else
            {
                MessageBox.Show("Please select an appointment to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteAppointment()
        {
            try
            {
                DataRow selectedRow = ((DataRowView)dataGridViewAppointments.SelectedRows[0].DataBoundItem).Row;
                int appointmentId = Convert.ToInt32(selectedRow["AppointmentID"]);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", appointmentId);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Appointment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAppointments();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count > 0)
            {
                DataRow selectedRow = ((DataRowView)dataGridViewAppointments.SelectedRows[0].DataBoundItem).Row;
                if (selectedRow["AppointmentDate"] != DBNull.Value)
                {
                    dateTimePickerNewDate.Value = Convert.ToDateTime(selectedRow["AppointmentDate"]);
                }
            }
        }
    }
}

