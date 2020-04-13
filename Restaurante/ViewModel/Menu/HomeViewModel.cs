using Restaurante.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.ViewModel.Menu
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ViewModelBase mainViewModel;

        public HomeViewModel(ViewModelBase mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public HomeModel Model
        {
            get;
            set;
        } = new HomeModel();
    }
}
