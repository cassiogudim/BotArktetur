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
using Newtonsoft.Json;
using BotArktetur.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using AdaptiveCards;

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
            var attachment = new Microsoft.Bot.Connector.Attachment()
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

            List<AdaptiveElement> elementosForm = new List<AdaptiveElement>();

            if (parceiros.Count > 0)
            {
                elementosForm.Add(new AdaptiveTextBlock(text: "Quais serviços sua empresa presta?"));
                foreach (var item in servicos)
                {
                    elementosForm.Add(item);
                }
            }

            if (clientes.Count > 0)
            {
                elementosForm.Add(new AdaptiveTextBlock(text: "Quais clientes sua empresa presta?"));
                foreach (var item in clientes)
                {
                    elementosForm.Add(item);
                }
            }

            if (fundadores.Count > 0)
            {
                elementosForm.Add(new AdaptiveTextBlock(text: "Quais fundadores sua empresa presta?"));
                foreach (var item in fundadores)
                {
                    elementosForm.Add(item);
                }
            }

            if (parceiros.Count > 0)
            {
                elementosForm.Add(new AdaptiveTextBlock(text: "Quais parceiros sua empresa presta?"));
                foreach (var item in parceiros)
                {
                    elementosForm.Add(item);
                }
            }

            AdaptiveCard card = new AdaptiveCard()
            //AdaptiveCard card = new AdaptiveCard()
            {
                Body = elementosForm,
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title = "Enviar",
                        DataJson = "{ \"Type\": \"FormularioFinal\" }"
                    }
                }
            };
            Microsoft.Bot.Connector.Attachment attachment = new Microsoft.Bot.Connector.Attachment()
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
            FormularioFinal formularioFinal = new FormularioFinal();
            var message = await result;

            //if (message.Value != null)

            var value = message.Value.ToString().Replace("{{", "{").Replace("}}", "}");
            var formulario = JsonConvert.DeserializeObject<dynamic>(value);

            foreach (var itemform in formulario)
            {
                if (itemform.Name.Contains("servico"))
                    formularioFinal.Servico.Add(new Servico { Nome = itemform.Value });
                else if (itemform.Name.Contains("clientes"))
                    formularioFinal.Clientes.Add(new Cliente { Nome = itemform.Value });
                else if (itemform.Name.Contains("fundador"))
                    formularioFinal.Fundadores.Add(new Fundador { Nome = itemform.Value });
                else if (itemform.Name.Contains("clientes"))
                    formularioFinal.Parceiros.Add(new Parceiro { Nome = itemform.Value });
            }

            //var formFinal = JsonConvert.SerializeObject(formularioFinal);

            context.Wait(EnvioFormulario);
        }

        private async Task EnvioFormulario(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            //Hotmail
            //SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            //Gmail
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("thunderbots05@gmail.com");
            mail.To.Add("wallacedba@gmail.com");
            mail.Subject = "Formulário crie seu bot";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = "Corpo da mensagem";
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            //Hotmail
            //SmtpServer.Credentials = new System.Net.NetworkCredential("cassio@hotmail.com", "senha do hotmail");
            //Gmail
            SmtpServer.Credentials = new System.Net.NetworkCredential("thunderbots05@gmail.com", "panela123");
            SmtpServer.EnableSsl = true;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(ms);
            ms.Position = 0;
            //writer.Write(formularioFinal);

            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);
            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, ct);
            attach.ContentDisposition.FileName = "formulario.json";

            // I guess you know how to send email with an attachment
            // after sending email
            mail.Attachments.Add(attach);
            ms.Close();

            SmtpServer.Send(mail);
            context.Done(true);

            try
            {
                //Envia o email
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                // erro eo enviar email
                //await context.PostAsync("erro eo enviar email");
                //await context.PostAsync("mensagem erro:" + e.Message);
            }
        }
    }
}