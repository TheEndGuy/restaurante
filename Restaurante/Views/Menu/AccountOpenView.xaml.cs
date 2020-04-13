using MahApps.Metro.Controls.Dialogs;
using Restaurante.ViewModel.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurante.Views.Menu
{
    /// <summary>
    /// Interação lógica para AccountOpenView.xam
    /// </summary>
    public partial class AccountOpenView : UserControl
    {
        public AccountOpenView()
        {
            InitializeComponent();

            DataContextChanged += (sender, args) =>
            {
                DialogParticipation.SetRegister(this, this.DataContext);
            };
        }
    }
}
