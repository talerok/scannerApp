using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(scannerApp.Views.CameraPreview), typeof(scannerApp.AndroidApp.Views.CameraPreviewRenderer))]
namespace scannerApp.AndroidApp.Views
{
    public class PreviewSurfaceTextureListner : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        private Action _onInitAction; 

        public PreviewSurfaceTextureListner(Action onInitAction)
        {
            _onInitAction = onInitAction;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _onInitAction();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }
    }


    public class CameraPreviewRenderer : ViewRenderer<scannerApp.Views.CameraPreview, TextureView>, TextureView.ISurfaceTextureListener
    {
        private TextureView _textureView = null;

        #region ISurfaceTextureListener
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height) // ждем ини
        {
            new Elements.CameraPreview(_textureView);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }
        #endregion

        public CameraPreviewRenderer(Context context) : base(context)
        {

        }


        protected override void OnElementChanged(ElementChangedEventArgs<scannerApp.Views.CameraPreview> args)
        {
            base.OnElementChanged(args);
            if (Control == null)
            {
                // создаем и настраиваем элемент
                _textureView = new TextureView(Context);
                _textureView.Visibility = ViewStates.Visible;
                _textureView.SetBackgroundColor(Android.Graphics.Color.Black);
                //Ждем инициализацию
                _textureView.SurfaceTextureListener = this;
                // устанавливаем элемент для класса 
                SetNativeControl(_textureView);
            }
        }
    }
}