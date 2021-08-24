﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class AddTagCommand : ICommand
    {
        private ObjectBox objectBox;
        private string name;

        public AddTagCommand(ObjectBox objectBox, string name)
        {
            this.objectBox = objectBox;
            this.name = name;
        }

        public async Task Do()
        {
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            await tagService.AddAsync(new DAL.Entities.Tag { Name = name });
            await tagService.SaveAsync();

            new TextResponseProcessor()
            {
                Keyboard = objectBox.Keyboard,
                ReceiverId = objectBox.User.UserId,
                Text = "Tag added"
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserLocation = DAL.Enums.UserLocation.NoWhere;
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
