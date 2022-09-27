using System;
using System.Collections.Generic;
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
        public DateTime Tempo_Voo { get; set; }

        //public Destino() { }

        public Destino(string iata, string continente, string pais, string cidade, string nome_aeroporto, DateTime tempo_voo)
        {
            IATA = iata;
            Continente = continente;
            Pais = pais;
            Cidade = cidade;
            Nome_Aeroporto = nome_aeroporto;
            Tempo_Voo = tempo_voo;
        }


    }
}
