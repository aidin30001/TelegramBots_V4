using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deployf.Botf;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBots_V4.Commands
{
  public static class FormInlineCommands
  {
    public static InlineKeyboardMarkup AllCommands()
    {
      return new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
      {
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("поиск файла", "/file"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("все файлы", "/allfile"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("помощь", "/h"),
        },
      });
    }

    public static InlineKeyboardMarkup Menu()
    {
      return new InlineKeyboardMarkup( new List<InlineKeyboardButton[]>()
      {
        new InlineKeyboardButton[] 
        {
          InlineKeyboardButton.WithCallbackData("меню", "/menu")
        }
      });
    }
  }
}
