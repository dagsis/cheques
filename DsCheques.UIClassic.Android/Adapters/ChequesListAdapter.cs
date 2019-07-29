using Android.App;
using Android.Views;
using Android.Widget;
using DsCheques.UIClassic.Android.Helpers;
using System.Collections.Generic;
using DsCheques.Common.Models;


namespace DsCheques.UIClassic.Android.Adapters
{
    public class ChequesListAdapter : BaseAdapter<Cheque>
    {
        private readonly List<Cheque> items;
        private readonly Activity context;

        public ChequesListAdapter(Activity context, List<Cheque> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Cheque this[int position] => items[position];

        public override int Count => items.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(item.ImageFullPath);

            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Resource.Layout.ChequeRow, null);
            }

            convertView.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.Cliente.Name ;
            convertView.FindViewById<TextView>(Resource.Id.priceTextView).Text = $"{item.Importe:C2}";
            convertView.FindViewById<ImageView>(Resource.Id.chequeImageView).SetImageBitmap(imageBitmap);

            return convertView;
        }
    }

}