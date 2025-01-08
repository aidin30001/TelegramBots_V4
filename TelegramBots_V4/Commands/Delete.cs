using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBots_V4.Commands
{
  public class Delete : BotController
  {
    //test method it went well
    // [Action("/delete", "delete message")]
    // public async void DeleteMessage()
    // {
    //   List<InlineKeyboardButton[]> inline = new List<InlineKeyboardButton[]>() // list модно вывести отдельно
    //   {
    //     new InlineKeyboardButton[]
    //     {
    //       InlineKeyboardButton.WithCallbackData("А это просто кнопка", "button1"),
    //     },
    //   };

    //   InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inline);

    //   await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId, inlineKeyboardMarkup);
    // }
  }
}
