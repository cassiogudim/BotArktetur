using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Models
{
    [Serializable]

    public class FormularioInicial
    {
        public string Type { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string nomeEmpresa { get; set; }
        public string nomeBot { get; set; }
        public string sobre { get; set; }
        public string servicos { get; set; }
        public string clientes { get; set; }
        public string fundadores { get; set; }
        public string parceiros { get; set; }
    }


}