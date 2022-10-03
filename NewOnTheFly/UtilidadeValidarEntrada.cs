using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class UtilidadeValidarEntrada
    {
        static bool PausaMensagem()
        {
            bool repetirdo;
            do
            {
                Console.WriteLine("\nPressione S para informar novamente ou C para cancelar:");
                ConsoleKeyInfo op = Console.ReadKey(true);
                if (op.Key == ConsoleKey.S)
                {
                    Console.Clear();
                    return false;
                }
                else
                {
                    if (op.Key == ConsoleKey.C)
                    {
                        Console.Clear();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Escolha uma opção válida!");
                        repetirdo = true;
                    }
                }
            } while (repetirdo == true);
            return true;
        }

        public static void Pausa()
        {
            Console.WriteLine("\nAperte 'ENTER' para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        public static string ValidarEntrada(string entrada)
        {
            string[] vetorletras = new string[] {"Ç","ç","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
            "T","U","V","W","X","Y","Z","Á","É","Í","Ó","Ú","À","È","Ì","Ò","Ù","Â","Ê","Î","Ô","Û","Ã","Õ"," "};
            string[] vetornumeros = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            bool encontrado;
            bool retornar = true;
            int qtdnumerosiguais = 0;


            switch (entrada)
            {
                case "menu":

                    #region menu

                    do
                    {
                        try
                        {
                            char[] vetortecla;
                            Console.CursorVisible = false;
                            ConsoleKeyInfo op = Console.ReadKey(true);
                            vetortecla = op.Key.ToString().ToCharArray();

                            if (vetortecla[0] == 'N')
                            {
                                if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                {
                                    return vetortecla[6].ToString();
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            else
                            {
                                if (vetortecla[0] == 'D')
                                {
                                    if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                    {
                                        return vetortecla[1].ToString();
                                    }
                                    else
                                    {
                                        encontrado = false;
                                    }
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            encontrado = false;
                        }
                    } while (encontrado == false);

                    return null;


                #endregion


                case "cpf":

                    #region CPF;

                    do
                    {
                        retornar = false;
                        qtdnumerosiguais = 0;
                        long cpf;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CPF: ");

                            cpf = long.Parse(Console.ReadLine());

                            char[] letras = cpf.ToString().ToCharArray();

                            //verifica se tem 11 caracteres:
                            if (letras.Length == 11)
                            {

                                //Verifica se é um cpf válido calculando os 2 últimos digitos, segundo a receita federal:
                                int soma = 0;
                                int resto = 0;
                                int digito1 = 0;
                                int digito2 = 0;

                                //Verifica se os números são iguais

                                for (int i = 0; i < 9; i++)
                                {
                                    if (letras[i] == letras[i + 1])
                                        qtdnumerosiguais = qtdnumerosiguais + 1;
                                }

                                //Se os 9 primeiros digitos forem todos iguais, invalida o cpf:
                                if (qtdnumerosiguais != 9)
                                {
                                    //calcula o primeiro digito verificador do cpf:
                                    for (int i = 1, j = 0; i < 10; i++, j++)
                                        soma = soma + (int.Parse(letras[j].ToString()) * i);

                                    resto = soma % 11;

                                    if (resto >= 10)
                                    {
                                        digito1 = 0;

                                    }
                                    else
                                    {
                                        digito1 = resto;
                                    }

                                    //Verifica se o primeiro digito digitado é igual ao que era pra ser:
                                    if (digito1 == int.Parse(letras[9].ToString()))
                                    {
                                        soma = 0; //seta o soma em 0 para o processo de soma do segundo digito do cpf:

                                        //calcula o segundo digito verificador do cpf:
                                        for (int i = 0, j = 0; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        resto = soma % 11;

                                        if (resto >= 10)
                                        {
                                            digito2 = 0;
                                        }
                                        else
                                        {
                                            digito2 = resto;
                                        }
                                        //Verifica se o segundo digito digitado é igual ao que era pra ser:
                                        if (digito2 == int.Parse(letras[10].ToString()))
                                        {
                                            //Se digitos validados, procura no banco de dados se já existe o cpf cadastrado:

                                            //Se achar no banco, não deixa prosseguir

                                            String comando = "select CPF from Passageiro where CPF = '" + cpf.ToString() + "';";

                                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                            if (encontrado == true)
                                            {
                                                //Se encontrar, invalida o cadastro
                                                Console.WriteLine("CPF já cadastrado!");
                                                retornar = PausaMensagem();
                                            }
                                            else return cpf.ToString();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Esse não é um CPF válido!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Esse não é um CPF válido!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("CPF com números sequenciais iguais não é válido!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Só aceita números válidos de 11 digitos");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Só aceita números válidos de 11 digitos");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion;


                case "cnpj":

                    #region CNPJ;

                    do
                    {
                        retornar = false; // só retorna se o usuário quiser
                        qtdnumerosiguais = 0;
                        long cnpj;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CNPJ: ");

                            cnpj = long.Parse(Console.ReadLine());

                            char[] letras = cnpj.ToString().ToCharArray();

                            //verifica se tem 14 caracteres:
                            if (letras.Length == 14)
                            {
                                //Qualquer valor que não seja um número invalida o cnpj:

                                //Verifica se os números são iguais
                                for (int i = 0; i < 12; i++)
                                {
                                    if (letras[i] == letras[i + 1])
                                        qtdnumerosiguais = qtdnumerosiguais + 1;
                                }

                                //Se os 12 primeiros digitos forem todos iguais, invalida o cnpj:
                                if (qtdnumerosiguais != 12)
                                {
                                    int soma = 0;
                                    int resto = 0;
                                    int digito1 = 0;
                                    int digito2 = 0;

                                    //calcula o primeiro digito verificador do cnpj:
                                    for (int i = 6, j = 0; i < 10; i++, j++)
                                        soma = soma + (int.Parse(letras[j].ToString()) * i);

                                    for (int i = 2, j = 4; i < 10; i++, j++)
                                        soma = soma + (int.Parse(letras[j].ToString()) * i);

                                    resto = soma % 11;

                                    if (resto >= 10)
                                    {
                                        digito1 = 0;

                                    }
                                    else
                                    {
                                        digito1 = resto;
                                    }

                                    //Verifica se o primeiro digito digitado é igual ao que era pra ser:
                                    if (digito1 == int.Parse(letras[12].ToString()))
                                    {
                                        soma = 0; //seta o soma em 0 para o processo de soma do segundo digito do cnpj:

                                        //calcula o segundo digito verificador do cpf:
                                        for (int i = 5, j = 0; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        for (int i = 2, j = 5; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        resto = soma % 11;

                                        if (resto >= 10)
                                        {
                                            digito2 = 0;

                                        }
                                        else
                                        {
                                            digito2 = resto;
                                        }
                                        //Verifica se o segundo digito digitado é igual ao que era pra ser:
                                        if (digito2 == int.Parse(letras[13].ToString()))
                                        {
                                            //Se digitos validados, procura no banco de dados se já existe o cnpj cadastrado:


                                            //Se achar, não deixa prosseguir
                                            String comando = "select CNPJ from CompanhiaAerea where CNPJ = '" + cnpj.ToString() + "';";

                                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                            if (encontrado == true)
                                            {
                                                //Se encontrar, invalida o cadastro
                                                Console.WriteLine("CNPJ já cadastrado!");
                                                retornar = PausaMensagem();
                                            }
                                            else return cnpj.ToString();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Esse não é um CNPJ válido!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Esse não é um CNPJ válido!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("CNPJ com números sequenciais iguais não é válido!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Só aceita números válidos de 14 digitos");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Só aceita números válidos de 14 digitos");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion;


                case "nome":

                    #region Nome

                    do
                    {
                        string nome;
                        encontrado = true;
                        retornar = false;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o Nome/Razão Social: ");

                            nome = Console.ReadLine();

                            char[] letras = nome.ToCharArray();

                            //Verifica se o nome tem no mínimo 3 e no máximo 50 caracteres:
                            if (letras.Length > 3 && letras.Length <= 50)
                            {
                                //Verifica se o nome só tem letras válidas:
                                for (int i = 0; i < letras.Length && encontrado != false; i++)
                                {
                                    foreach (var v in vetorletras)
                                    {
                                        if (letras[i].ToString().ToUpper().Equals(v))
                                        {
                                            encontrado = true;
                                            break;
                                        }
                                        else encontrado = false;
                                    }
                                }
                                //Se possuir somente letras válidas, prossegue:
                                if (encontrado == true)
                                {
                                    return nome;
                                }
                                else
                                {
                                    Console.WriteLine("Nome só aceita letras.");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nome informado não é válido! Insira o nome completo, máximo 50 caracteres!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um valor válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "sexo":

                    #region Sexo
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Informe o sexo:\n[M] - Masculino\n[F] - Feminino\n[N] - Não informar");
                        Console.CursorVisible = false;
                        ConsoleKeyInfo op = Console.ReadKey(true);

                        //Verificar se tecla pressionada foi M / F ou N (independente do CAPSLOCK estar ativado!)
                        if (op.Key == ConsoleKey.M)
                        {
                            Console.Clear();
                            return "M";
                        }
                        else
                        {
                            if (op.Key == ConsoleKey.F)
                            {
                                Console.Clear();
                                return "F";
                            }
                            else
                            {
                                if (op.Key == ConsoleKey.N)
                                {
                                    Console.Clear();
                                    return "N";
                                }
                                else
                                {
                                    Console.WriteLine("Escolha uma opção válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "datanascimento":

                    #region DataNascimento

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataNascimento = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data de Nascimento:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataNascimento = DataNascimento + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {

                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataNascimento = DataNascimento + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            int date = int.Parse(DataNascimento);
                            string datacombarras = date.ToString("00/00/0000");

                            if (encontrado == true)
                            {
                                if (DateTime.Compare(DateTime.Parse(datacombarras), System.DateTime.Now) < 0)
                                {
                                    return datacombarras;
                                }
                                else
                                {
                                    Console.WriteLine("Data de nascimento não aceita datas futuras, insira uma data válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;


                #endregion


                case "dataabertura":

                    #region DataAberturaCompanhiaAerea

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataAbertura = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data de abertura da Empresa:");

                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;

                            }

                            //Verificar se digitou só nrs válidos
                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataAbertura = DataAbertura + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {
                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataAbertura = DataAbertura + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            int date = int.Parse(DataAbertura);
                            string datacombarras = date.ToString("00/00/0000");

                            //Se só digitou números válidos, continua:
                            if (encontrado == true)
                            {
                                //Verificar se é data futura:
                                if (DateTime.Compare(DateTime.Parse(datacombarras), System.DateTime.Now) < 0)
                                {
                                    //Verificar se a abertura da empresa é maior que 6 meses:
                                    if (DateTime.Compare(DateTime.Parse(datacombarras), System.DateTime.Now.AddMonths(-6)) < 0)
                                    {
                                        ///////RETORNA A DATA DE ABERTURA
                                        return datacombarras;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nO Aeroporto não aceita cadastrar companhia aérea com menos de 6 meses de existência.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\nVocê será redirecionado para o menu anterior.");
                                        Pausa();

                                        //Retorna nullo direto, não tem a opção de digitar a data novamente
                                        return null;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Não aceita datas futuras, insira uma data válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);
                    Console.Clear();
                    return null;


                #endregion


                case "datavoo":

                    #region DataVoo

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataVoo = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_", " ", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data e hora do Voo:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7] + " " + vetordata[9] + vetordata[10] + ":" + vetordata[11] + vetordata[12]);
                                Console.CursorVisible = false;
                            }

                            //Verificar se digitou só nrs válidos
                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                //Verifica se foi teclado realmente um número:
                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataVoo = DataVoo + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {
                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataVoo = DataVoo + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            //Se todas entradas foram números válidos, continua:
                            //A variável DataVoo nesse instante só tem datas sem barras, ex: "12345678":
                            if (encontrado == true)
                            {
                                //Pede os dados da hora separado da data:
                                for (int i = 9; i < 13; i++)
                                {
                                    AtualizarTela(vetordata);

                                    teclaData = Console.ReadKey(true);

                                    vetortecla = teclaData.Key.ToString().ToCharArray();

                                    //Verifica se foi teclado realmente um número:
                                    if (vetortecla[0] == 'N')
                                    {
                                        if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[6].ToString();
                                            DataVoo = DataVoo + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        if (vetortecla[0] == 'D')
                                        {
                                            if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                            {
                                                encontrado = true;
                                                vetordata[i] = vetortecla[1].ToString();
                                                DataVoo = DataVoo + vetordata[i];
                                            }
                                            else
                                            {
                                                encontrado = false;
                                                break;
                                            }

                                            AtualizarTela(vetordata);
                                        }
                                    }
                                }

                                //A variável DataVoo nesse instante tem a data e a hora sem formatação, ex: "123456781234":
                                //Converte para: "12/34/5678 12:34":
                                long date = long.Parse(DataVoo);
                                string datacombarras = date.ToString("00/00/0000 00:00");

                                //Se dados da hora forem válidos, continua:
                                if (encontrado == true)
                                {
                                    //Verifica se a data de cadastro do voo não é data antiga:
                                    if (DateTime.Compare(DateTime.Parse(datacombarras), System.DateTime.Now) > 0)
                                    {
                                        //////RETORNA A DATA DO VOO "12/34/5678 12:34"
                                        return datacombarras;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Não é possível agendar voo em datas passadas!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Insira uma hora válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    Console.Clear();
                    return null;

                #endregion


                case "idaeronave":

                    #region IdAeronave

                    //Os prefixos de nacionalidade que identificam aeronaves privadas e comerciais do Brasil são PT, PR, PP, PS e PH.
                    string[] prefixoaeronave = new string[] { "PT", "PR", "PP", "PS", "PH" };

                    //A Agência Nacional de Aviação Civil(Anac) proíbe o registro de marcas de identificação em aeronaves iniciadas com a letra Q
                    //ou que tenham W como segunda letra.Os arranjos SOS, XXX, PAN, TTT, VFR, IFR, VMC e IMC não podem ser utilizados.
                    string[] idproibido = new string[] { "SOS", "XXX", "PAN", "TTT", "VFR", "IFR", "VMC", "IMC" };
                    encontrado = false;
                    string idaeronave;

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o código Nacional de identificação da Aeronave: ");

                            idaeronave = Console.ReadLine().ToUpper();

                            char[] letras = idaeronave.ToCharArray();

                            //Verifica se tem 6 caracteres obrigatoriamente:
                            if (letras.Length == 6)
                            {
                                //verifica se foi inserido o traço - na inscrição:
                                if (letras[2] == '-')
                                {
                                    //Verifica se tem Q e W onde não pode na matrícula da aeronave:
                                    if (letras[3] != 'Q' && letras[4] != 'W')
                                    {
                                        //Separa a escrita depois do traço, referente à matrícula do avião:
                                        string matriculaaviao = letras[3].ToString() + letras[4].ToString() + letras[5].ToString();
                                        //Verifica se a matrícula possui um nome proibido, contido no vetor idproibido;
                                        if (idproibido.Contains(matriculaaviao) == false)
                                        {
                                            //Separa os 2 primeiros prefixos e guarda na variável prefixoaviao:
                                            string prefixoaviao = letras[0].ToString() + letras[1].ToString();
                                            //Verifica se os 2 primeiros prefixos são válidos:
                                            if (prefixoaeronave.Contains(prefixoaviao) == true)
                                            {

                                                String comando = "select Inscricao from Aeronave where Inscricao = '" + idaeronave + "';";

                                                encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                                if (encontrado == true)
                                                {
                                                    //Se encontrar, invalida o cadastro
                                                    Console.WriteLine("Essa Aeronave já possui cadastro!");
                                                    retornar = PausaMensagem();
                                                }
                                                else return idaeronave;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Os prefixos devem ser obrigatóriamente PT ou PR ou PP ou PS ou PH ");
                                                retornar = PausaMensagem();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("As matrículas SOS, XXX, PAN, TTT, VFR, IFR, VMC e IMC não podem ser utilizadas");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Não é permitido a letra Q como primeira letra e nem a letra W como segunda letra da matrícula da aeronave");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Digite obrigatóriamente o traço - após prefixos de nacionalidade");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Digite obrigatóriamente o traço - Quantidade incorreta de dígitos de identificação.");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um valor válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "capacidade":

                    #region Capacidade

                    int capacidade;

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("Informe a quantidade de passageiros que a aeronave comporta: ");

                            capacidade = int.Parse(Console.ReadLine());

                            if (capacidade < 1000 && capacidade > 0)
                            {
                                return capacidade.ToString();
                            }
                            else
                            {
                                Console.WriteLine("Deve ter um valor de quantidade de passageiros, só aceita no máximo 3 dígitos!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Insira apenas números!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;



                #endregion


                case "situacao":

                    #region Situacao


                    do
                    {
                        Console.Clear();
                        Console.WriteLine("A situação ficará no sistema como:\n[A] - Ativa\n[I] - Inativa");
                        Console.CursorVisible = false;
                        ConsoleKeyInfo op = Console.ReadKey(true);

                        //Verificar se tecla pressionada foi A ou I (independente do CAPSLOCK estar ativado!)
                        if (op.Key == ConsoleKey.A)
                        {
                            Console.Clear();
                            return "A";
                        }
                        else
                        {
                            if (op.Key == ConsoleKey.I)
                            {
                                Console.Clear();
                                return "I";
                            }
                            else
                            {
                                Console.WriteLine("Escolha uma opção válida!");
                                retornar = PausaMensagem();
                            }
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "destino":

                    #region Destino

                    do
                    {
                        retornar = false;
                        try
                        {

                            Console.Write("Informe o código IATA do aeroporto de destino: ");
                            string iata = Console.ReadLine().ToUpper();

                            String comando = "select IATA from Destino where IATA = '" + iata + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return iata;
                            }
                            else
                            {
                                //Se não encontrar, invalida o cadastro do voo
                                Console.WriteLine("Código não encontrado! Insira um código IATA válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um código IATA válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "aeronave":

                    #region Aeronave

                    encontrado = false;
                    do
                    {
                        retornar = false;


                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o código Nacional de identificação da Aeronave: ");
                            idaeronave = Console.ReadLine().ToUpper();

                            String comando = "select Inscricao from Aeronave where Inscricao = '" + idaeronave + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                comando = "select Situacao from Aeronave where Inscricao = '" + idaeronave + "' and Situacao = 'Ativa';";

                                encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                if (encontrado == true)
                                {
                                    return idaeronave;
                                }
                                else
                                {
                                    //Se não estiver Ativa, invalida o cadastro do voo
                                    Console.WriteLine("Essa Aeronave está INATIVA no sistema!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                //Se não encontrar, invalida o cadastro do voo
                                Console.WriteLine("Aeronave não encontrada! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Insira um código válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "aeronaveeditar":

                    #region AeronaveEditar
                    encontrado = false;
                    do
                    {
                        retornar = false;


                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o código Nacional de identificação da Aeronave: ");

                            idaeronave = Console.ReadLine().ToUpper();

                            String comando = "select Inscricao from Aeronave where Inscricao = '" + idaeronave + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return idaeronave;
                            }
                            else
                            {
                                //Se não encontrar, invalida o cadastro do voo
                                Console.WriteLine("Código não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Código não encontrado! Insira um código válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "valorpassagem":

                    #region ValorPassagem

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("insira o valor da passagem: ");

                            float valor = float.Parse(Console.ReadLine());
                            if (valor > 0 && valor < 9999999.99)
                            {
                                return valor.ToString().Replace(',', '.');
                            }
                            else
                            {
                                Console.WriteLine("Não é possível vender passagens desse valor");
                                retornar = PausaMensagem();
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Erro: Escolha uma opção válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;

                #endregion


                case "cpfvenda":

                    #region CpfVenda

                    do
                    {
                        retornar = false;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CPF do Passageiro para prosseguir: ");

                            string cpf = Console.ReadLine();

                            //Verifica se existe o cpf no cadastro
                            String comando = "select CPF from Passageiro where CPF = '" + cpf + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                //Verifica se situação está Ativa
                                comando = "select Situacao from Passageiro where CPF = '" + cpf + "' and Situacao = 'Ativa';";

                                encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                if (encontrado == true)
                                {
                                    comando = "select Data_Nascimento from Passageiro where CPF = '" + cpf + "' and Situacao = 'Ativa';";


                                    SqlDataReader reader = ConexaoBanco.RetornarExecuteReader(comando);

                                    reader.Read();

                                    DateTime datanascimento = reader.GetDateTime(0);

                                    ConexaoBanco.FecharConexao();

                                    //Verifica se é maior de idade:
                                    if (DateTime.Compare(datanascimento, System.DateTime.Now.AddYears(-18)) <= 0)
                                    {
                                        //Verifica se está restrito
                                        comando = "select CPF from Restritos where CPF = '" + cpf + "';";

                                        encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                        if (encontrado == false)
                                        {
                                            return cpf;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Impossível prosseguir! Esse Passageiro se encontra restrito!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Impossível prosseguir! Esse Passageiro é menor de idade!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Passageiro Inativo no sistema!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("CPF não encontrado! Insira um CPF válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Insira um CPF válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;



                #endregion


                case "cnpjlogin":

                    #region CnpjlLogin

                    do
                    {
                        retornar = false;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CNPJ para prosseguir: ");

                            string cnpj = Console.ReadLine();

                            //Verifica se existe o cnpj no cadastro
                            String comando = "select CNPJ from CompanhiaAerea where CNPJ = '" + cnpj + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                //Verifica se situação está Ativa
                                comando = "select Situacao from CompanhiaAerea where CNPJ = '" + cnpj + "' and Situacao = 'Ativa';";

                                encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                if (encontrado == true)
                                {
                                    //Verifica se está bloqueada
                                    comando = "select CNPJ from Bloqueados where CNPJ = '" + cnpj + "';";

                                    encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                                    if (encontrado == false)
                                    {
                                        return cnpj;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Impossível prosseguir! Essa Companhia Aérea se encontra Bloqueada por motivos externos!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Companhia Aérea Inativa no sistema!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("CNPJ não encontrado! Insira um CNPJ válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Insira um CNPJ válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;



                #endregion


                case "cpfexiste":

                    #region CpfExiste

                    do
                    {
                        retornar = false;


                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CPF para prosseguir: ");

                            string cpf = Console.ReadLine();

                            String comando = "select CPF from Passageiro where CPF = '" + cpf + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return cpf;
                            }
                            else
                            {
                                Console.WriteLine("CPF não encontrado! Insira um CPF válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("CPF Inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "idvoo":

                    #region IDVoo

                    do
                    {
                        retornar = false;

                        try
                        {
                            Console.Write("Informe o ID do Voo para prosseguir: ");
                            string idvoo = Console.ReadLine().ToUpper();

                            String comando = "select ID_Voo from Voo where ID_Voo = '" + idvoo + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return idvoo;
                            }
                            else
                            {
                                Console.WriteLine("ID do Voo não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ID do Voo inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "idvenda":

                    #region idvenda

                    do
                    {
                        encontrado = false;
                        retornar = false;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o ID da venda: ");

                            string idvenda = Console.ReadLine().ToUpper();

                            String comando = "select ID_Venda from Venda where ID_Venda = '" + idvenda + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return idvenda;
                            }
                            else
                            {
                                Console.WriteLine("Código não encontrado! Insira um ID Venda válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um código ID Venda válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "cnpjexiste":

                    #region cnpjogin
                    do
                    {
                        retornar = false;
                        encontrado = false;
                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CNPJ para prosseguir: ");

                            string cnpj = Console.ReadLine();

                            String comando = "select CNPJ from CompanhiaAerea where CNPJ = '" + cnpj + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return cnpj;
                            }
                            else
                            {
                                Console.WriteLine("CNPJ não encontrado! Insira um CNPJ válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("CPF Inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);
                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;
                #endregion


                case "dataescolhida":

                    #region DataEscolhida - Filtrar Voos por dia

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string dataescolhida = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data Escolhida:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        dataescolhida = dataescolhida + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {

                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            dataescolhida = dataescolhida + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            int date = int.Parse(dataescolhida);
                            string datacombarras = date.ToString("00/00/0000");
                            DateTime datadatetime = DateTime.Parse(datacombarras); //converte só para garantir que é uma data válida
                            string dataretornar = datadatetime.ToString("yyyy-MM-dd"); //converte de volta para o formado desejado de retorno

                            if (encontrado == true)
                            {
                                return dataretornar;
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;


                #endregion


                case "datainicial":

                    #region DataInicial - Filtrar Voos por intervalo

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string dataescolhida = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data Inicial do intervalo:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        dataescolhida = dataescolhida + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {

                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            dataescolhida = dataescolhida + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            int date = int.Parse(dataescolhida);
                            string datacombarras = date.ToString("00/00/0000");
                            DateTime datadatetime = DateTime.Parse(datacombarras); //converte só para garantir que é uma data válida
                            string dataretornar = datadatetime.ToString("yyyy-MM-dd"); //converte de volta para o formado desejado de retorno

                            if (encontrado == true)
                            {
                                return dataretornar;
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;


                #endregion


                case "datafinal":

                    #region DataFinal - Filtrar Voos por intervalo

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string dataescolhida = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data Final do intervalo:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        dataescolhida = dataescolhida + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {

                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            dataescolhida = dataescolhida + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            int date = int.Parse(dataescolhida);
                            string datacombarras = date.ToString("00/00/0000");
                            DateTime datadatetime = DateTime.Parse(datacombarras); //converte só para garantir que é uma data válida
                            string dataretornar = datadatetime.ToString("yyyy-MM-dd"); //converte de volta para o formado desejado de retorno

                            if (encontrado == true)
                            {
                                return dataretornar;
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;


                #endregion


                case "velocidadevoo":

                    #region VelocidadeVoo

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("Insira a Velocidade de Voo em (km/h) da Aeronave: ");

                            float valor = float.Parse(Console.ReadLine());
                            if (valor > 300 && valor <= 4000.00)
                            {
                                return valor.ToString("N2").Replace(',', '.');
                            }
                            else
                            {
                                Console.WriteLine("Não é possível cadastrar Aeronaves dessa velocidade! Mínimo 300 km/h, Máximo 4.000 km/h");
                                retornar = PausaMensagem();
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Erro: Insira um valor nmérico válido! Mínimo 300 km/h, Máximo 4.000 km/h");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;

                #endregion


                case "distanciavoomax":

                    #region DistanciaVooMax

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("Autonomia - Insira a distância máxima de voo em quilômetros (km) da Aeronave: ");

                            float valor = float.Parse(Console.ReadLine());
                            if (valor > 500 && valor <= 25000)
                            {
                                return valor.ToString().Replace(',', '.');
                            }
                            else
                            {
                                Console.WriteLine("Aeroporto só aceita cadastros de Aeronaves com rota mínima 500 km e máxima 20000 km");
                                retornar = PausaMensagem();
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Erro: Insira um valor nmérico válido! Rota mínima 1000 km e máxima 20000 km");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;

                #endregion


                case "idpassagem":

                    #region IDPassagem

                    do
                    {
                        retornar = false;

                        try
                        {
                            Console.Write("Informe o ID da Passagem para prosseguir: ");

                            string idpassagem = Console.ReadLine().ToUpper();

                            String comando = "select ID_Passagem from Passagem where ID_Passagem = '" + idpassagem + "';";

                            encontrado = ConexaoBanco.InjetarSqlExecuteReader(comando);

                            if (encontrado == true)
                            {
                                return idpassagem;
                            }
                            else
                            {
                                Console.WriteLine("ID da Passagem não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ID da Passagem inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                default:
                    return null;
            }
        }

    }
}
