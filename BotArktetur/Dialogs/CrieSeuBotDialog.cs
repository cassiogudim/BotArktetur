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

            context.Wait(EnvioFormulario);
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