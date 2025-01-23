using Deployf.Botf;
using Newtonsoft.Json;
using TelegramBots_V4.Json;

namespace TelegramBots_V4.Commands.Games.Users.Connection
{
  class User
  {
    private string? userId = null;
    public string UserId
    {
      set
      {
        if (value != null)
          userId = value;
      }
    }
    private string? userName = null;
    public string UserName
    {
      set
      {
        if (value != null)
          userName = value;
      }
    }
    DeserializeMethod deserialize = JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;
    private string path = @"./Json/Users.json";
    public void UserConnection()
    {
      if (userId != null && userName != null)
      {
        using (StreamWriter sw = new StreamWriter(path))
        {
          sw.WriteLineAsync(JsonConvert.SerializeObject(new UserJsonDeserlalize(userId, userName)));
          sw.Dispose();
        }
      }
    }
    
  }
}
