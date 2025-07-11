using CRUDPersonasWinForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CRUDPersonasWinForms
{
    public partial class Form1 : Form
    {

        List<Persona> personas = new List<Persona>();
        int siguienteId = 1;
        public Form1()
        {
            InitializeComponent();
        }


        private void RefrescarGrid()
        {
            dgvPersonas.DataSource = null;
            dgvPersonas.DataSource = personas.ToList(); // clonar
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var persona = new Persona
            {
                Id = siguienteId++,
                Nombre = txtNombre.Text,
                Edad = int.Parse(txtEdad.Text),
                Correo = txtCorreo.Text
            };

            personas.Add(persona);
            RefrescarGrid();
            LimpiarFormulario();
        }

       
        private void dgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var persona = (Persona)dgvPersonas.Rows[e.RowIndex].DataBoundItem;

                txtId.Text = persona.Id.ToString();
                txtNombre.Text = persona.Nombre;
                txtEdad.Text = persona.Edad.ToString();
                txtCorreo.Text = persona.Correo;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            var persona = personas.FirstOrDefault(p => p.Id == id);

            if (persona != null)
            {
                persona.Nombre = txtNombre.Text;
                persona.Edad = int.Parse(txtEdad.Text);
                persona.Correo = txtCorreo.Text;

                RefrescarGrid();
                LimpiarFormulario();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            var persona = personas.FirstOrDefault(p => p.Id == id);

            if (persona != null)
            {
                personas.Remove(persona);
                RefrescarGrid();
                LimpiarFormulario();
            }
        }


        private void LimpiarFormulario()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtEdad.Clear();
            txtCorreo.Clear();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

      
    }
}
