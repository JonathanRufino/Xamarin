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

namespace FSWCore
{
    [Serializable()]
    public class RegistroDuplicadoException : System.Exception
    {
        public RegistroDuplicadoException() : base() { }
        public RegistroDuplicadoException(string mensagem) : base(mensagem) { }
    }
}