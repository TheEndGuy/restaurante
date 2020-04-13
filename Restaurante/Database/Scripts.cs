using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Database
{
    public static class Scripts
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly string MESA_SCRIPT = "SELECT COUNT(*) FROM MESA";

        /// <summary>
        ///
        /// </summary>
        public static readonly string MESA_SCRIPT_2 = "SELECT * FROM MESA";

        /// <summary>
        ///
        /// </summary>
        public static readonly string MESA_DISPONIVEL_SCRIPT = "SELECT COUNT(*) FROM MESA WHERE NOT EXISTS " +
            "                                    (SELECT * FROM CONTA WHERE CONTA.NR_MESA = MESA.NR_MESA AND " +
            "                                                               CONTA.VALOR_RECEBIDO IS NULL AND " +
            "                                                               CONTA.DATA_CONTA = CURDATE());";

        /// <summary>
        ///
        /// </summary>
        public static readonly string MESA_INDISPONIVEL_SCRIPT = "SELECT * FROM MESA WHERE EXISTS " +
            "                                    (SELECT * FROM CONTA WHERE CONTA.NR_MESA = MESA.NR_MESA AND " +
            "                                                               CONTA.VALOR_RECEBIDO IS NULL AND " +
            "                                                               CONTA.DATA_CONTA = CURDATE());";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string GARCON_SCRIPT = "SELECT COUNT(*) FROM GARCON";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string GARCON_SCRIPT_2 = "SELECT * FROM GARCON";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PRATO_SCRIPT = "SELECT COUNT(*) FROM PRATO";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PRATO_SCRIPT_2 = "SELECT * FROM PRATO";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PEDIDOS_SCRIPT = "SELECT COUNT(*) FROM PEDIDOS";
    }
}
