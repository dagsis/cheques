using DsCheques.Common.Models;
using DsCheques.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.UIForm.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private User user;

       

        public User User
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
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

        public ProfileViewModel()
        {
            this.apiService = new ApiService();
            this.User = MainViewModel.GetInstance().User;
            this.IsEnabled = true;
        }

     
    }

}
