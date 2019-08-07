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
using BotArktetur.Helper;

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
                    var botBody = CarrosselMenu.LerArquivoJsonBot().Dialogs.Sobre;
                    var fraseologia = CarrosselMenu.LerFraseologia().FraseologiaSaudacao;

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var reply = message.CreateReply(string.Format(fraseologia.Saudacao, 
                        "boa tarde", botBody.NomeBot, botBody.NomeBot));

                    await connector.Conversations.ReplyToActivityAsync(reply);

                    var reply2 = message.CreateReply($"Por favor, me informe os 3 últimos dígitos do CPF para prosseguirmos com o atendimento.");
                    await connector.Conversations.ReplyToActivityAsync(reply2);

                    var reply3 = message.CreateReply();                    
                    reply3.Attachments = MontarMenu();
                    reply3 = CarrosselMenu.SetarTipoCarrossel(reply3, AttachmentLayoutTypes.Carousel);
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
            List<ItemCarrossel> listaItensMenu = new List<ItemCarrossel>();

            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/alegarPagamento.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Sobre", value: "Sobre")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/alegarPagamento.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Serviços", value: "Serviços")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/alegarPagamento.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Clientes", value: "Clientes")
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/negociar.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Fundadores", value: "Fundadores"),
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/2ViadeBoleto.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Parceiros", value: "Parceiros")
            });           
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Contato", value: "Contato"),
            });
            listaItensMenu.Add(new ItemCarrossel()
            {
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
                Botao = new CardAction(ActionTypes.PostBack, "Crie seu Bot", value: "CriaBot"),
            });

            return card.GerarCarrosselImagem(listaItensMenu);
        }
    }
}