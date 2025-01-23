using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        new InlineKeyboardButton[]
        {
          InlineKeyboardButton.WithCallbackData("игры", "/games"),
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
#region create replykeyboard
    // public static ReplyKeyboardMarkup MenuGames()
    // {
    //   return new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
    //   {
    //     new KeyboardButton[]
    //     {
    //       new KeyboardButton("Привет!"),
    //       new KeyboardButton("Пока!"),
    //     },
    //     new KeyboardButton[]
    //     {
    //       new KeyboardButton("Позвони мне!")
    //     },
    //     new KeyboardButton[]
    //     {
    //       new KeyboardButton("Напиши моему соседу!")
    //     }
    //     })
    //     {
    //       // автоматическое изменение размера клавиатуры, если не стоит true,
    //       // тогда клавиатура растягивается чуть ли не до луны,
    //       // проверить можете сами
    //       ResizeKeyboard = true,
    //     };
    // }
#endregion

    // public static ReplyKeyboardMarkup Games()
    // {
    //   return new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
    //   {
    //     new KeyboardButton[]
    //     {
    //       new KeyboardButton("Карты")
    //     },
    //     new KeyboardButton[]
    //     {
    //       new KeyboardButton("Пинг-понг")
    //     },
    //   })
    //   {
    //     ResizeKeyboard = true,
    //   };
    // }

    public static InlineKeyboardMarkup GenerateInlineButtons(this List<string> items, string desc, bool del = false, bool isRegex = false)
    {
      int i = 0;
      if (items == null || items.Count == 0)
      {
        return null!;
      }

      var buttons = new List<InlineKeyboardButton[]>();

      if (!isRegex)
      {
        foreach (var item in items)
        {
          buttons.Add(new[]
          {
            InlineKeyboardButton.WithCallbackData(item, $"{desc}={i++}_delete={del}_")
          });
        }
      }
      else
      {
        foreach (var item in items)
        {
          Match match = Regex.Match(item, "id=<(.*?)>name=<(.*?)>");
          buttons.Add(new[]
          {
            InlineKeyboardButton.WithCallbackData(Convert.ToString(match.Groups[2])!, $"{desc}={i++}_delete={del}_")
          });
        }
      }
      buttons.Add(new[]
      {
        InlineKeyboardButton.WithCallbackData("меню", "/menu")
      });

      return new InlineKeyboardMarkup(buttons);
    }
  }
}
