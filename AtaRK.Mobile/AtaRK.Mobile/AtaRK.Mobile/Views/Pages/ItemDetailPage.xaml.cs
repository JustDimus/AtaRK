using AtaRK.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AtaRK.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}