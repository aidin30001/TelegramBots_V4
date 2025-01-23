using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBots_V4.Json
{
  public class UserJsonDeserlalize
  {
    public string? Id { get; set; }
    public string? Name { get; set; }

    public UserJsonDeserlalize(string id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
