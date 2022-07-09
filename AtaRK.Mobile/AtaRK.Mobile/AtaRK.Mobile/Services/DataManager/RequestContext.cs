using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.DataManager
{
    public class RequestContext
    {
        public bool IsSuccessful { get; set; }

        public static implicit operator bool(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.IsSuccessful;
        }


        public static RequestContext<TEntity> FromStatus<TEntity>(bool status)
        {
            return new RequestContext<TEntity>()
            {
                IsSuccessful = status
            };
        }
    }

    public class RequestContext<TEntity> : RequestContext
    {
        public TEntity Result { get; set; }
    }
}
