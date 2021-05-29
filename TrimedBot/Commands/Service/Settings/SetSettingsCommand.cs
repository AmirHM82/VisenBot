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
    public class SetSettingsCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        private BotServices _bot;
        private ISettings settingsServices;
        private IUser userServices;
        private string message;

        public SetSettingsCommand(IServiceProvider provider, string message)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            settingsServices = provider.GetRequiredService<ISettings>();
            userServices = provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public async Task Do()
        {
            try
            {
                decimal number = decimal.Parse(message);
                if (number > 0)
                {
                    switch (objectBox.User.UserPlace)
                    {
                        case UserPlace.Settings_PerMemberAdsPrice:
                            objectBox.Settings.PerMemberAdsPrice = number;
                            break;
                        case UserPlace.Settings_BasicAdsPrice:
                            objectBox.Settings.BasicAdsPrice = number;
                            break;
                        case UserPlace.Settings_NumberOfAdsPerDay:
                            objectBox.Settings.NumberOfAdsPerDay = (byte)number;
                            break;
                    }
                    settingsServices.Update(objectBox.Settings);
                    await settingsServices.SaveAsync();
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, "Saved", replyMarkup: Keyboard.AdsPropertiesKeyboard);
                    await userServices.Reset(objectBox.User, UserResetSection.UserPlace);
                }
                else await _bot.SendTextMessageAsync(objectBox.User.UserId, "Please send subject numbers:");
            }
            catch (FormatException)
            {
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Format was incorrect\nPlease send subject numbers:");
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
