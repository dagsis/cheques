using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using DsCheques.Common.Models;
using DsCheques.Common.Services;
using DsCheques.UIClassic.Android.Adapters;
using DsCheques.UIClassic.Android.Helpers;
using Newtonsoft.Json;

namespace DsCheques.UIClassic.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class ProductsActivity : AppCompatActivity
    {
        private TokenResponse token;
        private string email;
        private ApiService apiService;
        private ListView productsListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ChequesPage);

            this.productsListView = FindViewById<ListView>(Resource.Id.productsListView);

            this.email = Intent.Extras.GetString("email");
            var tokenString = Intent.Extras.GetString("token");
            this.token = JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetListAsync<Cheque>(
                "http://www.dscheques.ferozo.net",
                "/api",
                "/Cheques/" + this.email,
                "bearer",
                this.token.Token);

            if (!response.IsSuccess)
            {
                DiaglogService.ShowMessage(this, "Error", response.Message, "Accept");
                return;
            }

            var products = (List<Cheque>)response.Result;
            this.productsListView.Adapter = new ChequesListAdapter(this, products);
            this.productsListView.FastScrollEnabled = true;
        }
    }

}
