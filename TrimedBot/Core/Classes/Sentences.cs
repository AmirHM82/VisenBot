using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Core.Classes
{
    public static class Sentences
    {
        public readonly static string Help_Member = "You can use keyboard or these commands:\n" +
            "/SendAdminRequest Send an admin request to be an admin.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.";

        public readonly static string Help_Admin = "You can use keyboard or these commands:\n" +
            "/Posts It showss you posts that they didn't confirm.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.";

        public readonly static string Help_Manager = "You can use keyborad or these commands:\n" +
            "/AdminRequests Admin requests send you and you can accept or refuse them.\n" +
            "/Admins It shows you list of admins\n" +
            "/Posts It showss you posts that they didn't confirm.\n" +
            "/SearchInPosts You can find post here with sending it with inline mode.\n" +
            "/SearchInUsers You can search users here and do everything you want with them.\n" +
            "/Settings Shows you settings to change.\n" +
            "/SendMessageToAll Send a message to all users.";

        public readonly static string Access_Denied = "You don't have access to do that.";
    }
}
