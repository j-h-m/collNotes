using System;
using Xamarin.Forms;

namespace collnotes.Custom
{
    /// <summary>
    /// Custom picker to allow Placeholdercolor to be modified.
    /// </summary>
    public class CustomPicker : Picker
    {
        public static BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(CustomPicker), DefaultColor, BindingMode.TwoWay);

        public string PlaceholderColor
        {
            get { return (string)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public static string DefaultColor => "#CCCCCC";
    }
}