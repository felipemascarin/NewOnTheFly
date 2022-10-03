using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Bloqueado
    {

        public string CNPJ { get; set; }


        public Bloqueado() { }

        public Bloqueado(string cnpj)
        {
            CNPJ = cnpj;
        }

    }
}
