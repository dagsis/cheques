﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.UIForm.ViewModels
{
    using System.Windows.Input;
    using DsCheques.Common.Helpers;
    using DsCheques.Common.Models;
    using DsCheques.Common.Services;
    using DsCheques.UIForm.Views;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;

        public bool IsRemember { get; set; }
        //private string email;
        private readonly ApiService apiService;

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

  
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;

            this.IsRemember = false;
            Email = Settings.UserEmail;
            //Email = "dagsis@dagsis.com.ar";
            //Password = "123456";
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

  
            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrecta.", "Aceptar");
                return;
            }

            var token = (TokenResponse)response.Result;
            var response2 = await this.apiService.GetUserByEmailAsync(
            url,
            "/api",
            "/Account/GetUserByEmail",
            this.Email,
            "bearer",
            token.Token);

            var user = (User)response2.Result;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.User = user;
            mainViewModel.Token = token;
            mainViewModel.UserEmail = this.Email;
            mainViewModel.UserPassword = this.Password;

            mainViewModel.Cheques = new ChequesViewModel();


            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Email;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.User = JsonConvert.SerializeObject(user);


            Application.Current.MainPage = new MasterPage();


        }
    }

}
