using DsCheques.Common.Models;
using DsCheques.Common.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DsCheques.UIForm.ViewModels
{
   public class ChequesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<ChequeItemViewModel> cheques;
        private List<Cheque> myCheques;

        private bool isRefreshing;
        public ObservableCollection<ChequeItemViewModel> Cheques
        {
            get { return this.cheques; }
            set { this.SetValue(ref this.cheques, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ChequesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadCheques();
        }

        public ICommand RefreshCommand => new RelayCommand(this.RefreshVerCommand);
        public ICommand SearchCommand => new RelayCommand(this.RefresChequesList);


        public string Filter { get; set; }

        public async void LoadCheques()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Cheque>(
                url,
                "/api",
                "/Cheques/" + MainViewModel.GetInstance().UserEmail,
                "bearer",
                MainViewModel.GetInstance().Token.Token) ;

          

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            this.myCheques = (List<Cheque>)response.Result;
            this.RefresChequesList();
            this.IsRefreshing = false;
        }
        public void AddProductToList(Cheque cheque)
        {
            this.myCheques.Add(cheque);
            this.RefresChequesList();
        }

        public void UpdateProductInList(Cheque cheque)
        {
            var previousCheque = this.myCheques.Where(p => p.Id == cheque.Id).FirstOrDefault();
            if (previousCheque != null)
            {
                this.myCheques.Remove(previousCheque);
            }

            this.myCheques.Add(cheque);
            this.RefresChequesList();
        }

        public void DeleteProductInList(int ChequeId)
        {
            var previousProduct = this.myCheques.Where(p => p.Id == ChequeId).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myCheques.Remove(previousProduct);
            }

            this.RefresChequesList();
        }

        private void RefresChequesList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Cheques = new ObservableCollection<ChequeItemViewModel>(myCheques.Select(p => new ChequeItemViewModel
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    Destino = p.Destino,
                    FechaDeposito = p.FechaDeposito,
                    FechaIngreso = p.FechaIngreso,
                    Firmante = p.Firmante,
                    Importe = p.Importe,
                    Numero = p.Numero,
                    ImageUrl = p.ImageUrl,
                    Cliente = p.Cliente,
                    ImageFullPath = p.ImageFullPath == null ? "noImage" : p.ImageFullPath,
                    User = p.User
                })
           .OrderByDescending(p => p.FechaDeposito)
           .ToList());
            } else
            {
                this.Cheques = new ObservableCollection<ChequeItemViewModel>(myCheques.Select(p => new ChequeItemViewModel
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    Destino = p.Destino,
                    FechaDeposito = p.FechaDeposito,
                    FechaIngreso = p.FechaIngreso,
                    Firmante = p.Firmante,
                    Importe = p.Importe,
                    Numero = p.Numero,
                    ImageUrl = p.ImageUrl,
                    Cliente = p.Cliente,
                    ImageFullPath = p.ImageFullPath == null ? "noImage" : p.ImageFullPath,
                    User = p.User
                })
                 .OrderByDescending(p => p.FechaDeposito)
                 .Where(c=>c.Cliente.Name.ToLower().Contains(Filter.ToLower()) || c.Destino.ToLower().Contains(Filter.ToLower()) || c.Numero.Contains(Filter) || c.Firmante.ToLower().Contains(Filter.ToLower()) || c.Id.ToString() == Filter)
                 .ToList());
            }
           
        }
        private  void RefreshVerCommand()
        {
            LoadCheques();
        }
   }
}
