using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Enums
{
    public enum UserState : int
    {
        NoWhere = 0,
        AddMedia_SendTitle = 1,
        AddMedia_SendCaption = 2,
        AddMedia_SendMedia = 3,
        EditMedia_Title = 4,
        EditMedia_Caption = 5,
        EditMedia_Video = 6,
        //SeeAddedVideos_Member = 7,
        //SeeAddedVideos_Admin = 8,
        //SeeAddedVideos_Manager = 9,
        SeePrivateAddedVideos = 7,
        SeePublicAddedVideos = 8,
        SeeAdminRequests_Manager = 10,
        SeeAdmins_Manager = 11,
        Search_Posts = 12,
        Search_Users = 13,
        Settings_Menu = 14,
        Settings_PerMemberAdsPrice = 15,
        Settings_BasicAdsPrice = 16,
        Settings_NumberOfAdsPerDay = 17,
        Settings_ChangeSearchedUsersPic = 18,
        Send_Message_ToSomeone = 19,
        Send_Message_ToAll = 20,
        Send_Message_ToAdmins = 21,
        See_All_Tags = 22,
        See_Posts_Tags = 23,
        Add_General_Tag = 24,
        Search_Posts_Tag = 25,
        Search_User_Blocked_Tags = 26,
        EditChannel_Type = 27,
    }
}
