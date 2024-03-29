﻿using System;
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
using BotArktetur.Componente;
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

            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    var hour = DateTime.Now.Hour;
                    string periodo;

                    if (hour >= 5 && hour <= 12)
                    {
                        periodo = "Bom dia";
                    }
                    else if (hour > 12 && hour <= 17)
                    {
                        periodo = "Boa tarde";
                    }
                    else
                    {
                        periodo = "Boa noite";
                    }


                    var botBody = CarrosselMenu.LerArquivoJsonBot().Dialogs.Sobre;
                    var fraseologia = CarrosselMenu.LerFraseologia().FraseologiaSaudacao;

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var reply = message.CreateReply(string.Format(fraseologia.Saudacao,
                        periodo, $"**{botBody.NomeBot}**", botBody.NomeEmpresa));
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    //var replyDica = message.CreateReply(fraseologia.SaudacaoDica);
                    //await connector.Conversations.ReplyToActivityAsync(replyDica);

                    var reply3 = message.CreateReply();
                    reply3.Attachments = MenuPrincipalDialog.MontarMenu();
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


    }
}