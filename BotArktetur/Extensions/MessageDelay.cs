using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace BotArktetur.Extensions
{
    public static class MessageDelay
    {
        public static async Task PostAsyncDelay(this IDialogContext context, string texto)
        {
            await context.PostAsync(texto);
            await Task.Delay(1200);
        }
    }
}