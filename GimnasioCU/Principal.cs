using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace GimnasioCU
{
    public partial class Principal : Form
	{
        String con = "Data Source = DELL; Initial Catalog = gimnasio; Integrated Security = True";

        public Principal()
		{
			InitializeComponent();

            txtMatricula.MaxLength = 6;
		}

        void PopulateDataGridView()
        {
            using(SqlConnection sqlcon = new SqlConnection(con))
            {
                sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM Devolucion", sqlcon);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                dgvDevolucion.DataSource = dtbl;
            }
        }

		private void btnSalir_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnMaximizar_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Maximized;
			btnMaximizar.Visible = false;
			btnVentana.Visible = true;
		}

		private void btnVentana_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
			btnVentana.Visible = false;
			btnMaximizar.Visible = true;
		}

		private void btnMinimizar_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}


		[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
		private extern static void ReleaseCapture();
		[DllImport("user32.DLL", EntryPoint = "SendMessage")]

		private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

		private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(this.Handle, 0x112, 0xf012, 0);
		}

        private void btnServicios_Click(object sender, EventArgs e)
        {
            Servicios settingsForm = new Servicios();
            settingsForm.Show();
            this.Hide();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            String Confirmar;// Confirmar2;

            String Nombre = txtNombre.Text;
            String Programa = cBoxPrograma.GetItemText(cBoxPrograma.SelectedItem);
            String Matricula = txtMatricula.Text;
            String Servicio = cBoxServicio.GetItemText(cBoxServicio.SelectedItem);
            String Sexo;

            if (Masculino.Checked)
            {
                Sexo = Masculino.Text;
            }
            else
            {
                Sexo = Femenino.Text;
            }

           /* DateTime myDateTime = DateTime.Now;
            string Fecha = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");*/

            Confirmar = "INSERT INTO Devolucion VALUES(" + "'" + Matricula + "'" + "," + "'" + Nombre + "'" + "," + "'" + Programa + "'" + "," + "'" + Servicio + "'" + "," + "'" + Sexo + "');";
            //Confirmar2 = "INSERT INTO Prestamos VALUES(" + "'" + Matricula + "'" + "," + "'" + Nombre + "'" + "," + "'" + Programa + "'" + "," + "'" + Servicio + "'" + "," + "'" + Sexo + "," + "'" + Fecha + "');";

            SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                sqlcon.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("No se realizó la conexión");
                this.Close();
            }

            SqlCommand myCommand = new SqlCommand(Confirmar, sqlcon);
            int i = myCommand.ExecuteNonQuery();
            MessageBox.Show("Se guardo el registro");
            sqlcon.Close();
            PopulateDataGridView();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            hora.Text = DateTime.Now.ToShortTimeString();
            fecha.Text = DateTime.Now.ToLongDateString();
            timer1.Start();
        }

        private void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            contador.Text = txtMatricula.Text.Length.ToString();
        }

        private void txtMatricula_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back) && !char.IsWhiteSpace(e.KeyChar);
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void dgvDevolucion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDevolucion.Columns["fnDevolucion"].Index)
            {
                if (dgvDevolucion.CurrentRow.Cells["fillActivos"].Value != DBNull.Value)
                {
                    if(MessageBox.Show("¿Estás seguro de terminar el servicio?", "Devolución", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using(SqlConnection sqlcon = new SqlConnection(con))
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("EliminarActivosID", sqlcon);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ActivoID", Convert.ToInt32(dgvDevolucion.CurrentRow.Cells["fillActivos"].Value));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                PopulateDataGridView();
            }
        }

        private void btnConsultas_Click(object sender, EventArgs e)
        {
            Consultas settingsForm = new Consultas();
            settingsForm.Show();
            this.Hide();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            Reportes settingsForm = new Reportes();
            settingsForm.Show();
            this.Hide();
        }
    }
}