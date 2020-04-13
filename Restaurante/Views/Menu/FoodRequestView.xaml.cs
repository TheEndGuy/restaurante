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
    /// Interação lógica para FoodRequestView.xam
    /// </summary>
    public partial class FoodRequestView : UserControl
    {
        public FoodRequestView()
        {
            InitializeComponent();

            DataContextChanged += (sender, args) =>
            {
                DialogParticipation.SetRegister(this, this.DataContext);
            };

            DataContextChanged += (sender, args) =>
            {
                (this.DataContext as FoodRequestViewModel).Model.ReloadItems();
            };
        }

        #region Validação de inteiros
        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextBoxTextAllowed(e.Text);
        }

        private void textBoxValue_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string Text1 = (string)e.DataObject.GetData(typeof(string));

                if (!TextBoxTextAllowed(Text1))
                    e.CancelCommand();
            }

            else
                e.CancelCommand();
        }

        private bool TextBoxTextAllowed(string Text2)
        {
            return Array.TrueForAll(Text2.ToCharArray(), delegate (char c) { return char.IsDigit(c) || char.IsControl(c); });
        }
        #endregion
    }
}
