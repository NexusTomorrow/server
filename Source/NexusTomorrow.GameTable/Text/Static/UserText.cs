namespace NexusTomorrow.GameTable.Text.Static;

/// <remarks>
/// Each kind of text a player types, by the value the client registers for it as GameLib.CodeEnumUserText. The client's rule for
/// each is the server's <c>UserTextRules</c>.
/// </remarks>
public enum UserText
{
    CharacterName = 0,
    ScientistScanbotName = 1,
    GuildName = 2,
    GuildRankName = 4,
    GuildBankTabName = 5,
    HousingResidenceName = 6,
    Chat = 7,
    MailSubject = 8,
    MailBody = 9,
    ChatCustomChannelName = 10,
    ReadyCheck = 11,
    FriendshipNote = 12,
    ChatCustomChannelPassword = 13,
    GuildMessageOfTheDay = 14,
    GuildMemberNote = 15,
    GuildRecruitDescription = 16,
    GuildInfoMessage = 17,
    FriendshipAccountName = 18,
    FriendshipAccountPrivateNote = 19,
    FriendshipAccountPublicNote = 20,
    FriendshipAccountEmail = 21,
    FriendshipInviteNote = 22,
    PlayerTicketText = 24,
    CharacterNamePart = 25,
    PlayerTicketSubject = 26,
}
