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
using BotArktetur.Componente;
using Newtonsoft.Json;
using BotArktetur.Models;

namespace BotArktetur.Dialogs
{
    [Serializable]
    public class ServicosDialog : IDialog<IMessageActivity>
    {
        protected string conversationId;

        public BotBody botBody;
        public FraseologiaBot fraseologia;

        public ServicosDialog()
        {
            botBody = CarrosselMenu.LerArquivoJsonBot();
            fraseologia = CarrosselMenu.LerFraseologia();
        }
        public async Task StartAsync(IDialogContext context)
        {
            // carregar os itens da página sobre
            await CarregaTextoServicos(context);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task CarregaTextoServicos(IDialogContext context)
        {
            await context.PostAsync(botBody.Dialogs.Servicos.TextoServico);
            await Task.Delay(800);

            var carrossel = context.MakeMessage();
            carrossel.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            carrossel.Attachments = MontarMenu();
            await context.PostAsync(carrossel);
            await Task.Delay(800);

            await context.PostAsync(botBody.Dialogs.Sobre.OpcoesVoltar);
            await Task.Delay(800);

            context.Wait(VoltarMenuSobre);
        }

        public static IList<Attachment> MontarMenu()
        {
            // Mostra o item escolhido pelo usuário
            //ActionTypes.ImBack
            // Não mostra o item escolhido pelo usuário
            //ActionTypes.PostBack

            CarrosselMenu card = new CarrosselMenu();
            List<ItemCarrosselSemBotao> listaItensMenu = new List<ItemCarrosselSemBotao>();

            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "ARQUITETURA DE SOFTWARE",
                SubTitulo = "Desenvolvemos arquiteturas de software de alto nível para o seu negócio.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Sobre.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "CLOUD COMPUTING",
                SubTitulo = "Desenvolvemos sua arquitetura e levamos seu negócio para a nuvem.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Servicos.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "INTELIGENT BOT SERVICES",
                SubTitulo = "Criamos, conectamos, implantamos e gerenciamos bots inteligentes para interagir naturalmente com seus clientes.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Clientes.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "ARQUITETURA DE BANCO DE DADOS",
                SubTitulo = "Garanta mais segurança e consistência dos seus dados com um banco de dados bem estruturado.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Founders.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "DESENVOLVIMENTO BACK-END, FRONT-END E FULLSTACK",
                SubTitulo = "Possuímos uma equipe especializada em desenvolvimento de software\r\nque trabalha através de métodos ágeis. Entregamos seu software com mais agilidade, padrão e por um menor custo.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/cassiogudim/BotArktetur/master/BotArktetur/Images/Carrossel/Parceiros.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "CONSULTORIA EM NIST 800-53 e 171",
                SubTitulo = "",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "CONSULTORIA EM LGPD",
                SubTitulo = "A Falta de controle dos dados pode resultar em sanções duras que, atribuídas ao não respeito da LGPD e da GDPR, podem levar empresas a proibição total ou parcial de suas atividades baseadas em dados, ocasionando também multas e publicações oficiais das empresas que forem constatadas como não confiáveis para a atuação.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "CONSULTORIA EM PCI-DSS",
                SubTitulo = "O PCI DSS está composto por um conjunto de requerimentos e procedimentos de segurança cujo objetivo é proteger as informações pessoais dos titulares de cartão e, portanto, reduzir o risco de roubo de dados de cartão ou fraude.\r\n\r\nO PCI DSS é aplicado a qualquer negócio que processe ou transmita dados dos portadores de cartões.\r\n\r\nAjudamos a preparar a sua aplicação e infraestrutura para a certificação.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
            });
            listaItensMenu.Add(new ItemCarrosselSemBotao()
            {
                Texto = "CONSULTORIA EM ISO 27001",
                SubTitulo = "ABNT ISO/IEC 27001 auxilia as empresas na implantação de um sistema de gestão da segurança da informação. Por ser orientada ao negócio, não apenas à tecnologia, esta norma provê orientações para criação dos processos e qualificação das pessoas que utilizam as tecnologias responsáveis por permitir às empresas alcançarem seus objetivos socioeconômicos.",
                Imagem = new CardImage(url: "https://raw.githubusercontent.com/walldba/JL-Project/master/SkyCobranca_Imagens/sairSky.png"),
            });

            return card.GerarCarrosselCompletoSemBotao(listaItensMenu);
        }

        private async Task VoltarMenuSobre(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            try
            {
                conversationId = context.Activity.Conversation.Id;
                var message = await messageActivity;
                var textoDigitado = message.Text.Trim();

                if (textoDigitado.Contains("sair"))
                {
                    context.Done(true);
                }
                else
                {
                    context.Call(new MenuPrincipalDialog(), MessageResumeAfter);
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