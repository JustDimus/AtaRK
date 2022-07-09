using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Navigation
{
    public interface INavigationService
    {
        void MoveToPage(Pages page);

        void MoveBack();

        IObservable<Pages> CurrentPageKey { get; }
    }
}
