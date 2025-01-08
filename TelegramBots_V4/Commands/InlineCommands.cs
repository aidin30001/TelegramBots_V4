using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBots_V4.Commands
{
  public class InlineCommands : BotController
  {
    /// <summary>
    /// Testing method 
    /// </summary>
    [Action("/inline", "test inline button")]
    public void TestInline()
    {
      List<InlineKeyboardButton[]> inline = new List<InlineKeyboardButton[]>() // list модно вывести отдельно
      {
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithUrl("Это кнопка с сайтом", "https://habr.com/"), // url не нужно
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("А это просто кнопка", "button1"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("Тут еще одна", "button2"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("И здесь", "button3"),
        },
      };


      var inlineKeyboard = new InlineKeyboardMarkup(inline);
      Context.Bot.Client.SendTextMessageAsync(Context.Update.Message?.Chat.Id!, "test text", replyMarkup: inlineKeyboard);

    }

    [Action("button1")]
    public async void Write()
    {
      await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
      await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, $"Вы нажали на {Context.Update.CallbackQuery.Data}");
    }

    [Action("button2")]
    public async void Write2()
    {
      await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "Тут может быть ваш текст!");
      await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, $"Вы нажали на {Context.Update.CallbackQuery.Data}");
    }

    [Action("button3")]
    public async void Write3()
    {
      await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "А это полноэкранный текст!", showAlert: true);
      await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, $"Вы нажали на {Context.Update.CallbackQuery.Data}");
    }
  }
}
