using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FSWCore
{
    class MenuAdapter : BaseAdapter<string>
    {
        private string[] opcoes;
        private Activity activity;

        public MenuAdapter(Activity activity, string[] opcoes) : base()
        {
            this.activity = activity;
            this.opcoes = opcoes;
        }

        public override int Count
        {
            get { return opcoes.Length; }
        }

        public override string this[int position]
        {
            get { return opcoes[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = activity.LayoutInflater.Inflate(Resource.Layout.ItemMenu, null);
            }

            switch (position)
            {
                case 0:
                    view.FindViewById<ImageView>(Resource.Id.iv_icone).SetImageResource(Resource.Drawable.home);
                    break;
                case 1:
                    view.FindViewById<ImageView>(Resource.Id.iv_icone).SetImageResource(Resource.Drawable.lixeira);
                    break;
                case 2:
                    view.FindViewById<ImageView>(Resource.Id.iv_icone).SetImageResource(Resource.Drawable.configuracoes);
                    break;
                case 3:
                    view.FindViewById<ImageView>(Resource.Id.iv_icone).SetImageResource(Resource.Drawable.sair);
                    break;
            }
            
            view.FindViewById<TextView>(Resource.Id.tv_opcao).Text = opcoes[position];

            return view;
        }
    }
}