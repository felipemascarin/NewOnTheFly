using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Destino
    {
        public string IATA { get; set; }
        public string Continente { get; set; }
        public string Pais { get; set; }
        public string Cidade { get; set; }
        public string Nome_Aeroporto { get; set; }
        public float Distancia_Rota { get; set; }

        public Destino() { }

        public Destino(string iata, string continente, string pais, string cidade, string nome_aeroporto, float distancia_rota)
        {
            IATA = iata;
            Continente = continente;
            Pais = pais;
            Cidade = cidade;
            Nome_Aeroporto = nome_aeroporto;
            Distancia_Rota = distancia_rota;
        }


        public static void ImprimirDestino(String comando)
        {
            Console.Clear();

            SqlConnection connection = ConexaoBanco.AbrirConexao();

            SqlCommand sql_cmnd = new SqlCommand(comando, connection);

            sql_cmnd.Connection.Open();

            sql_cmnd.CommandText = "select IATA from Destino";

            SqlDataReader reader = sql_cmnd.ExecuteReader();

            if(reader.HasRows == true)
            {
                sql_cmnd.Connection.Close();

                sql_cmnd.Connection.Open();

                sql_cmnd.CommandText = comando;

                reader = sql_cmnd.ExecuteReader();

                Console.WriteLine("AEROPORTOS\n");

                while (reader.Read())
                {
                    Console.WriteLine("\n\n{0}, {1}, {2}, {3} \nDistância da Rota: {4} Quiômetros  ", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3).ToUpper(), reader.GetDecimal(4));

                }
            }
            else
            {
                Console.WriteLine("Nenhum dado para impressão!");
                UtilidadeValidarEntrada.Pausa();
                Menu.MenuCompanhiaAerea();
            }
            sql_cmnd.Connection.Close();
        }

    }
}
