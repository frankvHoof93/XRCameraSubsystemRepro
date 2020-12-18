using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class ManagedCameraSubsystem : XRCameraSubsystem
{
    private static readonly XRCameraSubsystemCinfo DEFAULT_INFO = new XRCameraSubsystemCinfo()
    {
        id = nameof(ManagedCameraSubsystem),
        implementationType = typeof(ManagedCameraSubsystem),
        supportsAverageBrightness = false,
        supportsAverageColorTemperature = true,
        supportsColorCorrection = false,
        supportsDisplayMatrix = true,
        supportsProjectionMatrix = true,
        supportsTimestamp = true,
        supportsCameraConfigurations = true,
        supportsCameraImage = false,
        supportsAverageIntensityInLumens = true,
        supportsFocusModes = false
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void RegisterDescriptor()
    {
        Register(DEFAULT_INFO);
    }

    protected override Provider CreateProvider()
    {
        return new CameraProvider();
    }

    public void RegisterCamera(ARCamera cam)
    {
        CameraProvider prov = (CameraProvider)provider;
        prov.RegisterCamera(cam);
    }

    public class CameraProvider : Provider
    {
        public CameraProvider()
        {
            mat = CreateCameraMaterial("Unlit/ARCoreBackground");
        }

        public override Material cameraMaterial => mat;

        private ARCamera camera;
        private Material mat;

        public void RegisterCamera(ARCamera cam)
        {
            camera = cam;
        }

        public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
        {
            cameraFrame = default;
            return camera != null;
        }

        public override NativeArray<XRTextureDescriptor> GetTextureDescriptors(XRTextureDescriptor defaultDescriptor, Allocator allocator)
        {
            List<XRTextureDescriptor> descriptors = camera.GetTextureDescriptors();
            return new NativeArray<XRTextureDescriptor>(descriptors.ToArray(), allocator);
        }
    }
}
