using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewOnTheFly
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UtilidadeAtualizacao.CarregarDestinos();
            Inicio();
            static void Inicio()
            {
                try
                {
                    UtilidadeAtualizacao.AtualizarSistema();
                    Menu.MenuInicial();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("\nAperte 'ENTER' para continuar...");
                    Console.ReadKey();
                    Inicio();
                }
            }

        }
    }
}
