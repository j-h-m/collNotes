using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using collnotes;
using collnotes.Custom;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace collnotes
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var customPicker = e.NewElement as CustomPicker;


                // get Bindable properties
                UIColor placeholderColor = GetUIColor(customPicker.PlaceholderColor);
                UIColor textColor = GetUIColor(customPicker.TextColor.ToString());
                float textSize = (float)customPicker.TextSize;


                // create font decsriptor
                var label = new UILabel();
                var fontDescriptor = label.Font.FontDescriptor;

                // adjusting font size
                var newDescriptor = fontDescriptor.CreateWithAttributes(new UIFontAttributes()
                {
                    Size = textSize
                });
                UIFont font = UIFont.FromDescriptor(newDescriptor, 0);


                // set attributes
                //var placeholderAttributes = new NSAttributedString(customPicker.Title, new UIStringAttributes()
                //{ ForegroundColor = placeholderColor, Font = font });

                //Control.AttributedPlaceholder = placeholderAttributes;


                //var textAttributes = new NSAttributedString(customPicker.Title, new UIStringAttributes()
                //{ ForegroundColor = textColor, Font = font });

                //Control.AttributedText = textAttributes;

            }
        }

        private UIColor GetUIColor(string color)
        {
            return UIColor.FromRGB(GetRed(color), GetGreen(color), GetBlue(color));
        }

        private float GetRed(string color)
        {
            Color c = Color.FromHex(color);
            return (float)c.R;
        }

        private float GetGreen(string color)
        {
            Color c = Color.FromHex(color);
            return (float)c.G;
        }

        private float GetBlue(string color)
        {
            Color c = Color.FromHex(color);
            return (float)c.B;
        }
    }
}