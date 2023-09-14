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
    public partial class Form1 : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        public Form1()
        {
            InitializeComponent();

            //String de conexão

            conexaoString = "Data Source=DESKTOP-4D8LF92;Initial Catalog=BuscaTec;Integrated Security=True";

            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }
        private bool ValidarCPF(string cpf)
        {

            cpf = new string(cpf.Where(char.IsDigit).ToArray());


            if (cpf.Length != 11)
            {
                return false;
            }
            if (new string(cpf[0], 11) == cpf)
            {
                return false;
            }

            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            int primeiroDigitoVerificador = 11 - (soma % 11);
            if (primeiroDigitoVerificador >= 10)
            {
                primeiroDigitoVerificador = 0;
            }


            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            int segundoDigitoVerificador = 11 - (soma % 11);
            if (segundoDigitoVerificador >= 10)
            {
                segundoDigitoVerificador = 0;
            }


            if (int.Parse(cpf[9].ToString()) == primeiroDigitoVerificador && int.Parse(cpf[10].ToString()) == segundoDigitoVerificador)
            {
                return true;
            }

            return false;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "" || txtEmail.Text == "" || mskCpf.Text == "" || txtSenha.Text == "" || CBPerfil.Text == "")
            {
                MessageBox.Show("PREENCHA TODAS AS COLUNAS", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            if(txtSenha.Text.Length <8)
            {
                MessageBox.Show("A senha deve contar mais de oito digitos!", "AVISO", MessageBoxButtons.OK);
            }
            else
            {
                // Verificar se o usuário já existe no banco de dados, pra isso usa um metodo
                if (UsuarioJaExiste(mskCpf.Text))
                {
                    MessageBox.Show("Este usuário já existe!", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Text = "";
                    txtEmail.Text = "";
                    mskCpf.Text = "";
                    txtSenha.Text = "";
                    mskCelular.Text = "";
                    CBPerfil.Text = "";
                }
                else if (!ValidarCPF(mskCpf.Text))
                {
                    MessageBox.Show("CPF inválido!", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mskCpf.Text = ""; 
                }
                else
                {
                    // Se o usuário não existe e o CPF é válido, então podemos adicionar
                    string sql = "INSERT INTO Usuarios (nome,email,cpf,senha,celular,perfil)" +
                            "VALUES (@nome,@email,@cpf,@senha,@celular,@perfil)";
                    try
                    {
                        SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);
                        sqlCmd.Parameters.AddWithValue("nome", txtNome.Text);
                        sqlCmd.Parameters.AddWithValue("email", txtEmail.Text);
                        sqlCmd.Parameters.AddWithValue("cpf", mskCpf.Text);
                        sqlCmd.Parameters.AddWithValue("senha", txtSenha.Text);
                        sqlCmd.Parameters.AddWithValue("celular", mskCelular.Text);
                        sqlCmd.Parameters.AddWithValue("perfil", CBPerfil.Text);

                        conexaoDB.Open();
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Cadastro Realizado com Sucesso!!!");
                        txtNome.Text = "";
                        txtEmail.Text = "";
                        mskCpf.Text = "";
                        txtSenha.Text = "";
                        mskCelular.Text = "";
                        
                        this.Hide();
                        Login loginform = new Login();
                        loginform.ShowDialog();
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
        }
        private bool UsuarioJaExiste(string cpf)
        {
            string sql = "SELECT COUNT(*) FROM Usuarios WHERE cpf = @cpf";
            SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);
            sqlCmd.Parameters.AddWithValue("cpf", cpf);

            conexaoDB.Open();
            int count = (int)sqlCmd.ExecuteScalar();
            conexaoDB.Close();

            return count > 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                txtSenha.PasswordChar = '\0';
            }
            else
            {
                txtSenha.PasswordChar = '*';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginform = new Login();
            this.Hide();
            loginform.ShowDialog();
            this.Close();
            
        }

        private void mskCpf_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
