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
    public static async Task<string> HashNames(this Update update, string fileStoragePath, string? fileName = null,  bool isCreateFolder = false, bool isCreateOnlyFolder = false)
    {
      string? result = null;

      string? path = null;
      if (update.Message is not null)
      {
        string key = update.Message.Chat.FirstName ?? "FN";
        key += update.Message.Chat.LastName ?? "LN";
        key += update.Message.Chat.Username ?? "U";
        key += Convert.ToString(update.Message.Chat.Id) ?? "I";

        string hashPathStr;

        using (var sha256 = SHA256.Create())
        {
          byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));

          hashPathStr = BitConverter.ToString(hash).Replace("-", "");
        }

        string _fileName = $@"\{fileName}";


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
        
        result = path;
      }

      await Task.CompletedTask;

      return result!;
    }
  }
}
