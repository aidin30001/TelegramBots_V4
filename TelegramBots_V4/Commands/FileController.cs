using System.Linq;
using System.Text.RegularExpressions;
using Deployf.Botf;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
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
    public void GetFile(string fileName)
    {
      GetFileFunction(fileName);
    }
    [Action("/file", "Get file by name")]
    public async void GetFile()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, isCreateOnlyFolder: true);
      List<string> files = Directory.GetFiles(filePath).Select(fn => System.IO.Path.GetFileName(fn)).ToList();
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          if (files.GenerateInlineButtons != null)
          {
            await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
            await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
            await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id!, "список", replyMarkup: files.GenerateInlineButtons());
          }
          else
          {
            await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "нету файлов");
          }
        }
      }
      else
      {
        if (Context.Update.Message is not null)
        {
          if (Context.Update.Message.Chat is not null)
          {
            if (files.GenerateInlineButtons != null)
            {
              await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id!, "список", replyMarkup: files.GenerateInlineButtons());
            }
            else
            {
              await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "нету файлов");
            }
          }
        }
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

        //переделать 
        PushL($"файл [{document.FileName}] был скачен");
      }
    }

    [Action("/df", "delete file by name")]
    [Action("/deletefile", "delete file by name")]
    public void DeleteFile(string fileName)
    {
      FileDelFunction(Context.Update, fileName);
    }
    [Action("/df", "delete file by name")]
    [Action("/deletefile", "delete file by name")]
    public async void DeleteFile()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, isCreateOnlyFolder: true);
      List<string> files = Directory.GetFiles(filePath).Select(fn => System.IO.Path.GetFileName(fn)).ToList();

      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "список", replyMarkup: files.GenerateInlineButtons(del: true));
        }
      }
      else if (Context.Update.Message is not null)
      {
        if (Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "список", replyMarkup: files.GenerateInlineButtons(del: true));
        }
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

    [Action("/", "testing")]
    public void Test(string test)
    {
      PushL($"test {test}");
    }

    [Action("fileIndex=0_delete=False_")]
    [Action("fileIndex=1_delete=False_")]
    [Action("fileIndex=2_delete=False_")]
    [Action("fileIndex=3_delete=False_")]
    [Action("fileIndex=4_delete=False_")]
    [Action("fileIndex=5_delete=False_")]
    [Action("fileIndex=6_delete=False_")]
    [Action("fileIndex=7_delete=False_")]
    [Action("fileIndex=8_delete=False_")]
    [Action("fileIndex=9_delete=False_")]
    [Action("fileIndex=10_delete=False_")]
    [Action("fileIndex=11_delete=False_")]
    [Action("fileIndex=12_delete=False_")]
    [Action("fileIndex=13_delete=False_")]
    [Action("fileIndex=14_delete=False_")]
    [Action("fileIndex=15_delete=False_")]
    [Action("fileIndex=16_delete=False_")]
    [Action("fileIndex=17_delete=False_")]
    [Action("fileIndex=18_delete=False_")]
    [Action("fileIndex=19_delete=False_")]
    [Action("fileIndex=20_delete=False_")]
    [Action("fileIndex=21_delete=False_")]
    [Action("fileIndex=22_delete=False_")]
    [Action("fileIndex=23_delete=False_")]
    [Action("fileIndex=24_delete=False_")]
    [Action("fileIndex=25_delete=False_")]
    [Action("fileIndex=26_delete=False_")]
    [Action("fileIndex=27_delete=False_")]
    [Action("fileIndex=28_delete=False_")]
    [Action("fileIndex=29_delete=False_")]
    [Action("fileIndex=30_delete=False_")]
    [Action("fileIndex=31_delete=False_")]
    [Action("fileIndex=32_delete=False_")]
    [Action("fileIndex=33_delete=False_")]
    [Action("fileIndex=34_delete=False_")]
    [Action("fileIndex=35_delete=False_")]
    [Action("fileIndex=36_delete=False_")]
    [Action("fileIndex=37_delete=False_")]
    [Action("fileIndex=38_delete=False_")]
    [Action("fileIndex=39_delete=False_")]
    [Action("fileIndex=40_delete=False_")]
    [Action("fileIndex=41_delete=False_")]
    [Action("fileIndex=42_delete=False_")]
    [Action("fileIndex=43_delete=False_")]
    [Action("fileIndex=44_delete=False_")]
    [Action("fileIndex=45_delete=False_")]
    [Action("fileIndex=46_delete=False_")]
    [Action("fileIndex=47_delete=False_")]
    [Action("fileIndex=48_delete=False_")]
    [Action("fileIndex=49_delete=False_")]
    [Action("fileIndex=50_delete=False_")]
    [Action("fileIndex=51_delete=False_")]
    [Action("fileIndex=52_delete=False_")]
    [Action("fileIndex=53_delete=False_")]
    [Action("fileIndex=54_delete=False_")]
    [Action("fileIndex=55_delete=False_")]
    [Action("fileIndex=56_delete=False_")]
    [Action("fileIndex=57_delete=False_")]
    [Action("fileIndex=58_delete=False_")]
    [Action("fileIndex=59_delete=False_")]
    [Action("fileIndex=60_delete=False_")]
    [Action("fileIndex=61_delete=False_")]
    [Action("fileIndex=62_delete=False_")]
    [Action("fileIndex=63_delete=False_")]
    [Action("fileIndex=64_delete=False_")]
    [Action("fileIndex=65_delete=False_")]
    [Action("fileIndex=66_delete=False_")]
    [Action("fileIndex=67_delete=False_")]
    [Action("fileIndex=68_delete=False_")]
    [Action("fileIndex=69_delete=False_")]
    [Action("fileIndex=70_delete=False_")]
    [Action("fileIndex=71_delete=False_")]
    [Action("fileIndex=72_delete=False_")]
    [Action("fileIndex=73_delete=False_")]
    [Action("fileIndex=74_delete=False_")]
    [Action("fileIndex=75_delete=False_")]
    [Action("fileIndex=76_delete=False_")]
    [Action("fileIndex=77_delete=False_")]
    [Action("fileIndex=78_delete=False_")]
    [Action("fileIndex=79_delete=False_")]
    [Action("fileIndex=80_delete=False_")]
    [Action("fileIndex=81_delete=False_")]
    [Action("fileIndex=82_delete=False_")]
    [Action("fileIndex=83_delete=False_")]
    [Action("fileIndex=84_delete=False_")]
    [Action("fileIndex=85_delete=False_")]
    [Action("fileIndex=86_delete=False_")]
    [Action("fileIndex=87_delete=False_")]
    [Action("fileIndex=88_delete=False_")]
    [Action("fileIndex=89_delete=False_")]
    [Action("fileIndex=90_delete=False_")]
    [Action("fileIndex=91_delete=False_")]
    [Action("fileIndex=92_delete=False_")]
    [Action("fileIndex=93_delete=False_")]
    [Action("fileIndex=94_delete=False_")]
    [Action("fileIndex=95_delete=False_")]
    [Action("fileIndex=96_delete=False_")]
    [Action("fileIndex=97_delete=False_")]
    [Action("fileIndex=98_delete=False_")]
    [Action("fileIndex=99_delete=False_")]
    [Action("fileIndex=100_delete=False_")]
    //
    [Action("fileIndex=0_delete=True_")]
    [Action("fileIndex=1_delete=True_")]
    [Action("fileIndex=2_delete=True_")]
    [Action("fileIndex=3_delete=True_")]
    [Action("fileIndex=4_delete=True_")]
    [Action("fileIndex=5_delete=True_")]
    [Action("fileIndex=6_delete=True_")]
    [Action("fileIndex=7_delete=True_")]
    [Action("fileIndex=8_delete=True_")]
    [Action("fileIndex=9_delete=True_")]
    [Action("fileIndex=10_delete=True_")]
    [Action("fileIndex=11_delete=True_")]
    [Action("fileIndex=12_delete=True_")]
    [Action("fileIndex=13_delete=True_")]
    [Action("fileIndex=14_delete=True_")]
    [Action("fileIndex=15_delete=True_")]
    [Action("fileIndex=16_delete=True_")]
    [Action("fileIndex=17_delete=True_")]
    [Action("fileIndex=18_delete=True_")]
    [Action("fileIndex=19_delete=True_")]
    [Action("fileIndex=20_delete=True_")]
    [Action("fileIndex=21_delete=True_")]
    [Action("fileIndex=22_delete=True_")]
    [Action("fileIndex=23_delete=True_")]
    [Action("fileIndex=24_delete=True_")]
    [Action("fileIndex=25_delete=True_")]
    [Action("fileIndex=26_delete=True_")]
    [Action("fileIndex=27_delete=True_")]
    [Action("fileIndex=28_delete=True_")]
    [Action("fileIndex=29_delete=True_")]
    [Action("fileIndex=30_delete=True_")]
    [Action("fileIndex=31_delete=True_")]
    [Action("fileIndex=32_delete=True_")]
    [Action("fileIndex=33_delete=True_")]
    [Action("fileIndex=34_delete=True_")]
    [Action("fileIndex=35_delete=True_")]
    [Action("fileIndex=36_delete=True_")]
    [Action("fileIndex=37_delete=True_")]
    [Action("fileIndex=38_delete=True_")]
    [Action("fileIndex=39_delete=True_")]
    [Action("fileIndex=40_delete=True_")]
    [Action("fileIndex=41_delete=True_")]
    [Action("fileIndex=42_delete=True_")]
    [Action("fileIndex=43_delete=True_")]
    [Action("fileIndex=44_delete=True_")]
    [Action("fileIndex=45_delete=True_")]
    [Action("fileIndex=46_delete=True_")]
    [Action("fileIndex=47_delete=True_")]
    [Action("fileIndex=48_delete=True_")]
    [Action("fileIndex=49_delete=True_")]
    [Action("fileIndex=50_delete=True_")]
    [Action("fileIndex=51_delete=True_")]
    [Action("fileIndex=52_delete=True_")]
    [Action("fileIndex=53_delete=True_")]
    [Action("fileIndex=54_delete=True_")]
    [Action("fileIndex=55_delete=True_")]
    [Action("fileIndex=56_delete=True_")]
    [Action("fileIndex=57_delete=True_")]
    [Action("fileIndex=58_delete=True_")]
    [Action("fileIndex=59_delete=True_")]
    [Action("fileIndex=60_delete=True_")]
    [Action("fileIndex=61_delete=True_")]
    [Action("fileIndex=62_delete=True_")]
    [Action("fileIndex=63_delete=True_")]
    [Action("fileIndex=64_delete=True_")]
    [Action("fileIndex=65_delete=True_")]
    [Action("fileIndex=66_delete=True_")]
    [Action("fileIndex=67_delete=True_")]
    [Action("fileIndex=68_delete=True_")]
    [Action("fileIndex=69_delete=True_")]
    [Action("fileIndex=70_delete=True_")]
    [Action("fileIndex=71_delete=True_")]
    [Action("fileIndex=72_delete=True_")]
    [Action("fileIndex=73_delete=True_")]
    [Action("fileIndex=74_delete=True_")]
    [Action("fileIndex=75_delete=True_")]
    [Action("fileIndex=76_delete=True_")]
    [Action("fileIndex=77_delete=True_")]
    [Action("fileIndex=78_delete=True_")]
    [Action("fileIndex=79_delete=True_")]
    [Action("fileIndex=80_delete=True_")]
    [Action("fileIndex=81_delete=True_")]
    [Action("fileIndex=82_delete=True_")]
    [Action("fileIndex=83_delete=True_")]
    [Action("fileIndex=84_delete=True_")]
    [Action("fileIndex=85_delete=True_")]
    [Action("fileIndex=86_delete=True_")]
    [Action("fileIndex=87_delete=True_")]
    [Action("fileIndex=88_delete=True_")]
    [Action("fileIndex=89_delete=True_")]
    [Action("fileIndex=90_delete=True_")]
    [Action("fileIndex=91_delete=True_")]
    [Action("fileIndex=92_delete=True_")]
    [Action("fileIndex=93_delete=True_")]
    [Action("fileIndex=94_delete=True_")]
    [Action("fileIndex=95_delete=True_")]
    [Action("fileIndex=96_delete=True_")]
    [Action("fileIndex=97_delete=True_")]
    [Action("fileIndex=98_delete=True_")]
    [Action("fileIndex=99_delete=True_")]
    [Action("fileIndex=100_delete=True_")]
    public async void GetFileInlineBtn()
    {
      if (Context.Update.CallbackQuery is not null)
      {
        if (Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.EditMessageReplyMarkupAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
          string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, isCreateOnlyFolder: true);
          List<string> files = Directory.GetFiles(filePath).Select(fn => System.IO.Path.GetFileName(fn)).ToList();

          string? pattern = Context.Update.CallbackQuery.Data;
          int index = -1;
          bool isDel = false;
          if (pattern != null)
          {
            Match match = Regex.Match(pattern, "fileIndex=(.*?)_delete=(.*?)_");
            index = Convert.ToInt32(match.Groups[1].Value);
            isDel = Convert.ToBoolean(match.Groups[2].Value);


            if (isDel)
            {
              string fileName = files[index];
              FileDelFunction(Context.Update, fileName);
            }
            else
            {
              string fileName = files[index];
              GetFileFunction(fileName);
            }
          }
          else
          {
            PushL("файл не найден");
          }
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message?.Chat.Id!, "все команды", replyMarkup: FormInlineCommands.AllCommands());
        }
      }
    }


    private async void FileDelFunction(Update update, string fileName)
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, fileName);
      if (filePath is not null)
      {
        if (System.IO.File.Exists(filePath))
        {
          System.IO.File.Delete(filePath);
        }
        else
        {
          PushL("файл не найден!");
        }
      }
      if (update.CallbackQuery is not null)
      {
        if (update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"файл удален [{fileName}]");
        }
      }
      else if (update.Message is not null)
      {
        if (update.Message.Chat is not null)
        {
          await Context.Bot.Client.SendTextMessageAsync(update.Message.Chat.Id, $"файл удален [{fileName}]");
        }
      }
    }
    private async void GetFileFunction(string fileName)
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, fileName);
      if (filePath is not null)
      {
        if (System.IO.File.Exists(filePath))
        {
          using (FileStream fs = new FileStream(filePath, FileMode.Open))
          {
            await Context.Bot.Client.SendDocumentAsync(Context.GetSafeChatId()!, InputFile.FromStream(fs, fileName));
          }
        }
        else
        {
          PushL("файл не найден!");
        }
      }
    }
  }
}

