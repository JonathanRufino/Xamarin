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
        Context context;

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

            EditText etNome = view.FindViewById<EditText>(Resource.Id.et_nome);
            EditText etDataNascimento = view.FindViewById<EditText>(Resource.Id.et_data_nascimento);
            EditText etCPF = view.FindViewById<EditText>(Resource.Id.et_cpf);
            EditText etSenha = view.FindViewById<EditText>(Resource.Id.et_senha);
            Button bCadastrar = view.FindViewById<Button>(Resource.Id.b_cadastrar);

            bCadastrar.Click += (object sender, EventArgs eventArgs) =>
            {
                Usuario usuario = new Usuario();
                usuario.Nome = etNome.Text;
                usuario.CPF = etCPF.Text;
                usuario.Senha = etSenha.Text;

                Console.WriteLine("Salvando Usuário: " + usuario.toString());

                ProgressDialog barraProgresso = new ProgressDialog(context);
                barraProgresso.SetMessage("Registrando usuário.");
                barraProgresso.SetProgressStyle(ProgressDialogStyle.Spinner);
                barraProgresso.Show();

                new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        DatabaseHelper db = new DatabaseHelper();
                        bool sucesso = db.salvarUsuario(usuario);

                        if (!sucesso)
                        {
                            Toast.MakeText(context, "Erro ao cadastrar usuário. Tente novamente.", ToastLength.Long).Show();
                        }
                        else
                        {
                            MainActivity.usuario = usuario;

                            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(context);
                            ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
                            editorPreferencias.PutString("CPF", usuario.CPF);
                            editorPreferencias.Apply();

                            HomeFragment homeFragment = new HomeFragment(context);

                            ((MainActivity)context).trocarFragment(homeFragment, "HomeFragment");
                        }
                    }
                    catch (RegistroDuplicadoException excecao)
                    {
                        Log.Info(TAG, "Exceção: " + excecao.Message);
                        Toast.MakeText(context, "Usuário já registrado", ToastLength.Long).Show();
                    }
                    finally
                    {
                        barraProgresso.Dismiss();
                    }
                })).Start();
            };

            return view;
        }
    }
}