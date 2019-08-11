using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using BotArktetur.Componente;
using BotArktetur.Extensions;
using BotArktetur.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using AdaptiveCards;
using Chronic;
using Lime.Protocol;
using Newtonsoft.Json;
using Attachment = Microsoft.Bot.Connector.Attachment;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class CrieSeuBotDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public async Task StartAsync(IDialogContext context)
        {
            // carregar formulario adaptive cards
            await CarregaFormularioCrieSeuBot(context);
        }

        private async Task CarregaFormularioCrieSeuBot(IDialogContext context)
        {
            var botBody = CarrosselMenu.LerArquivoJsonBot().Dialogs.Contatos;
            var fraseologia = CarrosselMenu.LerFraseologia().FraseologiaSaudacao;

            //pedir usuário para mandar algo
            await context.PostAsyncDelay(botBody.TextoContato);
            await Task.Delay(800);

            //var reply = context.MakeMessage();
            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //reply.Attachments = MontarMenuServicos(botBody.ListaContato);
            //await context.PostAsync(reply);

            await ShowOptionQuantidade(context);
        }

        public async Task ShowOptionQuantidade(IDialogContext context)
        {

            AdaptiveCard card = new AdaptiveCard()
            //AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock() { Text = "Qual email para contato?" },
                    new AdaptiveTextInput()
                    {
                        Id = "email",
                        Placeholder = "Exemplo: empresa@arktetur.com",
                        Style = AdaptiveTextInputStyle.Email,
                    },
                    new AdaptiveTextBlock() { Text = "Qual telefone para contato?" },
                    new AdaptiveTextInput()
                    {
                        Id = "telefone",
                        Placeholder = "Exemplo: 3100000000",
                        Style = AdaptiveTextInputStyle.Tel,
                    },
                    new AdaptiveTextBlock() { Text = "Qual é o nome da sua empresa?" },
                    new AdaptiveTextInput()
                    {
                        Id = "nomeEmpresa",
                        Placeholder = "Exemplo: Arktetur",
                        Style = AdaptiveTextInputStyle.Text,
                    },
                    new AdaptiveTextBlock() { Text = "Qual é o nome do seu assistente virtual?" },
                    new AdaptiveTextInput()
                    {
                        Id = "nomeBot",
                        Placeholder = "Exemplo: Ark",
                        Style = AdaptiveTextInputStyle.Text,
                    },
                    new AdaptiveTextBlock() { Text = "Me conte sobre sua empresa:" },
                    new AdaptiveTextInput()
                    {
                        Id = "sobre",
                        Placeholder = "Exemplo: Somos uma empresa criada em 2010 com intuito de fornecer serviços especializados.",
                        Style = AdaptiveTextInputStyle.Text,
                    },
                    new AdaptiveTextBlock() { Text = "Quantos serviços vocês fornecem?" },
                    new AdaptiveNumberInput()
                    {
                        Id = "servicos",
                        Min = 1,
                    },
                    new AdaptiveTextBlock() { Text = "Para quantos clientes vocês prestam serviço?" },
                    new AdaptiveNumberInput()
                    {
                        Id = "clientes",
                        Min = 0,
                    },
                    new AdaptiveTextBlock() { Text = "São quantos fundadores?" },
                    new AdaptiveNumberInput()
                    {
                        Id = "fundadores",
                        Min = 1,
                    },
                    new AdaptiveTextBlock() { Text = "Quantos parceiros?" },
                    new AdaptiveNumberInput()
                    {
                        Id = "parceiros",
                        Min = 0,
                    },
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title = "Enviar",
                        DataJson = "{ \"Type\": \"FormularioInicial\" }"
                    }
                }
            };
            Attachment attachment = new Microsoft.Bot.Connector.Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            var reply = context.MakeMessage();
            reply.Attachments.Add(attachment);

            await context.PostAsync(reply);

            context.Wait(RecebeFormularioInicial);
        }

        private async Task RecebeFormularioInicial(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            //if (message.Value != null)

            var value = message.Value.ToString().Replace("{{", "{").Replace("}}", "}");
            var formulario = JsonConvert.DeserializeObject<FormularioInicial>(value);

            var servicos = new List<AdaptiveTextInput>();
            for (var i = 0; i < Convert.ToInt32(formulario.servicos); i++)
            {
                servicos.Add(new AdaptiveTextInput
                {
                    Id = "servico" + i,
                    Style = AdaptiveTextInputStyle.Text,
                });
            }

            var clientes = new List<AdaptiveTextInput>();
            for (var i = 0; i < Convert.ToInt32(formulario.clientes); i++)
            {
                clientes.Add(new AdaptiveTextInput
                {
                    Id = "clientes" + i,
                    Style = AdaptiveTextInputStyle.Text,
                });
            }

            var fundadores = new List<AdaptiveTextInput>();
            for (var i = 0; i < Convert.ToInt32(formulario.fundadores); i++)
            {
                fundadores.Add(new AdaptiveTextInput
                {
                    Id = "fundadores" + i,
                    Style = AdaptiveTextInputStyle.Text,
                });
            }

            var parceiros = new List<AdaptiveTextInput>();
            for (var i = 0; i < Convert.ToInt32(formulario.parceiros); i++)
            {
                parceiros.Add(new AdaptiveTextInput
                {
                    Id = "parceiros" + i,
                    Style = AdaptiveTextInputStyle.Text,
                });
            }


            AdaptiveCard card = new AdaptiveCard()
            //AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock() { Text = "Quais serviços sua empresa presta?" },
                    servicos.Select(x => new AdaptiveTextInput{Id = x.Id, Style = x.Style} ).FirstOrDefault(),

                    new AdaptiveTextBlock() { Text = "Quais são os seus clientes?" },
                    clientes.Select(x => new AdaptiveTextInput{Id = x.Id, Style = x.Style} ).FirstOrDefault(),

                    new AdaptiveTextBlock() { Text = "Qual nome dos fundadores?" },
                    fundadores.Select(x => new AdaptiveTextInput{Id = x.Id, Style = x.Style} ).FirstOrDefault(),

                    new AdaptiveTextBlock() { Text = "Qual nome dos parceiros?" },
                    parceiros.Select(x => new AdaptiveTextInput{Id = x.Id, Style = x.Style} ).FirstOrDefault(),

                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title = "Enviar",
                        DataJson = "{ \"Type\": \"FormularioFinal\" }"
                    }
                }
            };
            Attachment attachment = new Microsoft.Bot.Connector.Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            var reply = context.MakeMessage();
            reply.Attachments.Add(attachment);

            await context.PostAsync(reply);

            context.Wait(RecebeFormularioFinal);

        }

        private async Task RecebeFormularioFinal(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            //if (message.Value != null)

            var value = message.Value.ToString().Replace("{{", "{").Replace("}}", "}");
            var formulario = JsonConvert.DeserializeObject<FormularioInicial>(value);
        }


        private async Task EnvioFormulario(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            //Hotmail
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            //Gmail
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");            
            var mail = new MailMessage();
            mail.From = new MailAddress("de@email.com");
            mail.To.Add("para@email.com");
            mail.Subject = "Formulário crie seu bot";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = "Corpo da mensagem";
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            //Hotmail
            SmtpServer.Credentials = new System.Net.NetworkCredential("cassio@hotmail.com", "senha do hotmail");
            //Gmail
            //SmtpServer.Credentials = new System.Net.NetworkCredential("cassio@gmail.com", "senha do gmail");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

            try
            {
                //Envia o email
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                // erro eo enviar email
                await context.PostAsync("erro eo enviar email");
                await context.PostAsync("mensagem erro:" + e.Message);
            }
        }
    }
}