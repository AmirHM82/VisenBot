using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Service.Settings
{
    public class SetSettingsCommand : ICommand
    {
        private ObjectBox objectBox;
        private ISettings settingsServices;
        private IUser userServices;
        private string message;

        public SetSettingsCommand(ObjectBox objectBox, string message)
        {
            this.objectBox = objectBox;
            settingsServices = objectBox.Provider.GetRequiredService<ISettings>();
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public async Task Do()
        {
            Processor Pmessage;
            try
            {
                decimal number = decimal.Parse(message);
                if (number > 0)
                {
                    switch (objectBox.User.UserLocation)
                    {
                        case UserLocation.Settings_PerMemberAdsPrice:
                            objectBox.Settings.PerMemberAdsPrice = number;
                            break;
                        case UserLocation.Settings_BasicAdsPrice:
                            objectBox.Settings.BasicAdsPrice = number;
                            break;
                        case UserLocation.Settings_NumberOfAdsPerDay:
                            objectBox.Settings.NumberOfAdsPerDay = (byte)number;
                            break;
                    }
                    settingsServices.Update(objectBox.Settings);
                    await settingsServices.SaveAsync();
                    Pmessage = new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Saved",
                        Keyboard = objectBox.Keyboard
                    };
                    await userServices.Reset(objectBox.User, UserResetSection.UserPlace);
                }
                else Pmessage = new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Please send subject numbers:"
                };
            }
            catch (FormatException)
            {
                Pmessage = new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Format was incorrect\nPlease send subject numbers:"
                };
            }
            Pmessage.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
