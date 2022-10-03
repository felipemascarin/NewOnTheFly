using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Aeronave
    {

        public string Inscricao { get; set; }
        public int Capacidade { get; set; }
        public DateTime Ultima_Venda { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string Situacao { get; set; }
        public string CNPJ { get; set; }
        public string Velocidade_Media { get; set; }


        public Aeronave() { }

        public Aeronave(string inscricao, int capacidade, DateTime ultima_Venda, DateTime data_Cadastro, string situacao, string cNPJ, string velocidade_Media)
        {
            Inscricao = inscricao;
            Capacidade = capacidade;
            Ultima_Venda = ultima_Venda;
            Data_Cadastro = data_Cadastro;
            Situacao = situacao;
            CNPJ = cNPJ;
            Velocidade_Media = velocidade_Media;
        }

        public static void CadastrarAeronave()
        {
            string idaeronave, velocidadevoo, distanciavoomax, cnpj, situacao = "Ativa";
            int capacidade;
            char situac;

            cnpj = UtilidadeValidarEntrada.ValidarEntrada("cnpjlogin");
            if (cnpj == null) Menu.MenuCompanhiaAerea();

            idaeronave = UtilidadeValidarEntrada.ValidarEntrada("idaeronave");
            if (idaeronave == null) Menu.MenuCompanhiaAerea();

            capacidade = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("capacidade"));
            if (capacidade.Equals(null)) Menu.MenuCompanhiaAerea();

            velocidadevoo = UtilidadeValidarEntrada.ValidarEntrada("velocidadevoo");
            if (velocidadevoo.Equals(null)) Menu.MenuCompanhiaAerea();

            distanciavoomax = UtilidadeValidarEntrada.ValidarEntrada("distanciavoomax");
            if (distanciavoomax.Equals(null)) Menu.MenuCompanhiaAerea();

            situac = char.Parse(UtilidadeValidarEntrada.ValidarEntrada("situacao"));
            if (situac.Equals(null)) Menu.MenuCompanhiaAerea();
            if (situac == 'A') situacao = "Ativa";
            else if (situac == 'I') situacao = "Inativa";

            String comando = "insert into Aeronave values('" + idaeronave + "', '" + cnpj + "', '" + capacidade + "', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "', '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "', '" + situacao + "', '" + velocidadevoo + "', '" + distanciavoomax + "');";

            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            Console.Clear();
            Console.WriteLine("\nCadastro realizado com sucesso!\n");
            Console.WriteLine("\nAperte 'ENTER' para continuar...");
            Console.ReadKey();
        }

        public static void ImprimirAeronave(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DA AERONAVE\n");

            while (reader.Read())
            {
                Console.WriteLine("\nCódigo: {0}", reader.GetString(0));
                Console.WriteLine("\nCapacidade de Assentos: {0}", reader.GetString(1));
                Console.WriteLine("\nSituacao: {0}", reader.GetString(2));
                Console.WriteLine("\nVelocidade de Voo: {0} km/h", reader.GetDecimal(3).ToString().Replace(',', '.'));
                Console.WriteLine("\nDistância Máxima de Voo: {0} km", reader.GetDecimal(4).ToString().Replace(',', '.'));
                Console.WriteLine("\nData de Cadastro: {0}", reader.GetDateTime(5).ToString("dd/MM/yyyy HH:mm"));
            }

            ConexaoBanco.FecharConexao();

        }
    }
}
