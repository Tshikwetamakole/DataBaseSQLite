using DataBaseSQLite.Model;
using DataBaseSQLite.ViewModel;
using DataBaseSQLite.ViewModel;
using static DataBaseSQLite.Model.ModelCustomers;

namespace DataBaseSQLite.View;

public partial class ViewPage : ContentPage
{
	public ViewPage()
	{
		InitializeComponent();

		BindingContext = new ViewModelSave(this);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		//MessageCenter to show alerts
		MessagingCenter.Subscribe<ViewModelSave, string>(this, "DisplayAlert", async (sender, messege) =>
		{
			//Show the alert message
			await DisplayAlert("Message", messege, "OK");
		});
    }

	protected override void OnDisappearing() 
	{ 
		base.OnDisappearing();
		MessagingCenter.Unsubscribe<ViewModelSave, string>(this, "DisplayAlert");
	}

	private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) 
	{
		var viewModel = (ViewModelSave)BindingContext;
		await viewModel.OnCustomerTapped((Customer)e.Item);
	}
}