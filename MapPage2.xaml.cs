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

public partial class MapPage2 : ContentPage
{
    private Location moDepartureLocation = new(53.5, -113.5);
    private Location moDestinationLocation = new(51.5, -114);
    private List<CustomPin> mcolPins = new();
    private bool mbLoaded = false;

    private Map map;

    public MapPage2()
    {
        InitializeComponent();

        Console.WriteLine("****** MapPage2 Constructor");

        SetPinColor();
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

    private void SetPinColor()
    {
        try
        {
#if IOS || MACCATALYST

            Microsoft.Maui.Maps.Handlers.MapHandler.Mapper.AppendToMapping("MapPinColor", (handler, view) =>
            {
                var loMapView = handler.PlatformView;
                Console.WriteLine($"loMapView={loMapView}, view={view}");
                loMapView.GetViewForAnnotation += GetViewForAnnotation;
            });

#elif ANDROID

            Microsoft.Maui.Maps.Handlers.MapPinHandler.Mapper.AppendToMapping("PinColor", (handler, view) =>
            {
                var loPinColor = ((CustomPin)view).PinColor;
                var loPlatformColor = loPinColor.ToPlatform();
                Console.WriteLine($"PinColor={loPinColor}, PlatformColor={loPlatformColor}, view={view}");
			    var loMarker = handler.PlatformView;
                var loPin = (CustomPin)view;
                float hue = loPlatformColor.GetHue() % 360.0f;
                Console.WriteLine($"PinColor={loPin.PinColor}, Hue={hue}");
                loMarker.SetIcon(Android.Gms.Maps.Model.BitmapDescriptorFactory.DefaultMarker(hue));
            });
#endif
        }
        catch (Exception ex)
        {
            Console.WriteLine($"**** Set Pin Color ex={ex.Message}");
        }
    }

#if IOS || MACCATALYST
    private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
    {
        var loMarkerView = new MKMarkerAnnotationView(annotation, "");
        var loCoordinate = annotation.Coordinate;
        foreach (CustomPin loPin in map.Pins)
        {
            if (loPin.Location.Latitude == loCoordinate.Latitude && loPin.Location.Longitude == loCoordinate.Longitude)
            {
                loMarkerView.MarkerTintColor = loPin.PinColor.ToPlatform();
            }
        }
        return loMarkerView;
    }
#endif

    private void Load()
    {
        try
        {
            Console.WriteLine("****** MapPage2 Load");
            map = new Map()
            {
                IsShowingUser = false,
                MapType = MapType.Street
            };
            Grid.SetRow(map, 1);
            map.MapClicked += map_Clicked;
            grdContent.Children.Add(map);

            //--- add pins

            var loDeparturePin = new CustomPin
            {
                Label = "Departure",
                Location = moDepartureLocation,
                PinColor = Colors.Green
            };
            map.Pins.Add(loDeparturePin);
            var loDestinationPin = new CustomPin
            {
                Label = "Destination",
                Location = moDestinationLocation,
                PinColor = Colors.Red
            };
            map.Pins.Add(loDestinationPin);


            Location loCenterLocation = new(52.5, -113.75);
            var loCenterPin = new CustomPin
            {
                Label = "Center",
                Location = loCenterLocation,
                PinColor = Colors.LightBlue
            };
            map.Pins.Add(loCenterPin);

            //--- add route line

            Polyline moRoutePolyline = new Polyline
            {
                StrokeColor = Color.FromRgba(1, 253, 207, 155),
#if ANDROID
                StrokeWidth = 20,
#else
            StrokeWidth = 5,
#endif
                Geopath =
            {
                new Location(53.5, -113.5),
                new Location(52, -114),
                new Location(51, -114.2)
            }
            };
            map.MapElements.Add(moRoutePolyline);

            //--- add weather polygon

            Polygon moWeatherPolygon = new Polygon
            {
                FillColor = Color.FromRgba(255, 0, 0, 75),
                StrokeColor = Colors.Red,
#if ANDROID
                StrokeWidth = 6,
#else
            StrokeWidth = 2,
#endif
                Geopath =
            {
                new Location(53.5, -113.5),
                new Location(53.4, -114),
                new Location(52.9, -113.5),
                new Location(53.5, -112.5)
            }
            };
            map.MapElements.Add(moWeatherPolygon);

            var loMapSpan = new MapSpan(loCenterLocation, 2.5, 2.5);
            map.MoveToRegion(loMapSpan);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"**** Map2 Load ex={ex.Message}");
        }




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

