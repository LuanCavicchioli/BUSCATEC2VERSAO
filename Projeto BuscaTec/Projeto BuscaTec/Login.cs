using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Projeto_BuscaTec
{

    public partial class Login : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        public Login()
        {
            InitializeComponent();

            //String de conexão

            conexaoString = "Data Source=DESKTOP-4D8LF92;Initial Catalog=BuscaTec;Integrated Security=True";

            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }




        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string senha = txtSenha.Text;
            string perfil;

            if (AutenticarUsuario(email, senha, out perfil))
            {
                MessageBox.Show("Login bem-sucedido!");

                if (perfil == "Usuarios")
                {
                    TelaUsuario tela = new TelaUsuario();
                    this.Hide();
                    tela.ShowDialog();
                    this.Close();
                }
                else if (perfil == "Administrador")
                {
                    TelaAdmin telaform = new TelaAdmin();
                    this.Hide();
                    telaform.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Login falhou. Verifique suas credenciais.");
            }
        }
        //Metodo utilizado para puxar os dados do cadastro, e assim ser utilizados para logar
        private bool AutenticarUsuario(string email, string senha, out string perfil)
        {
            string conexaoString = "Data Source=DESKTOP-4D8LF92;Initial Catalog=BuscaTec;Integrated Security=True";
            using (SqlConnection conexaoDB = new SqlConnection(conexaoString))
            {
                string sql = "SELECT perfil FROM Usuarios WHERE email = @email AND senha = @senha";
                SqlCommand cmd = new SqlCommand(sql, conexaoDB);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@senha", senha);

                conexaoDB.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    perfil = reader.GetString(0); // Lê o perfil diretamente
                    return true;
                }

                perfil = null;
                return false;
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Empresa empresa = new Empresa();
            this.Hide();
            empresa.ShowDialog();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtSenha.PasswordChar = '\0';
            }
            else
            {
                txtSenha.PasswordChar = '*';
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
