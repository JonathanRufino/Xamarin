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
    public class LoginFragment : Fragment
    {
        private Context context;

        public LoginFragment(Context context)
        {
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Login, container, false);

            EditText etCPF = view.FindViewById<EditText>(Resource.Id.et_cpf);
            EditText etSenha = view.FindViewById<EditText>(Resource.Id.et_senha);
            Button bLogar = view.FindViewById<Button>(Resource.Id.b_logar);
            TextView tvCadastreSe = view.FindViewById<TextView>(Resource.Id.tv_cadastrar);

            bLogar.Click += (object sender, EventArgs eventArgs) =>
            {
                Toast.MakeText(context, "Realizar login", ToastLength.Long).Show();
            };

            tvCadastreSe.Click += (object sender, EventArgs eventArgs) =>
            {
                CadastroFragment cadastroFragment = new CadastroFragment(context);

                var gerenciadorFragments = FragmentManager.BeginTransaction();
                gerenciadorFragments.Replace(Resource.Id.main_layout, cadastroFragment);
                gerenciadorFragments.Commit();
            };

            return view;
        }
    }
}