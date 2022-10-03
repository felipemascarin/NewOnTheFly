using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class UtilidadeAtualizacao
    {
        public static void AtualizarSistema()
        {
            AtualizarVoo();
            AtualizarReserva();
        }

        public static void CarregarDestinos()
        {
            /*Verifica tabela de aeroportos de destino do banco de dados, se não possuir nenhuma linha,
            puxa os arquivos do Destino.Dat e alimenta o banco de dados.*/

            try
            {
                System.IO.Directory.CreateDirectory(@"C:\NewOnTheFly");

                bool temlinhas = ConexaoBanco.InjetarSqlExecuteReader("select iata from Destino;");

                if (temlinhas == false)
                {
                    string[] lines = System.IO.File.ReadAllLines(@"C:\NewOnTheFly\Destino.dat");

                    string[] dados;

                    foreach (var line in lines)
                    {
                        dados = line.Split(';');

                        Destino destino = new Destino(dados[4], dados[0], dados[1], dados[2], dados[3], float.Parse(dados[5]));

                        String comando = "insert into dbo.Destino Values('" + destino.IATA + "', '" + destino.Continente + "', '" + destino.Pais + "', '" + destino.Cidade + "', '" + destino.Nome_Aeroporto + "', " + destino.Distancia_Rota.ToString().Replace(',', '.') + ");";

                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("\nO programa executou a criação da pasta C:\\NewOnTheFly \n\nCertifique-se de existir o arquivo Destino.dat contendo as informações dos aeroportos na pasta C:\\NewOnTheFly." +
                    " \nEsse arquivo alimentará uma única vez o banco de dados com todas as informações dos aeroportos de destino." +
                    "\n\nSem esse arquivo não é possível cadastrar voo." +
                    "\n\nDownload do arquivo Destino.dat https://github.com/felipemascarin/NewOnTheFly \n\n" +
                    "Verifique também a conexão correta com o banco de dados.");
                UtilidadeValidarEntrada.Pausa();
            }
        }

        private static void AtualizarVoo()
        {
            String comando = "update Voo set Situacao = 'Em Rota' where Data_Hora_Voo < '" + System.DateTime.Now + "' and Data_Chegada >  '" + System.DateTime.Now + "'";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

            comando = "update Voo set Situacao = 'Realizado' where Data_Hora_Voo < '" + System.DateTime.Now + "' and Data_Chegada <  '" + System.DateTime.Now + "'";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
        }

        private static void AtualizarReserva()
        {
            String comando = "update Passagem set Situacao = 'Livre' where Data_Ultima_Operacao < '" + System.DateTime.Now.AddDays(-2) + "' and Situacao = 'Reservada';";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
        }
    }
}
