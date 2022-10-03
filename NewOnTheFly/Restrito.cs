using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class Restrito
    {

        public string CPF { get; set; }
        
        public Restrito() { }

        public Restrito(string cpf)
        {
            CPF = cpf;
        }
    }
}
