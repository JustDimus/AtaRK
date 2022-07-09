using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public interface IPageViewModel
    {
        void OnPageLoaded();

        void OnPageUnloaded();
    }
}
