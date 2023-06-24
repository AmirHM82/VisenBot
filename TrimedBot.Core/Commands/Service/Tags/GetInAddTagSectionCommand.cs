using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class GetInAddTagSectionCommand : ICommand
    {
        private ObjectBox objectBox;

        public GetInAddTagSectionCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            //await new TempMessages(objectBox).Delete();
            objectBox.IsNeedDeleteTemps = false;

            new TextResponseProcessor(objectBox)
            {
                Keyboard = Keyboard.CancelKeyboard(),
                ReceiverId = objectBox.User.UserId,
                Text = "Send name of the tag"
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserState = DAL.Enums.UserState.Add_General_Tag;
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
