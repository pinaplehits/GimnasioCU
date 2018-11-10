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
		public Principal()
		{
			InitializeComponent();

            txtMatricula.MaxLength = 6;
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
            SqlConnection con = new SqlConnection("Data Source = DELL; Initial Catalog = gimnasio; Integrated Security = True"); try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se realizó la conexión");
                this.Close();
            }

            String Confirmar;

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


            Confirmar = "INSERT INTO activos VALUES(" + "'" + Matricula + "'" + "," + "'" + Nombre + "'" + "," + "'" + Programa + "'" + "," + "'" + Servicio + "'" + "," + "'" + Sexo + "');";

            SqlCommand myCommand = new SqlCommand(Confirmar, con);
            int i = myCommand.ExecuteNonQuery();
            MessageBox.Show("Se guardo el registro");
            con.Close();
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
    }
}
