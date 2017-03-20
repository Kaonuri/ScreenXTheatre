using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UnityVideoTexturePlugin {
	private const string PluginName = "UnityVideoTexture";

#if (UNITY_ANDROID && !UNITY_EDITOR)
	[DllImport(PluginName)]
	private static extern System.IntPtr RenderInitFunc();
	
	[DllImport(PluginName)]
	private static extern System.IntPtr RenderUpdateFunc();

	[DllImport(PluginName)]
	public static extern void LoadPlugin();

	[DllImport(PluginName)]
	public static extern System.IntPtr AddVideoTexture(System.IntPtr texturePtr, int width, int height, bool generateMipmap);

	[DllImport(PluginName)]
	public static extern void RemoveVideoTexture(System.IntPtr texturePtr);

	[DllImport(PluginName)]
	public static extern void LoadVideo(string uri, bool isStreamingAsset, 
                                        string objectToReceiveCallback,
                                        string callbackPrepared, 
                                        string callbackFirstFrameAvailable,
                                        string callbackVideoSizeChanged, 
                                        string callbackBufferingStart,
                                        string callbackBufferingEnd, 
                                        string callbackPlaybackCompleted,
                                        string callbackErrorOccurred);

	[DllImport(PluginName)]
	public static extern void Play();

	[DllImport(PluginName)]
	public static extern void Pause();

	[DllImport(PluginName)]
	public static extern void Resume();

	[DllImport(PluginName)]
	public static extern void Stop();

	[DllImport(PluginName)]
	public static extern void Reset();

	[DllImport(PluginName)]
	public static extern int GetWidth();

	[DllImport(PluginName)]
	public static extern int GetHeight();

	[DllImport(PluginName)]
	public static extern void UnloadPlugin();

#else
	public static void LoadPlugin() {}
	public static System.IntPtr AddVideoTexture(System.IntPtr texturePtr, int width, int height, bool generateMipmap) { return System.IntPtr.Zero; }
	public static void RemoveVideoTexture(System.IntPtr texturePtr) {}
	public static void LoadVideo(string uri, bool isStreamingAsset, 
                                 string objectToReceiveCallback,
                                 string callbackPrepared, 
                                 string callbackFirstFrameAvailable,
                                 string callbackVideoSizeChanged, 
                                 string callbackBufferingStart,
                                 string callbackBufferingEnd, 
                                 string callbackPlaybackCompleted,
                                 string callbackErrorOccurred) {}
	public static void Play() {}
	public static void Pause() {}
	public static void Resume() {}
	public static void Stop() {}
	public static void Reset() {}
	public static int GetWidth() { return 0; }
	public static int GetHeight() { return 0; }
	public static void UnloadPlugin() {}
#endif

	public static void IssueRenderInit() {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		GL.IssuePluginEvent(RenderInitFunc(), 0);
#endif
	}

	public static void IssueRenderUpdate() {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		GL.IssuePluginEvent(RenderUpdateFunc(), 0);
#endif
	}
}
