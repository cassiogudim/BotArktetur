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

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;
        public string nome;
        private string cpfCliente;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                string cpfQueryString = string.Empty;
                string cpfCliente = string.Empty;
                var enviroment = ConfigurationManager.AppSettings["enviroment"];

                //teste digitação 11 dígitos CPF 
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

        public async Task AfterPlanoSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var selection = await result;
            var separador = selection.Text.Split('-');
        }

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}