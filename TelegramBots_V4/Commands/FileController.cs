using Deployf.Botf;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using TelegramBots_V4;
using TelegramBots_V4.Json;

namespace TelegramBots_V2
{
  public class FilesController : BotController
  {
    DeserializeMethod deserialize = JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;

    [Action("/start", "Start bot")]
    public void Start()
    {
      if (Context.Update.Message is not null)
      {
        var message = Context.Update.Message;
        //потому-что получаемый ответ когда я делая PushL мне не нравится 
        Context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "👋"); // этот метод для того что для анимации
        PushL($"привет {message.Chat.FirstName ?? message.Chat.LastName ?? message.Chat.Username ?? "полные аноним"}");
      }
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
    public void InfoCorrect()
    {
      PushL("вы ввели не правильно пример:\n/file файл.расширение");
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
      List<string> files = Directory.GetFiles(filePath).Select(fn => System.IO.Path.GetFileName(fn)).ToList();

      string? filesNames = null;
      int i = 1;
      foreach (var item in files)
      {
        filesNames += $"{i} - {item}";
        filesNames += "\n";
        i++;
      }

      if (filesNames is not null)
      {
        PushL(filesNames);
      }
    }
  }
}

