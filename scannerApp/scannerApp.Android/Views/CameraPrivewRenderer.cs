using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(scannerApp.Views.CameraPreview), typeof(scannerApp.Android.Views.CameraPreviewRenderer))]
namespace scannerApp.Android.Views
{
    public class CameraPreviewRenderer : ViewRenderer<scannerApp.Views.CameraPreview, TextureView>
    {

        public CameraPreviewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<scannerApp.Views.CameraPreview> args)
        {
            base.OnElementChanged(args);
            if (Control == null)
            {
                // создаем и настраиваем элемент
                TextureView textureView = new TextureView(Context);
                new Elements.CameraPreview(textureView);

                // устанавливаем элемент для класса 
                SetNativeControl(textureView);
            }
        }
    }
}