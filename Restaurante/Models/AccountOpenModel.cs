using MahApps.Metro.Controls.Dialogs;
using Restaurante.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Restaurante.Models
{
    public class AccountOpenModel : ModelBase
    {
        public AccountOpenModel()
        {
            LoadItems();
        }

        private ObservableCollection<ItemAccount> items;

        public ObservableCollection<ItemAccount> Items
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
            Items = new ObservableCollection<ItemAccount>(DatabaseManager.GetAccountItems());
        }

        public void ReloadItems()
        {
            Items.Clear();
            Items = new ObservableCollection<ItemAccount>(DatabaseManager.GetAccountItems());
        }

        public int AbrirNovaConta(int numeroMesa)
        {
           return DatabaseManager.ProcuredureAbrirConta(numeroMesa);
        }

        public void FecharConta(int numeroMesa)
        {
            DatabaseManager.ProcuredureFecharConta(numeroMesa);
        }
    }
}
