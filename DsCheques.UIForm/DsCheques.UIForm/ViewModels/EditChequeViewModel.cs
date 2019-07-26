using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using DsCheques.Common.Helpers;
using DsCheques.Common.Models;
using DsCheques.Common.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
    

    public class EditChequeViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        private MediaFile file;

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }
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
                Cheque.ClienteId = Convert.ToInt32(_selectedCliente.Id);
            }

        }

        private List<Cliente> myClientes;

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

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        public EditChequeViewModel(Cheque cheque)
        {
            this.apiService = new ApiService();

            this.GetClientesAsync();
            this.Cheque = cheque;
            this.ImageSource = cheque.ImageFullPath;
            this.IsEnabled = true;
        }

        private async void GetClientesAsync()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Cliente>(
                url,
                "/api",
                "/Clientes/" + MainViewModel.GetInstance().UserEmail,
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            this.myClientes = (List<Cliente>)response.Result;
            this.ListClientes = new ObservableCollection<Cliente>(myClientes);
            this.SetCliente();
        }

        private void SetCliente()
        {
            foreach (var cliente in this.myClientes)
            { 
                if (cliente.Id == Cheque.ClienteId)
                {
                    this.SeletedCliente = cliente;
                    return;
                }
              
            }
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
                await Application.Current.MainPage.DisplayAlert("Error", "Debe Ingresar un importe.", "Aceptar");
                return;
            }

            //if (this.SeletedCliente == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Debe Seleccionar un Cliente", "Accept");
            //    return;
            //}

            this.IsRunning = true;
            this.IsEnabled = false;

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Cheque.ImageArray = imageArray;
            }

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
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var modifiedProduct = (Cheque)response.Result;
            MainViewModel.GetInstance().Cheques.UpdateProductInList(modifiedProduct);
            await App.Navigator.PopAsync();
        }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "Esta Seguro de eliminar este cheque?", "Si", "No");
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
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            MainViewModel.GetInstance().Cheques.DeleteProductInList(this.Cheque.Id);
            await App.Navigator.PopAsync();
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "De donde Captura la Imagen?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }

            if (source == "From Camera")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
    }

}

