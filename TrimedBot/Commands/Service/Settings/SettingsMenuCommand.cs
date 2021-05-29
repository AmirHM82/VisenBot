using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Service.Settings
{
    public class SettingsMenuCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;
        private string command;

        public SettingsMenuCommand(IServiceProvider provider, string command)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.command = command;
        }

        public async Task Do()
        {
            switch (command.ToLower())
            {
                case "ads properties":
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, "Choose, wich one you wanna change:", replyMarkup: Keyboard.AdsPropertiesKeyboard);
                    break;
                case "permemberadsprice":
                case "basicadsprice":
                case "numberofadsperday":
                    decimal value = 0;
                    switch (command.ToLower())
                    {
                        case "permemberadsprice":
                            userServices.ChangeUserPlace(objectBox.User, UserPlace.Settings_PerMemberAdsPrice);
                            value = objectBox.Settings.PerMemberAdsPrice;
                            break;
                        case "basicadsprice":
                            userServices.ChangeUserPlace(objectBox.User, UserPlace.Settings_BasicAdsPrice);
                            value = objectBox.Settings.BasicAdsPrice;
                            break;
                        case "numberofadsperday":
                            userServices.ChangeUserPlace(objectBox.User, UserPlace.Settings_NumberOfAdsPerDay);
                            value = objectBox.Settings.NumberOfAdsPerDay;
                            break;
                    }
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, $"Current value: {value}\nSend your new value:", replyMarkup: Keyboard.CancelKeyboard);
                    await userServices.SaveAsync();
                    break;
                default:
                    break;
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
