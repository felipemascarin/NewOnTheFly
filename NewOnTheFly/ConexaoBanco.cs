using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class ConexaoBanco
    {
        static string Conexao = "Data Source=localhost; Initial Catalog=OnTheFly; User id=sa; Password=159357;";
        static SqlConnection conn;

        public ConexaoBanco() { }

        public static SqlConnection AbrirConexao()
        {
            conn = new SqlConnection(Conexao);
            return conn;
        }

        public static void FecharConexao()
        {
            conn.Close();
        }

        public static SqlDataReader RetornarExecuteReader(String comando)
        {
            SqlConnection connection = ConexaoBanco.AbrirConexao();

            SqlCommand sql_cmnd = new SqlCommand(comando, connection);

            sql_cmnd.Connection.Open();

            sql_cmnd.CommandText = comando;

            SqlDataReader reader = sql_cmnd.ExecuteReader(); 

            return reader;
        }

        public static bool InjetarSqlExecuteReader(String comando)
        {
            SqlConnection connection = ConexaoBanco.AbrirConexao();

            SqlCommand sql_cmnd = new SqlCommand(comando, connection);

            sql_cmnd.Connection.Open();

            sql_cmnd.CommandText = comando;

            SqlDataReader reader = sql_cmnd.ExecuteReader();

            bool existelinha = reader.HasRows;

            sql_cmnd.Connection.Close();

            return existelinha;
        }

        public static void InjetarSqlExecuteNonQuery(String comando)
        {
            SqlConnection connection = ConexaoBanco.AbrirConexao();

            SqlCommand sql_cmnd = new SqlCommand(comando, connection);

            sql_cmnd.Connection.Open();

            sql_cmnd.CommandText = comando;

            sql_cmnd.ExecuteNonQuery();

            sql_cmnd.Connection.Close();
        }

    }
}
