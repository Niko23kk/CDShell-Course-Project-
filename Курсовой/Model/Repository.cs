using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Курсовой.Model
{
    public interface IRepository<IEntity>:IDisposable where IEntity : class
    {
        void Create(IEntity item);
        IEntity FindById(int id);
        void AddCollection(IEnumerable<IEntity> entities);
        void RemoveCollection(IEnumerable<IEntity> entities);
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

        public void AddCollection(IEnumerable<IEntity> entities)
        {
            foreach (var item in entities)
            {
                dbSet.Add(item);
            }
            context.SaveChanges();
        }

        public void RemoveCollection(IEnumerable<IEntity> entities)
        {
            if (typeof(IEntity) == typeof(WorkField))
            {
                var time = dbSet.ToList();
                foreach (var item in entities)
                {
                    dbSet.Remove(time.Where(s => (s as WorkField).ID == (item as WorkField).ID).First());
                }
                context.SaveChanges();
            }
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
            if (typeof(IEntity) == typeof(UserProject))
            {
                dbSet.Remove(dbSet.ToList().First(s => (s as UserProject).ID == (item as UserProject).ID));
                context.SaveChanges();
                return;
            }
            if (typeof(IEntity) == typeof(Project))
            {
                dbSet.Remove(dbSet.ToList().First(s => (s as Project).ID_Project == (item as Project).ID_Project));
                context.SaveChanges();
                return;
            }
            if (typeof(IEntity) == typeof(User))
            {
                dbSet.Remove(dbSet.ToList().First(s => (s as User).ID_User == (item as User).ID_User));
                context.SaveChanges();
                return;
            }

        }

        public void Update(IEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

