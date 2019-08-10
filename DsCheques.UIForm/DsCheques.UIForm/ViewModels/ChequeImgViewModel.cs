using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
  public class ChequeImgViewModel : BaseViewModel
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }



    }
}
