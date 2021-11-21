using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services
{
    public interface IApplicationProperties
    {
        bool Contains(object key);
        void AddOrUpdate(object key, object value);
        object Get(object key);
    }
}
