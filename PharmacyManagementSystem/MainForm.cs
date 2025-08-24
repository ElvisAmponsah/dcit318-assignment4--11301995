using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace PharmacyManagementSystem
{
    public partial class MainForm : Form
    {
        private string connectionString;

        public MainForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["PharmacyDBConnection"].ConnectionString;
            LoadAllMedicines();
        }

        private void LoadAllMedicines()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAllMedicines", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridViewMedicines.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading medicines: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            if (ValidateAddForm())
            {
                AddMedicine();
            }
        }

        private bool ValidateAddForm()
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Please enter medicine name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBoxCategory.Text))
            {
                MessageBox.Show("Please enter medicine category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(textBoxPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(textBoxQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void AddMedicine()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("AddMedicine", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", textBoxName.Text.Trim());
                        command.Parameters.AddWithValue("@Category", textBoxCategory.Text.Trim());
                        command.Parameters.AddWithValue("@Price", decimal.Parse(textBoxPrice.Text));
                        command.Parameters.AddWithValue("@Quantity", int.Parse(textBoxQuantity.Text));

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Medicine added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearAddForm();
                            LoadAllMedicines();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding medicine: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                SearchMedicine();
            }
            else
            {
                LoadAllMedicines();
            }
        }

        private void SearchMedicine()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SearchMedicine", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", textBoxSearch.Text.Trim());

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridViewMedicines.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching medicine: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dataGridViewMedicines.SelectedRows.Count > 0)
            {
                UpdateStock();
            }
            else
            {
                MessageBox.Show("Please select a medicine to update stock.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateStock()
        {
            try
            {
                DataRow selectedRow = ((DataRowView)dataGridViewMedicines.SelectedRows[0].DataBoundItem).Row;
                int medicineId = Convert.ToInt32(selectedRow["MedicineID"]);
                int currentQuantity = Convert.ToInt32(selectedRow["Quantity"]);

                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Update Stock", currentQuantity.ToString());
                if (int.TryParse(input, out int newQuantity) && newQuantity >= 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("UpdateStock", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@MedicineID", medicineId);
                            command.Parameters.AddWithValue("@Quantity", newQuantity);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAllMedicines();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            if (dataGridViewMedicines.SelectedRows.Count > 0)
            {
                RecordSale();
            }
            else
            {
                MessageBox.Show("Please select a medicine to record sale.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RecordSale()
        {
            try
            {
                DataRow selectedRow = ((DataRowView)dataGridViewMedicines.SelectedRows[0].DataBoundItem).Row;
                int medicineId = Convert.ToInt32(selectedRow["MedicineID"]);
                int availableQuantity = Convert.ToInt32(selectedRow["Quantity"]);

                if (availableQuantity <= 0)
                {
                    MessageBox.Show("This medicine is out of stock.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string input = Microsoft.VisualBasic.Interaction.InputBox($"Enter quantity to sell (max: {availableQuantity}):", "Record Sale", "1");
                if (int.TryParse(input, out int quantityToSell) && quantityToSell > 0 && quantityToSell <= availableQuantity)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("RecordSale", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@MedicineID", medicineId);
                            command.Parameters.AddWithValue("@QuantitySold", quantityToSell);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Sale recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAllMedicines();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording sale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadAllMedicines();
        }

        private void ClearAddForm()
        {
            textBoxName.Clear();
            textBoxCategory.Clear();
            textBoxPrice.Clear();
            textBoxQuantity.Clear();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                LoadAllMedicines();
            }
        }
    }
}

