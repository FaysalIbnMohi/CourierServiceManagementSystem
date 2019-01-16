using CSSEntity;
using CSSRepository;
using System.Collections.Generic;

namespace CSSRepository
{
    interface IMailModelRepository : IRepository<MailModel>
    {
        List<MailModel> GetAllById(string id, string session);
        int InsertMail(MailModel mailModel);
        int DeleteMail(int id);
    }
}
