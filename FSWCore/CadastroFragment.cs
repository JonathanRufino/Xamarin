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
using Java.Util;
using System.Threading;
using Android.Util;
using Android.Preferences;

namespace FSWCore
{
    public class CadastroFragment : Fragment
    {
        string TAG = "CadastroFragment";
        private Context context;
        private EditText etNome;
        private EditText etDataNascimento;
        private EditText etCPF;
        private EditText etSenha;
        private Button bCadastrar;

        public CadastroFragment(Context context)
        {
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Cadastro, container, false);

            etNome = view.FindViewById<EditText>(Resource.Id.et_nome);
            etDataNascimento = view.FindViewById<EditText>(Resource.Id.et_data_nascimento);
            etCPF = view.FindViewById<EditText>(Resource.Id.et_cpf);
            etSenha = view.FindViewById<EditText>(Resource.Id.et_senha);
            bCadastrar = view.FindViewById<Button>(Resource.Id.b_cadastrar);

            etCPF.AddTextChangedListener(new MascaraCampo(etCPF, MascaraCampo.CPF));
            etDataNascimento.AddTextChangedListener(new MascaraCampo(etDataNascimento, MascaraCampo.DATA));

            bCadastrar.Click += (object sender, EventArgs eventArgs) =>
            {
                Usuario usuario = new Usuario();
                usuario.Nome = etNome.Text;
                usuario.CPF = MascaraCampo.removerMascara(etCPF.Text);
                usuario.Senha = Hash.gerarHash(etSenha.Text);

                ProgressDialog barraProgresso = new ProgressDialog(context);
                barraProgresso.SetMessage("Registrando usuário.");
                barraProgresso.SetProgressStyle(ProgressDialogStyle.Spinner);
                barraProgresso.Show();

                new Thread(new ThreadStart(delegate
                {
                    DatabaseHelper db = new DatabaseHelper();

                    if (db.buscarUsuario(usuario.CPF) == null)
                    {
                        if (db.salvarUsuario(usuario))
                        {
                            MainActivity.usuario = usuario;

                            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(context);
                            ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
                            editorPreferencias.PutString("CPF", usuario.CPF);
                            editorPreferencias.Apply();

                            ((MainActivity)context).trocarFragment(new HomeFragment(context), "HomeFragment");
                        }
                        else
                        {
                            Toast.MakeText(context, "Erro ao cadastrar usuário. Tente novamente.", ToastLength.Long).Show();
                            
                        }
                    }
                    else
                    {
                        Toast.MakeText(context, "Usuário já registrado", ToastLength.Long).Show();
                    }
                    barraProgresso.Dismiss();
                })).Start();
            };

            return view;
        }
    }
}