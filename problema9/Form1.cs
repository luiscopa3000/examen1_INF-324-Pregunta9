using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace problema9
{
    public partial class Form1 : Form
    {
        ServiceReference1.WebService1SoapClient serviceWeb;
        public Form1()
        {
            InitializeComponent();
        }




        private void Form1_Load(object sender, EventArgs e)
        {


            serviceWeb = new ServiceReference1.WebService1SoapClient();


            panel2.Visible = false;

            panel3.Visible = false;

            panel4.Visible = false;


            panel1.Dock = DockStyle.Fill;

            DataSet dt = serviceWeb.Listar();
            dataGridView1.DataSource = dt.Tables[0];


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.Invalidate();

            panel2.Visible = false;

            panel3.Visible = false;

            panel4.Visible = false;

            DataSet dt = serviceWeb.Listar();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt.Tables[0];
        }

        //Boton para mostar el panel de Agregar
        private void button2_Click(object sender, EventArgs e)
        {
            comboBoxDepartamento.Items.Clear();
            comboBoxgenero.Items.Clear();
            comboBoxRol.Items.Clear();


            comboBoxDepartamento.Items.AddRange(LlenarComboBoxDepartamentos());
            comboBoxgenero.Items.AddRange(cargarGenero());
            comboBoxRol.Items.AddRange(cargarRol());

            // Establecer el índice del elemento que deseas que esté seleccionado por defecto
            comboBoxgenero.SelectedIndex = 0; // Esto seleccionará el primer elemento en el ComboBox
            comboBoxDepartamento.SelectedIndex = 0;
            comboBoxRol.SelectedIndex = 0;

            panel2.Visible = true;
            panel2.Invalidate();
            panel2.Dock = DockStyle.Fill;




            panel1.Visible = false;

            panel3.Visible = false;

            panel4.Visible = false;



        }

        //El boton que carga el contenido llenado y lo envia al servicio
        private void button5_Click(object sender, EventArgs e)
        {
            string ci = textBoxCI.Text;
            string nombres = textBoxNombres.Text;
            string paterno = TextBoxPaterno.Text;
            string materno = textBoxMaterno.Text;
            string fechaNacimiento = textBoxFecha_nac.Text; // Asumiendo que tienes un TextBox para la fecha de nacimiento
            string genero = comboBoxgenero.SelectedItem.ToString(); // Asumiendo que tienes un ComboBox para el género
            string direccion = textBoxDireccion_dom.Text;
            string telefono = textBoxTelefono.Text;
            string celular = textBoxCelular.Text;
            string correo = textBoxCorreo.Text;
            string password = textBoxPassword.Text;
            string rol = comboBoxRol.SelectedItem.ToString(); // Asumiendo que tienes un TextBox para el rol
            string departamento = comboBoxDepartamento.SelectedItem.ToString(); // Asumiendo que tienes un ComboBox para el departamento

            fechaNacimiento = convertirFecha(fechaNacimiento);

            bool sw = VerificarDatos(ci,nombres,paterno, materno, fechaNacimiento,genero, direccion, telefono,celular, correo, password, rol, departamento);
            if (sw) {
                string resp = serviceWeb.Alta(ci, nombres, paterno, materno, fechaNacimiento, generoParser(genero), direccion, telefono, celular, correo, password, rol, departamento);
                if (resp.Equals("1"))
                {
                    MessageBox.Show("Se insertaron los datos de manera correcta");


                }
                else
                {
                    MessageBox.Show("Hubo un error al insertar los datos", "vuelva a intentar mas tarde", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        //El boton que carga el panel de eliminar
        private void button3_Click(object sender, EventArgs e)
        {


            panel3.Visible = true;
            panel3.Invalidate();
            panel3.Dock = DockStyle.Fill;



            panel1.Visible = false;

            panel2.Visible = false;

            panel4.Visible = false;
            DataSet lsUsuarios = serviceWeb.Listar();

            // Obtener la columna "ci" del DataSet y guardarla en una lista
            List<string> listaCi = lsUsuarios.Tables[0].AsEnumerable()
                                    .Select(row => row.Field<string>("ci"))
                                    .ToList();
            comboBoxCi_elimnar.Items.Clear();
            // Agregar los elementos al ComboBox
            foreach (string ci in listaCi)
            {
                comboBoxCi_elimnar.Items.Add(ci);
            }
            comboBoxCi_elimnar.SelectedIndex = 0;


        }

        private void comboBoxCi_elimnar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valorSeleccionado = comboBoxCi_elimnar.SelectedItem.ToString();
            DataSet dttemp = serviceWeb.mostra1(valorSeleccionado);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = dttemp.Tables[0];
        }

        private void button_eliminar_Click(object sender, EventArgs e)
        {
            string valorSeleccionado = comboBoxCi_elimnar.SelectedItem.ToString();

            // Mostrar el cuadro de diálogo de confirmación
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (resultado == DialogResult.Yes)
            {
                int sw = serviceWeb.Baja(valorSeleccionado);
                if (sw == 1)
                {
                    MessageBox.Show("Usuario eliminado");

                    //REcargamos el panel de eliminado
                    button3.PerformClick();
                }
                else {
                    MessageBox.Show("Ocurrio un error al eliminar a la perssona");
                }
            }

        }
        //Botones para editar registros
        private void button4_Click(object sender, EventArgs e)
        {

            panel4.Visible = true;
            panel4.Invalidate();
            panel4.Dock = DockStyle.Fill;



            panel1.Visible = false;

            panel2.Visible = false;

            panel3.Visible = false;


            comboBoxDEpartamento2.Items.Clear();
            comboBoxGenero2.Items.Clear();
            comboBoxRol2.Items.Clear();


            comboBoxDEpartamento2.Items.AddRange(LlenarComboBoxDepartamentos());
            comboBoxGenero2.Items.AddRange(cargarGenero());
            comboBoxRol2.Items.AddRange(cargarRol());




            DataSet lsUsuarios = serviceWeb.Listar();

            // Obtener la columna "ci" del DataSet y guardarla en una lista
            List<string> listaCi = lsUsuarios.Tables[0].AsEnumerable()
                                    .Select(row => row.Field<string>("ci"))
                                    .ToList();
            comboBoxCI_Selector.Items.Clear();
            // Agregar los elementos al ComboBox
            foreach (string ci in listaCi)
            {
                comboBoxCI_Selector.Items.Add(ci);
            }
            comboBoxCI_Selector.SelectedIndex = 0;



        }
        //Seleccion del ComboBox
        private void comboBoxCI_Selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valorSeleccionado = comboBoxCI_Selector.SelectedItem.ToString();
            DataSet dttemp = serviceWeb.mostra1(valorSeleccionado);
            DataTable tabla = dttemp.Tables[0];
            DataRow fila = tabla.Rows[0];
            // Parseamos el objeto a string
            object valor = fila["ci"];
            string ci = valor.ToString();

            valor = fila["nombres"];
            string nombres = valor.ToString();
            textBoxNombres2.Text = nombres;

            valor = fila["paterno"];
            string paterno = valor.ToString();
            textBoxPaterno2.Text = paterno;

            valor = fila["materno"];
            string materno = valor.ToString();
            textBoxMaterno2.Text = materno;


            valor = fila["fecha_nac"];
            string fecha_nac_completo = valor.ToString();
            string fecha_nac = fecha_nac_completo.Replace(" 00:00:00", "");
            textBoxFecha_nac2.Text = fecha_nac;


            valor = fila["genero"];
            string genero = valor.ToString();
            if (genero.Equals("1")) {
                comboBoxGenero2.SelectedIndex = 1;
            } else {
                comboBoxGenero2.SelectedIndex = 2;
            }


            valor = fila["direccion_dom"];
            string direccion_dom = valor.ToString();
            textBoxDireccion_com2.Text = direccion_dom;


            valor = fila["telefono"];
            string telefono = valor.ToString();
            textBoxTelefono2.Text = telefono;

            valor = fila["celular"];
            string celular = valor.ToString();
            textBoxCelular2.Text = celular;


            valor = fila["correo"];
            string correo = valor.ToString();
            textBoxCorreo2.Text = correo;


            valor = fila["departamento"];
            string departamento = valor.ToString();
            int index = comboBoxDEpartamento2.Items.IndexOf(departamento);
            if (index >= 0)
            {
                comboBoxDEpartamento2.SelectedIndex = index;
            }
            else {
                MessageBox.Show("Departamento incorrecto");
            }



            valor = fila["rol"];
            string rol = valor.ToString();
            index = comboBoxRol2.Items.IndexOf(rol);
            if (index >= 0)
            {
                comboBoxRol2.SelectedIndex = index;
            }
            else
            {
                MessageBox.Show("Rol incorrecto debe cambiarlo");
            }



            valor = fila["password"];
            string password = valor.ToString();
            textBoxPassword2.Text = password;



        }

        //Boton para enviar registro editado
        private void button_modificar_Click(object sender, EventArgs e)
        {
            string ci = comboBoxCI_Selector.SelectedItem.ToString(); ;
            string nombres = textBoxNombres2.Text;
            string paterno = textBoxPaterno2.Text;
            string materno = textBoxMaterno2.Text;
            string fechaNacimiento = textBoxFecha_nac2.Text; // Asumiendo que tienes un TextBox para la fecha de nacimiento
            string genero = comboBoxGenero2.SelectedItem.ToString(); // Asumiendo que tienes un ComboBox para el género
            string direccion = textBoxDireccion_com2.Text;
            string telefono = textBoxTelefono2.Text;
            string celular = textBoxCelular2.Text;
            string correo = textBoxCorreo2.Text;
            string password = textBoxPassword2.Text;
            string rol = comboBoxRol2.SelectedItem.ToString(); // Asumiendo que tienes un TextBox para el rol
            string departamento = comboBoxDEpartamento2.SelectedItem.ToString(); // Asumiendo que tienes un ComboBox para el departamento


            fechaNacimiento = convertirFecha(fechaNacimiento);


            bool sw = VerificarDatos(ci, nombres, paterno, materno, fechaNacimiento, generoParser(genero), direccion, telefono, celular, correo, password, rol, departamento);
            if (sw)
            {
                int resp = serviceWeb.Cambio(ci, nombres, paterno, materno, fechaNacimiento, generoParser(genero), direccion, telefono, celular, correo, password, rol, departamento);
                if (resp == 1)
                {
                    MessageBox.Show("Se insertaron los datos de manera correcta");

                }
                else
                {
                    MessageBox.Show("Hubo un error al insertar los datos", "vuelva a intentar mas tarde", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }


        }

        private string[] LlenarComboBoxDepartamentos()
        {
            // Lista de nombres de departamentos de Bolivia
            string[] departamentosBolivia = {"Seleccion un departamento", "La Paz", "Cochabamba", "Santa Cruz", "Potosí", "Oruro", "Tarija", "Chuquisaca", "Beni", "Pando" };
            return departamentosBolivia;
        }
        private string[] cargarGenero() {
            string[] generos = { "Seleccione un genero", "Masculino", "Femenino" };
            return generos;
        }
        private string[] cargarRol() {
            string[] rol = {"Seleccione un rol", "gerente", "cliente", "cajero" };
            return rol;
        }

        private bool VerificarDatos(string ci, string nombres, string paterno, string materno, string fechaNacimiento,
                                     string genero, string direccion, string telefono, string celular, string correo,
                                     string password, string rol, string departamento)
        {
            // Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(ci) ||
                string.IsNullOrWhiteSpace(nombres) ||
                string.IsNullOrWhiteSpace(paterno) ||
                string.IsNullOrWhiteSpace(materno) ||
                string.IsNullOrWhiteSpace(fechaNacimiento) ||
                string.IsNullOrWhiteSpace(genero) ||
                string.IsNullOrWhiteSpace(direccion) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(celular) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(rol) ||
                string.IsNullOrWhiteSpace(departamento))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Devuelve falso si hay campos vacíos
            }


            DateTime fecha;
            if (DateTime.TryParseExact(fechaNacimiento, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fecha))
            {
                string fechaFormateada = fecha.ToString("dd/MM/yyyy");
                fechaNacimiento = fechaFormateada; // guarda la fecha formateada en el formato dd/MM/yyyy
            }
            else
            {
                Console.WriteLine("Formato de fecha incorrecto");
            }

            // Validación de formato de fecha de nacimiento

            DateTime fechaNacimientoDateTime;
            if (!DateTime.TryParseExact(fechaNacimiento, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fechaNacimientoDateTime))
            {
                MessageBox.Show("Por favor, ingrese una fecha de nacimiento válida en el formato dd/MM/yyyy.", "Formato de Fecha Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Devuelve falso si el formato de fecha es incorrecto
            }

            // Validación de dirección de correo electrónico
            if (!IsValidEmail(correo))
            {
                MessageBox.Show("Por favor, ingrese una dirección de correo electrónico válida.", "Correo Electrónico Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Devuelve falso si la dirección de correo es inválida
            }

            // Otras validaciones necesarias, como la longitud de la contraseña, etc.

            // Si todas las validaciones pasan, devuelve true
            return true;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public string generoParser(String genero) {
            if (genero.Equals("Masculino") ){
                return "1";
            } else {
                return "0";
            }
        }

        public string convertirFecha(string fechaString) {
            DateTime fecha;
            if (DateTime.TryParseExact(fechaString, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fecha))
            {
                string fechaFormateada = fecha.ToString("dd/MM/yyyy");
                return fechaFormateada;
            }
            else
            {
                Console.WriteLine("Formato de fecha incorrecto");
                return fechaString;
            }
        }

    }
}
