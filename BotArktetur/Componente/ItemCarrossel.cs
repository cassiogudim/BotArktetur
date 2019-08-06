using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Helper
{
    public class ItemCarrossel
    {
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string Texto { get; set; }
        public CardImage Imagem { get; set; }
        public CardAction Botao { get; set; }
    }
}