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
using Android.Preferences;

namespace FSWCore
{
    public class LoginFragment : Fragment
    {
        private string TAG = "Prova Oral - LoginFragment";
        private Context context;
        private EditText etCPF;
        private EditText etSenha;
        private Button bLogar;
        private TextView tvCadastreSe;

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

            etCPF = view.FindViewById<EditText>(Resource.Id.et_cpf);
            etSenha = view.FindViewById<EditText>(Resource.Id.et_senha);
            bLogar = view.FindViewById<Button>(Resource.Id.b_logar);
            tvCadastreSe = view.FindViewById<TextView>(Resource.Id.tv_cadastrar);

            etCPF.AddTextChangedListener(new MascaraCampo(etCPF, MascaraCampo.CPF));

            bLogar.Click += (object sender, EventArgs eventArgs) =>
            {
                logar();
            };

            tvCadastreSe.Click += (object sender, EventArgs eventArgs) =>
            {
                ((MainActivity)context).trocarFragment(new CadastroFragment(context), "CadastroFragment");
            };

            return view;
        }

        private void logar()
        {
            DatabaseHelper db = new DatabaseHelper();
            Usuario usuario = db.buscarUsuario(MascaraCampo.removerMascara(etCPF.Text));

            if (usuario == null)
            {
                Toast.MakeText(context, "Usuário ou senha inválido.", ToastLength.Long).Show();
            }
            else
            {
                if (Hash.verificarHash(Hash.gerarHash(etSenha.Text), usuario.Senha))
                {
                    MainActivity.usuario = usuario;

                    ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(context);
                    ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
                    editorPreferencias.PutString("CPF", usuario.CPF);
                    editorPreferencias.Apply();

                    ((MainActivity)context).trocarFragment(new HomeFragment(context), "HomeFragment");
                }
            }
        }
    }
}