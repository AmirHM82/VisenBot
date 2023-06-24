using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes
{
    public static class Send
    {
        public static void SendText(this Message message, long ReceiverId, ObjectBox objectBox)
        {
            new TextResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId, //long.Parse(objectBox.User.Temp)
                Text = $"Message from: {objectBox.User.Access}\n{message.Text}"
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendPhoto(this PhotoSize[] photo, string caption, long ReceiverId, ObjectBox objectBox)
        {
            new PhotoResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Photo = photo[0].FileId,
                Text = $"Message from: {objectBox.User.Access}\n{caption}"
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendVideo(this Video video, string caption, long ReceiverId, ObjectBox objectBox)
        {
            new VideoResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Text = $"Message from: {objectBox.User.Access}\n{caption}",
                Video = video.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendVoice(this Voice voice, string caption, long ReceiverId, ObjectBox objectBox)
        {
            new VoiceResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Text = $"Message from: {objectBox.User.Access}\n{caption}",
                Voice = voice.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendAudio(this Audio audio, string caption, long ReceiverId, ObjectBox objectBox)
        {
            new AudioResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Text = $"Message from: {objectBox.User.Access}\n{caption}",
                Audio = audio.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendSticker(this Sticker sticker, long ReceiverId, ObjectBox objectBox)
        {
            new TextResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Text = $"Message from: {objectBox.User.Access}"
            }.AddThisMessageToService(objectBox.Provider);

            new StickerResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Sticker = sticker.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendDocument(this Document document, string caption, long ReceiverId, ObjectBox objectBox)
        {
            new DocumentResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                Text = $"Message from: {objectBox.User.Access}\n{caption}",
                Document = document.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendContact(this Contact contact, long ReceiverId, ObjectBox objectBox)
        {
            new ContactResponseProcessor(objectBox)
            {
                ReceiverId = ReceiverId,
                PhoneNumber = contact.PhoneNumber,
                FirstName = contact.FirstName,
                LastName = contact.LastName
            }.AddThisMessageToService(objectBox.Provider);
        }

        public static void SendMessage(this Message message, long ReceiverId, ObjectBox objectBox)
        {
            switch (message.Type)
            {
                case MessageType.Text:
                    message.SendText(ReceiverId, objectBox);
                    break;
                case MessageType.Photo:
                    message.Photo.SendPhoto(message.Caption, ReceiverId, objectBox);
                    break;
                case MessageType.Video:
                    message.Video.SendVideo(message.Caption, ReceiverId, objectBox);
                    break;
                case MessageType.Voice:
                    message.Voice.SendVoice(message.Caption, ReceiverId, objectBox);
                    break;
                case MessageType.Audio:
                    message.Audio.SendAudio(message.Caption, ReceiverId, objectBox);
                    break;
                case MessageType.Sticker:
                    message.Sticker.SendSticker(ReceiverId, objectBox);
                    break;
                case MessageType.Document:
                    message.Document.SendDocument(message.Caption, ReceiverId, objectBox);
                    break;
                case MessageType.Contact:
                    message.Contact.SendContact(ReceiverId, objectBox);
                    break;
                default:
                    new TextResponseProcessor(objectBox)
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Sorry! This kind of message doesn't support here."
                    }.AddThisMessageToService(objectBox.Provider);
                    break;
            }
        }
    }
}
