using Restaurante.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Models
{
    public class FoodRequestModel : ModelBase
    {
        public FoodRequestModel()
        {
            LoadItems();
        }

        private ObservableCollection<string> listaMesas;

        public ObservableCollection<string> ListaMesas
        {
            get { return listaMesas; }
            set
            {
                listaMesas = value;
                OnPropertyChanged("ListaMesas");
            }
        }

        private ObservableCollection<Garcon> listaGarcons;

        public ObservableCollection<Garcon> ListaGarcons
        {
            get { return listaGarcons; }
            set
            {
                listaGarcons = value;
                OnPropertyChanged("ListaGarcons");
            }
        }

        private ObservableCollection<Prato> listaPratos;

        public ObservableCollection<Prato> ListaPratos
        {
            get { return listaPratos; }
            set
            {
                listaPratos = value;
                OnPropertyChanged("ListaPratos");
            }
        }

        private void LoadItems()
        {
            listaMesas = new ObservableCollection<string>();
            listaMesas = new ObservableCollection<string>(DatabaseManager.ReadMesas());
            listaPratos = new ObservableCollection<Prato>(DatabaseManager.ReadPratos());
            listaGarcons = new ObservableCollection<Garcon>(DatabaseManager.ReadGarcons());
        }

        public void ReloadItems()
        {
            ListaMesas.Clear();
            ListaMesas = new ObservableCollection<string>(DatabaseManager.ReadMesas());
            ListaPratos = new ObservableCollection<Prato>(DatabaseManager.ReadPratos());
            ListaGarcons = new ObservableCollection<Garcon>(DatabaseManager.ReadGarcons());
        }

        public void NovoPedido(int mesaId, int pratoId, int garconId, int qtd)
        {
            DatabaseManager.ProcuedureNovoPedido(mesaId, pratoId, garconId, qtd);
        }
    }
}
