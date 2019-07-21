using DsCheques.Common.Models;
using DsCheques.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
   public class ChequesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Cheque> cheques;
        private bool isRefreshing;
        public ObservableCollection<Cheque> Cheques
        {
            get { return this.cheques; }
            set { this.SetValue(ref this.cheques, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ChequesViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = true;
            this.LoadCheques();
        }

        private async void LoadCheques()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Cheque>(
                url,
                "/api",
                "/Cheques",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var cheques = (List<Cheque>)response.Result;
            this.Cheques = new ObservableCollection<Cheque>(cheques);

        }
    }
}
