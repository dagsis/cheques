﻿using DsCheques.Common.Models;
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
        public ObservableCollection<Cheque> Cheques
        {
            get { return this.cheques; }
            set { this.SetValue(ref this.cheques, value); }
        }

        public ChequesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadCheques();
        }

        private async void LoadCheques()
        {
            var response = await this.apiService.GetListAsync<Cheque>("http://www.dscheques.ferozo.net", "/api", "/Cheques");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var myCheques = (List<Cheque>)response.Result;
            this.Cheques = new ObservableCollection<Cheque>(myCheques);

        }
    }
}
