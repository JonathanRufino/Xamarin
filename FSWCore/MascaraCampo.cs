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
using Android.Text;
using Java.Lang;

namespace FSWCore
{
    class MascaraCampo : Java.Lang.Object, ITextWatcher
    {
        public static string CPF = "###.###.###-##";
        public static string DATA = "##/##/####";
        private EditText editText;
        private string _mascara;
        string old;
        bool estaAtualizando;

        public MascaraCampo(EditText editText, string mascara)
        {
            this.editText = editText;
            this._mascara = mascara;
        }

        public static string removerMascara(string texto)
        {
            return texto.Replace(".", "").Replace("-", "").Replace("/", "").Replace("(", "").Replace(")", "");
        }

        public void AfterTextChanged(IEditable s)
        {
            
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            string texto = removerMascara(s.ToString());
            string mascara = "";

            if (estaAtualizando)
            {
                old = texto;
                estaAtualizando = false;
                return;
            }

            int i = 0;

            foreach (var m in _mascara.ToCharArray())
            {
                if (m != '#' && texto.Length > old.Length)
                {
                    mascara += m;
                    continue;
                }

                try
                {
                    mascara += texto[i];
                }
                catch (System.Exception excecao)
                {
                    break;
                }

                i++;
            }

            estaAtualizando = true;
            editText.Text = mascara;
            editText.SetSelection(mascara.Length);
        }
    }
}