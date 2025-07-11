using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRUDPersonasWinForms.Models;
using CRUDPersonasWinForms.DAO;


namespace CRUDPersonasWinForms
{
    public partial class frnPersonasBD : Form
    {
        PersonaDAO dao = new PersonaDAO();
        public frnPersonasBD()
        {
            InitializeComponent();
        }
        private void CargarPersonas()
        {
            dgvPersonas.DataSource = null;
            dgvPersonas.DataSource = dao.ObtenerTodas();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Persona persona = new Persona
            {
                Nombre = txtNombre.Text,
                Edad = int.Parse(txtEdad.Text),
                Correo = txtCorreo.Text
            };
            dao.Agregar(persona);
            CargarPersonas();
            Limpiar();

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Persona persona = new Persona
            {
                Id = int.Parse(txtId.Text),
                Nombre = txtNombre.Text,
                Edad = int.Parse(txtEdad.Text),
                Correo = txtCorreo.Text
            };
            dao.Actualizar(persona);
            CargarPersonas();
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            dao.Eliminar(id);
            CargarPersonas();
            Limpiar();
        }
        private void Limpiar() 
        {
            txtId.Clear();
            txtNombre.Clear();
            txtEdad.Clear();
            txtCorreo.Clear();

        }

        private void dgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Persona persona = (Persona)dgvPersonas.Rows[e.RowIndex].DataBoundItem;
                txtId.Text = persona.Id.ToString();
                txtNombre.Text = persona.Nombre;
                txtEdad.Text = persona.Edad.ToString();
                txtCorreo.Text = persona.Correo;
            }
        }
    }
}
