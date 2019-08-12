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
using BotArktetur.Extensions;
using Newtonsoft.Json;
using BotArktetur.Models;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class SobreDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public BotBody botBody;
        public FraseologiaBot fraseologia;

        public SobreDialog()
        {
            botBody = CarrosselMenu.LerArquivoJsonBot();
            fraseologia = CarrosselMenu.LerFraseologia();
        }
        public async Task StartAsync(IDialogContext context)
        {
            // carregar os itens da página sobre
            await CarregaTextoSobre(context);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task CarregaTextoSobre(IDialogContext context)
        {
            await context.PostAsyncDelay(botBody.Dialogs.Sobre.Introducao);
            await context.PostAsyncDelay(botBody.Dialogs.Sobre.SobreEmpresa);
            await Task.Delay(800);
            await context.PostAsyncDelay(botBody.Dialogs.Sobre.OpcoesVoltar);

            context.Wait(VoltarMenuSobre);
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