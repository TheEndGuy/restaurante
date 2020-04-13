using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Models
{
    public class ItemAccount
    {
        public ItemAccount()
        {
        }

        public string Banco
        {
            get;
            set;
        }

        public int Mesa
        {
            get;
            set;
        }

        public int Conta
        {
            get;
            set;
        }

        public bool Disponivel
        {
            get;
            set;
        }
    }
}
