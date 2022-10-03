using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Venda
    {
        public int ID_Venda { get; set; }
        public DateTime Data_Venda { get; set; }
        public float Valor_Total { get; set; }

        public Venda() { }

        public Venda(int iD_Venda, DateTime data_Venda, float valor_Total)
        {
            ID_Venda = iD_Venda;
            Data_Venda = data_Venda;
            Valor_Total = valor_Total;
        }

        public static void FiltrarVenda()
        {
            string cpf = UtilidadeValidarEntrada.ValidarEntrada("cpfexiste");
            if (cpf == null) Menu.MenuPassagem();

            String comando = "select  p.ID_Passagem, p.ID_Voo, p.Assento, v.ID_Venda, v.Data_Venda, iv.Valor_Unitario, v.Valor_Total from Passagem as p, Venda as v, ItemVenda as iv where v.CPF = '" + cpf + "' and p.ID_Venda = v.ID_Venda and v.ID_Venda = iv.ID_Venda Order By p.Data_Ultima_Operacao";

            if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
            {
                ImprimirVenda(comando);
                UtilidadeValidarEntrada.Pausa();
            }
            else
            {
                Console.WriteLine("Nenhum dado de venda encontrado!");
                UtilidadeValidarEntrada.Pausa();
            }
        }

        public static void ImprimirVenda(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DA VENDA\n");

            while (reader.Read())
            {
                Console.WriteLine("\nID Venda: {0}", reader.GetInt32(3));
                Console.WriteLine("\nID Passagem: {0}", reader.GetString(0));
                Console.WriteLine("\nID Voo: {0}", reader.GetString(1));
                Console.WriteLine("\nPoltrona de assento: {0}", reader.GetString(2));
                Console.WriteLine("\nData da Venda: {0}", reader.GetDateTime(3).ToString("dd/MM/yyyy"));
                Console.WriteLine("\nValor Unitário da Passagem: {0}", reader.GetDecimal(4));
                Console.WriteLine("\nValor Total da Venda: {0}", reader.GetDecimal(5));
            }

            ConexaoBanco.FecharConexao();
        }

        public static void GerarVenda(int quantPassagem, string cpf, string idvoo, decimal passagemvalor)
        {
            for (int i = 0; i < quantPassagem; i++)
            {
                //cria venda
                string valortotal = quantPassagem + passagemvalor.ToString().Replace(',', '.');
                String comando = "insert into Venda(CPF, Data_Venda, Valor_Total) values('" + cpf + "', '" + System.DateTime.Now + "', " + valortotal + ")";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                comando = "select ID_Venda from Venda where CPF = '" + cpf + "' and Data_Venda > '" + System.DateTime.Now.AddSeconds(-5) + "' and Valor_Total = '" + valortotal + "';";
                SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);
                reader.Read();
                int idvend = reader.GetInt32(0);
                string idvenda = idvend.ToString();

                //cria item venda
                ItemVenda.GerarItemVenda(idvenda, passagemvalor);

                //Atualiza Passagem Vendida
                comando = "Update Top (1) Passagem set ID_Venda = '" + idvenda + "', Situacao = 'Vendida', CPF = '" + cpf + "', Data_Ultima_Operacao = '" + System.DateTime.Now + "' where Situacao = 'Livre' and ID_Voo = '" + idvoo + "';";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                //Atualiza Assentos Ocupados
                comando = "Update Top (1) Voo set Assentos_Ocupados = (Assentos_Ocupados + 1) where ID_Voo = '" + idvoo + "'; ";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                //Atualizada ultima compra passageiro
                comando = "update Passageiro set Data_Ultima_Compra = '" + System.DateTime.Now + "' where CPF = '" + cpf + "';";
            }

        }

        public static void GerarVendaReservada(string idpassagem, string cpf, string idvoo, decimal passagemvalordecimal)
        {
            //cria venda
            string passagemvalor = passagemvalordecimal.ToString().Replace(',', '.');
            String comando = "insert into Venda(CPF, Data_Venda, Valor_Total) values('" + cpf + "', '" + System.DateTime.Now + "', " + passagemvalor + ")";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            comando = "select ID_Venda from Venda where CPF = '" + cpf + "' and Data_Venda > '" + System.DateTime.Now.AddSeconds(-5) + "' and Valor_Total = '" + passagemvalor + "';";
            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);
            reader.Read();
            int idvend = reader.GetInt32(0);
            string idvenda = idvend.ToString();


            //cria item venda
            ItemVenda.GerarItemVenda(idvenda, passagemvalordecimal);

            //Atualiza Passagem de Reservada para Vendida
            comando = "Update Top (1) Passagem set ID_Venda = '" + idvenda + "', Situacao = 'Vendida', CPF = '" + cpf + "', Data_Ultima_Operacao = '" + System.DateTime.Now + "' where Situacao = 'Reservada' and ID_Voo = '" + idvoo + "' and ID_Passagem = '" + idpassagem + "';";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            //Atualiza Assentos Ocupados
            comando = "Update Top (1) Voo set Assentos_Ocupados = (Assentos_Ocupados + 1) where ID_Voo = '" + idvoo + "'; ";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            //Atualizada ultima compra passageiro
            comando = "update Passageiro set Data_Ultima_Compra = '" + System.DateTime.Now + "' where CPF = '" + cpf + "';";
        }
    }
}
