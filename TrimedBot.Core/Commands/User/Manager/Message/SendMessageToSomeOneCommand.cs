using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Services;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Telegram.Bot.Types.Message message;

        public SendMessageToSomeOneCommand(ObjectBox objectBox, Telegram.Bot.Types.Message message)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public Task Do()
        {
            message.SendMessage(long.Parse(objectBox.User.Temp), objectBox);
            // List<Processor> processor = new();

            // switch (message.Type)
            // {
            //     case MessageType.Text:
            //         processor.Add(new TextResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Text}",
            //             ParseMode = ParseMode.MarkdownV2
            //         });
            //         break;
            //     case MessageType.Photo:
            //         processor.Add(new PhotoResponseProcessor()
            //         {
            //             Photo = message.Photo[0].FileId,
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Caption}",
            //             ParseMode = ParseMode.MarkdownV2,
            //             ReceiverId = long.Parse(objectBox.User.Temp)
            //         });
            //         break;
            //     case MessageType.Video:
            //         processor.Add(new VideoResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Caption}",
            //             ParseMode = ParseMode.MarkdownV2,
            //             Video = message.Video.FileId
            //         });
            //         break;
            //     case MessageType.Voice:
            //         processor.Add(new VoiceResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Caption}",
            //             ParseMode = ParseMode.MarkdownV2,
            //             Voice = message.Voice.FileId
            //         });
            //         break;
            //     case MessageType.Audio:
            //         processor.Add(new AudioResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Caption}",
            //             ParseMode = ParseMode.MarkdownV2,
            //             Audio = message.Voice.FileId
            //         });
            //         break;
            //     case MessageType.Sticker:
            //         processor.Add(new TextResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}",
            //             ParseMode = ParseMode.MarkdownV2
            //         });

            //         processor.Add(new StickerResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Sticker = message.Sticker.FileId
            //         });
            //         break;
            //     case MessageType.Document:
            //         processor.Add(new DocumentResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             Text = $"Message from: {objectBox.User.Access}\n{message.Caption}",
            //             ParseMode = ParseMode.MarkdownV2,
            //             Document = message.Document.FileId
            //         });
            //         break;
            //     case MessageType.Contact:
            //         processor.Add(new ContactResponseProcessor()
            //         {
            //             ReceiverId = long.Parse(objectBox.User.Temp),
            //             PhoneNumber = message.Contact.PhoneNumber,
            //             FirstName = message.Contact.FirstName,
            //             LastName = message.Contact.LastName
            //         });
            //         break;
            //     default:
            //         processor.Add(new TextResponseProcessor()
            //         {
            //             ReceiverId = objectBox.User.UserId,
            //             Text = "Sorry! This kind of message doesn't support here."
            //         });
            //         break;
            // }

            // new MultiProcessor(processor).AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
