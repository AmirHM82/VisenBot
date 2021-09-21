using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Core.Classes
{
    public static class Sentences
    {
        public const string Help_Member = "You can use keyboard or these commands:\n" +
            "/AddVideo Add a video.\n" +
            "/SendAdminRequest Send an admin request to be an admin.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.";

        public const string Help_Admin = "You can use keyboard or these commands:\n" +
            "/Posts It showss you posts that they didn't confirm.\n" +
            "/Tags To manage posts' tags.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.";

        public const string Help_Manager = "You can use keyborad or these commands:\n" +
            "/AdminRequests Admin requests send you and you can accept or refuse them.\n" +
            "/Admins It shows you list of admins\n" +
            "/Posts It showss you posts that they didn't confirm.\n" +
            "/Tags To manage posts' tags.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.\n" +
            "/SearchInUsers You can search users here and do everything you want with them.\n" +
            "/Settings Shows you settings to change.\n" +
            "/SendMessageToAll Send a message to all users.";

        public const string Access_Denied = "You don't have access to do that.";

        public const string User_Banned = "Admin banned u. Bot won't answer ur requests till u unbanned.\nSend /chatwithadmin if u wanna talk to an admin.";

        public const string User_Unbanned = "Congratulations. You unbanned.";

        public const string Admin_Deleted = "Manager deleted you from admins.";

        public const string Admin_Request_Accepted = "Your admin request accepted.";

        public const string Admin_Request_Refused = "Your admin request refused";

        public const string Channel_Add_Guide = "To add a new channel first get a token from bot via /token command or token button on keyboard.\n" +
            "Then make sure ur bot is admin in the channel.\n" +
            "After that send the token in the channel.\n";

        public const string Bot_Not_Available = "Sorry we are working on something; bot is not available for a few moments.\nUr state will be save u can resume ur work.";
    }
}
