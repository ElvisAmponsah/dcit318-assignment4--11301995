using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class DoctorListForm : Form
    {
        private string connectionString;

        public DoctorListForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Use SqlCommand with CommandType.Text to fetch doctors
                    string query = "SELECT DoctorID, FullName, Specialty, Availability FROM Doctors ORDER BY FullName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridViewDoctors.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

