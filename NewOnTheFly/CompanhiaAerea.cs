using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class CompanhiaAerea
    {

        public string CNPJ { get; set; }
        public string Razao_Social { get; set; }
        public DateTime Data_Abertura { get; set; }
        public DateTime Ultimo_Voo { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string Situacao { get; set; }

        public CompanhiaAerea() { }

        public CompanhiaAerea(string cNPJ, string razao_Social, DateTime data_Abertura, DateTime ultimo_Voo, DateTime data_Cadastro, string situacao)
        {
            CNPJ = cNPJ;
            Razao_Social = razao_Social;
            Data_Abertura = data_Abertura;
            Ultimo_Voo = ultimo_Voo;
            Data_Cadastro = data_Cadastro;
            Situacao = situacao;
        }

        public static void CadastrarCompanhiaAerea()
        {
            string razaosocial;
            string cnpj;
            string dataabertura;

            razaosocial = UtilidadeValidarEntrada.ValidarEntrada("nome");
            if (razaosocial == null) Menu.MenuCompanhiaAerea();

            cnpj = UtilidadeValidarEntrada.ValidarEntrada("cnpj");
            if (cnpj == null) Menu.MenuCompanhiaAerea();

            dataabertura = UtilidadeValidarEntrada.ValidarEntrada("dataabertura");
            if (dataabertura == null) Menu.MenuCompanhiaAerea();

            String comando = "insert into CompanhiaAerea values('" + cnpj + "', '" + razaosocial + "', '" + dataabertura + "', 'Ativa', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "');";

            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            Console.Clear();
            Console.WriteLine("\nCadastro realizado com sucesso!\n");
            Console.WriteLine("\nAperte 'ENTER' para continuar...");
            Console.ReadKey();
        }

        public static void ImprimirCompanhiaAerea(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DA COMPANHIA AÉREA\n");
            
            while (reader.Read())
            {
                Console.WriteLine("\nCNPJ: {0}", reader.GetString(0));
                Console.WriteLine("\nRazão Social: {0}", reader.GetString(1));
                Console.WriteLine("\nSituacao: {0}", reader.GetString(2));
                Console.WriteLine("\nData de Abertura: {0}", reader.GetDateTime(3).ToString("dd/MM/yyyy"));
                Console.WriteLine("\nData de Cadastro: {0}", reader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm"));
            }

            ConexaoBanco.FecharConexao();
        }

    }
}
