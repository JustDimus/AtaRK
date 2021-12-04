using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class DeviceInfoViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        private bool pageLoaded = false;

        public DeviceInfoViewModel()
        {

        }

        public void OnPageLoaded()
        {
            if (this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = true;
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = false;
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //Managed resources
                }

                //Unmanaged resources

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
