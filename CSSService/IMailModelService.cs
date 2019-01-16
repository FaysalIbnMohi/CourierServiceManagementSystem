using CSSEntity;
using CSSRepository;
using System.Collections.Generic;

namespace CSSService
{
    public interface IMailModelService : IService<MailModel>
    {
        List<MailModel> GetAllById(string id, string session);
        int InsertMail(MailModel mailModel);
        int DeleteMail(int id);
    }
}
