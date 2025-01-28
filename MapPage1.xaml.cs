#if IOS || MACCATALYST
using MapKit;
#endif
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Platform;
using Microsoft.Maui.Platform;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MauiMapTest;

public partial class MapPage1 : ContentPage
{
    private Location moDepartureLocation = new(53.5, -113.5);
    private Location moDestinationLocation = new(51.5, -114);
    private List<CustomPin> mcolPins = new();
    private bool mbLoaded = false;

    private Map map;

    public MapPage1()
    {
        InitializeComponent();

        Loaded += (sender, e) => mbLoaded = true;
        Load();
    }

    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    while (!mbLoaded)
    //    {
    //        await Task.Delay(128);
    //    }
    //    Load();
    //}



    private void Load()
    {
        map = new Map()
        {
            IsShowingUser = false,
            MapType = MapType.Street
        };
        Grid.SetRow(map, 1);
        map.MapClicked += map_Clicked;
        grdContent.Children.Add(map);

    }

    private void btnMode_Clicked(object sender, EventArgs e)
    {
        if (map.MapType == MapType.Street)
        {
            map.MapType = MapType.Hybrid;
        }
        else
        {
            map.MapType = MapType.Street;
        }
    }

    private void map_Clicked(object sender, MapClickedEventArgs e)
    {
        Navigation.PopAsync();
    }
}

