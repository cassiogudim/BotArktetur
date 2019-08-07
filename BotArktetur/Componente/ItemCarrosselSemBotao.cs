using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace BotArktetur.Componente
{
    public class ItemCarrosselSemBotao
    {
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string Texto { get; set; }
        public CardImage Imagem { get; set; }
    }
}