using DsCheques.Common.Models;
using DsCheques.UIForm.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DsCheques.UIForm.ViewModels
{
  public class MainViewModel : BaseViewModel

    {
        private static MainViewModel instance;
        private User user;
        public User User
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
        }

        public LoginViewModel Login { get; set; }
        public ChequesViewModel Cheques { get; set; }
        public AddChequeViewModel addCheque { get; set; }
        public ProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }

        public TokenResponse Token { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        //public User User { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public AddChequeViewModel AddCheque { get; set; }
        public EditChequeViewModel EditCheque { get; set; }
        public ICommand AddChequeCommand => new RelayCommand(this.GoAddProduct);
        private async void GoAddProduct()
        {
            this.AddCheque = new AddChequeViewModel();
            await App.Navigator.PushAsync(new AddChequePage());
        }

        public MainViewModel()
        {
            instance = this;
            this.LoadMenus();
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null){
                return new MainViewModel();
            }

            return instance;
        }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_info",
                    PageName = "AboutPage",
                    Title = "Acerca de.."
                },
                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ProfilePage",
                    Title = "Modificar Usuario"
                },

                new Menu
                {
                    Icon = "ic_phonelink_setup",
                    PageName = "SetupPage",
                    Title = "Setup"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Cerrar sesión"
                }
            };

            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }


    }
}
