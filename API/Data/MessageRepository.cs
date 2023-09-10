using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMeessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
<<<<<<< HEAD
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username 
                    && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                    && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null
                    && u.RecipientDeleted == false)
=======
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
                _
                    => query.Where(
                        u => u.RecipientUsername == messageParams.Username && u.DateRead == null
                    )
>>>>>>> 98e41f5f3b6b8e998056840b1cc1495fb5ca7260
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(
                messages,
                messageParams.PageNumber,
                messageParams.PageSize
            );
        }

        public async Task<IEnumerable<MessageDto>> GetMessageTread(
            string currentUserName,
            string recipientUserName
        )
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .Where(
                    m =>
<<<<<<< HEAD
                        m.RecipientUsername == currentUserName && m.RecipientDeleted == false
                            && m.SenderUsername == recipientUserName 
                        || m.RecipientUsername == recipientUserName && m.SenderDeleted == false
                            && m.SenderUsername == currentUserName
                )
                .OrderBy(messages => messages.MessageSent)
=======
                        m.RecipientUsername == currentUserName
                            && m.SenderUsername == recipientUserName
                        || m.RecipientUsername == recipientUserName
                            && m.SenderUsername == currentUserName
                )
                .OrderByDescending(messages => messages.MessageSent)
>>>>>>> 98e41f5f3b6b8e998056840b1cc1495fb5ca7260
                .ToListAsync();

            var unreadMessages = messages
                .Where(m => m.DateRead == null && m.RecipientUsername == currentUserName)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
