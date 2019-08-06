using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Web.Http;
using Autofac;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Configuration;
using System.Linq;
using System.Web.Caching;
using BotArktetur.Dialogs;
using System.IO;
using Newtonsoft.Json;
using BotArktetur.Models;

namespace BotArktetur.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        public MessagesController()
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            var build = new ContainerBuilder();
            //build.RegisterType<ConversationLog>().AsImplementedInterfaces().InstancePerDependency();
            build.Update(Conversation.Container);
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    Activity isTypingReply = activity.CreateReply();
                    isTypingReply.Type = ActivityTypes.Typing;
                    await connector.Conversations.ReplyToActivityAsync(isTypingReply);
                    await Conversation.SendAsync(activity, () => new GreetingDialog().DefaultIfException());
                }
                else
                {
                    await this.HandleSystemMessage(activity);
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            if (message.Type == ActivityTypes.Event)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                if (!string.IsNullOrWhiteSpace(message.Value.ToString()))
                {
                    var reply = message.CreateReply($"Olá eu sou o atendente virtual da Sky2.");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    var reply2 = message.CreateReply($"Por favor, me informe os 3 últimos dígitos do CPF para prosseguirmos com o atendimento.");
                    await connector.Conversations.ReplyToActivityAsync(reply2);
                }
                else
                {
                    var reply = message.CreateReply($"Olá eu sou o atendente virtual da Sky2.");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    var reply2 = message.CreateReply($"Por favor, me informe o CPF completo para prosseguirmos com o atendimento.");
                    await connector.Conversations.ReplyToActivityAsync(reply2);
                }
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    // ler arquivo json
                    using (StreamReader sr = new StreamReader(@ConfigurationManager.AppSettings["ArquivoJsonBot"]))
                    {
                        var arquivoJson = sr.ReadToEnd();
                        var dialogos = JsonConvert.DeserializeObject<BotBody>(arquivoJson);
                    }

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var reply = message.CreateReply($"Bem vindo ao bot Arktetur 234.");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    var reply2 = message.CreateReply($"Por favor, me informe os 3 últimos dígitos do CPF para prosseguirmos com o atendimento.");
                    await connector.Conversations.ReplyToActivityAsync(reply2);

                    var item = MontarMenu();
                    var reply3 = message.CreateReply();
                    reply3.Attachments = item;
                    reply3.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await connector.Conversations.ReplyToActivityAsync(reply3);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
        }

        public static IList<Attachment> MontarMenu()
        {
            // Mostra o item escolhido pelo usuário
            //ActionTypes.ImBack
            // Não mostra o item escolhido pelo usuário
            //ActionTypes.PostBack

            CarrosselMenu card = new CarrosselMenu();
            return new List<Attachment>()
            {
                card.ListaCarrosselImagem(
                    new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/alegarPagamento.png"),
                    new CardAction(ActionTypes.PostBack, "Sobre", value: "Sobre")),
                card.ListaCarrosselImagem(
                    new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/2ViadeBoleto.png"),
                    new CardAction(ActionTypes.PostBack, "Funcionários", value: "Funcionários")),
                card.ListaCarrosselImagem(
                    new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/negociar.png"),
                    new CardAction(ActionTypes.PostBack, "Fornecedores", value: "Fornecedores")),
                card.ListaCarrosselImagem(
                    new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
                    new CardAction(ActionTypes.PostBack, "Contato", value: "Contato")),
            };
        }
    }
}