using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.DAL.Infrastructure.Interfaces
{
    public interface IIndexRepository<T>:IRepository<T> where T: class,IIndex
    {
        Task<IEnumerable<IndexEntity>> GetLiteList();
    }
}
