using DsCheques.Common.Models;
using DsCheques.UIForm.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace DsCheques.UIForm.ViewModels
{
    public  class ChequeItemViewModel : Cheque
    {
        public ICommand SelectChequeCommand => new RelayCommand(this.SelectCheque);

        private async void SelectCheque()
        {
            MainViewModel.GetInstance().EditCheque = new EditChequeViewModel((Cheque)this);
            await App.Navigator.PushAsync(new EditChequePage());
        }

    }
}
