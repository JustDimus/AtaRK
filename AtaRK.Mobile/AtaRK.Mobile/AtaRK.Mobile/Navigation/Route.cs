using AtaRK.Mobile.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Navigation
{
    public class Route
    {
        private string pageName = null;

        public Pages Page { get; set; }

        public string PageName
        {
            get
            {
                if (this.pageName == null)
                {
                    this.pageName = Enum.GetName(typeof(Pages), this.Page);
                }

                return this.pageName;
            }
        }

        public BasePage PageType { get; set; }
    }
}
