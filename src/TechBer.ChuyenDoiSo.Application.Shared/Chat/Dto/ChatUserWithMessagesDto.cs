using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}