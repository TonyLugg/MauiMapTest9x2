


namespace MauiMapTest;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
    }

    private void btnMap1_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MapPage1());
    }

    private void btnMap2_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MapPage2());
    }

    private void btnMap3_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MapPage3());
    }

}

