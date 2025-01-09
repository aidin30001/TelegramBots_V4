using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;

namespace TelegramBots_V4.Commands
{
  public class MenuCommands : BotController
  {
    [Action("/menu", "menu")]
    public async void Menu()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "меню команды", replyMarkup: FormInlineCommands.AllCommands());
        }
      }
      else
      {
        await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message?.Chat.Id!, "меню команды", replyMarkup: FormInlineCommands.AllCommands());
      }
    }
  }
}
