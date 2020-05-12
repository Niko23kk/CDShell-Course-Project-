using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Курсовой.Model
{
    public interface IRepository<IEntity> where IEntity : class
    {
        void Create(IEntity item);
        void CreateTexture(Elements item,string front,string side);
        IEntity FindById(int id);
        void AddCollection(IEnumerable<IEntity> entities);
        void RemoveCollection(IEnumerable<IEntity> entities);
        void UpdateAvatar(User user, string Image);
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

        public void CreateTexture(Elements item,string front,string side)
        {
            context.Database.ExecuteSqlCommand($"insert into Elements (TitleEng,TypeEng,TitleRu,TypeRu,Price,[Front view],[Side view],Size)"+
                $@" values('{item.TitleEng}', 'User','{item.TitleEng}', 'Пользователь',{item.Price}, (SELECT * FROM OPENROWSET(BULK N'{front}', SINGLE_BLOB)"+
                $@"as [Front view]), (SELECT * FROM OPENROWSET(BULK N'{side}', SINGLE_BLOB) as [Side view]),{item.Size})");
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

        public void UpdateAvatar(User user, string Image)
        {
            context.Database.ExecuteSqlCommand($"update [User] set photo=(SELECT * FROM OPENROWSET(BULK N'{Image}', SINGLE_BLOB) as [Photo]) where [ID User]={user.ID_User}");
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
    }
}

