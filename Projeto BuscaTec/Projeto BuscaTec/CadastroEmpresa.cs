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

namespace Projeto_BuscaTec
{
    public partial class CadastroEmpresa : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        public CadastroEmpresa()
        {
            InitializeComponent();

            //String de conexão

            conexaoString = "Data Source=DESKTOP-4D8LF92;Initial Catalog=BuscaTec;Integrated Security=True";

            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }

        private void CadastroEmpresa_Load(object sender, EventArgs e)
        {

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if(txtNome.Text == ""||txtPerfil.Text ==""||txtSenha.Text == "" || mskCelular.Text == ""||mskCnpj.Text=="" || mskCpf.Text == "")
            {
                MessageBox.Show("PREENCHA TODOS OS CAMPOS","AVISO",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string sql = "INSERT INTO Empresa (nome,cpnj,senha,nomevisual,cpf,celular)" +
                    "VALUES (@nome,@cpnj,@senha,@nomevisual,@cpf,@celular)";
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                    sqlCmd.Parameters.AddWithValue("nome", txtNome.Text);
                    sqlCmd.Parameters.AddWithValue("cpnj", mskCnpj.Text);
                    sqlCmd.Parameters.AddWithValue("senha", txtSenha.Text);
                    sqlCmd.Parameters.AddWithValue("nomevisual",txtPerfil.Text);
                    sqlCmd.Parameters.AddWithValue("cpf",mskCpf.Text);
                    sqlCmd.Parameters.AddWithValue("celular",mskCelular.Text);

                    conexaoDB.Open();
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Cadastro Realizado com Sucesso!!!");
                    txtNome.Text = "";
                    txtPerfil.Text = "";
                    txtSenha.Text = "";
                    mskCelular.Text = "";
                    mskCnpj.Text = "";
                    mskCpf.Text = "";

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex);
                }
                finally
                {
                    conexaoDB.Close();
                }
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }
    }
}
