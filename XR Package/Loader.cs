using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

public class Loader : XRLoaderHelper
{
    public override bool Initialize()
    {
        CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(new List<XRCameraSubsystemDescriptor>(), nameof(ManagedCameraSubsystem));
        return true;
    }

    public override bool Deinitialize()
    {
        DestroySubsystem<XRCameraSubsystem>();
        return true;
    }
}
