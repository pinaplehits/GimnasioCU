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
    public partial class Servicios : Form
    {
        public Servicios()
        {
            InitializeComponent();
        }

        String con = "Data Source = DELL; Initial Catalog = gimnasio; Integrated Security = True";

        void PopulateDataGridView()
        {
            using (SqlConnection sqlcon = new SqlConnection(con))
            {
                sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM Servicios", sqlcon);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
            }
        }
        private void btnVentana_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnVentana.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnVentana.Visible = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            Principal settingsForm = new Principal();
            settingsForm.Show();
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back) && !char.IsWhiteSpace(e.KeyChar);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            String Confirmar;

            String Servicio = txtAgregar.Text;

            Confirmar = "INSERT INTO Servicios VALUES (" + "'" + Servicio + "');";

            SqlConnection sqlcon = new SqlConnection(con);

            if (txtAgregar.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Por favor verifique que todos los campos estén llenados");
                return;
            }
            else
            {
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
                MessageBox.Show("Se agrego el servicio");
                sqlcon.Close();
            }
            PopulateDataGridView();
        }

        private void Servicios_Load(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Eliminar"].Index)
            {
                if (dataGridView1.CurrentRow.Cells["fillServiciosID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("¿Estás seguro de eliminar el servicio?", "Eliminar servicio", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection sqlcon = new SqlConnection(con))
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("EliminarServicios", sqlcon);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ServicioID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["fillServiciosID"].Value));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                PopulateDataGridView();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                using(SqlConnection sqlcon = new SqlConnection(con))
                {
                    sqlcon.Open();
                    DataGridViewRow dvgRow = dataGridView1.CurrentRow;
                    SqlCommand cmd = new SqlCommand("Editar", sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Servicio", dvgRow.Cells["fillServicio"].Value == DBNull.Value ? "" : dvgRow.Cells["fillServicio"].Value.ToString());
                    cmd.ExecuteNonQuery();
                }
                PopulateDataGridView();
            }
        }
    }
}
