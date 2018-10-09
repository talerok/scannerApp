using scannerApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace scannerApp
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Content = new StackLayout
            {
                Children =
                {
                    new CameraPreview{
                         HorizontalOptions = LayoutOptions.FillAndExpand,
                         VerticalOptions = LayoutOptions.FillAndExpand
                    }
                }
            };
        }

    }
}
