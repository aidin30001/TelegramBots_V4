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
    public async void InfoGetFile()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId, FormInlineCommands.Menu());
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "введите имя файла с расширение пример: /file файл.расширение");
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
        await Context.Bot.Client.DownloadFileAsync(file.FilePath!, fs);
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
          await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId, FormInlineCommands.Menu());
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
  }
}

