using NetCoreApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Model
{
    public interface IAggregateRoot
    {

    }
    [Serializable]
    public abstract class BaseModel<TKey>:IAggregateRoot
    {
        public abstract TKey Id { get; set; }

       // public virtual string UniqueId { get; set; } = CommonHelper.NewMongodbId().ToString();
    }
}
