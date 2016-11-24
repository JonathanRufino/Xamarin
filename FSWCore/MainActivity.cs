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

namespace FSWCore
{
    [Activity(Label = "FSWCore", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string TAG = "MainActivity";
        public static Usuario usuario;
        string[] opcoesMenu = { "Sair" };
        ListView menu;
        DrawerLayout layoutMenu;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            DatabaseHelper db = new DatabaseHelper();
            db.criarDB();

            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, opcoesMenu);

            layoutMenu = FindViewById<DrawerLayout>(Resource.Id.dl_menu);
            menu = FindViewById<ListView>(Resource.Id.menu);
            menu.Adapter = adapter;
            menu.ItemClick += (object sender, ItemClickEventArgs evento) =>
            {
                if (menu.GetItemAtPosition(evento.Position).Equals("Sair"))
                {
                    deslogar();
                }
            };

            carregarFragment();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            // implementar
        }

        private void carregarFragment()
        {
            Fragment fragment;
            string fragmentTag;
            usuario = verificarUsuarioLogado();

            if (usuario == null)
            {
                Log.Info(TAG, "Usuário deslogado");
                fragment = new LoginFragment(this);
                fragmentTag = "LoginFragment";
            }
            else
            {
                Log.Info(TAG, "Usuário já está logado");
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
            var gerenciadorFragments = FragmentManager.BeginTransaction();
            gerenciadorFragments.Replace(Resource.Id.main_layout, fragment, fragmentTag);
            gerenciadorFragments.SetTransition(FragmentTransit.EnterMask);
            gerenciadorFragments.Commit();
        }

        private void deslogar()
        {
            removerUsuario(usuario.CPF);
            usuario = null;
            layoutMenu.CloseDrawers();
            trocarFragment(new LoginFragment(this), "LoginFragment");
        }

        private void removerUsuario(string CPF)
        {
            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorPreferencias = preferencias.Edit();
            editorPreferencias.Remove(CPF);
            editorPreferencias.Apply();
        }
    }
}
