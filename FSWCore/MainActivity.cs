using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using Android.Util;
using Android.Content;
using Android.Preferences;

namespace FSWCore
{
    [Activity(Label = "FSWCore", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string TAG = "MainActivity";
        public static Usuario usuario;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

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
            usuario = verificarUsuarioLogado();

            if (usuario == null)
            {
                Log.Info(TAG, "Usuário deslogado");
                fragment = new LoginFragment(this);
            }
            else
            {
                Log.Info(TAG, "Usuário já está logado");
                fragment = new HomeFragment(this);
            }

            var gerenciadorFragments = FragmentManager.BeginTransaction();
            gerenciadorFragments.Replace(Resource.Id.main_layout, fragment);
            gerenciadorFragments.Commit();
        }

        private Usuario verificarUsuarioLogado()
        {
            ISharedPreferences preferencias = PreferenceManager.GetDefaultSharedPreferences(this);
            string CPF = preferencias.GetString("CPF", "");
            DatabaseHelper db = new DatabaseHelper();

            return db.buscarUsuario(CPF);
        }
    }
}
