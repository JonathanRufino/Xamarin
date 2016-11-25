using Android.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSWCore
{
    public class DatabaseHelper
    {
        string TAG = "DatabaseHelper";
        string nomeDB = "database.db3";
        string caminhoDB = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        SQLiteConnection db;

        public DatabaseHelper()
        {
            db = new SQLiteConnection(Path.Combine(caminhoDB, nomeDB));
        }

        public bool criarDB()
        {
            try
            {
                db.CreateTable<Usuario>();

                return true;
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, excecao.Message);
                return false;
            }
        }

        public bool salvarUsuario(Usuario usuario)
        {
            try
            {
                db.Insert(usuario);

                return true;
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, "Exceção: " + excecao.Message);

                if (excecao.Message.Equals("Constraint"))
                {
                    throw new RegistroDuplicadoException("Chave primária já registrada");
                }

                return false;
            }
        }

        public bool atualizarUsuario(string CPF, Usuario usuario)
        {
            try
            {
                db.Query<Usuario>("UPDATE Usuario SET CPF=?, Nome=?, Senha=?, DataNascimento=? WHERE CPF=?", usuario.CPF, usuario.Nome, usuario.Senha, usuario.DataNascimento, CPF);

                return true;
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, "Exceção: " + excecao.Message);
                return false;
            }
        }

        public Usuario buscarUsuario(string CPF)
        {
            try
            {
                return db.Find<Usuario>(CPF);
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, "Exceção: " + excecao.Message);
                return null;
            }
        }

        public bool removerUsuario(string CPF)
        {
            try
            {
                db.Delete<Usuario>(CPF);

                return true;
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, "Exceção: " + excecao.Message);
                return false;
            }
        }

        public List<Usuario> buscarTodosUsuarios()
        {
            try
            {
                return db.Table<Usuario>().ToList();
            }
            catch (SQLiteException excecao)
            {
                Log.Info(TAG, "Exceção: " + excecao.Message);
                return null;
            }
        }
    }
}