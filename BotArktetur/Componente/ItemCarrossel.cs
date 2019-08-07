﻿using Microsoft.Bot.Connector;

namespace BotArktetur.Componente
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