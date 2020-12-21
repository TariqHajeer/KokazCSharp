using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.DAL.Classes
{
    public class GenericRepository<T> where T :class
    {
        KokazContext Context;
        DbSet<T> Entities;
        public GenericRepository(KokazContext context)
        {
            this.Context = context;
            this.Entities = this.Context.Set<T>();
        }
        public  bool Exisist (int id)
        {
            return this.Entities.Find(id) != null;
        }
        
    }
}
