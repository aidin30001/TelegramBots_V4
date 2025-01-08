using Deployf.Botf;
using Telegram.Bot;

namespace TelegramBots_V4.Commands
{
  public class HelpCommands : BotController
  {
    [Action("/h", "Information about commands")]
    [Action("/help", "Information about commands")]
    public async void InfoCommands()
    {
      string result = "null";
      using (StreamReader sr = new StreamReader(@"./Information.txt"))
      {
        result = sr.ReadToEnd();
      }

      if (Context.Update.CallbackQuery is not null)
      {
        await Context.Bot.Client.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
        await Context.Bot.Client.SendTextMessageAsync(Context.Update.CallbackQuery.Message?.Chat.Id ?? 2235189871, result);
      }
      else
      {
        PushL(result);
      }
    }
  }
}
