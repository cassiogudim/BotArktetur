using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Configuration;
using Newtonsoft.Json;
using BotArktetur.Models;
using BotArktetur.Helper;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class FundadoresDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public async Task StartAsync(IDialogContext context)
        {
            // carregar os itens da página sobre
            await CarregaCarrosselServicos(context);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task CarregaCarrosselServicos(IDialogContext context)
        {
            var botBody = CarrosselMenu.LerArquivoJsonBot().Dialogs.Fundadores;
            var fraseologia = CarrosselMenu.LerFraseologia().FraseologiaSaudacao;

            await context.PostAsync(botBody.TextoFundador);
            await Task.Delay(800);

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = MontarMenuServicos(botBody.ListaFundador);
            await context.PostAsync(reply);

            context.Wait(VoltarMenuFundador);
        }

        public static IList<Attachment> MontarMenuServicos(List<Fundador> listaFundadores)
        {
            // Mostra o item escolhido pelo usuário
            //ActionTypes.ImBack
            // Não mostra o item escolhido pelo usuário
            //ActionTypes.PostBack

            CarrosselMenu card = new CarrosselMenu();
            List<ItemCarrossel> listaItensMenu = new List<ItemCarrossel>();

            foreach(Fundador fundador in listaFundadores)
            {
                listaItensMenu.Add(new ItemCarrossel()
                {
                    Titulo = fundador.Nome,
                    SubTitulo = fundador.Descricao,
                    Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/alegarPagamento.png"),
                    //Botao = new CardAction(ActionTypes.PostBack, "Sobre", value: "Sobre")
                });
            }

            return card.GerarCarrosselCompleto(listaItensMenu);
        }

        private async Task VoltarMenuFundador(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                conversationId = context.Activity.Conversation.Id;
                var message = await messageActivity;
                var textoDigitado = message.Text.Trim();
             
                //recupera informação
                //info = context.PrivateConversationData.GetValueOrDefault(conversationId, new InfoConversation());

                //seta informação
                //context.PrivateConversationData.SetValue(conversationId, info);                           
            }
            catch (Exception ex)
            {
                var error = new Dictionary<string, string>
                {
                    {"Type", ex.GetType().ToString()},
                    {"Message", ex.Message},
                    {"StackTrace", ex.StackTrace}
                };

                string json = JsonConvert.SerializeObject(error, Formatting.Indented);

                await context.PostAsync("Erro método MessageReceivedAsync do diálogo ServicosDialog: " + json);
            }
        }        

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}