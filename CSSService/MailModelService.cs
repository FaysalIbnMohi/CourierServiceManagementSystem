using CSSEntity;
using CSSRepository;
using System.Collections.Generic;
using System.Linq;

namespace CSSService
{
    public class MailModelService : Service<MailModel>, IMailModelService
    {
        MailModelRepository repo = new MailModelRepository();

        public List<MailModel> GetAllById(string id, string session)
        {
            return repo.GetAllById(id, session);
        }
        public int InsertMail(MailModel mailModel)
        {
            return repo.InsertMail(mailModel);
        }
        public int DeleteMail(int id)
        {
            return repo.DeleteMail(id);
        }
        public MailModel GetMail(int id)
        {
            return repo.GetMail(id);
        }

    }
}
