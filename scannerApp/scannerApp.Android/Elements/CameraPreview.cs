using System;
using System.Collections.Generic;
using System.Linq;

using Android.Hardware.Camera2;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;

namespace scannerApp.Android.Elements
{

    public enum CameraState
    {
        Opened,
        Disconected,
        Configurated
    }

    public class CameraCallback : CameraDevice.StateCallback
    {
        private Action<CameraDevice, CameraState> _callback = null;

        public CameraCallback(Action<CameraDevice, CameraState> callback)
        {
            _callback = callback;
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            _callback(camera, CameraState.Disconected);
        }

        public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
        {
            throw new Exception("Camera init error");
        }

        public override void OnOpened(CameraDevice camera)
        {
            _callback(camera, CameraState.Opened);
        }
    }

    public class CamaraConfigurationCallback : CameraCaptureSession.StateCallback
    {
        private Action<CameraDevice, CameraState> _callback = null;
        private CaptureRequest _captureRequest = null;
        public CamaraConfigurationCallback(Action<CameraDevice, CameraState> callback, CaptureRequest captureRequest)
        {
            _callback = callback;
            _captureRequest = captureRequest;
        }

        public override void OnConfigured(CameraCaptureSession session)
        {
            session.SetRepeatingRequest(_captureRequest, null, null);
            _callback(session.Device, CameraState.Configurated);
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            throw new Exception("Camera configuarion error");
        }
    }

    public class CameraPreview
    {
        //сервис андройда
        CameraManager CameraService = null;

        public TextureView Texture { get; private set; }

        public CameraPreview(TextureView texture)
        {
            //получаем сервис 
            CameraService = Application.Context.GetSystemService(Context.CameraService) as CameraManager;
            //если не нашли
            if (CameraService == null)
                throw new NullReferenceException("cameraService not found");

            Texture = texture;
        }
        /// <summary>
        /// Получить Id нужной камеры
        /// </summary>
        /// <returns></returns>
        private string _getCameraId()
        {

            //получаем список камер
            var cameraIdList = CameraService.GetCameraIdList();
            if (cameraIdList.Length == 0)
                throw new ArgumentOutOfRangeException("there are no cameras on this device");
            //получаем характеристики камер
            var cameraCharacteristics = cameraIdList.Select(x => new { id = x, info = CameraService.GetCameraCharacteristics(x) });
            //Ищем заднюю
            var camera = cameraCharacteristics.FirstOrDefault(x => (int)x.info.Get(CameraCharacteristics.LensFacing) == (int)LensFacing.Back);
            //Если такой нет, то берем первую
            if (camera == null)
                camera = cameraCharacteristics.First();
            return camera.id;
        }


        /// <summary>
        /// Обработка событий от камеры
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="state"></param>
        private void _cameraStateChange(CameraDevice camera, CameraState state)
        {
            switch (state)
            {
                case CameraState.Opened:
                    var surfaceTexture = Texture.SurfaceTexture;
                    surfaceTexture.SetDefaultBufferSize(1920, 1080);
                    var surface = new Surface(surfaceTexture);

                    //переод. событие для обновл. картинки 
                    var captureRequestBuilder = camera.CreateCaptureRequest(CameraTemplate.Preview);
                    captureRequestBuilder.AddTarget(surface);

                    camera.CreateCaptureSession(new List<Surface> { surface }, new CamaraConfigurationCallback(_cameraStateChange, captureRequestBuilder.Build()), null);
                    break;
                case CameraState.Disconected:
                    camera.Close();
                    break;
                case CameraState.Configurated:
                    break;
            }
        }

        /// <summary>
        /// Инициализация камеры
        /// </summary>
        private void _initCamera()
        {
            //получаем id нужной камеры
            var cameraId = _getCameraId();
            //создаем обработчик запуска камеры с колбеком
            var cameraCallBack = new CameraCallback(_cameraStateChange);
            //запускаем камеру
            CameraService.OpenCamera(cameraId, cameraCallBack, null);
        }

    }
}