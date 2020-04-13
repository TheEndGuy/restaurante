using MahApps.Metro.IconPacks;
using Restaurante.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Models
{
    public class HomeModel : ModelBase
    {
        public HomeModel()
        {
            LoadItems();
        }

        private ObservableCollection<ItemHome> items;

        public ObservableCollection<ItemHome> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private void LoadItems()
        {
            items = new ObservableCollection<ItemHome>
            {
                new ItemHome() { Banco = "O Baratão", Nome = "Mesas cadastradas", Quantidade = DatabaseManager.GetMesasCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Mesas disponíveis", Quantidade = DatabaseManager.GetMesasDisponiveisCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Garçons cadastrados", Quantidade = DatabaseManager.GetGarconsCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Pratos cadastrados", Quantidade = DatabaseManager.GetPratosCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Pedidos cadastrados", Quantidade = DatabaseManager.GetPedidosCount() }
            };
        }

        public void ReloadItems()
        {
            Items.Clear();

            Items = new ObservableCollection<ItemHome>
            {
                new ItemHome() { Banco = "O Baratão", Nome = "Mesas cadastradas", Quantidade = DatabaseManager.GetMesasCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Mesas disponíveis", Quantidade = DatabaseManager.GetMesasDisponiveisCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Garçons cadastrados", Quantidade = DatabaseManager.GetGarconsCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Pratos cadastrados", Quantidade = DatabaseManager.GetPratosCount() },
                new ItemHome() { Banco = "O Baratão", Nome = "Pedidos cadastrados", Quantidade = DatabaseManager.GetPedidosCount() }
            };
        }
    }
}
