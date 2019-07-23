using System;
using System.Collections.Generic;
using System.Text;
using DsCheques.Common.Models;

namespace DsCheques.UIForm.ViewModels
{
    public class EditChequeViewModel : BaseViewModel
    {
        public Cheque Cheque;

        public EditChequeViewModel(Cheque cheque)
        {
            this.Cheque = cheque;
        }
    }
}
