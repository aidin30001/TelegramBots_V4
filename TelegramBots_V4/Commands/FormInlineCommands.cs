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
          InlineKeyboardButton.WithCallbackData("загрузит файл", "/download"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("поиск файла", "/file"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("помощь", "/h"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("удалить файл", "/deletefile"),
        },
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("переименовывать файл", "/rename"),
        },
      });
    }

    public static InlineKeyboardMarkup Menu()
    {
      return new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
      {
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("меню", "/menu")
        }
      });
    }

    public static InlineKeyboardMarkup GenerateInlineButtons(this List<string> items, bool del = false)
    {
      int i = 0;
      if (items == null || items.Count == 0)
      {
        return null!;
      }

      var buttons = new List<InlineKeyboardButton[]>();

      foreach (var item in items)
      {
        buttons.Add(new[]
        {
          InlineKeyboardButton.WithCallbackData(item, $"fileIndex={i++}_delete={del}_")
        });
      }
      buttons.Add(new[] 
      {
        InlineKeyboardButton.WithCallbackData("меню", "/menu")
      }); 

      return new InlineKeyboardMarkup(buttons);
    }
  }
}
