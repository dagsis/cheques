﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DsCheques.UIForm.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddChequePage : ContentPage
    {
        public AddChequePage()
        {
            InitializeComponent();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}