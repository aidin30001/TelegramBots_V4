using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;

namespace TelegramBots_V4.Commands
{
  public class Start : BotController
  {
    [Action("/start", "Запустить бота")]
    public async void Starting()
    {
      var message = Context.Update.Message;
      if (message is not null)
      {
        //потому-что получаемый ответ когда я делая PushL мне не нравится 
        await Context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "👋"); // этот метод для того что для анимации
        await Context.Bot.Client.SendTextMessageAsync(message.Chat.Id,$"привет {message.Chat.FirstName ?? message.Chat.LastName ?? message.Chat.Username ?? "полные аноним"}");
        await Task.Delay(2000);
        await Context.Bot.Client.DeleteMessageAsync(message.Chat.Id, message.MessageId + 1);
        await Context.Bot.Client.DeleteMessageAsync(message.Chat.Id, message.MessageId + 2);
        if (Context.Update.Message is not null)
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "все команды", replyMarkup: FormInlineCommands.AllCommands());
      }
    }
  }
}
