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
    public class AccountOpenViewModel : ViewModelBase
    {
        private readonly ViewModelBase mainViewModel;

        private IDialogCoordinator dialogCoordinator;
        
        public AccountOpenViewModel(ViewModelBase mainViewModel, IDialogCoordinator instance)
        {
            this.mainViewModel = mainViewModel;
            dialogCoordinator = instance;
        }

        public AccountOpenModel Model
        {
            get;
            set;
        } = new AccountOpenModel();

        #region Open new account
        private ICommand _openAccount;

        public ICommand OpenAccount
        {
            get
            {
                if (null == _openAccount)
                    _openAccount = new DelegateCommand<object>(OpenNewAccount);

                return _openAccount;
            }
        }

        public void OpenNewAccount(object param)
        {
            var settings = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Accented
            };

            var result = ((System.Collections.IEnumerable)param).Cast<object>().ToList();

            if (result.Count <= 0)
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Nenhuma mesa foi selecionada.", MessageDialogStyle.Affirmative, settings);

            else if ((result.First() as ItemAccount).Disponivel == false)
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Mesa indisponível.", MessageDialogStyle.Affirmative, settings);

            else
            {
                var numeroMesa = (result.First() as ItemAccount).Mesa;
                var contaAberta = Model.AbrirNovaConta(numeroMesa);

                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "A conta " + contaAberta + " foi reservada para a mesa " + numeroMesa + ".", MessageDialogStyle.Affirmative, settings);
                Model.ReloadItems();
            }
        }
        #endregion

        #region Close account
        private ICommand _closeAccount;

        public ICommand CloseAccount
        {
            get
            {
                if (null == _closeAccount)
                    _closeAccount = new DelegateCommand<object>(Close);

                return _closeAccount;
            }
        }

        public void Close(object param)
        {
            var settings = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Accented
            };

            var result = ((System.Collections.IEnumerable)param).Cast<object>().ToList();

            if (result.Count <= 0)
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Nenhuma mesa foi selecionada.", MessageDialogStyle.Affirmative, settings);

            else if ((result.First() as ItemAccount).Disponivel == true)
                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "Impossível fechar a conta de uma mesa disponível.", MessageDialogStyle.Affirmative, settings);

            else
            {
                var itemAcc = (result.First() as ItemAccount);
                Model.FecharConta(itemAcc.Mesa);

                dialogCoordinator.ShowModalMessageExternal(this, "Aviso", "A conta " + itemAcc.Conta + " foi fechada.", MessageDialogStyle.Affirmative, settings);
                Model.ReloadItems();
            }
        }
        #endregion
    }
}
