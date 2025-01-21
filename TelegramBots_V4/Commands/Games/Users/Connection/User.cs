// using System.Threading.Tasks;
// // using Newtonsoft.Json;
// using System.Text.Json;
// using Telegram.Bot.Types;
// using TelegramBots_V4.Commands.Games.Users.Connection.Json;
// using TelegramBots_V4.Json;

// namespace TelegramBots_V4.Commands.Games.Users.Connection
// {
//   public static class User
//   {
//     private static string docFilePath = @"\User";
//     public static async Task UsersRegistration(this Update update, string id)
//     {
//       DeserializeMethod deserialize = Newtonsoft.Json.JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;
//       string? name = "Аноним";
//       string filePath = await update.HashNames(deserialize.FileStoragePath!, docFilePath, isCreateOnlyFolder: true);
//       filePath = Path.Combine(filePath, @"userJson.json");   

//       if (update.CallbackQuery is not null)
//         if (update.CallbackQuery.Message is not null)
//           name = update.CallbackQuery.Message.Chat.FirstName;

//       else if (update.Message is not null)
//         if (update.Message.Chat is not null)
//           name = update.Message.Chat.FirstName;

//       using (FileStream fs = new FileStream(filePath, FileMode.Create))
//       {
//         UserJsonDeserlalize userJson = new UserJsonDeserlalize(id, name!);
//         await JsonSerializer.SerializeAsync<UserJsonDeserlalize>(fs, userJson);
//       }
//     }

//     public static async Task GetUsers(Update update)
//     {
//       DeserializeMethod deserialize = Newtonsoft.Json.JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;
//       string filePath = Path.Combine(await update.HashNames(deserialize.FileStoragePath!, docFilePath, isCreateOnlyFolder: true), @"\userJson.json");

//       if (!Directory.Exists(filePath))
//       {
//         Directory.CreateDirectory(filePath);
//       }

//       using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
//       {
//         UserJsonDeserlalize? userJson = await JsonSerializer.DeserializeAsync<UserJsonDeserlalize>(fs);
//       }
//     }
//   }
// }


using Deployf.Botf;
using Newtonsoft.Json;
using TelegramBots_V4.Json;

namespace TelegramBots_V4.Commands.Games.Users.Connection
{
  class User : BotController
  {
    private string docFilePath = @"\User";
    private string descUserId = "userIdIndex";
    DeserializeMethod deserialize = JsonConvert.DeserializeObject<DeserializeMethod>(System.IO.File.ReadAllText(@"appsettings.json"))!;
  }
}
