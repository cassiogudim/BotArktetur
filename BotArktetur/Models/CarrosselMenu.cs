using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace BotArktetur.Models
{
    public class CarrosselMenu
    {
        /// <summary>
        /// Lista de carrossel com menu e imagem apenas
        /// </summary>
        /// <param name="cardImage"></param>
        /// <param name="cardAction"></param>
        /// <returns></returns>
        public Attachment ListaCarrosselImagem(CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }        

        /// <summary>
        /// Carrosel Completo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        /// <param name="text"></param>
        /// <param name="cardImage"></param>
        /// <param name="cardAction"></param>
        /// <returns></returns>
        public Attachment ListaCarroselCompleto(string title, string subtitle, string text, CardImage cardImage, List<CardAction> cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>()
            };

            foreach (var item in cardAction)
            {
                heroCard.Buttons.Add(item);
            }

            return heroCard.ToAttachment();
        }

        /// <summary>
        /// Lista de botões
        /// </summary>
        /// <param name="cardAction"></param>
        /// <returns></returns>
        public Attachment ListaBotoes(CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}