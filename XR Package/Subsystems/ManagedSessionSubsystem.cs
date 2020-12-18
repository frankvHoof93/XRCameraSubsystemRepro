using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class ManagedSessionSubsystem : XRSessionSubsystem, ISubsystem
{
    private static readonly XRSessionSubsystemDescriptor.Cinfo cInfo = new XRSessionSubsystemDescriptor.Cinfo
    {
        id = nameof(ManagedSessionSubsystem),
        subsystemImplementationType = typeof(ManagedSessionSubsystem),
        supportsInstall = true,
        supportsMatchFrameRate = false
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void RegisterDescriptor()
    {
        XRSessionSubsystemDescriptor.RegisterDescriptor(cInfo);
    }


    protected override Provider CreateProvider()
    {
        return new SessionProvider();
    }

    public class SessionProvider : Provider
    {
    }
}
