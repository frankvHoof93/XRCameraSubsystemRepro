using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class ManagedSessionSubsystem : XRSessionSubsystem, ISubsystem
{
    private static readonly XRSessionSubsystemDescriptor.Cinfo cInfo = new XRSessionSubsystemDescriptor.Cinfo
    {
        id = nameof(ManagedSessionSubsystem),
#if UNITY_2020_2_OR_NEWER
        providerType = typeof(SessionProvider),
        subsystemTypeOverride = typeof(ManagedSessionSubsystem),
#else
        subsystemImplementationType = typeof(ManagedSessionSubsystem),
#endif
        supportsInstall = true,
        supportsMatchFrameRate = false
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void RegisterDescriptor()
    {
        XRSessionSubsystemDescriptor.RegisterDescriptor(cInfo);
    }

#if !UNITY_2020_2_OR_NEWER
    protected override Provider CreateProvider()
    {
        return new SessionProvider();
    }
#endif

    public class SessionProvider : Provider
    {
    }
}
