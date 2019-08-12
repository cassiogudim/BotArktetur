using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Models
{
    [Serializable]
    public class FormularioFinal
    {
        public FormularioFinal()
        {
            this.Servico = new List<Servico>();
            this.Clientes = new List<Cliente>();
            this.Fundadores = new List<Fundador>();
            this.Parceiros = new List<Parceiro>();
        }

        public string Type { get; set; }
        public List<Servico> Servico { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Fundador> Fundadores { get; set; }
        public List<Parceiro> Parceiros { get; set; }
    }

}