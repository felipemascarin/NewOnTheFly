using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Voo
    {

        public string ID_Voo { get; set; }
        public string Inscricao { get; set; }
        public string IATA { get; set; }
        public string Situacao { get; set; }
        public string Tempo_Voo { get; set; }
        public int Assentos_Ocupados { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public DateTime Data_Hora_Voo { get; set; }
        public DateTime Data_Chegada { get; set; }

        public Voo() { }

        public Voo(string iD_Voo, string inscricao, string iATA, int assentos_Ocupados, DateTime data_Cadastro, DateTime data_Hora_Voo, string situacao, string tempo_Voo, DateTime data_Chegada)
        {
            ID_Voo = iD_Voo;
            Inscricao = inscricao;
            IATA = iATA;
            Assentos_Ocupados = assentos_Ocupados;
            Data_Cadastro = data_Cadastro;
            Data_Hora_Voo = data_Hora_Voo;
            Situacao = situacao;
            Tempo_Voo = tempo_Voo;
            Data_Chegada = data_Chegada;
        }

        public static void CadastrarVoo(string continente)
        {
            string cnpjcadastrovoo = UtilidadeValidarEntrada.ValidarEntrada("cnpjlogin");
            if (cnpjcadastrovoo == null) Menu.MenuCompanhiaAerea();

            String comando = "select Pais, Cidade, Nome_Aeroporto, IATA, Distancia_Rota from Destino where" +
            " Continente = '" + continente + "' Order By Pais;";

            Destino.ImprimirDestino(comando);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nINFORME O CÓDIGO IATA DO AEROPORTO DE DESTINO\n");
            Console.ForegroundColor = ConsoleColor.White;

            string idvoo, iatadestino, idaeronave, auxData, tempodevoo;
            DateTime datavoo, datachegada;
            float valor;

            iatadestino = UtilidadeValidarEntrada.ValidarEntrada("destino");
            if (iatadestino == null) Menu.MenuCompanhiaAerea();

            idvoo = GeradorIdVoo();
            if (idvoo == null) Menu.MenuCompanhiaAerea();

            idaeronave = UtilidadeValidarEntrada.ValidarEntrada("aeronave");
            if (idaeronave == null) Menu.MenuCompanhiaAerea();

            auxData = UtilidadeValidarEntrada.ValidarEntrada("datavoo");
            if (auxData == null) Menu.MenuCompanhiaAerea();
            datavoo = DateTime.Parse(auxData);

            tempodevoo = CalcularTempoVoo(idaeronave, iatadestino);

            datachegada = datavoo.Add(TimeSpan.Parse(tempodevoo));

            if (ValidarAeronaveParaVoo(idaeronave, iatadestino, tempodevoo, datavoo, datachegada) == true)
            {
                valor = float.Parse(UtilidadeValidarEntrada.ValidarEntrada("valorpassagem"));
                if (valor.Equals(null)) Menu.MenuCompanhiaAerea();

                comando = "insert into Voo values('" + idvoo + "', '" + idaeronave + "', '" + iatadestino + "', 0 , '" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "', '" + datavoo + "', 'Agendado', '" + tempodevoo + "', '" + datachegada + "');";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                comando = "select capacidade from Aeronave where Inscricao = '" + idaeronave + "';";
                SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);
                reader.Read();
                string capacidade = reader.GetString(0);
                int capacidadeassentos = int.Parse(capacidade);
                ConexaoBanco.FecharConexao();

                Passagem.GerarPassagens(capacidadeassentos, idvoo, valor);

                Console.Clear();
                Console.WriteLine("\nCadastro realizado com sucesso!\n");
                Console.WriteLine("\nAperte 'ENTER' para continuar...");
                Console.ReadKey();
                Menu.MenuCompanhiaAerea();
            }

        }

        public static bool ValidarAeronaveParaVoo(string idaeronave, string iatadestino, string tempodevoo, DateTime datavootentativa, DateTime datavootentativachegada)
        {
            String comando = "select Distancia_Voo_Max from Aeronave where Inscricao = '" + idaeronave + "';";

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            reader.Read();

            decimal distanciamaxvoo = reader.GetDecimal(0);

            ConexaoBanco.FecharConexao();

            comando = "select Distancia_Rota from Destino where IATA = '" + iatadestino + "';";

            reader = ConexaoBanco.RetornarExecuteReader(comando);

            reader.Read();

            decimal distanciarota = reader.GetDecimal(0);

            ConexaoBanco.FecharConexao();

            //Verifica se a aeronave pode realizar esse percurso
            if (distanciamaxvoo > distanciarota)
            {
                //Processo de verificação se a data do voo pode ser cadastrada para aquela aeronave

                comando = "select Data_Hora_Voo from Voo where Inscricao = '" + idaeronave + "' and (('" + datavootentativa + "' BETWEEN Voo.Data_Hora_Voo and Voo.Data_Chegada OR '" + datavootentativachegada + "' BETWEEN Voo.Data_Hora_Voo and Voo.Data_Chegada) OR (Voo.Data_Hora_Voo BETWEEN '" + datavootentativa + "' and '" + datavootentativachegada + "' OR Voo.Data_Chegada  BETWEEN '" + datavootentativa + "' and '" + datavootentativachegada + "')) and (Voo.Situacao = 'Agendado' OR Voo.Situacao = 'Em Rota');";


                if (ConexaoBanco.InjetarSqlExecuteReader(comando) == false)
                {
                    ConexaoBanco.FecharConexao();
                    return true;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEssa Aeronave não pode realizar esse voo proximo a essa data escolhida");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("- Está relacionada a outro voo nesse período de tempo\n");
                    UtilidadeValidarEntrada.Pausa();
                    return false;
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEssa Aeronave não pode realizar esse voo");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("- Não tem autonomia para realizar um voo dessa distância!\n");
                UtilidadeValidarEntrada.Pausa();
                return false;
            }
        }

        public static string CalcularTempoVoo(string idaeronave, string iatadestino)
        {
            String comando = "select Destino.Distancia_Rota, Aeronave.Velocidade_Media from Destino, Aeronave where Aeronave.Inscricao = '" + idaeronave + "' and Destino.IATA = '" + iatadestino + "';";

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            reader.Read();

            decimal distancia = reader.GetDecimal(0);

            decimal velocidadevoo = reader.GetDecimal(1);

            ConexaoBanco.FecharConexao();

            decimal TempoCalculado = (distancia / velocidadevoo);

            TimeSpan Tempodevoo = TimeSpan.FromHours(Convert.ToDouble(TempoCalculado));

            string tempovoo = Tempodevoo.ToString(@"dd\.hh\:mm\:ss");

            return tempovoo;
        }

        public static void CancelarVoo()
        {
            string idvoo = UtilidadeValidarEntrada.ValidarEntrada("idvoo");
            if (idvoo == null) Menu.MenuInicial();
            
            String comando = "select ID_Voo from Voo where ID_Voo = '" + idvoo + "';";

            if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
            {
                Console.Clear();
                comando = "Update Voo set Situacao = 'Cancelado' where ID_Voo = '" + idvoo + "' and Situacao = 'Agendado';";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                comando = "Update Passagem set Situacao = 'Cancelada' where ID_Voo = '" + idvoo + "';";
                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                Console.WriteLine("\nVoo ID: " + idvoo + " Cancelado com Sucesso!\n" +
                    "\nNão é possível mais alterar a situação desse voo, deve ser cadastrado um novo voo");

                UtilidadeValidarEntrada.Pausa();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nNão é possível cancelar o Voo ID: " + idvoo);
                Console.WriteLine("\nMotivos: Pode já estar cancelado ou em Rota ou já foi realizado");
                UtilidadeValidarEntrada.Pausa();
            }
        }

        public static string GeradorIdVoo()
        {
            Random random = new Random();
            bool repetir;

            do
            {
                try
                {
                    string idvoo = "V" + random.Next(9999).ToString();

                    String comando = "select ID_Voo from Voo where ID_Voo = '" + idvoo + "';";

                    bool existe = ConexaoBanco.InjetarSqlExecuteReader(comando);

                    if (existe == false) return idvoo;
                    else repetir = true;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Erro: não foi possível gerar ID do Voo!\nVerifique a conexão com o Banco de Dados");
                    UtilidadeValidarEntrada.Pausa();
                    return null;
                }
            } while (repetir == true);

            return null;
        }

        public static void ImpirmirVoo(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DE VOO\n");

            while (reader.Read())
            {
                Console.WriteLine("\nID Voo: {0}", reader.GetString(0));
                Console.WriteLine("Aeronave: {0}", reader.GetString(1));
                Console.WriteLine("Destino: {0}, {1}, {2}, {3} - {4}  ", reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(2).ToUpper(), reader.GetString(6));
                Console.WriteLine("Data de Partida: {0}", reader.GetDateTime(7).ToString("dd/MM/yyyy HH:mm"));
                TimeSpan TempoVoo = TimeSpan.Parse(reader.GetString(8));
                string Tempo_Voo = TempoVoo.ToString(@"dd\.hh\:mm");
                char[] caracteres = Tempo_Voo.ToCharArray();
                string Temp_Voo = caracteres[0].ToString() + caracteres[1].ToString() + " Dias, " + caracteres[3].ToString() + caracteres[4].ToString() + " Horas e  " + caracteres[6].ToString() + caracteres[7].ToString() + " Minutos";
                Console.WriteLine("Tempo de Voo: {0}", Temp_Voo);
                Console.WriteLine("Companhia Aérea: {0}", reader.GetString(10));
                Console.Write("Situação: ");
                if (reader.GetString(9) == "Em Rota") Console.ForegroundColor = ConsoleColor.Blue;
                else if (reader.GetString(9) == "Cancelado") Console.ForegroundColor = ConsoleColor.Red;
                else if (reader.GetString(9) == "Agendado") Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", reader.GetString(9));
                Console.ForegroundColor = ConsoleColor.White;
            }

            ConexaoBanco.FecharConexao();
        }

        public void FiltrarVoo(string filtro, string buscar)
        {
            Console.Clear();
            String comandobusca;
            String comando;
            bool existe;

            switch (filtro)
            {

                case "idvoo":

                    comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto,  Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.ID_Voo = '" + buscar + "' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.ID_Voo;";

                    ImpirmirVoo(comando);
                    UtilidadeValidarEntrada.Pausa();
                    break;



                case "aeroporto":

                    comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.IATA = '" + buscar + "' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.Situacao;";

                    ImpirmirVoo(comando);
                    UtilidadeValidarEntrada.Pausa();
                    break;



                case "aeronave":

                    comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.Inscricao = '" + buscar + "' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.Data_Hora_Voo;";

                    ImpirmirVoo(comando);
                    UtilidadeValidarEntrada.Pausa();
                    break;



                case "dataescolhida": //data escolhida como 2022-09-28

                    comandobusca = "select Data_Hora_Voo from Voo where convert(date,Data_Hora_Voo,1) = Cast('" + buscar + "' as Date)";

                    existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where convert(date,Voo.Data_Hora_Voo,1) = Cast('" + buscar + "' as Date) and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.ID_Voo;";

                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }


                    break;

            }

        }

        public void FiltrarVoo(string filtro, string datainicial, string datafinal)
        {
            Console.Clear();

            switch (filtro)
            {
                case "intervalo": //datas em 2022-09-29

                    String comandobusca = "select Data_Hora_Voo from Voo where Data_Hora_Voo BETWEEN convert(datetime, '" + datainicial + "', 121) and convert(datetime, '" + datafinal + "', 121);";

                    bool existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        String comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where convert(date,Voo.Data_Hora_Voo,1) BETWEEN Cast('" + datainicial + "' as Date) and Cast('" + datafinal + "' as Date) and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.ID_Voo;";

                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }
                    break;
            }
        }

        public void FiltrarVoo(string situacao)
        {
            Console.Clear();
            String comandobusca;
            String comando;
            bool existe;

            switch (situacao)
            {
                case "agendados":

                    comandobusca = "select Situacao from Voo where Situacao = 'Agendado';";

                    existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.Situacao = 'Agendado' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order by Data_Hora_Voo;";
                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }
                    break;



                case "emrota":

                    comandobusca = "select Situacao from Voo where Situacao = 'Em Rota';";

                    existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.Situacao = 'Em Rota' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order by Data_Hora_Voo;";

                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }
                    break;



                case "cancelados":

                    comandobusca = "select Situacao from Voo where Situacao = 'Cancelado';";

                    existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.Situacao = 'Cancelado' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order by Data_Hora_Voo;";

                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }
                    break;



                case "finalizados":

                    comandobusca = "select Situacao from Voo where Situacao = 'Finalizado';";

                    existe = ConexaoBanco.InjetarSqlExecuteReader(comandobusca);

                    if (existe == true)
                    {
                        comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.Situacao = 'Finalizado' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order by Data_Hora_Voo;";

                        ImpirmirVoo(comando);
                        UtilidadeValidarEntrada.Pausa();
                    }
                    else
                    {
                        Console.WriteLine("Não encontrado dados para exibição.");
                        UtilidadeValidarEntrada.Pausa();
                    }
                    break;

            }
        }
    }
}
