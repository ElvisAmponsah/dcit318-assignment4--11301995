using System;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnViewDoctors_Click(object sender, EventArgs e)
        {
            var doctorForm = new DoctorListForm();
            doctorForm.Show();
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            var appointmentForm = new AppointmentForm();
            appointmentForm.Show();
        }

        private void btnManageAppointments_Click(object sender, EventArgs e)
        {
            var manageForm = new ManageAppointmentsForm();
            manageForm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

