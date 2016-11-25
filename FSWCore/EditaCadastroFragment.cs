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
    public class EditaCadastroFragment : Fragment
    {
        private string TAG = "Prova Oral - EditaCadastroFragment";
        private Usuario usuarioLogado;
        private Context context;
        private EditText etCPF;
        private EditText etNome;
        private EditText etDataNascimento;
        private EditText etSenha;
        private Button bSalvar;

        public EditaCadastroFragment(Context context)
        {
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.EditaCadastro, container, false);

            usuarioLogado = MainActivity.usuario;

            etCPF = view.FindViewById<EditText>(Resource.Id.et_cpf);
            etNome = view.FindViewById<EditText>(Resource.Id.et_nome);
            etDataNascimento = view.FindViewById<EditText>(Resource.Id.et_data_nascimento);
            etSenha = view.FindViewById<EditText>(Resource.Id.et_senha);
            bSalvar = view.FindViewById<Button>(Resource.Id.b_salvar);

            //etCPF.AddTextChangedListener(new MascaraCampo(etCPF, MascaraCampo.CPF));
            //etDataNascimento.AddTextChangedListener(new MascaraCampo(etDataNascimento, MascaraCampo.DATA));
            bSalvar.Click += (object sender, EventArgs eventArgs) =>
            {
                salvarAlteracoes();
            };

            popularCampos();

            return view;
        }

        private void salvarAlteracoes()
        {
            DatabaseHelper db = new DatabaseHelper();

            if (Hash.gerarHash(etSenha.Text).Equals(db.buscarUsuario(usuarioLogado.CPF).Senha))
            {
                Usuario usuario = new Usuario();
                usuario.CPF = MascaraCampo.removerMascara(etCPF.Text);
                usuario.Nome = etNome.Text;
                //usuario.DataNascimento = MascaraCampo.removerMascara(etDataNascimento.Text);
                usuario.Senha = Hash.gerarHash(etSenha.Text);

                if (db.atualizarUsuario(usuarioLogado.CPF, usuario))
                {
                    ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(context);
                    ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
                    editorPreferencias.PutString("CPF", usuario.CPF);
                    editorPreferencias.Apply();

                    MainActivity.usuario = usuario;

                    Toast.MakeText(context, "Cadastro atualizado com sucesso.", ToastLength.Long).Show();

                    ((MainActivity)context).trocarFragment(new HomeFragment(context), "HomeFragment");
                }
                else
                {
                    Toast.MakeText(context, "Erro ao salvar informações. Tente novamente.", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(context, "Senha inválida.", ToastLength.Long).Show();
            }
        }

        private void popularCampos()
        {
            etCPF.Text = usuarioLogado.CPF;
            etNome.Text = usuarioLogado.Nome;
            etDataNascimento.Text = usuarioLogado.DataNascimento.ToShortDateString();
        }
    }
}