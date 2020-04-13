using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Restaurante.Database;
using Restaurante.ViewModel.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {
        public MainMenuViewModel()
        {
            CreateMenuItems();
        }

        private HamburgerMenuItemCollection menuItems;

        public HamburgerMenuItemCollection MenuItems
        {
            get { return menuItems; }

            set
            {
                if (Equals(value, menuItems))
                    return; 

                menuItems = value;
                OnPropertyChanged();
            }
        }

        public void CreateMenuItems()
        {
            MenuItems = new HamburgerMenuItemCollection
            {
                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconMaterial() {Kind = PackIconMaterialKind.Home},
                    Label = "Home",
                    ToolTip = "Estado atual do sistema.",
                    Tag = new HomeViewModel(this)
                },

                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconMaterial() {Kind = PackIconMaterialKind.Plus},
                    Label = "Abrir uma conta",
                    ToolTip = "Abre uma nova conta na mesa escolhida.",
                    Tag = new AccountOpenViewModel(this, DialogCoordinator.Instance)
                },

                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconMaterial() {Kind = PackIconMaterialKind.FoodForkDrink},
                    Label = "Efetuar um pedido",
                    ToolTip = "Efetua um novo pedido na mesa escolhida.",
                    Tag = new FoodRequestViewModel(this, DialogCoordinator.Instance)
                },
            };
            
        }
    }
}
