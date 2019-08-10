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
using BotArktetur.Componente;
using BotArktetur.Extensions;
using Newtonsoft.Json;
using BotArktetur.Models;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class FundadoresDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public BotBody botBody;
        public FraseologiaBot fraseologia;

        public FundadoresDialog()
        {
            botBody = CarrosselMenu.LerArquivoJsonBot();
            fraseologia = CarrosselMenu.LerFraseologia();
        }
        public async Task StartAsync(IDialogContext context)
        {
            // carregar os itens da página sobre
            await CarregaTextoFundadores(context);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task CarregaTextoFundadores(IDialogContext context)
        {
            await context.PostAsyncDelay(botBody.Dialogs.Clientes.TextoCliente);
            await Task.Delay(800);

            var carrossel = context.MakeMessage();
            carrossel.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            carrossel.Attachments = MontarMenu();
            await context.PostAsync(carrossel);
            await Task.Delay(800);

            await context.PostAsyncDelay(botBody.Dialogs.Sobre.OpcoesVoltar);
            await Task.Delay(800);

            context.Wait(VoltarMenuSobre);
        }

        public IList<Attachment> MontarMenu()
        {
            // Mostra o item escolhido pelo usuário
            //ActionTypes.ImBack
            // Não mostra o item escolhido pelo usuário
            //ActionTypes.PostBack

            CarrosselMenu card = new CarrosselMenu();
            List<ItemCarrossel> listaItensMenu = new List<ItemCarrossel>();

            foreach (var item in botBody.Dialogs.Fundadores.ListaFundador)
            {
                listaItensMenu.Add(new ItemCarrossel()
                {
                    Titulo = $"{item.Nome}",
                    Texto = item.Descricao,
                    Imagem = new CardImage(url: item.Imagem),
                    Botao = new CardAction(ActionTypes.OpenUrl, "Linkedin", value: item.Site)
                });
            }

            return card.GerarCarrosselCompleto(listaItensMenu);
        }

        private async Task VoltarMenuSobre(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                conversationId = context.Activity.Conversation.Id;
                var message = await messageActivity;
                var textoDigitado = message.Text.Trim();

                if (textoDigitado.Contains("sair"))
                {
                    context.Done(true);
                }
                else
                {
                    context.Call(new MenuPrincipalDialog(), MessageResumeAfter);
                }
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

                await context.PostAsyncDelay("Erro método MessageReceivedAsync do diálogo GreetingDialog: " + json);
            }
        }

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}