using MahApps.Metro.Controls;
using Restaurante.Database;
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
using System.Windows.Shapes;

namespace Restaurante.Views
{
    /// <summary>
    /// Lógica interna para TelaPrincipal.xaml
    /// </summary>
    public partial class TelaPrincipal : MetroWindow
    {
        public TelaPrincipal()
        { 
            if (DatabaseManager.LoadDatabase() == 0)
                MessageBox.Show("Ocorreu um erro ao carregar o banco de dados.");

            InitializeComponent();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            DatabaseManager.CloseDatabase();
        }
    }
}
