using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOnTheFly
{
    internal class ItemVenda
    {
        public int IDItemVenda { get; set; }
        public int IDVenda { get; set; }
        public decimal Valor_Unitario { get; set; }

        public ItemVenda() { }

        public ItemVenda(int iDItemVenda, int iDVenda, decimal valor_Unitario)
        {
            IDItemVenda = iDItemVenda;
            IDVenda = iDVenda;
            Valor_Unitario = valor_Unitario;
        }

        public static void GerarItemVenda(string idvenda, decimal passagemvalor)
        {
            String comando = "insert into ItemVenda (ID_Venda,Valor_Unitario) values(" + idvenda + ", " + passagemvalor.ToString().Replace(',', '.') + ")";
            ConexaoBanco.InjetarSqlExecuteNonQuery(comando);
        }
    }
}
