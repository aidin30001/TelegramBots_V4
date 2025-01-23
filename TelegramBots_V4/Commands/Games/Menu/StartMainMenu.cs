using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBots_V4.Commands.Games.Menu
{
  public class StartMainMenu : BotController
  {
//     [Action("Карты", "игра про карты")]
//     [Action("Пинг-понг", "игра про пинг-понг")]
//     [Action("/games", "игры")]
//     public async Task MenuGame()
//     {
//       if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
//       {
//           await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
//           await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
//           await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "", replyMarkup: FormInlineCommands.Games());
//       }
//       else if (Context.Update.Message is not null && Context.Update.Message.Chat is not null && Context.Update.Message.Text is not null)
//       {
//         string _games = Context.Update.Message.Text
//       }
//     }
// #region delete replykeyboard
//     // [Action("/delkeyboard")]
//     // public async Task DelKeyboard()
//     // {
//     //   if (Context.Update.Message is not null)
//     //   {
//     //     if (Context.Update.Message.Chat is not null)
//     //     {
//     //       await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "del", replyMarkup: new ReplyKeyboardRemove() { Selective = true });
//     //     }
//     //   }
//     // }
// #endregion

  }
}
