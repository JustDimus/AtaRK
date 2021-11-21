using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Services.Implementations
{
    public class ApplicationProperties : IApplicationProperties
    {
        private Dictionary<object, object> propertyCollection = new Dictionary<object, object>();

        public void AddOrUpdate(object key, object value)
        {
            if(this.Contains(key))
            {
                this.propertyCollection[key] = value;
            }
            else
            {
                this.propertyCollection.Add(key, value);
            }
        }

        public bool Contains(object key)
        {
            return this.propertyCollection.ContainsKey(key);
        }

        public object Get(object key)
        {
            return this.Contains(key) ? this.propertyCollection[key] : null;
        }
    }
}
