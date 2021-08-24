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
    public class SettingsMenuCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string command;

        public SettingsMenuCommand(ObjectBox objectBox, string command)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.command = command;
        }

        public async Task Do()
        {
            Processor message = null;
            switch (command.ToLower())
            {
                case "ads properties":
                    message = new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Choose, wich one you wanna change:",
                        Keyboard = Keyboard.AdsPropertiesKeyboard()
                    };
                    break;
                case "permemberadsprice":
                case "basicadsprice":
                case "numberofadsperday":
                    decimal value = 0;
                    switch (command.ToLower())
                    {
                        case "permemberadsprice":
                            userServices.ChangeUserPlace(objectBox.User, UserLocation.Settings_PerMemberAdsPrice);
                            value = objectBox.Settings.PerMemberAdsPrice;
                            break;
                        case "basicadsprice":
                            userServices.ChangeUserPlace(objectBox.User, UserLocation.Settings_BasicAdsPrice);
                            value = objectBox.Settings.BasicAdsPrice;
                            break;
                        case "numberofadsperday":
                            userServices.ChangeUserPlace(objectBox.User, UserLocation.Settings_NumberOfAdsPerDay);
                            value = objectBox.Settings.NumberOfAdsPerDay;
                            break;
                    }
                    message = new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = $"Current value: {value}\nSend your new value:",
                        Keyboard = Keyboard.CancelKeyboard()
                    };
                    await userServices.SaveAsync();
                    break;
                default:
                    break;
            }
            message?.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
