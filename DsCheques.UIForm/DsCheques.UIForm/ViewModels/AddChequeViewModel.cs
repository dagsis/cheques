﻿using DsCheques.Common.Models;
using DsCheques.Common.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
    public class AddChequeViewModel : BaseViewModel
    {

        //private Dictionary<int, string> PickerItems =
        // new Dictionary<int, string>() { { 1, "Afghanistan" }, { 2, "Albania" } };

        //public List<KeyValuePair<int, string>> PickerItemList
        //{
        //    get => PickerItems.ToList();
        //}

        //private KeyValuePair<string, string> _selectedItem;
        //public KeyValuePair<string, string> SelectedItem
        //{
        //    get => _selectedItem;
        //    set => _selectedItem = value;
        //}

        //public List<Cliente> ListClientes
        //{
        //    get;
        //    set;
        //}

        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
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

        public string Destino { get; set; }

        public string Importe { get; set; }

        public string Numero { get; set; }


        public ICommand SaveCommand => new RelayCommand(this.Save);

        private ObservableCollection<Cliente> listClientes;
        public ObservableCollection<Cliente> ListClientes
        {
            get { return this.listClientes; }
            set { this.SetValue(ref this.listClientes, value); }
        }


        private Cliente _selectedCliente;
        public Cliente SeletedCliente
        {
            get { return _selectedCliente;
            }
            set
            {
                this.SetValue(ref this._selectedCliente, value);
                Idcliente = Convert.ToInt32(_selectedCliente.Id);
            }

        }

        private int _idCliente;
        public int Idcliente
        {
            get
            {
                return _idCliente;
            }
            set => this.SetValue(ref this._idCliente, value);
        }

        public AddChequeViewModel()
        {
            this.apiService = new ApiService();

             this.GetClientesAsync();

            this.ImageSource = "noImage";
            this.IsEnabled = true;
            this.Destino = "En Cartera";
        }

        private async void GetClientesAsync(){
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Cliente>(
                url,
                "/api",
                "/Clientes/" + MainViewModel.GetInstance().Login.Email,
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var clientes =  (List<Cliente>)response.Result;
            this.ListClientes = new ObservableCollection<Cliente>(clientes);

        }

        private async void Save()
        {

            //if (string.IsNullOrEmpty(this.Name))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product name.", "Accept");
            //    return;
            //}

            //if (string.IsNullOrEmpty(this.Price))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product price.", "Accept");
            //    return;
            //}

            //var price = decimal.Parse(this.Price);
            //if (price <= 0)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "The price must be a number greather than zero.", "Accept");
            //    return;
            //}

            this.IsRunning = true;
            this.IsEnabled = false;

            //TODO: Add image
            var cheque = new Cheque
            {
                Destino = this.Destino,
                Importe =Convert.ToDecimal( this.Importe),
                ClienteId = SeletedCliente.Id,
                Numero = this.Numero,
                User = new User { UserName = MainViewModel.GetInstance().UserEmail }
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/Cheques",
                cheque,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var newCheque = (Cheque)response.Result;
            MainViewModel.GetInstance().Cheques.Cheques.Add(newCheque);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

    }
}