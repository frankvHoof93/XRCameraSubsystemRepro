using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

[RequireComponent(typeof(Camera))]
public class ARCamera : MonoBehaviour
{
    [SerializeField]
    private Vector2Int textureSize = new Vector2Int(1280, 720);

    private List<XRTextureDescriptor> textureDescriptors = new List<XRTextureDescriptor>();

    private RenderTexture mainTexture;
    private Texture2D tex2D;
    private IntPtr texturePtr;

    private void RegisterCamera()
    {
        XRCameraSubsystem subsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRCameraSubsystem>();
        (subsystem as ManagedCameraSubsystem).RegisterCamera(this);
    }

    void Start()
    {
        mainTexture = new RenderTexture(textureSize.x, textureSize.y, 16);
        tex2D = new Texture2D(mainTexture.width, mainTexture.height, Convert(mainTexture.format), false, false);
        texturePtr = mainTexture.GetNativeTexturePtr();
        GetComponent<Camera>().targetTexture = mainTexture;
        RegisterCamera();
    }

    internal List<XRTextureDescriptor> GetTextureDescriptors()
    {
        textureDescriptors.Clear();
        textureDescriptors.Add(GetMainTex());
        return textureDescriptors;
    }

    private XRTextureDescriptor GetMainTex()
    {
        Graphics.CopyTexture(mainTexture, tex2D);
        return GetTextureDescriptor(tex2D.GetNativeTexturePtr(), new Vector3Int(tex2D.width, tex2D.height, mainTexture.depth), Shader.PropertyToID("_MainTex"), Convert(mainTexture.format));
    }



    internal static XRTextureDescriptor GetTextureDescriptor(IntPtr tex, Vector3Int size, int propId, TextureFormat format, int mipmapCount = 0, TextureDimension dimension = TextureDimension.Tex2D)
    {
        XRTextureDescriptor descriptor = new XRTextureDescriptor();
        SetField(ref descriptor, "m_NativeTexture", tex);
        SetField(ref descriptor, "m_Width", size.x);
        SetField(ref descriptor, "m_Height", size.y);
        SetField(ref descriptor, "m_Depth", dimension == TextureDimension.Tex3D ? size.z : 1);
        SetField(ref descriptor, "m_PropertyNameId", propId);
        SetField(ref descriptor, "m_MipmapCount", mipmapCount);
        SetField(ref descriptor, "m_Format", format);
        SetField(ref descriptor, "m_Dimension", dimension);
        return descriptor;
    }

    private static readonly Dictionary<string, FieldInfo> descriptorFields = typeof(XRTextureDescriptor).GetFields(BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField).ToDictionary(f => f.Name);

    private static void SetField(ref XRTextureDescriptor obj, string fieldName, object value)
    {
        object boxedObj = obj;
        descriptorFields[fieldName].SetValue(boxedObj, value);
        obj = (XRTextureDescriptor)boxedObj;
    }

    public static TextureFormat Convert(RenderTextureFormat format)
    {
        switch (format)
        {
            case RenderTextureFormat.ARGB32:
                return TextureFormat.ARGB32;
            case RenderTextureFormat.ARGBHalf:
            case RenderTextureFormat.Depth:
            case RenderTextureFormat.Shadowmap:
            case RenderTextureFormat.ARGB1555:
            case RenderTextureFormat.ARGB2101010:
            case RenderTextureFormat.ARGB64:
            case RenderTextureFormat.ARGBFloat:
            case RenderTextureFormat.ARGBInt:
            case RenderTextureFormat.RGInt:
            case RenderTextureFormat.RInt:
            case RenderTextureFormat.RGB111110Float:
            case RenderTextureFormat.RG32:
            case RenderTextureFormat.BGRA10101010_XR:
            case RenderTextureFormat.BGR101010_XR:
            default:
                throw new ArgumentOutOfRangeException($"Cannot Convert {format}");
            case RenderTextureFormat.RGBAUShort:
                return TextureFormat.RGBAHalf;
            case RenderTextureFormat.RGB565:
                return TextureFormat.RGB565;
            case RenderTextureFormat.ARGB4444:
                return TextureFormat.ARGB4444;
            case RenderTextureFormat.Default:
            case RenderTextureFormat.DefaultHDR:
                return default;
            case RenderTextureFormat.RGFloat:
                return TextureFormat.RGFloat;
            case RenderTextureFormat.RGHalf:
                return TextureFormat.RGHalf;
            case RenderTextureFormat.RFloat:
                return TextureFormat.RFloat;
            case RenderTextureFormat.RHalf:
                return TextureFormat.RHalf;
            case RenderTextureFormat.R8:
                return TextureFormat.R8;
            case RenderTextureFormat.BGRA32:
                return TextureFormat.BGRA32;
            case RenderTextureFormat.RG16:
                return TextureFormat.RG16;
            case RenderTextureFormat.R16:
                return TextureFormat.R16;
        }
    }
}
