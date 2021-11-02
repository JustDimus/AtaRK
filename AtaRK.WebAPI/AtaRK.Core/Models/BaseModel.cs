using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models
{
    public abstract class BaseModel : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
