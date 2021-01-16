using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

public class Loader : XRLoaderHelper
{
    public override bool Initialize()
    {
        CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(new List<XRSessionSubsystemDescriptor>(), nameof(ManagedSessionSubsystem));
        CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(new List<XRCameraSubsystemDescriptor>(), nameof(ManagedCameraSubsystem));
        return true;
    }

    public override bool Deinitialize()
    {
        DestroySubsystem<XRSessionSubsystem>();
        DestroySubsystem<XRCameraSubsystem>();
        return true;
    }
}
