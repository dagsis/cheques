using DsCheques.Common.Models;
using DsCheques.Common.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
  public  class AddChequeViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public string Name { get; set; }

        public string Price { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public AddChequeViewModel()
        {
            this.apiService = new ApiService();
            this.ImageSource = "noImage";
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product price.", "Accept");
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The price must be a number greather than zero.", "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            //TODO: Add image
            var cheque = new Cheque
            {
                
                User = new User { UserName = MainViewModel.GetInstance().UserEmail }
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/Cheques",
                cheque,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var newCheque = (Cheque)response.Result;
            MainViewModel.GetInstance().Cheques.Cheques.Add(newCheque);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

    }
}
