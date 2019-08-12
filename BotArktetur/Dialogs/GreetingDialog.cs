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
using Take.SmartContacts.Utils;
using BotArktetur.Models;
using BotArktetur.Componente;
using BotArktetur.Extensions;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;
        public BotBody botBody;
        public FraseologiaBot fraseologia;
        public GreetingDialog()
        {
            botBody = CarrosselMenu.LerArquivoJsonBot();
            fraseologia = CarrosselMenu.LerFraseologia();
        }
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(EscolhaMenuPrincipal);
        }

        private async Task EscolhaMenuPrincipal(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                var conversationId = context.Activity.Conversation.Id;
                var message = await messageActivity;
                var textoDigitado = message.Text.Trim().ToLower();

                if (textoDigitado.Contains("sobre"))
                {
                    context.Call(new SobreDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("servi"))
                {
                    context.Call(new ServicosDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("parce"))
                {
                    context.Call(new ParceirosDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("found") || textoDigitado.Contains("fund"))
                {
                    context.Call(new FundadoresDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("clie"))
                {
                    context.Call(new ClientesDialog(), MessageResumeAfter);
                }
                else if (textoDigitado.Contains("cria") || textoDigitado.Contains("crie") || textoDigitado.Contains("bot"))
                {
                    context.Call(new CrieSeuBotDialog(), MessageResumeAfter);
                }
                else
                {
                    await context.PostAsyncDelay("Nenhum item encontrado. Selecione um dos itens acima");
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

                await context.PostAsyncDelay("Erro método MessageReceivedAsync do diálogo GreetingDialog: " + json);
            }
        }

        private async Task EscolheDicas(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var sinonimosSim = new List<string>
            {
                "sim", "quero", "preciso", "ok", "yes", "aham", "uhum", "pode", "diga", "fala", "fale", "s", "y"
            };
            var message = await result;
            var textoDigitado = message.Text.Trim().ToLower();


            if (sinonimosSim.Any(x => x.Contains(textoDigitado)))
            {
                await context.PostAsyncDelay(fraseologia.FraseologiaSaudacao.Dicas);
            }

            await context.PostAsyncDelay(fraseologia.FraseologiaSaudacao.PossoAjudar);
            //Apresentar o menu principal

            context.Call(new MenuPrincipalDialog(), MessageResumeAfter);
        }

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }






    }
}