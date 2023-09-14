using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_BuscaTec
{   
    public partial class TelaAdmin : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        DataGridViewRow registroSelecionado;
        public TelaAdmin()
        {
            InitializeComponent();

            //String de conexão

            conexaoString = "Data Source=DESKTOP-4D8LF92;Initial Catalog=BuscaTec;Integrated Security=True";

            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }
        private void carregarDadosUsuario(int ID = 0)
        {
            try
            {
                conexaoDB.Open();

              
                string sql = "SELECT * FROM Usuarios";

                if (ID == 0)
                {
                    //sql = "SELECT * FROM Usuarios";
                    
                }

                else
                {
                    sql = "SELECT * FROM Usuarios WHERE ID=" + ID;
                }
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conexaoDB);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataTable.Columns["ID"].ColumnName = "ID";
                dataTable.Columns["nome"].ColumnName = "Nome";
                dataTable.Columns["email"].ColumnName = "Email";
                dataTable.Columns["cpf"].ColumnName = "CPF";
                dataTable.Columns["senha"].ColumnName = "Senha";
                dataTable.Columns["celular"].ColumnName = "Celular";
                dataTable.Columns["perfil"].ColumnName = "Perfil";

                dataGridView1.DataSource = dataTable;

                conexaoDB.Close();  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex);
            }
        }
        private void TelaAdmin_Load(object sender, EventArgs e)
        {
            btnExcluir.Enabled = false;
            btnAtualizar.Enabled = false;

            carregarDadosUsuario();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "INSERT INTO Usuarios(nome,email,cpf,senha,celular,perfil) VALUES (@nome,@email,@cpf,@senha,@celular,@perfil)";

                SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                sqlCmd.Parameters.AddWithValue("@nome", txtNome.Text);
                sqlCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                sqlCmd.Parameters.AddWithValue("@cpf", mskCpf.Text);
                sqlCmd.Parameters.AddWithValue("@senha", txtSenha.Text);
                sqlCmd.Parameters.AddWithValue("@celular", mskCelular.Text);
                sqlCmd.Parameters.AddWithValue("@perfil", CBPerfil.Text);

                conexaoDB.Open();

                sqlCmd.ExecuteNonQuery();

                MessageBox.Show("Cadastro Realizado");

                txtNome.Text = "";
                txtEmail.Text = "";
                mskCelular.Text = "";
                mskCpf.Text = "";
                txtSenha.Text = "";
                CBPerfil.Text = "";

                conexaoDB.Close();

                carregarDadosUsuario();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao Inserir os Dados: " + ex);
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            int ID;
            if(int.TryParse(txtPesquisar.Text, out ID))
            {
                carregarDadosUsuario(ID);
            }
            else
            {
                MessageBox.Show("Código do livro inválido");
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(registroSelecionado.Cells["ID"].Value.ToString());

                string sql = "UPDATE Usuarios SET " +
                    "nome=@nome," +
                    "email=@email," +
                    "cpf=@cpf," +
                    "senha=@senha," +
                    "celular=@celular," +
                    "perfil=@perfil " + 
                    "WHERE ID=@ID";

                conexaoDB.Open();

                SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                sqlCmd.Parameters.AddWithValue("@ID", ID);
                sqlCmd.Parameters.AddWithValue("@nome", txtNome.Text);
                sqlCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                sqlCmd.Parameters.AddWithValue("@cpf", mskCpf.Text);
                sqlCmd.Parameters.AddWithValue("@senha", txtSenha.Text);
                sqlCmd.Parameters.AddWithValue("@celular", mskCelular.Text);
                sqlCmd.Parameters.AddWithValue("@perfil", CBPerfil.Text);

                sqlCmd.ExecuteNonQuery();

                MessageBox.Show("Atualização Realizada com Sucesso!!!");
                txtNome.Text = "";
                txtEmail.Text = "";
                mskCelular.Text = "";
                mskCpf.Text = "";
                txtSenha.Text = "";
                CBPerfil.Text = "";

                conexaoDB.Close();

                carregarDadosUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao Atualizar os Dados: " + ex);
            }
        }


        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (registroSelecionado != null)
            {
                DialogResult resultado = MessageBox.Show("Tem certeza que deseja exlcuir o aluno", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {

                    try
                    {
                        int id = Convert.ToInt32(registroSelecionado.Cells["id"].Value.ToString());

                        string sql = "DELETE FROM Usuarios " +
                                     "WHERE ID=@ID";

                        conexaoDB.Open();

                        SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                        sqlCmd.Parameters.AddWithValue("@id", id);
                        sqlCmd.ExecuteNonQuery();

                        MessageBox.Show("Aluno excluido com Sucesso!!!");

                        conexaoDB.Close();

                        carregarDadosUsuario();

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Erro ao Excluir os Dados: " + ex);
                    }

                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                registroSelecionado = dataGridView1.Rows[e.RowIndex];

                txtNome.Text = registroSelecionado.Cells["Nome"].Value.ToString();
                txtEmail.Text = registroSelecionado.Cells["Email"].Value.ToString();
                mskCpf.Text = registroSelecionado.Cells["CPF"].Value.ToString();
                txtSenha.Text = registroSelecionado.Cells["Senha"].Value.ToString();
                mskCelular.Text = registroSelecionado.Cells["Celular"].Value.ToString();
                CBPerfil.Text = registroSelecionado.Cells["Perfil"].Value.ToString();

                btnAdicionar.Enabled = true;
                btnAtualizar.Enabled = true;
                btnExcluir.Enabled = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
             Form1 cadastro = new Form1();
            this.Hide();
            cadastro.ShowDialog();
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
 
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 cadastro = new Form1();
            this.Hide();
            cadastro.ShowDialog();
            this.Close();
        }

        private void cadastrarEmpresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroEmpresa empresa = new CadastroEmpresa();
            this.Hide();
            empresa.ShowDialog();
            this.Close();
        }

        private void sairToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
    
}
