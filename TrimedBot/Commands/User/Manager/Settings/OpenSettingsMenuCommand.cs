using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.User.Manager.Settings
{
    public class OpenSettingsMenuCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;

        public OpenSettingsMenuCommand(IServiceProvider provider)
        {
            Run = Do;
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
        }

        public Func<Task> Run { get; set; }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Settings menu:", replyMarkup: Keyboard.SettingsKeyboard);
                userServices.ChangeUserPlace(objectBox.User, UserPlace.Settings_Menu);
                await userServices.SaveAsync();
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
