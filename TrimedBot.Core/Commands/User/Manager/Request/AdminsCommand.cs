using System.Threading.Tasks;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class AdminsCommand : ICommand
    {
        private int pageNumber;
        private ObjectBox objectBox;

        public AdminsCommand(ObjectBox objectBox, int pageNumber)
        {
            this.objectBox = objectBox;
            this.pageNumber = pageNumber;
        }

        public async Task Do()
        {
            if (pageNumber > 0)
            {
                await new TempMessages(objectBox).Delete();
                await new Admins(objectBox).Send(pageNumber);
                new NPMessage(objectBox).Send(pageNumber, CallbackSection.Admin);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
