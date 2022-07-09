using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AtaRK.Mobile.Services.Implementations
{
    public class BackgroundWorkerWrapper : BackgroundWorker, IBackgroundWorkerWrapper
    {
        public BackgroundWorkerWrapper(
            DoWorkEventHandler doWork,
            RunWorkerCompletedEventHandler completeHandler = null,
            bool workerSupportCancellation = false)
        {
            DoWork += doWork;

            if (completeHandler != null)
            {
                RunWorkerCompleted += completeHandler;
            }

            WorkerSupportsCancellation = workerSupportCancellation;
        }
    }
}
