using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Restaurante.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Database
{
    public class DatabaseManager
    {
        private static readonly string CONFIGURATION_FILE = @"\DatabaseConfiguration.json";
        private static string m_databaseName;

        private static MySqlConnection Database
        {
            get;
            set;
        }

        public static int LoadDatabase()
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(directory))
                return 0;

            string fileDirectory = Path.GetFullPath(Path.Combine(directory, @"..\..\..\Database")) + CONFIGURATION_FILE;

            if (!File.Exists(fileDirectory))
                return 0;

            DatabaseConfiguration databaseConfiguration = JsonConvert.DeserializeObject<DatabaseConfiguration>(File.ReadAllText(fileDirectory));

            if (databaseConfiguration.Database == null)
                return 0;

            m_databaseName = databaseConfiguration.Database;

            Database = new MySqlConnection(databaseConfiguration.ToString());
            return 1;
        }

        public static void CloseDatabase()
        {
            Database.Dispose();
        }

        #region Gets
        public static int GetMesasCount()
        {
            if (Database == null)
                return 0;

            return Convert.ToInt32(ExecuteScalar(Scripts.MESA_SCRIPT));
        }

        public static int GetPratosCount()
        {
            if (Database == null)
                return 0;

            return Convert.ToInt32(ExecuteScalar(Scripts.PRATO_SCRIPT));
        }

        public static int GetGarconsCount()
        {
            if (Database == null)
                return 0;

            return Convert.ToInt32(ExecuteScalar(Scripts.GARCON_SCRIPT));
        }

        public static int GetMesasDisponiveisCount()
        {
            if (Database == null)
                return 0;

            return Convert.ToInt32(ExecuteScalar(Scripts.MESA_DISPONIVEL_SCRIPT));
        }

        public static int GetPedidosCount()
        {
            if (Database == null)
                return 0;

            return Convert.ToInt32(ExecuteScalar(Scripts.PEDIDOS_SCRIPT));
        }

        public static List<ItemAccount> GetAccountItems()
        {
            if (Database == null)
                return new List<ItemAccount>();

            return ReadItemsAccount(Scripts.MESA_SCRIPT_2);
        }
        #endregion

        #region Procedures
        public static int ProcuredureAbrirConta(int numeroMesa)
        {
            using (Database)
            using (MySqlCommand command = new MySqlCommand(string.Format("{0}.ABRIR_CONTA", m_databaseName), Database))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@NR_MESA", MySqlDbType.Int32);
                command.Parameters.Add("@NR_CONTA", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                command.Parameters["@NR_MESA"].Value = numeroMesa;

                Database.Open();
                command.ExecuteNonQuery();

                int contaId = Convert.ToInt32(command.Parameters["@NR_CONTA"].Value);

                Database.Close();
                return contaId;
            }
        }

        public static void ProcuredureFecharConta(int numeroMesa)
        {
            using (Database)
            using (MySqlCommand command = new MySqlCommand(string.Format("{0}.FECHAR_CONTA", m_databaseName), Database))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@NR_MESA", MySqlDbType.Int32);
                command.Parameters["@NR_MESA"].Value = numeroMesa;

                Database.Open();
                command.ExecuteNonQuery();
                Database.Close();
            }
        }

        public static void ProcuedureNovoPedido(int mesaId, int pratoId, int garconId, int qtd)
        {
            using (Database)
            using (MySqlCommand command = new MySqlCommand(string.Format("{0}.EFETUAR_PEDIDO", m_databaseName), Database))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@NR_MESA", MySqlDbType.Int32);
                command.Parameters["@NR_MESA"].Value = mesaId;

                command.Parameters.Add("@NR_PRATO", MySqlDbType.Int32);
                command.Parameters["@NR_PRATO"].Value = pratoId;

                command.Parameters.Add("@QTD", MySqlDbType.Int32);
                command.Parameters["@QTD"].Value = qtd;

                command.Parameters.Add("@ID_GARCON", MySqlDbType.Int32);
                command.Parameters["@ID_GARCON"].Value = garconId;

                Database.Open();
                command.ExecuteNonQuery();
                Database.Close();
            }
        }
        #endregion

        #region Read
        public static List<string> ReadMesas()
        {
            var command = Database.CreateCommand();
            List<string> itemMesas = new List<string>();

            try
            {
                if (Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = Scripts.MESA_INDISPONIVEL_SCRIPT;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                        itemMesas.Add(string.Format("Mesa {0}", Convert.ToInt32(reader["NR_MESA"])));

                    return itemMesas;
                }
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }

        public static List<Prato> ReadPratos()
        {
            var command = Database.CreateCommand();
            List<Prato> itemPratos = new List<Prato>();

            try
            {
                if (Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = Scripts.PRATO_SCRIPT_2;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                    {
                        itemPratos.Add(new Prato()
                        {
                           Id = Convert.ToInt32(reader["ID_PRATO"]),
                           Nome = Convert.ToString(reader["NOME"])
                        });
                    }

                    return itemPratos;
                }
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }

        public static List<Garcon> ReadGarcons()
        {
            var command = Database.CreateCommand();
            List<Garcon> itemGarcons = new List<Garcon>();

            try
            {
                if (Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = Scripts.GARCON_SCRIPT_2;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                    {
                        itemGarcons.Add(new Garcon()
                        {
                            Id = Convert.ToInt32(reader["NR_GARCON"]),
                            Nome = Convert.ToString(reader["NOME"])
                        });
                    }

                    return itemGarcons;
                }
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }

        private static List<ItemAccount> ReadItemsAccount(string commandText)
        {
            var command = Database.CreateCommand();
            List<ItemAccount> itemAccountList = new List<ItemAccount>();

            try
            {
                if (Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = commandText;

                // Criando as instâncias de mesas
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                    {
                        var itemRead = new ItemAccount()
                        {
                            Banco = "O Baratão",
                            Mesa = Convert.ToInt32(reader["NR_MESA"])
                        };

                        itemAccountList.Add(itemRead);
                    }
                }

                // Checando sua disponibilidade e conta
                foreach (var item in itemAccountList)
                {
                    item.Disponivel = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM CONTA WHERE CONTA.NR_MESA = " + item.Mesa + " AND CONTA.VALOR_RECEBIDO IS NULL AND CONTA.DATA_CONTA = CURDATE()"))
                                      <= 0 ? true : false;

                    if (item.Disponivel == true)
                        item.Conta = 0;

                    else
                        item.Conta = Convert.ToInt32(ExecuteScalar("SELECT CONTA.NR_CONTA FROM CONTA WHERE CONTA.NR_MESA = " + item.Mesa + " AND CONTA.VALOR_RECEBIDO IS NULL AND CONTA.DATA_CONTA = CURDATE()"));
                }

                return itemAccountList;
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }
        #endregion

        private static void ExecuteQuery(string commandText)
        {
            var command = Database.CreateCommand();

            try
            {
                if (Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }

        private static object ExecuteScalar(string commandText)
        {
            var command = Database.CreateCommand();

            try
            {
                if(Database.State != ConnectionState.Open)
                    Database.Open();

                command.CommandText = commandText;
                return command.ExecuteScalar();
            }

            finally
            {
                if (Database.State == ConnectionState.Open)
                    Database.Close();
            }
        }
    }
}
