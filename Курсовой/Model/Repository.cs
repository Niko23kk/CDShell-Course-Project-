using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Курсовой.Model
{
    public interface IRepository<IEntity> where IEntity : class
    {
        void Create(IEntity item);
        IEntity FindById(int id);
        IEnumerable<IEntity> GetAll();
        void Remove(IEntity item);
        void Update(IEntity item);
    }

    public class DBRepository<IEntity> : IRepository<IEntity> where IEntity:class
    {
        BuildEntities context;
        DbSet<IEntity> dbSet;

        public DBRepository(BuildEntities _context)
        {
            context = _context;
            dbSet = context.Set<IEntity>();
        }

        public void Create(IEntity item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public IEntity FindById(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<IEntity> GetAll()
        {
            return dbSet.AsNoTracking().ToList();
        }

        public void Remove(IEntity item)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public void Update(IEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
