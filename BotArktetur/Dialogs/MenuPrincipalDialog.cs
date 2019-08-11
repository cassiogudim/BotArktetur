using BotArktetur.Componente;
using BotArktetur.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BotArktetur.Extensions;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class MenuPrincipalDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var carrossel = context.MakeMessage();
            carrossel.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            carrossel.Attachments = MontarMenu();
            await context.PostAsync(carrossel);

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
                else if (textoDigitado.Contains("cria"))
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

        public static IList<Attachment> MontarMenu()
        {
            // Mostra o item escolhido pelo usuário
            //ActionTypes.ImBack
            // Não mostra o item escolhido pelo usuário
            //ActionTypes.PostBack

            CarrosselMenu card = new CarrosselMenu();
            List<ItemCarrossel> listaItensMenu = new List<ItemCarrossel>();

            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Sobre.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Sobre", value: "Sobre")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Servicos.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Serviços", value: "Serviços")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Clientes.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Clientes", value: "Clientes")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Founders.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Founders", value: "Founders"),
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Parceiros.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Parceiros", value: "Parceiros")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/seuBot.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Crie seu Bot", value: "CriaBot"),
            });

            return card.GerarCarrosselImagem(listaItensMenu);
        }

        public async Task MessageResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}