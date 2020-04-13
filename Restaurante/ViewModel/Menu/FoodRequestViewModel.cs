using MahApps.Metro.Controls.Dialogs;
using Restaurante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurante.ViewModel.Menu
{
    public class FoodRequestViewModel : ViewModelBase
    {
        private readonly ViewModelBase mainViewModel;

        private IDialogCoordinator dialogCoordinator;

        public FoodRequestViewModel(ViewModelBase mainViewModel, IDialogCoordinator instance)
        {
            this.mainViewModel = mainViewModel;
            dialogCoordinator = instance;
        }

        public FoodRequestModel Model
        {
            get;
            set;
        } = new FoodRequestModel();

        #region RequestFood
        private ICommand _foodRequest;

        public ICommand FoodRequest
        {
            get
            {
                if (null == _foodRequest)
                    _foodRequest = new DelegateCommand<object[]>(OpenNewAccount);

                return _foodRequest;
            }
        }

        private void OpenNewAccount(object[] param)
        {
            var settings = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Accented
            };

            if (param == null)
            {
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Nenhum dado selecionado.", MessageDialogStyle.Affirmative, settings);
                return;
            }

            if (Convert.ToInt32(param[0]) == -1 || Convert.ToInt32(param[1]) == -1 || Convert.ToInt32(param[2]) == -1 || param[3].Equals(""))
            {
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Alguns dados estão faltando para realização de um pedido.", MessageDialogStyle.Affirmative, settings);
                return;
            }

            Execute(param, settings);
        }

        private void Execute(object[] par, MetroDialogSettings settings)
        {
            // Parâmetro 0 -> Id da mesa / Parâmetro 1 -> Id do prato / Parâmetro 2 -> Id do garçon / Parâmetro 3 -> Quantidade

            int mesaId = int.Parse(new string(Model.ListaMesas[(int)par[0]].Where(char.IsDigit).ToArray()));
            int pratoId = Model.ListaPratos[(int)par[1]].Id;
            int garconId = Model.ListaGarcons[(int)par[2]].Id;
            int qtd = Convert.ToInt32(par[3]);

            Model.NovoPedido(mesaId, pratoId, garconId, qtd);
            dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "O pedido referente ao prato '" + Model.ListaPratos[(int)par[1]].Nome + "' para a mesa " + mesaId + " foi realizado com sucesso.", MessageDialogStyle.Affirmative, settings);
        }
        #endregion
    }
}
