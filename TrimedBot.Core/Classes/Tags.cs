﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.DAL.Sections;
using Telegram.Bot.Types;

namespace TrimedBot.Core.Classes
{
    public class Tags
    {
        private ObjectBox objectBox;

        public Tags(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task<Tuple<List<Processor>, bool>> GetMessages(int pageNum)
        {
            bool needNP = false;
            List<Processor> result = new List<Processor>();
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tags = await tagService.GetTagsAsync(pageNum);


            if (tags.Count > 0)
            {
                result.Add(new TextResponseProcessor()
                {
                    IsDeletable = true,
                    Keyboard = Keyboard.CancelKeyboard(),
                    ReceiverId = objectBox.User.UserId,
                    Text = "Tags:"
                });

                foreach (var t in tags)
                {
                    result.Add(new TextResponseProcessor()
                    {
                        IsDeletable = true,
                        Keyboard = Keyboard.Tag(t),
                        ReceiverId = objectBox.User.UserId,
                        Text = t.Name
                    });
                }
                needNP = true;
            }

            if (tags.Count < 5)
                result.Add(new TextResponseProcessor()
                {
                    IsDeletable = true,
                    Keyboard = Keyboard.AddTag(),
                    ReceiverId = objectBox.User.UserId,
                    Text = "Press add for add a new tag"
                });

            objectBox.User.UserLocation = DAL.Enums.UserLocation.See_All_Tags;
            objectBox.UpdateUserInfo();

            return new Tuple<List<Processor>, bool>(result, needNP);
        }

        public async Task<Tuple<List<Processor>, bool>> GetMessages(Guid postId, int pageNum)
        {
            bool needNP = false;
            List<Processor> result = new List<Processor>();
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tags = await tagService.GetTagsAsync(postId, pageNum);


            if (tags.Count > 0)
            {
                result.Add(new TextResponseProcessor()
                {
                    IsDeletable = true,
                    Keyboard = Keyboard.CancelKeyboard(),
                    ReceiverId = objectBox.User.UserId,
                    Text = "Tags:"
                });

                foreach (var t in tags)
                {
                    result.Add(new TextResponseProcessor()
                    {
                        IsDeletable = true,
                        Keyboard = Keyboard.Tag(t),
                        ReceiverId = objectBox.User.UserId,
                        Text = t.Name
                    });
                }
                needNP = true;
            }

            if (tags.Count < 5)
            {
                result.Add(new TextResponseProcessor()
                {
                    IsDeletable = true,
                    Keyboard = Keyboard.AddPostsTag(postId),
                    ReceiverId = objectBox.User.UserId,
                    Text = "Press add for add a new tag"
                });
                needNP = false;
            }

            objectBox.User.UserLocation = DAL.Enums.UserLocation.See_All_Tags;
            objectBox.UpdateUserInfo();

            return new Tuple<List<Processor>, bool>(result, needNP);
        }
    }
}