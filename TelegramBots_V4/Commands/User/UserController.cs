using System.Text;
using System.Text.RegularExpressions;
using Deployf.Botf;
using Newtonsoft.Json;
using Telegram.Bot;
using TelegramBots_V4.Json;

namespace TelegramBots_V4.Commands.User
{
  public class UserController : BotController
  {
    private string docFilePath = @"\User";
    private string descUserId = "userIdIndex";
    DeserializeMethod deserialize = JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;

    [Action("/adduser")]
    public async Task AddUser(long userId, string name)
    {
      if (Context.Update.Message is not null && Context.Update.Message.Chat is not null)
      {
        Games.Users.Connection.User _user = new Games.Users.Connection.User();
        _user.UserId = Convert.ToString(userId);
        _user.UserName = name;
        _user.UserConnection();
        string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, docFilePath, "data.txt", isCreateFolder: true);

        if (!System.IO.File.Exists(filePath))
        {
          using (FileStream fs = new FileStream(filePath, FileMode.CreateNew))
            fs.Dispose();
        }
        using (FileStream fs = new FileStream(filePath, FileMode.Append))
        {
          using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
          {
            await sw.WriteLineAsync($"id=<{Convert.ToString(userId)}>name=<{name}>");
            await sw.DisposeAsync();
            await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "Успешно 👍");
          }
          await fs.DisposeAsync();
        }
      }
    }

    [Action("/getuser", "получит пользователя")]
    public async Task GetUser()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, docFilePath, "data.txt");
      List<string> userId = new List<string>();
      string? allUserId = null;
      if (!System.IO.File.Exists(filePath))
      {
        if (Context.Update.Message is not null && Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.Message.Chat.Id, Context.Update.Message.MessageId - 1);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "нету пользователя", replyMarkup: userId.GenerateInlineButtons(descUserId));
        }
        else if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "нету пользователя", replyMarkup: userId.GenerateInlineButtons(descUserId));
        }
        return;
      }
      using (FileStream fs = new FileStream(filePath, FileMode.Open))
      {
        using (StreamReader sr = new StreamReader(fs))
        {
          allUserId = await sr.ReadToEndAsync();
          sr.Dispose();
        }
        await fs.DisposeAsync();
      }
      if (allUserId != null)
      {
        userId = allUserId.Split("\n").ToList();
        if (Context.Update.Message is not null && Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.Message.Chat.Id, Context.Update.Message.MessageId - 1);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "список", replyMarkup: userId.GenerateInlineButtons(descUserId, isRegex: true));
        }
        else if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "список", replyMarkup: userId.GenerateInlineButtons(descUserId, isRegex: true));
        }
      }
    }

    [Action("/deluser", "удалить пользователя")]
    public async Task DelUser()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, docFilePath, "data.txt");
      List<string> userId = new List<string>();
      string? allUserId = null;
      if (!System.IO.File.Exists(filePath))
      {
        if (Context.Update.Message is not null && Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.Message.Chat.Id, Context.Update.Message.MessageId - 1);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "нету пользователя", replyMarkup: userId.GenerateInlineButtons(descUserId));
        }
        else if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "нету пользователя", replyMarkup: userId.GenerateInlineButtons(descUserId));
        }
        return;
      }
      using (FileStream fs = new FileStream(filePath, FileMode.Open))
      {
        using (StreamReader sr = new StreamReader(fs))
        {
          allUserId = await sr.ReadToEndAsync();
          sr.Dispose();
        }
        await fs.DisposeAsync();
      }
      if (allUserId != null)
      {
        userId = allUserId.Split("\n").ToList();
        if (Context.Update.Message is not null && Context.Update.Message.Chat is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.Message.Chat.Id, Context.Update.Message.MessageId - 1);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, "список", replyMarkup: userId.GenerateInlineButtons(descUserId, isRegex: true, del: true));
        }
        else if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
        {
          await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
          await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, "список", replyMarkup: userId.GenerateInlineButtons(descUserId, isRegex: true, del: true));
        }
      }
    }

    [Action("userIdIndex=0_delete=False_")]
    [Action("userIdIndex=1_delete=False_")]
    [Action("userIdIndex=2_delete=False_")]
    [Action("userIdIndex=3_delete=False_")]
    [Action("userIdIndex=4_delete=False_")]
    [Action("userIdIndex=5_delete=False_")]
    [Action("userIdIndex=6_delete=False_")]
    [Action("userIdIndex=7_delete=False_")]
    [Action("userIdIndex=8_delete=False_")]
    [Action("userIdIndex=9_delete=False_")]

    [Action("userIdIndex=10_delete=False_")]
    [Action("userIdIndex=11_delete=False_")]
    [Action("userIdIndex=12_delete=False_")]
    [Action("userIdIndex=13_delete=False_")]
    [Action("userIdIndex=14_delete=False_")]
    [Action("userIdIndex=15_delete=False_")]
    [Action("userIdIndex=16_delete=False_")]
    [Action("userIdIndex=17_delete=False_")]
    [Action("userIdIndex=18_delete=False_")]
    [Action("userIdIndex=19_delete=False_")]
    [Action("userIdIndex=20_delete=False_")]
    [Action("userIdIndex=21_delete=False_")]
    [Action("userIdIndex=22_delete=False_")]
    [Action("userIdIndex=23_delete=False_")]
    [Action("userIdIndex=24_delete=False_")]
    [Action("userIdIndex=25_delete=False_")]
    [Action("userIdIndex=26_delete=False_")]
    [Action("userIdIndex=27_delete=False_")]
    [Action("userIdIndex=28_delete=False_")]
    [Action("userIdIndex=29_delete=False_")]
    [Action("userIdIndex=30_delete=False_")]
    [Action("userIdIndex=31_delete=False_")]
    [Action("userIdIndex=32_delete=False_")]
    [Action("userIdIndex=33_delete=False_")]
    [Action("userIdIndex=34_delete=False_")]
    [Action("userIdIndex=35_delete=False_")]
    [Action("userIdIndex=36_delete=False_")]
    [Action("userIdIndex=37_delete=False_")]
    [Action("userIdIndex=38_delete=False_")]
    [Action("userIdIndex=39_delete=False_")]
    [Action("userIdIndex=40_delete=False_")]
    [Action("userIdIndex=41_delete=False_")]
    [Action("userIdIndex=42_delete=False_")]
    [Action("userIdIndex=43_delete=False_")]
    [Action("userIdIndex=44_delete=False_")]
    [Action("userIdIndex=45_delete=False_")]
    [Action("userIdIndex=46_delete=False_")]
    [Action("userIdIndex=47_delete=False_")]
    [Action("userIdIndex=48_delete=False_")]
    [Action("userIdIndex=49_delete=False_")]
    [Action("userIdIndex=50_delete=False_")]
    [Action("userIdIndex=51_delete=False_")]
    [Action("userIdIndex=52_delete=False_")]
    [Action("userIdIndex=53_delete=False_")]
    [Action("userIdIndex=54_delete=False_")]
    [Action("userIdIndex=55_delete=False_")]
    [Action("userIdIndex=56_delete=False_")]
    [Action("userIdIndex=57_delete=False_")]
    [Action("userIdIndex=58_delete=False_")]
    [Action("userIdIndex=59_delete=False_")]
    [Action("userIdIndex=60_delete=False_")]
    [Action("userIdIndex=61_delete=False_")]
    [Action("userIdIndex=62_delete=False_")]
    [Action("userIdIndex=63_delete=False_")]
    [Action("userIdIndex=64_delete=False_")]
    [Action("userIdIndex=65_delete=False_")]
    [Action("userIdIndex=66_delete=False_")]
    [Action("userIdIndex=67_delete=False_")]
    [Action("userIdIndex=68_delete=False_")]
    [Action("userIdIndex=69_delete=False_")]
    [Action("userIdIndex=70_delete=False_")]
    [Action("userIdIndex=71_delete=False_")]
    [Action("userIdIndex=72_delete=False_")]
    [Action("userIdIndex=73_delete=False_")]
    [Action("userIdIndex=74_delete=False_")]
    [Action("userIdIndex=75_delete=False_")]
    [Action("userIdIndex=76_delete=False_")]
    [Action("userIdIndex=77_delete=False_")]
    [Action("userIdIndex=78_delete=False_")]
    [Action("userIdIndex=79_delete=False_")]
    [Action("userIdIndex=80_delete=False_")]
    [Action("userIdIndex=81_delete=False_")]
    [Action("userIdIndex=82_delete=False_")]
    [Action("userIdIndex=83_delete=False_")]
    [Action("userIdIndex=84_delete=False_")]
    [Action("userIdIndex=85_delete=False_")]
    [Action("userIdIndex=86_delete=False_")]
    [Action("userIdIndex=87_delete=False_")]
    [Action("userIdIndex=88_delete=False_")]
    [Action("userIdIndex=89_delete=False_")]
    [Action("userIdIndex=90_delete=False_")]
    [Action("userIdIndex=91_delete=False_")]
    [Action("userIdIndex=92_delete=False_")]
    [Action("userIdIndex=93_delete=False_")]
    [Action("userIdIndex=94_delete=False_")]
    [Action("userIdIndex=95_delete=False_")]
    [Action("userIdIndex=96_delete=False_")]
    [Action("userIdIndex=97_delete=False_")]
    [Action("userIdIndex=98_delete=False_")]
    [Action("userIdIndex=99_delete=False_")]
    [Action("userIdIndex=100_delete=False_")]
    //
    [Action("userIdIndex=0_delete=True_")]
    [Action("userIdIndex=1_delete=True_")]
    [Action("userIdIndex=2_delete=True_")]
    [Action("userIdIndex=3_delete=True_")]
    [Action("userIdIndex=4_delete=True_")]
    [Action("userIdIndex=5_delete=True_")]
    [Action("userIdIndex=6_delete=True_")]
    [Action("userIdIndex=7_delete=True_")]
    [Action("userIdIndex=8_delete=True_")]
    [Action("userIdIndex=9_delete=True_")]

    [Action("userIdIndex=10_delete=True_")]
    [Action("userIdIndex=11_delete=True_")]
    [Action("userIdIndex=12_delete=True_")]
    [Action("userIdIndex=13_delete=True_")]
    [Action("userIdIndex=14_delete=True_")]
    [Action("userIdIndex=15_delete=True_")]
    [Action("userIdIndex=16_delete=True_")]
    [Action("userIdIndex=17_delete=True_")]
    [Action("userIdIndex=18_delete=True_")]
    [Action("userIdIndex=19_delete=True_")]
    [Action("userIdIndex=20_delete=True_")]
    [Action("userIdIndex=21_delete=True_")]
    [Action("userIdIndex=22_delete=True_")]
    [Action("userIdIndex=23_delete=True_")]
    [Action("userIdIndex=24_delete=True_")]
    [Action("userIdIndex=25_delete=True_")]
    [Action("userIdIndex=26_delete=True_")]
    [Action("userIdIndex=27_delete=True_")]
    [Action("userIdIndex=28_delete=True_")]
    [Action("userIdIndex=29_delete=True_")]
    [Action("userIdIndex=30_delete=True_")]
    [Action("userIdIndex=31_delete=True_")]
    [Action("userIdIndex=32_delete=True_")]
    [Action("userIdIndex=33_delete=True_")]
    [Action("userIdIndex=34_delete=True_")]
    [Action("userIdIndex=35_delete=True_")]
    [Action("userIdIndex=36_delete=True_")]
    [Action("userIdIndex=37_delete=True_")]
    [Action("userIdIndex=38_delete=True_")]
    [Action("userIdIndex=39_delete=True_")]
    [Action("userIdIndex=40_delete=True_")]
    [Action("userIdIndex=41_delete=True_")]
    [Action("userIdIndex=42_delete=True_")]
    [Action("userIdIndex=43_delete=True_")]
    [Action("userIdIndex=44_delete=True_")]
    [Action("userIdIndex=45_delete=True_")]
    [Action("userIdIndex=46_delete=True_")]
    [Action("userIdIndex=47_delete=True_")]
    [Action("userIdIndex=48_delete=True_")]
    [Action("userIdIndex=49_delete=True_")]
    [Action("userIdIndex=50_delete=True_")]
    [Action("userIdIndex=51_delete=True_")]
    [Action("userIdIndex=52_delete=True_")]
    [Action("userIdIndex=53_delete=True_")]
    [Action("userIdIndex=54_delete=True_")]
    [Action("userIdIndex=55_delete=True_")]
    [Action("userIdIndex=56_delete=True_")]
    [Action("userIdIndex=57_delete=True_")]
    [Action("userIdIndex=58_delete=True_")]
    [Action("userIdIndex=59_delete=True_")]
    [Action("userIdIndex=60_delete=True_")]
    [Action("userIdIndex=61_delete=True_")]
    [Action("userIdIndex=62_delete=True_")]
    [Action("userIdIndex=63_delete=True_")]
    [Action("userIdIndex=64_delete=True_")]
    [Action("userIdIndex=65_delete=True_")]
    [Action("userIdIndex=66_delete=True_")]
    [Action("userIdIndex=67_delete=True_")]
    [Action("userIdIndex=68_delete=True_")]
    [Action("userIdIndex=69_delete=True_")]
    [Action("userIdIndex=70_delete=True_")]
    [Action("userIdIndex=71_delete=True_")]
    [Action("userIdIndex=72_delete=True_")]
    [Action("userIdIndex=73_delete=True_")]
    [Action("userIdIndex=74_delete=True_")]
    [Action("userIdIndex=75_delete=True_")]
    [Action("userIdIndex=76_delete=True_")]
    [Action("userIdIndex=77_delete=True_")]
    [Action("userIdIndex=78_delete=True_")]
    [Action("userIdIndex=79_delete=True_")]
    [Action("userIdIndex=80_delete=True_")]
    [Action("userIdIndex=81_delete=True_")]
    [Action("userIdIndex=82_delete=True_")]
    [Action("userIdIndex=83_delete=True_")]
    [Action("userIdIndex=84_delete=True_")]
    [Action("userIdIndex=85_delete=True_")]
    [Action("userIdIndex=86_delete=True_")]
    [Action("userIdIndex=87_delete=True_")]
    [Action("userIdIndex=88_delete=True_")]
    [Action("userIdIndex=89_delete=True_")]
    [Action("userIdIndex=90_delete=True_")]
    [Action("userIdIndex=91_delete=True_")]
    [Action("userIdIndex=92_delete=True_")]
    [Action("userIdIndex=93_delete=True_")]
    [Action("userIdIndex=94_delete=True_")]
    [Action("userIdIndex=95_delete=True_")]
    [Action("userIdIndex=96_delete=True_")]
    [Action("userIdIndex=97_delete=True_")]
    [Action("userIdIndex=98_delete=True_")]
    [Action("userIdIndex=99_delete=True_")]
    [Action("userIdIndex=100_delete=True_")]
    public async Task GetUserBtn()
    {
      string filePath = await Context.Update.HashNames(deserialize.FileStoragePath!, docFilePath, "data.txt");
      List<string> userIdList = new List<string>();
      string? allUserId = null;
      string? user = null;
      Games.Users.Connection.User _user = new Games.Users.Connection.User();

      if (Context.Update.CallbackQuery is not null && Context.Update.CallbackQuery.Message is not null)
      {
        await Context.Bot.Client.DeleteMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, Context.Update.CallbackQuery.Message.MessageId);
        await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
          using (StreamReader sr = new StreamReader(fs))
          {
            allUserId = await sr.ReadToEndAsync();
            sr.Dispose();
          }
          await fs.DisposeAsync();
        }
        if (allUserId != null)
        {
          userIdList = allUserId.Split("\n").ToList();

          string? patternCallData = Context.Update.CallbackQuery.Data;
          int indexCallData = -1;
          bool isDel = false;

          if (patternCallData != null)
          {
            Match matchCallData = Regex.Match(patternCallData, "userIdIndex=(.*?)_delete=(.*?)_");
            indexCallData = Convert.ToInt32(matchCallData.Groups[1].Value);
            isDel = Convert.ToBoolean(matchCallData.Groups[2].Value);
            user = userIdList[indexCallData];

            if (isDel)
            {
              string writeSW = OneDelUserList(user, ref userIdList);
              using (StreamWriter sw = new StreamWriter(filePath))
              {
                await sw.WriteLineAsync(writeSW);
                await sw.DisposeAsync();
              }
              if (Context.Update.CallbackQuery is not null)
              {
                if (Context.Update.CallbackQuery.Message is not null)
                {
                  await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, $"удален пользователь");
                  await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message.Chat.Id, $"все команды", replyMarkup: FormInlineCommands.AllCommands());
                }
              }
              else if (Context.Update.Message is not null)
              {
                if (Context.Update.Message.Chat is not null)
                {
                  await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, $"удален пользователь");
                  await Context.Bot.Client.SendTextMessageAsync(Context.Update.Message.Chat.Id, $"все команды", replyMarkup: FormInlineCommands.AllCommands());
                }
              }
            }
            else
            {
              Match matchUserList = Regex.Match(user, "id=<(.*?)>name=<(.*?)>");
              _user.UserId = Convert.ToString(matchUserList.Groups[1])!;
              _user.UserName = Convert.ToString(matchUserList.Groups[2])!;
              _user.UserConnection();

            }
          }
        }
      }
    }

    private string OneDelUserList(string user, ref List<string> userList)
    {
      string? result = null;
      bool one = true;
      foreach (var item in userList)
      {
        if (item != "\r" && item != "")
        {
          if (one == true)
          {
            if (item != user)
            {
              result += item + "\n";
            }
            if (item == user)
            {
              one = false;
              continue;
            }
          }
          if (one == false)
          {
            result += item + "\n";
          }
        }
      }
      return result!;
    }
  }
}
