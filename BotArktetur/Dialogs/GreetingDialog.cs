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

        public async Task StartAsync(IDialogContext context)
        {
            // espera ver o que o usuário vai escolher de menu
            context.Wait(EscolhaMenuPrincipal);
        }

        private async Task EscolhaMenuPrincipal(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                conversationId = context.Activity.Conversation.Id;
                var message = await messageActivity;
                var textoDigitado = message.Text.Trim().ToLower();

                if(textoDigitado.Contains("sobre"))
                {
                    context.Call(new SobreDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("servi"))
                {
                    context.Call(new ServicosDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("conta"))
                {
                    context.Call(new ContatosDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("parce"))
                {
                    context.Call(new ParceirosDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("fund"))
                {
                    context.Call(new FundadoresDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("clie"))
                {
                    context.Call(new ClientesDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("cria"))
                {
                    context.Call(new FormularioBotDialog(), MessageResumeAfter);
                }
                else
                {
                    await context.PostAsync("Nenhum item encontrado. Selecione um dos itens acima");
                    context.Wait(EscolhaMenuPrincipal);
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

                await context.PostAsync("Erro método MessageReceivedAsync do diálogo GreetingDialog: " + json);
            }
        }

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}