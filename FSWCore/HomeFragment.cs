using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FSWCore
{
    public class HomeFragment : Fragment
    {
        private Context context;

        public HomeFragment(Context context)
        {
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Home, container, false);

            TextView tvNomeUsuario = view.FindViewById<TextView>(Resource.Id.tv_nome_usuario);
            tvNomeUsuario.Text = MainActivity.usuario.Nome;

            return view;
        }
    }
}