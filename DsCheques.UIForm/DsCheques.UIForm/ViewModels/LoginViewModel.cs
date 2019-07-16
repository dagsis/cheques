using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.UIForm.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

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

            await Application.Current.MainPage.DisplayAlert("Ok", "Fuck yeah!!!", "Accept");
        }
    }

}
