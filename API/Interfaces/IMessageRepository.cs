using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMeessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<MessageDto>> GetMessageTread(
            string currentUserName,
            string recipientUserName
        );
    }
}
