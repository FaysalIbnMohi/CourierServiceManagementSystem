using CSSEntity;
using CSSRepository;
using System.Collections.Generic;
using System.Linq;

namespace CSSRepository
{
    public class MailModelRepository : Repository<MailModel>, IMailModelRepository
    {
        private DataContext context;
        public MailModelRepository() { context = DataContext.getInstance(); }

        public List<MailModel> GetAllById(string id, string session)
        {
            if (session == "Admin")
                return this.context.MailModels.Where(c => c.To == id || c.To == "Admin").ToList();
            else
                return this.context.MailModels.Where(c => c.To == id).ToList();
        }

        public MailModel GetMail(int id)
        {
            return this.context.Set<MailModel>().Find(id);
        }

        public int InsertMail(MailModel mailModel)
        {
            context.MailModels.Add(mailModel);
            return context.SaveChanges();
        }

        public int DeleteMail(int id)
        {
            MailModel entity = this.context.MailModels.Find(id);
            context.Set<MailModel>().Remove(entity);
            return context.SaveChanges();
        }

    }
}
