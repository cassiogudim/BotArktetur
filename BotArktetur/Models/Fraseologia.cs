using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Models
{
    [Serializable]
    public class FraseologiaBot
    {
        public Fraseologiasaudacao FraseologiaSaudacao { get; set; }
    }
    [Serializable]
    public class Fraseologiasaudacao
    {
        public string Saudacao { get; set; }
        public string SaudacaoDica { get; set; }
        public string Dicas { get; set; }
        public string PossoAjudar { get; set; }
    }

}