using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using BotArktetur.Helper;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace BotArktetur.Models
{
    public class CarrosselMenu
    {
        public List<Attachment> GerarCarrosselImagem(List<ItemCarrossel> itemCarrossel)
        {
            List<Attachment> ListaMenu = new List<Attachment>();
            for (int i = 0; i < itemCarrossel.Count; i++)
            {
                var heroCard = new HeroCard
                {
                    Images = new List<CardImage>() { itemCarrossel[i].Imagem },
                    Buttons = new List<CardAction>() { itemCarrossel[i].Botao },
                };
                ListaMenu.Add(heroCard.ToAttachment());
            }

            return ListaMenu;
        }

        public List<Attachment> GerarCarrosselCompleto(List<ItemCarrossel> itemCarrossel)
        {
            List<Attachment> ListaMenu = new List<Attachment>();
            for (int i = 0; i < itemCarrossel.Count; i++)
            {
                var heroCard = new ThumbnailCard
                {
                    Title = itemCarrossel[i].Titulo,
                    Subtitle = itemCarrossel[i].SubTitulo,
                    Text = itemCarrossel[i].Texto,
                    Images = new List<CardImage>() { itemCarrossel[i].Imagem },
                    Buttons = new List<CardAction>() { itemCarrossel[i].Botao }
                };
            }

            return ListaMenu;
        }

        public List<Attachment> GerarListaBotoes(List<CardAction> botoesCarrossel)
        {
            List<Attachment> ListaMenu = new List<Attachment>();
            for (int i = 0; i < botoesCarrossel.Count; i++)
            {
                var heroCard = new ThumbnailCard
                {
                    Buttons = new List<CardAction>() { botoesCarrossel[i] },
                };
            }

            return ListaMenu;
        }

        public static Activity SetarTipoCarrossel(Activity activity, string tipoCarrossel)
        {
            activity.AttachmentLayout = tipoCarrossel;
            return activity;
        }

        public static void LerArquivoJsonBot()
        {
            BotBody corpoJson = new BotBody();
            var jsonFile = ConfigurationManager.AppSettings["ArquivoJsonBot"];
            if (jsonFile.StartsWith("http"))
            {
                // ler arquivo json de uma URL
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(ConfigurationManager.AppSettings["ArquivoJsonBot"]);
                    corpoJson = JsonConvert.DeserializeObject<BotBody>(json);
                }
            }
            else
            {
                // ler arquivo json local
                using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["ArquivoJsonBot"]))
                {
                    var arquivoJson = sr.ReadToEnd();
                    corpoJson = JsonConvert.DeserializeObject<BotBody>(arquivoJson);
                }
            }
        }
    }
}