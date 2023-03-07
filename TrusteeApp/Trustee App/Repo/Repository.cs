using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using TrusteeApp.Domain.Dtos;

namespace TrusteeApp.Repo
{
    public static class Repository<T> where T : class
    {
        public static void Add(T entity, string _key)
        {
            RoutesController<T>.PostDbSet(entity, _key);
        }

        public static T Find(Predicate<T> filter, string _key)
        {
            var dbSet = RoutesController<T>.GetDbSet(_key);

            if (dbSet != null && dbSet.Count() > 0)
            {
                var matched = dbSet.Find(filter);

                return matched;
            }

            return null;
        }

        public static List<T> GetAll(string _key, Expression<Func<T, bool>> filter = null)
        {
            var dbSet = RoutesController<T>.GetDbSet(_key);

            if (dbSet != null && dbSet.Count() > 0)
            {
                IQueryable<T> query = dbSet.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return query.ToList();
            }

            return null;
        }
    }
}


//public static void Remove(string _key, string id)
//{
//    //dbSet.Remove(entity);
//    RoutesController<T>.DeleteDbSet(id, _key);
//}


//public static void Update(T obj, string _key, string id)
//{
//    RoutesController<T>.UpdateDbSet(obj, _key, id);
//}