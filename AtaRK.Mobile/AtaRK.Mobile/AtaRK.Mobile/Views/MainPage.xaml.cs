using AtaRK.Mobile.ViewModels;
using AtaRK.Mobile.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AtaRK.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : NavigationPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (this.BindingContext is MainViewModel mainViewModel)
            {
                mainViewModel.Navigation = this.Navigation;
                mainViewModel.Start();
            }
        }
    }
}