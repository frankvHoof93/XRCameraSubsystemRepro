using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;

class PackageData : IXRPackage
{
    class LoaderMetadata : IXRLoaderMetadata
    {
        public string loaderName { get; set; }
        public string loaderType { get; set; }
        public List<BuildTargetGroup> supportedBuildTargets { get; set; }
    }

    class PackageMetadata : IXRPackageMetadata
    {
        public string packageName { get; set; }
        public string packageId { get; set; }
        public string settingsType { get; set; }
        public List<IXRLoaderMetadata> loaderMetadata { get; set; }
    }

    static readonly IXRPackageMetadata data = new PackageMetadata
    {
        packageName = "ManagedSubsystemTest",
        packageId = "com.unity.testmanagesubsystem",
        settingsType = typeof(Loader).FullName,
        loaderMetadata = new List<IXRLoaderMetadata> {
                new LoaderMetadata {
                    loaderName = "XRTest",
                    loaderType = typeof(Loader).FullName,
                    supportedBuildTargets = new List<BuildTargetGroup> {
                        BuildTargetGroup.Standalone
                    }
                }
            }
    };

    public IXRPackageMetadata metadata => data;

    public bool PopulateNewSettingsInstance(ScriptableObject obj)
    {
        return true;
    }
}
