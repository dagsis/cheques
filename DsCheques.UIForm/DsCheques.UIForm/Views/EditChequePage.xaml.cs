using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DsCheques.UIForm.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DsCheques.UIForm.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditChequePage : ContentPage
    {
        public EditChequePage()
        {
            InitializeComponent();
        }

        public static implicit operator EditChequePage(EditChequeViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}