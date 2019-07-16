using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.UIForm.ViewModels
{
  public class MainViewModel
    {
        private static MainViewModel instance;
        public LoginViewModel Login { get; set; }
        public ChequesViewModel Cheques { get; set; }


        public MainViewModel()
        {
            instance = this;
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null){
                return new MainViewModel();
            }

            return instance;
        }

    }
}
