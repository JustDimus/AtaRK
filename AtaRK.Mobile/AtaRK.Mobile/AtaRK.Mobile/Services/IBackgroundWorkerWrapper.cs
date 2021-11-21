using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AtaRK.Mobile.Services
{
    public interface IBackgroundWorkerWrapper : IDisposable
    {
        bool IsBusy { get; }
        void RunWorkerAsync();
        event DoWorkEventHandler DoWork;
        event RunWorkerCompletedEventHandler RunWorkerCompleted;
    }
}
