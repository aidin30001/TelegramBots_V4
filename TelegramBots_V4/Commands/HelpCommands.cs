using Deployf.Botf;

namespace TelegramBots_V4.Commands
{
  public class HelpCommands : BotController
  {
    [Action("/help", "Information about commands")]
    public  void InfoCommands()
    {
      string result = "null";
      using (StreamReader sr = new StreamReader(@"./Information.txt"))
      {
        result = sr.ReadToEnd();
      }
      PushL(result);
    }
  }
}
