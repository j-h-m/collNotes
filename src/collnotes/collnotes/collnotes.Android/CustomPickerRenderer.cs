using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using collnotes;
using collnotes.Custom;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace collnotes
{
    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var customPicker = e.NewElement as CustomPicker;
                Control.TextSize *= (customPicker.TextSize * 0.01f);
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
            }
        }
    }
}