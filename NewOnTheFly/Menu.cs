using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Menu
    {
        public static void MenuInicial()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Sistema de gerenciamento Aeroporto: New On The Fly\n");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Companhia Aérea\n");
                Console.WriteLine(" 2 - Passageiro\n");
                Console.WriteLine(" 3 - Venda de Passagem\n");
                Console.WriteLine(" 4 - Cancelar Voo\n");
                Console.WriteLine(" 5 - Exibir Voos\n");
                Console.WriteLine("\n 0 - Encerrar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        Console.WriteLine("Encerrado");
                        Environment.Exit(0);

                        break;

                    case 1:

                        MenuCompanhiaAerea();

                        break;

                    case 2:

                        MenuPassageiro();

                        break;

                    case 3:

                        MenuPassagem();

                        break;

                    case 4:

                        Voo.CancelarVoo();

                        break;

                    case 5:

                        MenuVoo();

                        break;
                }

            } while (true);

        }

        public static void MenuCompanhiaAerea()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Cadastrar Companhia Aérea\n");
                Console.WriteLine(" 2 - Cadastrar Aeronave\n");
                Console.WriteLine(" 3 - Cadastrar Voo\n");
                Console.WriteLine(" 4 - Editar Companhia Aérea cadastrada\n");
                Console.WriteLine(" 5 - Editar Aeronave\n");
                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuInicial();

                        break;

                    case 1:

                        CompanhiaAerea.CadastrarCompanhiaAerea();

                        break;

                    case 2:

                        Aeronave.CadastrarAeronave();

                        break;


                    case 3:

                        MenuCadastrarVoo();

                        break;


                    case 4:

                        MenuEditarCompanhiaAerea();

                        break;

                    case 5:

                        MenuEditarAeronave();

                        break;
                }

            } while (opc != 0);
        }

        public static void MenuPassageiro()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Cadastrar Passageiro\n");
                Console.WriteLine(" 2 - Editar Passageiro cadastrado\n");
                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuInicial();

                        break;

                    case 1:

                        Passageiro.CadastrarPassageiro();

                        break;

                    case 2:

                        MenuEditarPassageiro();

                        break;
                }

            } while (opc != 0);
        }

        public static void MenuPassagem()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Vender Passagem\n");
                Console.WriteLine(" 2 - Reservar Passagem\n");
                Console.WriteLine(" 3 - Vendas Realizadas\n");
                Console.WriteLine(" 4 - Reservas Realizadas\n");
                Console.WriteLine(" 5 - Vender Passagem Reservada\n");
                Console.WriteLine(" 6 - Cancelar Passagem Reservada\n");
                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuInicial();

                        break;

                    case 1:

                        Passagem.VenderPassagem("vender");

                        break;

                    case 2:

                        Passagem.VenderPassagem("reservar");

                        break;

                    case 3:

                        Venda.FiltrarVenda();

                        break;

                    case 4:

                        Passagem.FiltrarPassagemReservada("exibir");

                        break;

                    case 5:

                        Passagem.FiltrarPassagemReservada("vender");

                        break;

                    case 6:

                        Passagem.FiltrarPassagemReservada("cancelar");

                        break;

                }

            } while (true);

        }

        public static void MenuVoo()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Sistema de gerenciamento Aeroporto: New On The Fly");
                Console.WriteLine("\nFiltrar por:\n");
                Console.WriteLine(" 1  - Intervalo de tempo\n");
                Console.WriteLine(" 2  - Dia\n");
                Console.WriteLine(" 3  - ID Voo \n");
                Console.WriteLine(" 4  - Aeroporto de Destino \n");
                Console.WriteLine(" 5  - Aeronave\n");
                Console.WriteLine(" 6  - Agendados\n");
                Console.WriteLine(" 7  - Em Rota no momento\n");
                Console.WriteLine(" 8  - Cancelados\n");
                Console.WriteLine(" 9  - Realizados\n");

                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                Voo voo = new Voo();

                switch (opc)
                {
                    case 0:
                        MenuInicial();
                        break;

                    case 1:

                        string datainicial = UtilidadeValidarEntrada.ValidarEntrada("datainicial");
                        if (datainicial == null) MenuVoo();
                        string datafinal = UtilidadeValidarEntrada.ValidarEntrada("datafinal");
                        if (datafinal == null) MenuVoo();

                        if (DateTime.Compare(DateTime.Parse(datainicial), DateTime.Parse(datafinal)) <= 0)
                            voo.FiltrarVoo("intervalo", datainicial, datafinal);
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Intervalo inválido! A data incial deve ser menor que a data final!");
                            Console.WriteLine("\nPESSIONE ENTER PARA CONTINUAR");
                            Console.ReadKey();
                            MenuVoo();
                        }

                        break;

                    case 2:

                        string dataescolhida = UtilidadeValidarEntrada.ValidarEntrada("dataescolhida");
                        if (dataescolhida == null) MenuVoo();

                        voo.FiltrarVoo("dataescolhida", dataescolhida);

                        break;

                    case 3:

                        string idvoo = UtilidadeValidarEntrada.ValidarEntrada("idvoo");
                        if (idvoo == null) MenuVoo();

                        voo.FiltrarVoo("idvoo", idvoo);

                        break;

                    case 4:

                        string destino = UtilidadeValidarEntrada.ValidarEntrada("destino");
                        if (destino == null) MenuVoo();

                        voo.FiltrarVoo("aeroporto", destino);

                        break;

                    case 5:

                        string inscricao = UtilidadeValidarEntrada.ValidarEntrada("aeronaveeditar");
                        if (inscricao == null) MenuVoo();

                        voo.FiltrarVoo("aeronave", inscricao);

                        break;

                    case 6:

                        voo.FiltrarVoo("agendados");

                        break;

                    case 7:

                        voo.FiltrarVoo("emrota");

                        break;

                    case 8:

                        voo.FiltrarVoo("cancelados");

                        break;

                    case 9:

                        voo.FiltrarVoo("finalizados");

                        break;

                }
            } while (true);







        }

        public static void MenuEditarCompanhiaAerea()
        {

            string cnpj = UtilidadeValidarEntrada.ValidarEntrada("cnpjexiste");
            if (cnpj == null) MenuCompanhiaAerea();

            int opc = 0;
            do
            {
                Console.Clear();
                String comando = "select CNPJ, Razao_Social, Situacao, Data_Abertura, Data_Cadastro from CompanhiaAerea where CNPJ = '" + cnpj + "';";
                CompanhiaAerea.ImprimirCompanhiaAerea(comando);

                Console.WriteLine("\nEditar dados\n");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Razão Social\n");
                Console.WriteLine(" 2 - Situacao\n");

                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuCompanhiaAerea();

                        break;

                    case 1:

                        string razaosocial = UtilidadeValidarEntrada.ValidarEntrada("nome");
                        if (razaosocial == null) MenuCompanhiaAerea();

                        comando = "update CompanhiaAerea set Razao_Social = '" + razaosocial + "' where CNPJ = '" + cnpj + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                    case 2:

                        string situacao = UtilidadeValidarEntrada.ValidarEntrada("situacao");
                        if (situacao == null) MenuCompanhiaAerea();
                        if (situacao == "A") comando = "update CompanhiaAerea set Situacao = 'Ativa' where CNPJ = '" + cnpj + "';";
                        if (situacao == "I") comando = "update CompanhiaAerea set Situacao = 'Inativa' where CNPJ = '" + cnpj + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                        break;
                }

            } while (true);
        }

        public static void MenuEditarAeronave()
        {
            string idaeronave = UtilidadeValidarEntrada.ValidarEntrada("aeronaveeditar");
            if (idaeronave == null) MenuCompanhiaAerea();

            int opc = 0;
            do
            {
                Console.Clear();
                String comando = "select Inscricao, Capacidade, Situacao, Velocidade_Media, Distancia_Voo_Max, Data_Cadastro from Aeronave where Inscricao = '" + idaeronave + "';";
                Aeronave.ImprimirAeronave(comando);

                Console.WriteLine("\nEditar dados\n");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Capacidade\n");
                Console.WriteLine(" 2 - Situacao\n");
                Console.WriteLine(" 3 - Velocidade de Voo\n");
                Console.WriteLine(" 4 - Distância de Voo\n");

                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuCompanhiaAerea();

                        break;

                    case 1:

                        string capacidade = UtilidadeValidarEntrada.ValidarEntrada("capacidade");
                        if (capacidade == null) MenuCompanhiaAerea();

                        comando = "update Aeronave set Capacidade = '" + capacidade + "' where Inscricao = '" + idaeronave + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                    case 2:

                        string situacao = UtilidadeValidarEntrada.ValidarEntrada("situacao");
                        if (situacao == null) MenuCompanhiaAerea();
                        if (situacao == "A") comando = "update Aeronave set Situacao = 'Ativa' where Inscricao = '" + idaeronave + "';";
                        if (situacao == "I") comando = "update Aeronave set Situacao = 'Inativa' where Inscricao = '" + idaeronave + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                    case 3:

                        string velocidadevoo = UtilidadeValidarEntrada.ValidarEntrada("velocidadevoo");
                        if (velocidadevoo == null) MenuCompanhiaAerea();

                        comando = "update Aeronave set Velocidade_Media = '" + velocidadevoo + "' where Inscricao = '" + idaeronave + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                    case 4:

                        string distanciavoomax = UtilidadeValidarEntrada.ValidarEntrada("distanciavoomax");
                        if (distanciavoomax == null) MenuCompanhiaAerea();

                        comando = "update Aeronave set Distancia_Voo_Max = '" + distanciavoomax + "' where Inscricao = '" + idaeronave + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;
                }

            } while (true);
        }

        public static void MenuEditarPassageiro()
        {
            string cpf = UtilidadeValidarEntrada.ValidarEntrada("cpfexiste");
            if (cpf == null) MenuPassageiro();

            int opc = 0;
            do
            {
                Console.Clear();
                String comando = "select CPF, Nome, Situacao, Sexo, Data_Nascimento, Data_Cadastro from Passageiro where CPF = '" + cpf + "';";
                Passageiro.ImprimirPassageiro(comando);

                Console.WriteLine("\nEditar dados\n");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Nome\n");
                Console.WriteLine(" 2 - Situacao\n");
                Console.WriteLine(" 3 - Sexo\n");
                Console.WriteLine(" 4 - Data de Nascimento\n");

                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuPassageiro();

                        break;

                    case 1:

                        string nome = UtilidadeValidarEntrada.ValidarEntrada("nome");
                        if (nome == null) MenuPassageiro();

                        comando = "update Passageiro set Nome = '" + nome + "' where CPF = '" + cpf + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                    case 2:

                        string situacao = UtilidadeValidarEntrada.ValidarEntrada("situacao");
                        if (situacao == null) MenuPassageiro();
                        if (situacao == "A") comando = "update Passageiro set Situacao = 'Ativa' where CPF = '" + cpf + "';";
                        if (situacao == "I") comando = "update Passageiro set Situacao = 'Inativa' where CPF = '" + cpf + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                        break;

                    case 3:

                        string sexo = UtilidadeValidarEntrada.ValidarEntrada("sexo");
                        if (sexo == null) MenuPassageiro();
                        if (sexo == "M") comando = "update Passageiro set Sexo = 'Masculino' where CPF = '" + cpf + "';";
                        if (sexo == "F") comando = "update Passageiro set Sexo = 'Feminino' where CPF = '" + cpf + "';";
                        if (sexo == "N") comando = "update Passageiro set Sexo = 'Não Informado' where CPF = '" + cpf + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
                        break;

                    case 4:

                        string datanascimento = UtilidadeValidarEntrada.ValidarEntrada("datanascimento");
                        if (datanascimento == null) MenuPassageiro();


                        comando = "update Passageiro set Data_Nascimento = '" + DateTime.Parse(datanascimento) + "' where CPF = '" + cpf + "';";
                        ConexaoBanco.InjetarSqlExecuteNonQuery(comando);

                        break;

                }

            } while (true);

        }

        public static void MenuCadastrarVoo()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nSelecione o Destino:\n");
                Console.WriteLine(" 1 - América do Norte\n");
                Console.WriteLine(" 2 - América Central\n");
                Console.WriteLine(" 3 - América do Sul\n");
                Console.WriteLine(" 4 - Europa\n");
                Console.WriteLine(" 5 - África\n");
                Console.WriteLine(" 6 - Oceania\n");
                Console.WriteLine(" 7 - Ásia\n");

                Console.WriteLine("\n 0 - Voltar\n");
                opc = int.Parse(UtilidadeValidarEntrada.ValidarEntrada("menu"));
                Console.Clear();
                UtilidadeAtualizacao.AtualizarSistema();
                switch (opc)
                {
                    case 0:

                        MenuCompanhiaAerea();

                        break;


                    case 1:

                        Voo.CadastrarVoo("América do Norte");

                        break;


                    case 2:

                        Voo.CadastrarVoo("América Central");

                        break;


                    case 3:

                        Voo.CadastrarVoo("América do Sul");

                        break;


                    case 4:

                        Voo.CadastrarVoo("Europa");

                        break;


                    case 5:

                        Voo.CadastrarVoo("África");

                        break;


                    case 6:

                        Voo.CadastrarVoo("Oceania");

                        break;


                    case 7:

                        Voo.CadastrarVoo("Ásia");

                        break;
                }

            } while (opc != 0);
        }
    }

}
