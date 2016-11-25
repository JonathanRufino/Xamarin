using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using Android.Util;
using Android.Content;
using Android.Preferences;
using static Android.Widget.AdapterView;
using Android.Support.V4.Widget;
using Java.Lang;

namespace FSWCore
{
    [Activity(Label = "FSWCore", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static Usuario usuario;
        private string TAG = "Prova Oral - MainActivity";
        private string[] opcoesMenu = { "Início", "Excluir Conta", "Editar Conta", "Sair" };
        private ListView menu;
        private DrawerLayout layoutMenu;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            DatabaseHelper db = new DatabaseHelper();
            db.criarDB();

            //ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, opcoesMenu);
            MenuAdapter adapter = new MenuAdapter(this, opcoesMenu);

            layoutMenu = FindViewById<DrawerLayout>(Resource.Id.dl_menu);
            menu = FindViewById<ListView>(Resource.Id.menu);
            menu.Adapter = adapter;
            menu.ItemClick += (object sender, ItemClickEventArgs evento) =>
            {
                if (menu.GetItemAtPosition(evento.Position).Equals("Início"))
                {
                    trocarFragment(new HomeFragment(this), "HomeFragment");
                }
                else if (menu.GetItemAtPosition(evento.Position).Equals("Excluir Conta"))
                {
                    layoutMenu.CloseDrawers();

                    AlertDialog.Builder alertaDeletarConta = new AlertDialog.Builder(this);
                    alertaDeletarConta.SetTitle("Atenção");
                    alertaDeletarConta.SetMessage("Tem certeza que deseja excluir esta conta?");
                    alertaDeletarConta.SetPositiveButton("Excluir", (senderAlert, args) =>
                    {
                        if (db.removerUsuario(usuario.CPF))
                        {
                            deslogar();
                        }
                        else
                        {
                            Toast.MakeText(this, "Erro ao excluir conta. Tente novamente", ToastLength.Long).Show();
                        }
                    });
                    alertaDeletarConta.SetNegativeButton("Cancelar", (senderAlert, args) =>
                    {

                    }).Show();
                }
                else if (menu.GetItemAtPosition(evento.Position).Equals("Editar Conta"))
                {
                    trocarFragment(new EditaCadastroFragment(this), "EditaCadastroFragment");
                }
                else if (menu.GetItemAtPosition(evento.Position).Equals("Sair"))
                {
                    deslogar();
                }
            };

            carregarFragment();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            // implementar retorno de fragments
        }

        private void carregarFragment()
        {
            Fragment fragment;
            string fragmentTag;
            usuario = verificarUsuarioLogado();

            if (usuario == null)
            {
                fragment = new LoginFragment(this);
                fragmentTag = "LoginFragment";
            }
            else
            {
                fragment = new HomeFragment(this);
                fragmentTag = "HomeFragment";
            }

            trocarFragment(fragment, fragmentTag);
        }

        private Usuario verificarUsuarioLogado()
        {
            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(this);
            string CPF = preferencias.GetString("CPF", "00000000000");
            DatabaseHelper db = new DatabaseHelper();

            return db.buscarUsuario(CPF);
        }

        public void trocarFragment(Fragment fragment, string fragmentTag)
        {
            layoutMenu.CloseDrawers();
            var gerenciadorFragments = FragmentManager.BeginTransaction();
            gerenciadorFragments.Replace(Resource.Id.main_layout, fragment, fragmentTag);
            gerenciadorFragments.SetTransition(FragmentTransit.EnterMask);
            gerenciadorFragments.Commit();
        }

        private void deslogar()
        {
            removerUsuario("CPF");
            usuario = null;
            layoutMenu.CloseDrawers();
            trocarFragment(new LoginFragment(this), "LoginFragment");
        }

        private void removerUsuario(string chave)
        {
            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
            editorPreferencias.Remove(chave);
            editorPreferencias.Apply();
        }
    }
}
