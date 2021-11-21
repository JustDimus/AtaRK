using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ReplaySubject<Pages> _currentPageKey = new ReplaySubject<Pages>(1);

        public NavigationService()
        {
            this._currentPageKey.OnNext(Pages.Intro);
        }

        public IObservable<Pages> CurrentPageKey => this._currentPageKey;

        public void MoveBack()
        {
            throw new NotImplementedException();
        }

        public void MoveToPage(Pages page)
        {
            this._currentPageKey.OnNext(page);
        }
    }
}
