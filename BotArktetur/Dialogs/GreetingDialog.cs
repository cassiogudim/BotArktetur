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
            context.Wait(EscolheDicas);
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