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

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class SobreDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public async Task StartAsync(IDialogContext context)
        {
            // carregar os itens da página sobre
            await CarregaTextoSobre(context);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task CarregaTextoSobre(IDialogContext context)
        {
            var botBody = CarrosselMenu.LerArquivoJsonBot().Dialogs.Sobre;
            var fraseologia = CarrosselMenu.LerFraseologia().FraseologiaSaudacao;

            await context.PostAsync(botBody.SobreEmpresa);
            await Task.Delay(800);
            await context.PostAsync(botBody.OpcoesVoltar);
            context.Wait(VoltarMenuSobre);
        }

        private async Task VoltarMenuSobre(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
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

                await context.PostAsync("Erro método MessageReceivedAsync do diálogo GreetingDialog: " + json);
            }
        }        

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}