using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.UIForm.ViewModels
{
    using System.Windows.Input;
    using DsCheques.UIForm.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel()
        {
            Email = "dagsis@dagsis.com.ar";
            Password = "123456";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingrese un email", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingrese una contraseña", "Aceptar");
                return;
            }

            if (!this.Email.Equals("dagsis@dagsis.com.ar") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrecto usuario o contraseña", "Aceptar");
                return;
            }

            MainViewModel.GetInstance().Cheques = new ChequesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ChequesPage());
        }
    }

}
