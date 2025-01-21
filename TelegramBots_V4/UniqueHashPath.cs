using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBots_V4
{
  public static class UniqueHashPath
  {
    public static async Task<string> HashNames(this Update update, string fileStoragePath, string fileSystem, string? fileName = null,  bool isCreateFolder = false, bool isCreateOnlyFolder = false)
    {
      string? result = null;
      string? key = null;

      if (update.Message is not null)
      {
        key = update.Message.Chat.FirstName ?? "FN";
        key += update.Message.Chat.LastName ?? "LN";
        key += update.Message.Chat.Username ?? "U";
        key += Convert.ToString(update.Message.Chat.Id) ?? "I";
      }
      else if (update.CallbackQuery is not null)
      {
        if (update.CallbackQuery.Message is not null)
        {
          key = update.CallbackQuery.Message.Chat.FirstName ?? "FN";
          key += update.CallbackQuery.Message.Chat.LastName ?? "LN";
          key += update.CallbackQuery.Message.Chat.Username ?? "U";
          key += Convert.ToString(update.CallbackQuery.Message.Chat.Id) ?? "I";
        }
      }

      result = HashPath(key: key!, fileStoragePath: fileStoragePath, fileName: fileName, isCreateFolder: isCreateFolder, isCreateOnlyFolder: isCreateOnlyFolder, fileSystem: fileSystem);
      await Task.CompletedTask;

      return result!;
    }

    private static string HashPath(string fileStoragePath, string key, string? fileName, bool isCreateFolder, bool isCreateOnlyFolder, string fileSystem)
    {
      string hashPathStr;
      string? path = null;

      using (var sha256 = SHA256.Create())
      {
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));

        hashPathStr = BitConverter.ToString(hash).Replace("-", "");
      }

      string _fileName = $@"\{fileName}";

      if (fileSystem is not null)
      {
        fileStoragePath += fileSystem;
      }
      
      if (isCreateFolder || isCreateOnlyFolder)
      {
        path = Path.Combine(fileStoragePath, hashPathStr);
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
        }
        if (isCreateOnlyFolder)
        {
          return path;
        }
      }

      hashPathStr += _fileName;

      if (!isCreateOnlyFolder)
      {
        path = Path.Combine(fileStoragePath, hashPathStr);
      }

      return  path!;
    }
  }
}
