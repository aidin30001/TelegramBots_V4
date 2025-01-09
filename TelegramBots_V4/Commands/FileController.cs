using Deployf.Botf;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBots_V4;
using TelegramBots_V4.Commands;
using TelegramBots_V4.Json;

namespace TelegramBots_V2
{
  public class FilesController : BotController
  {
    DeserializeMethod deserialize = JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;

    [Action(null)]
    [Action("/start", "Start bot")]
    public async void Start()
    {
      if (Context.Update.Message is not null)
      {
        var message = Context.Update.Message;
        //потому-что получаемый ответ когда я делая PushL мне не нравится 
        await Context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "👋"); // этот метод для того что для анимации
        PushL($"привет {message.Chat.FirstName ?? message.Chat.LastName ?? message.Chat.Username ?? "полные аноним"}");
      }
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
        }
      }
      if (Context.Update.Message is not null)
        await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "все команды", replyMarkup: FormInlineCommands.AllCommands());
    }

    [Action("/file", "Get file by name")]
    public async void GetFile(string fileName)
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, fileName);
      if (filePath is not null)
      {
        if (System.IO.File.Exists(filePath))
        {
          using (FileStream fs = new FileStream(filePath, FileMode.Open))
          {
            await Context.Bot.Client.SendDocumentAsync(Context.GetSafeChatId()!, new InputOnlineFile(fs, fileName));
          }
        }
        else
        {
          PushL("файл не найден!");
        }
      }
    }
    [Action("/file", "Get file by name")]
    public async void GetFile()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "введите команду имя файла с расширение пример: /file файл.расширение\nвводит через пробел", showAlert: true);
        }
      }
      else
      {
        PushL("введите команду имя файла с расширение пример: /file файл.расширение\nвводит через пробел");
      }
    }

    [Action("/download", "download info")]
    public async void InfoDownload()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "просто загрузите файл", showAlert: true);
        }
      }
      else if (Context.Update.Message is not null)
      {
        if (Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "просто загрузите файл ");
        }
      }
    }

    [On(Handle.Unknown)]
    public async Task Unknown()
    {
      if (Context.Update.Message?.Document is not null)
      {
        var document = Context.Update.Message.Document;
        var file = await Context.Bot.Client.GetFileAsync(document.FileId);
        string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, document.FileName, isCreateFolder: true);

        await using FileStream fs = new FileStream(filePath, FileMode.Create);
        {
          await Context.Bot.Client.DownloadFileAsync(file.FilePath!, fs);
          fs.Close();
        }
        PushL($"файл [{document.FileName}] был скачен");
      }
    }

    [Action("/allfile", "List all file")]
    public async void AllFile()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, isCreateOnlyFolder: true);

      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
          string fileInlinePath = await Context.Update.HashNames(deserialize.FileStoragePath!, isCreateOnlyFolder: true);

          string? fileName = null;

          DirectoryInfo info = new DirectoryInfo(fileInlinePath);
          foreach (FileInfo file in info.GetFiles())
          {
            fileName += file.Name;
            fileName += "\n";
            
          }

          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, fileName!);
        }
      }
      else
      {
        List<string> files = Directory.GetFiles(filePath).Select(fn => System.IO.Path.GetFileName(fn)).ToList();

        string? filesNames = null;

        foreach (var item in files)
        {
          filesNames += item;
          filesNames += "\n";
        }

        if (filesNames is not null)
        {
          PushL(filesNames);
        }
        else
        {
          PushL("файлов нету");
        }
      }
    }

    [Action("/df", "delete file by name")]
    [Action("/deletefile", "delete file by name")]
    public async void DeleteFile(string fileName)
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, fileName);
      if (filePath is not null)
      {
        if (System.IO.File.Exists(filePath))
        {
          System.IO.File.Delete(filePath);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message?.Chat.Id!, $"файл удален [{fileName}]");
        }
        else
        {
          PushL("файл не найден!");
        }
      }
    }
    [Action("/df", "delete file by name")]
    [Action("/deletefile", "delete file by name")]
    public async void DelInfoFile()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "введите команду имя файла с расширение пример: \n/deletefile файл.расширение \nили /df файл.расширение\nвводит через пробел", showAlert: true);
        }
      }
      else 
      {
        PushL("введите команду имя файла с расширение пример: \n/deletefile файл.расширение \nили /df файл.расширение\nвводит через пробел");
      }
    }

    [Action("/rename", "rename file")]
    public async void RenameFile(string oldname, string newname)
    {
      string oldFilePath = await Context.Update.HashNames(deserialize.FileStoragePath!, oldname);
      string newFilePath = await Context.Update.HashNames(deserialize.FileStoragePath!, newname);

      if (System.IO.File.Exists(oldFilePath))
      {
        System.IO.File.Move(oldFilePath, newFilePath);
        PushL($"файл был переименован с [{oldname}] в [{newname}]");
      }
      else
      {
        PushL("файл не найден!");
      }
    }
    [Action("/rename", "rename file")]
    public async void InfoRenameFile()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id, "введите команду имя файла с расширение пример: \n/rename старый_файл.расширение новый_файл.расширение\nвводит через пробел", showAlert: true);
        }
      }
      else
      {
        PushL("введите команду имя файла с расширение пример: \n/rename старый_файл.расширение новый_файл.расширение\nвводит через пробел");
      }
    }
  }
}

