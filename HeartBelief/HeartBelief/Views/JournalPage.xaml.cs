using HeartBelief.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeartBelief.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JournalPage : ContentPage
    {
        JournalViewModel viewModel;
        public JournalPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new JournalViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Entries.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await viewModel.AddNewEntry();
        }
    }
}
