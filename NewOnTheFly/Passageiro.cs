using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Passageiro
    {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime Data_Nascimento { get; set; }
        public string Sexo { get; set; }
        public DateTime Data_Ultima_Compra { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string Situacao { get; set; }

        public Passageiro() { }

        public Passageiro (string cPF, string nome, DateTime data_Nascimento, string sexo, DateTime data_Ultima_Compra, DateTime data_Cadastro, string situacao)
        {
            CPF = cPF;
            Nome = nome;
            Data_Nascimento = data_Nascimento;
            Sexo = sexo;
            Data_Ultima_Compra = data_Ultima_Compra;
            Data_Cadastro = data_Cadastro;
            Situacao = situacao;
        }


        public static void CadastrarPassageiro()
        {
            string nome, cpf, sexo = "Não Informado";
            string datanascimento;
            char sexochar;

            nome = UtilidadeValidarEntrada.ValidarEntrada("nome");
            if (nome == null) Menu.MenuPassageiro();

            cpf = UtilidadeValidarEntrada.ValidarEntrada("cpf");
            if (cpf == null) Menu.MenuPassageiro();

            datanascimento = UtilidadeValidarEntrada.ValidarEntrada("datanascimento");
            if (datanascimento == null) Menu.MenuPassageiro();

            sexochar = char.Parse(UtilidadeValidarEntrada.ValidarEntrada("sexo"));
            if (sexochar.Equals(null)) Menu.MenuPassageiro();

            if (sexochar == 'M') sexo = "Masculino";
            else if (sexochar == 'F') sexo = "Feminino";
            else if (sexochar == 'N') sexo = "Não Informado";


            String comando = "insert into Passageiro values('" + cpf + "', 'Ativa', '" + sexo + "','" + nome + "', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "', '" + datanascimento + "', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "');";

            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            Console.Clear();
            Console.WriteLine("\nCadastro realizado com sucesso!\n");
            Console.WriteLine("\nAperte 'ENTER' para continuar...");
            Console.ReadKey();
        }

        public static void ImprimirPassageiro(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DO PASSAGEIRO\n");

            while (reader.Read())
            {
                Console.WriteLine("\nCPF: {0}", reader.GetString(0));
                Console.WriteLine("\nNome: {0}", reader.GetString(1));
                Console.WriteLine("\nSituacao: {0}", reader.GetString(2));
                Console.WriteLine("\nSexo: {0}", reader.GetString(3));
                Console.WriteLine("\nData de Nascimento: {0}", reader.GetDateTime(4).ToString("dd/MM/yyyy"));
                Console.WriteLine("\nData de Cadastro: {0}", reader.GetDateTime(5).ToString("dd/MM/yyyy HH:mm"));
            }

            ConexaoBanco.FecharConexao();
        }

    }
}
