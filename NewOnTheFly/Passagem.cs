using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Passagem
    {
        public string ID_Passagem { get; set; }
        public string ID_Voo { get; set; }
        public string ID_venda { get; set; }
        public float Valor { get; set; }
        public DateTime Data_Ulitma_Operacao { get; set; }
        public string Situacao { get; set; }
        public int Assento { get; set; }

        public Passagem() { }

        public Passagem(string iD_Passagem, string iD_Voo, string iD_venda, float valor, DateTime data_Ulitma_Operacao, string situacao, int assento)
        {
            ID_Passagem = iD_Passagem;
            ID_Voo = iD_Voo;
            ID_venda = iD_venda;
            Valor = valor;
            Data_Ulitma_Operacao = data_Ulitma_Operacao;
            Situacao = situacao;
            Assento = assento;
        }

        public static void GerarPassagens(int capacidadeassentos, string idvoo, float valor)
        {
            string id;
            for (int i = 1; i <= capacidadeassentos; i++)
            {
                id = "PA" + i.ToString();

                String comando = "insert into Passagem(ID_Passagem, ID_Voo, Valor, Data_Ultima_Operacao, Situacao, Assento)" +
                    " Values('" + id + "', '" + idvoo + "', '" + valor.ToString() + "','" + System.DateTime.Now + "', 'Livre', '" + i.ToString() + "');";

                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
            }
        }

        private static void TelaVooSelecionado(String comando)
        {
            Voo.ImpirmirVoo(comando);
        }

        public static void VenderPassagem(string operacao)
        {
            string cpf, iata, data, idvoo;

            cpf = UtilidadeValidarEntrada.ValidarEntrada("cpfvenda");
            if (cpf == null) Menu.MenuPassagem();

            iata = UtilidadeValidarEntrada.ValidarEntrada("destino");
            if (iata == null) Menu.MenuPassagem();

            data = UtilidadeValidarEntrada.ValidarEntrada("dataescolhida");
            if (data == null) Menu.MenuPassagem();

            String comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto, Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where convert(date,Voo.Data_Hora_Voo,1) = Cast('" + data + "' as Date) and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ and Voo.Situacao = 'Agendado' Order By Voo.ID_Voo;";

            if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
            {
                TelaVooSelecionado(comando);
                Console.WriteLine("\n\nESCOLHA NESSA LISTA O ID DO VOO DESEJADO E INSIRA A SEGUIR\n");

                idvoo = UtilidadeValidarEntrada.ValidarEntrada("idvoo");
                if (idvoo == null) Menu.MenuPassagem();

                comando = "select ID_Voo from Voo where ID_Voo = '" + idvoo + "';";

                if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
                {

                    comando = "select Voo.ID_Voo, Voo.Inscricao, Voo.IATA, Destino.Continente, Destino.Pais, Destino.Cidade, Destino.Nome_Aeroporto,  Voo.Data_Hora_Voo, Voo.Tempo_Voo, Voo.Situacao, CompanhiaAerea.Razao_Social from Voo, Destino, CompanhiaAerea, Aeronave where Voo.ID_Voo = '" + idvoo + "' and Voo.IATA = Destino.IATA and Voo.Inscricao = Aeronave.Inscricao and Aeronave.CNPJ = CompanhiaAerea.CNPJ Order By Voo.ID_Voo;";

                    TelaVooSelecionado(comando);
                    Console.WriteLine("\n\nVoo escolhido\n");
                    UtilidadeValidarEntrada.Pausa();

                    comando = "select Voo.Assentos_Ocupados, Aeronave.Capacidade, Passagem.Valor from Voo, Aeronave, Passagem where Voo.ID_Voo = '" + idvoo + "' and Voo.Inscricao = Aeronave.Inscricao and Voo.Situacao = 'Agendado' and Voo.ID_Voo = Passagem.ID_Voo";


                    SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

                    reader.Read();

                    var quantidadeocupada = reader.GetInt16(0);
                    string capmaxima = reader.GetString(1);
                    int capacidademaxima = Convert.ToInt32(capmaxima);
                    decimal passagemvalor = reader.GetDecimal(2);

                    ConexaoBanco.FecharConexao();

                    int quantidadedisponivel = capacidademaxima - quantidadeocupada;

                    Console.WriteLine("\nQuantidade de Passagens Disponíveis para esse voo: {0}\nPreço da Passagem: R$ {1}", quantidadedisponivel, passagemvalor);

                    if (quantidadedisponivel > 0)
                    {
                        int quantPassagem;
                        bool repetir = false;

                        do
                        {
                            do
                            {
                                Console.WriteLine("\nInforme a quantidade de passagens (máximo 4): \n1  2  3  4");
                                quantPassagem = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));

                            } while (quantPassagem < 1 && quantPassagem > 4);

                            if (quantPassagem <= quantidadedisponivel)
                            {
                                if (operacao == "reservar")
                                {
                                    for (int i = 0; i < quantPassagem; i++)
                                    {
                                        //Atualiza Passagem Reservada
                                        comando = "Update Top (1) Passagem set Situacao = 'Reservada', CPF = '" + cpf + "', Data_Ultima_Operacao = '" + System.DateTime.Now + "' where Situacao = 'Livre' and ID_Voo = '" + idvoo + "';";
                                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                                        //Atualiza Assentos Ocupados
                                        comando = "Update Top (1) Voo set Assentos_Ocupados = (Assentos_Ocupados + 1) where ID_Voo = '" + idvoo + "'; ";
                                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                                    }
                                    Console.Clear();
                                    Console.WriteLine("Reserva realizada com sucesso!");
                                    UtilidadeValidarEntrada.Pausa();
                                    repetir = false;
                                }
                                else if (operacao == "vender")
                                {
                                    Venda.GerarVenda(quantPassagem, cpf, idvoo, passagemvalor);
                                    Console.WriteLine("\nVenda realizada com sucesso!");
                                    UtilidadeValidarEntrada.Pausa();
                                    Menu.MenuPassagem();
                                    repetir = false;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\nNão possui esse tanto de passagens disponíveis.");
                                UtilidadeValidarEntrada.Pausa();
                                repetir = true;
                            }
                        } while (repetir == true);

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\nEsse voo não possui mais passagens disponíveis.");
                        UtilidadeValidarEntrada.Pausa();
                        Menu.MenuPassagem();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nVoo Não Encontrado!");
                    UtilidadeValidarEntrada.Pausa();
                    Menu.MenuPassagem();
                }

            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nNão existe nenhum voo nessa data.");
                UtilidadeValidarEntrada.Pausa();
                Menu.MenuPassagem();
            }


        }

        public static void FiltrarPassagemReservada(string operacao)
        {
            string cpf;
            int opc;

            cpf = UtilidadeValidarEntrada.ValidarEntrada("cpfexiste");
            if (cpf == null) Menu.MenuPassagem();

            String comando = "select ID_Passagem, ID_Voo, Valor, Data_Ultima_Operacao from Passagem" +
                " where CPF = '" + cpf + "' and Situacao = 'Reservada';";

            if (operacao == "exibir")
            {
                if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
                {
                    ImprimirPassagemReservada(comando);
                    UtilidadeValidarEntrada.Pausa();
                    Menu.MenuPassagem();
                }
                else
                {
                    Console.WriteLine("Nenhum dado de passagem reservada encontrado para esse passageiro!");
                    UtilidadeValidarEntrada.Pausa();
                }
            }


            else if (operacao == "cancelar")
            {
                if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
                {
                    ImprimirPassagemReservada(comando);

                    string idpassagem = UtilidadeValidarEntrada.ValidarEntrada("idpassagem");
                    if (idpassagem == null) Menu.MenuPassagem();
                    Console.Clear();

                    do
                    {
                        Console.WriteLine("\n 1 - Efetuar cancelamento da Passagem\n");

                        Console.WriteLine("\n 0 - Cancelar Operação \n");
                        opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                        Console.Clear();

                        switch (opc)
                        {
                            case 0:

                                Menu.MenuPassagem();

                                break;

                            case 1:

                                comando = "update Passagem set Situacao = 'Livre', CPF = null, Data_Ultima_Operacao = '" + System.DateTime.Now + "' where CPF = '" + cpf + "' and Situacao = 'Reservada' and ID_Passagem = '" + idpassagem + "';";
                                ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                                Console.WriteLine("Reserva Cancelada com Sucesso!");
                                UtilidadeValidarEntrada.Pausa();
                                break;

                        }
                    } while (opc > 2);
                }
                else
                {
                    Console.WriteLine("Nenhum dado de passagem reservada encontrado para esse passageiro!");
                    UtilidadeValidarEntrada.Pausa();
                }

            }
            else if (operacao == "vender")
            {
                if (ConexaoBanco.InjetarSqlExecuteReader(comando) == true)
                {
                    ImprimirPassagemReservada(comando);

                    string idpassagem = UtilidadeValidarEntrada.ValidarEntrada("idpassagem");
                    if (idpassagem == null) Menu.MenuPassagem();
                    Console.Clear();

                    do
                    {
                        Console.WriteLine("\n 1 - Efetuar Venda da Passagem\n");

                        Console.WriteLine("\n 0 - Cancelar Operação \n");
                        opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                        Console.Clear();

                        switch (opc)
                        {
                            case 0:

                                Menu.MenuPassagem();

                                break;

                            case 1:

                                comando = "select CPF, Valor, ID_Voo from Passagem where CPF = '" + cpf + "' and Situacao = 'Reservada' and ID_Passagem = '" + idpassagem + "';";
                                SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

                                reader.Read();

                                cpf = reader.GetString(0);
                                decimal passagemvalor = reader.GetDecimal(1);
                                string idvoo = reader.GetString(2);
                                ConexaoBanco.FecharConexao();

                                Venda.GerarVendaReservada(idpassagem, cpf, idvoo, passagemvalor);
                                
                                Console.WriteLine("Venda realizada com Sucesso!");
                                UtilidadeValidarEntrada.Pausa();
                                Menu.MenuPassagem();
                                break;

                        }
                    } while (opc > 2);
                }
                else
                {
                    Console.WriteLine("Nenhum dado de passagem reservada encontrado para esse passageiro!");
                    UtilidadeValidarEntrada.Pausa();
                }
            }
        }

        public static void ImprimirPassagemReservada(String comando)
        {
            Console.Clear();
            ConexaoBanco.AbrirConexao();

            SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

            Console.WriteLine("DADOS DE PASSAGEM RESERVADA\n");

            while (reader.Read())
            {
                Console.WriteLine("\nID Passagem: {0}", reader.GetString(0));
                Console.WriteLine("\nID Voo: {0}", reader.GetString(1));
                Console.WriteLine("\nValor da Passagem: {0}", reader.GetDecimal(2));
                Console.WriteLine("\nData da Reserva: {0}", reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm"));
                DateTime datareserva = reader.GetDateTime(3);
                DateTime dataexpirareserva = datareserva.AddDays(+2);
                Console.WriteLine("\nReserva Expira em: {0}\n\n", dataexpirareserva.ToString("dd/MM/yyyy HH:mm"));
            }

            ConexaoBanco.FecharConexao();
        }
    }
}
