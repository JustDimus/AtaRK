using AtaRK.Mobile.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Views.Pages
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            this.Appearing += BasePage_Appeared;
            this.Disappearing += BasePage_Disappeared;
        }

        private void BasePage_Appeared(object sender, EventArgs e)
        {
            if (BindingContext is IPageViewModel viewModel)
            {
                viewModel.OnPageLoaded();
            }
        }

        private void BasePage_Disappeared(object sender, EventArgs e)
        {
            if (BindingContext is IPageViewModel viewModel)
            {
                viewModel.OnPageUnloaded();
            }
        }
    }
}
