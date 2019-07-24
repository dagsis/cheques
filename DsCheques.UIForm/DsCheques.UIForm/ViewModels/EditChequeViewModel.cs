using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using DsCheques.Common.Models;
using DsCheques.Common.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
    

    public class EditChequeViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public Cheque Cheque { get; set; }

        private ObservableCollection<Cliente> listClientes;
        public ObservableCollection<Cliente> ListClientes
        {
            get { return this.listClientes; }
            set { this.SetValue(ref this.listClientes, value); }
        }


        private Cliente _selectedCliente;
        public Cliente SeletedCliente
        {
            get
            {
                return _selectedCliente;
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

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand DeleteCommand => new RelayCommand(this.Delete);


        public EditChequeViewModel(Cheque cheque)
        {
            this.apiService = new ApiService();

            this.GetClientesAsync();
            this.Cheque = cheque;
            this.IsEnabled = true;
        }

        private async void GetClientesAsync()
        {
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

            var clientes = (List<Cliente>)response.Result;
            this.ListClientes = new ObservableCollection<Cliente>(clientes);

        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Cheque.Destino))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe Ingresar un destino.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Cheque.Firmante))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe Ingresar un firmante.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Cheque.Numero))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe Ingresar un numero de cheque.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Convert.ToString(this.Cheque.Importe)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe Ingresar un importe.", "Accept");
                return;
            }

            //if (this.SeletedCliente == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Debe Seleccionar un Cliente", "Accept");
            //    return;
            //}

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/Cheques",
                this.Cheque.Id,
                this.Cheque,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var modifiedProduct = (Cheque)response.Result;
            MainViewModel.GetInstance().Cheques.UpdateProductInList(modifiedProduct);
            await App.Navigator.PopAsync();
        }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the product?", "Yes", "No");
            if (!confirm)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.DeleteAsync(
                url,
                "/api",
                "/Cheques",
                this.Cheque.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().Cheques.DeleteProductInList(this.Cheque.Id);
            await App.Navigator.PopAsync();
        }
    }

}

