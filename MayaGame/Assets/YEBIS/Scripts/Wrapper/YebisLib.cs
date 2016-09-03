//// define
#if UNITY_IOS && !UNITY_EDITOR
#define YEBIS_LIB_GLES
#define YEBIS_LIB_GLES_IPHONE
#elif UNITY_ANDROID && !UNITY_EDITOR
#define YEBIS_LIB_GLES
#define YEBIS_LIB_GLES_ANDROID
#else
#define YEBIS_LIB_GLCORE
#endif

//// using
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace YEBIS
{

    public class YebisLib : MonoBehaviour
    {

#if YEBIS_LIB_GLES_IPHONE
    const string dll        = "__Internal";
    const string dll_ES2    = "__Internal";
#elif YEBIS_LIB_GLES_ANDROID
    // Android ES2/ES3
    const string dll        = "YebisPlugin_GLES3";
    const string dll_ES2    = "YebisPlugin_GLES2";
#elif YEBIS_LIB_GLCORE
        // GL Core
        const string dll = "YebisPlugin_GLCore";
#endif


        //// GetSetupPostEffectEventFunc
        [DllImport(dll)]
        private static extern System.IntPtr _GetSetupPostEffectEventFunc();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern System.IntPtr _GetSetupPostEffectEventFunc_ES2();
#endif

        public static void CallSetupPostEffectEvent()
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            GL.IssuePluginEvent(_GetSetupPostEffectEventFunc_ES2(), 1);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                GL.IssuePluginEvent(_GetSetupPostEffectEventFunc(), 1);
            }
        }


        //// GetBeginPostEffectEventFunc
        [DllImport(dll)]
        private static extern System.IntPtr _GetBeginPostEffectEventFunc();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern System.IntPtr _GetBeginPostEffectEventFunc_ES2();
#endif

        public static void CallBeginPostEffectEvent()
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            GL.IssuePluginEvent(_GetBeginPostEffectEventFunc_ES2(), 1);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                GL.IssuePluginEvent(_GetBeginPostEffectEventFunc(), 1);
            }
        }


        //// GetEndPostEffectEventFunc
        [DllImport(dll)]
        private static extern System.IntPtr _GetEndPostEffectEventFunc();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern System.IntPtr _GetEndPostEffectEventFunc_ES2();
#endif

        public static void CallEndPostEffectEvent()
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            GL.IssuePluginEvent(_GetEndPostEffectEventFunc_ES2(), 1);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                GL.IssuePluginEvent(_GetEndPostEffectEventFunc(), 1);
            }
        }


        //// SetSourceBuffer
        [DllImport(dll)]
        private static extern void _SetSourceBuffer(System.IntPtr bufferPtr, int w, int h);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetSourceBuffer_ES2(System.IntPtr bufferPtr, int w, int h);
#endif

        public static void SetSourceBuffer(System.IntPtr bufferPtr, int w, int h)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetSourceBuffer_ES2(bufferPtr, w, h);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetSourceBuffer(bufferPtr, w, h);
            }
        }

        //// SetSourceBuffer
        [DllImport(dll)]
        private static extern void _SetDestinationBuffer(System.IntPtr bufferPtr, int w, int h);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetDestinationBuffer_ES2(System.IntPtr bufferPtr, int w, int h);
#endif

        public static void SetDestinationBuffer(System.IntPtr bufferPtr, int w, int h)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetDestinationBuffer_ES2(bufferPtr, w, h);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetDestinationBuffer(bufferPtr, w, h);
            }
        }


        //// Initialize
        [DllImport(dll)]
        private static extern void _Initialize(
            int iWindowWidth,
            int iWindowHeight,
            int iGlareQuality = 0,
            int iDepthOfFieldQuality = 0,
            bool bHDR = true,
            int iApertureShape = -1);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _Initialize_ES2(
        int iWindowWidth,
        int iWindowHeight,
        int iGlareQuality = 0,
        int iDepthOfFieldQuality = 0,
        bool bHDR = false,
        int iApertureShape = -1);
#endif

        public static void Initialize(
            int iWindowWidth,
            int iWindowHeight,
            int iGlareQuality = 0,
            int iDepthOfFieldQuality = 0,
            bool bHDR = true,
            int iApertureShape = -1)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _Initialize_ES2(iWindowWidth, iWindowHeight, iGlareQuality, iDepthOfFieldQuality, bHDR, iApertureShape);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _Initialize(iWindowWidth, iWindowHeight, iGlareQuality, iDepthOfFieldQuality, bHDR, iApertureShape);
            }
        }


        //// GetDepthOfFieldFactorScaleOffset
        [DllImport(dll)]
        private static extern void _GetDepthOfFieldFactorScaleOffset(float[] pDest);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _GetDepthOfFieldFactorScaleOffset_ES2(float[] pDest);
#endif

        public static Vector4 GetDepthOfFieldFactorScaleOffset()
        {
            float[] pDest = new float[4];
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _GetDepthOfFieldFactorScaleOffset_ES2(pDest);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _GetDepthOfFieldFactorScaleOffset(pDest);
            }
            return new Vector4(pDest[0], pDest[1], pDest[2], pDest[3]);
        }



        //// GetFocusDistance
        [DllImport(dll)]
        private static extern float _GetFocusDistance();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern float _GetFocusDistance_ES2();
#endif

        public static float GetFocusDistance()
        {
            float ret = 0.0f;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _GetFocusDistance_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _GetFocusDistance();
            }
            return ret;
        }

        //// SetRenderScenePreviousViewMatrix
        [DllImport(dll)]
        private static extern void _SetRenderScenePreviousViewMatrix(float[] pMatrix);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderScenePreviousViewMatrix_ES2(float[] pMatrix);
#endif

        public static void SetRenderScenePreviousViewMatrix(float[] pMatrix = null)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderScenePreviousViewMatrix_ES2(pMatrix);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderScenePreviousViewMatrix(pMatrix);
            }
        }

        //// SetRenderSceneViewMatrix
        [DllImport(dll)]
        private static extern void _SetRenderSceneViewMatrix(float[] pMatrix);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderSceneViewMatrix_ES2(float[] pMatrix);
#endif

        public static void SetRenderSceneViewMatrix(float[] pMatrix = null)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderSceneViewMatrix_ES2(pMatrix);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderSceneViewMatrix(pMatrix);
            }
        }

        //// SetRenderScenePreviousPerspective
        [DllImport(dll)]
        private static extern void _SetRenderScenePreviousPerspective(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderScenePreviousPerspective_ES2(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f);
#endif

        public static void SetRenderScenePreviousPerspective(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderScenePreviousPerspective_ES2(fNear, fFar, fVerticalFov);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderScenePreviousPerspective(fNear, fFar, fVerticalFov);
            }
        }

        //// SetRenderScenePerspective
        [DllImport(dll)]
        private static extern void _SetRenderScenePerspective(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderScenePerspective_ES2(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f);
#endif

        public static void SetRenderScenePerspective(float fNear = 0.0f, float fFar = 0.0f, float fVerticalFov = 0.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderScenePerspective_ES2(fNear, fFar, fVerticalFov);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderScenePerspective(fNear, fFar, fVerticalFov);
            }
        }

        //// SetRenderSceneElapsedTime
        [DllImport(dll)]
        private static extern void _SetRenderSceneElapsedTime(float fElapsedTimeInSeconds = 1.0f / 60.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderSceneElapsedTime_ES2(float fElapsedTimeInSeconds = 1.0f / 60.0f);
#endif

        public static void SetRenderSceneElapsedTime(float fElapsedTimeInSeconds = 1.0f / 60.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderSceneElapsedTime_ES2(fElapsedTimeInSeconds);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderSceneElapsedTime(fElapsedTimeInSeconds);
            }
        }

        //// SetRenderSceneLuminanceScale
        [DllImport(dll)]
        private static extern void _SetRenderSceneLuminanceScale(float fSceneLuminanceScale = 1.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRenderSceneLuminanceScale_ES2(float fSceneLuminanceScale = 1.0f);
#endif

        public static void SetRenderSceneLuminanceScale(float fSceneLuminanceScale = 1.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRenderSceneLuminanceScale_ES2(fSceneLuminanceScale);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRenderSceneLuminanceScale(fSceneLuminanceScale);
            }
        }

        //// SetRemapCompressionLuminanceScale
        [DllImport(dll)]
        private static extern void _SetRemapCompressionLuminanceScale(float fRemapLuminanceScale = 1.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetRemapCompressionLuminanceScale_ES2(float fRemapLuminanceScale = 1.0f);
#endif

        public static void SetRemapCompressionLuminanceScale(float fRemapLuminanceScale = 1.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetRemapCompressionLuminanceScale_ES2(fRemapLuminanceScale);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetRemapCompressionLuminanceScale(fRemapLuminanceScale);
            }
        }

        //// SetAntialiasEnable
        [DllImport(dll)]
        private static extern void _SetAntialiasEnable(bool bAntialiasEnable = true);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAntialiasEnable_ES2(bool bAntialiasEnable = true);
#endif

        public static void SetAntialiasEnable(bool bAntialiasEnable = true)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAntialiasEnable_ES2(bAntialiasEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAntialiasEnable(bAntialiasEnable);
            }
        }

        //// SetAutoExposureEnable
        [DllImport(dll)]
        private static extern void _SetAutoExposureEnable(bool bAutoExposureEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAutoExposureEnable_ES2(bool bAutoExposureEnable = false);
#endif

        public static void SetAutoExposureEnable(bool bAutoExposureEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAutoExposureEnable_ES2(bAutoExposureEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAutoExposureEnable(bAutoExposureEnable);
            }
        }

        //// GetAutoExposureAdjusted
        [DllImport(dll)]
        private static extern float _GetAutoExposureAdjusted();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern float _GetAutoExposureAdjusted_ES2();
#endif

        public static float GetAutoExposureAdjusted()
        {
            float ret = 0.0f;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _GetAutoExposureAdjusted_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _GetAutoExposureAdjusted();
            }
            return ret;
        }

        //// SetAutoExposureMiddleGray
        [DllImport(dll)]
        private static extern void _SetAutoExposureMiddleGray(float fMiddleGray = 0.18f, float fInfluencedByGlare = 0.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAutoExposureMiddleGray_ES2(float fMiddleGray = 0.18f,float fInfluencedByGlare = 0.0f);
#endif

        public static void SetAutoExposureMiddleGray(float fMiddleGray = 0.18f, float fInfluencedByGlare = 0.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAutoExposureMiddleGray_ES2(fMiddleGray, fInfluencedByGlare);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAutoExposureMiddleGray(fMiddleGray, fInfluencedByGlare);
            }
        }


        //// SetTonemapParameters
        [DllImport(dll)]
        private static extern void _SetTonemapParameters(float fExposure = 1.0f, float fGamma = 1.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetTonemapParameters_ES2(float fExposure = 1.0f, float fGamma = 1.0f);
#endif

        public static void SetTonemapParameters(float fExposure = 1.0f, float fGamma = 1.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetTonemapParameters_ES2(fExposure, fGamma);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetTonemapParameters(fExposure, fGamma);
            }
        }

        //// SetGlareShape
        [DllImport(dll)]
        private static extern void _SetGlareShape(int shape);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetGlareShape_ES2(int shape);
#endif

        public static void SetGlareShape(int shape)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetGlareShape_ES2(shape);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetGlareShape(shape);
            }
        }

        //// SetGlareParameters
        [DllImport(dll)]
        private static extern void _SetGlareParameters(
            float fGlareLuminance = 1.0f,
            float fGlareThreshold = 0.0f,
            float fGlareRemapFactor = 1.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetGlareParameters_ES2(
        float fGlareLuminance = 1.0f,
        float fGlareThreshold = 0.0f,
        float fGlareRemapFactor = 1.0f);
#endif

        public static void SetGlareParameters(
            float fGlareLuminance = 1.0f,
            float fGlareThreshold = 0.0f,
            float fGlareRemapFactor = 1.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetGlareParameters_ES2(fGlareLuminance, fGlareThreshold, fGlareRemapFactor);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetGlareParameters(fGlareLuminance, fGlareThreshold, fGlareRemapFactor);
            }
        }

        //// SetLightShaftEnable
        [DllImport(dll)]
        private static extern void _SetLightShaftEnable(bool bLightShaftEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLightShaftEnable_ES2(bool bLightShaftEnable = false);
#endif

        public static void SetLightShaftEnable(bool bLightShaftEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLightShaftEnable_ES2(bLightShaftEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLightShaftEnable(bLightShaftEnable);
            }
        }


        //// SetLightShaftPosition
        [DllImport(dll)]
        private static extern void _SetLightShaftPosition(float fScreenPositionX = 0.5f, float fScreenPositionY = 0.5f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLightShaftPosition_ES2(float fScreenPositionX = 0.5f, float fScreenPositionY = 0.5f);
#endif

        public static void SetLightShaftPosition(float fScreenPositionX = 0.5f, float fScreenPositionY = 0.5f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLightShaftPosition_ES2(fScreenPositionX, fScreenPositionY);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLightShaftPosition(fScreenPositionX, fScreenPositionY);
            }
        }

        //// SetLightShaftLightColor
        [DllImport(dll)]
        private static extern void _SetLightShaftLightColor(float[] vLightColor);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLightShaftLightColor_ES2(float[] vLightColor);
#endif

        public static void SetLightShaftLightColor(Color vLightColor)
        {
            float[] pSrc = { vLightColor.r, vLightColor.g, vLightColor.b, vLightColor.a };
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLightShaftLightColor_ES2(pSrc);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLightShaftLightColor(pSrc);
            }
        }

        //// SetLightShaftParameters
        [DllImport(dll)]
        private static extern void _SetLightShaftParameters(
            float fLightShaftScale,
            float fLightShaftLength,
            float fLightShaftGlareRatio,
            float fLightShaftMaskDepthThereshold,
            float fLightShaftAngleAttenuation,
            float fLightShaftNoiseMask,
            float fLightShaftNoiseFrequency);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLightShaftParameters_ES2(
        float fLightShaftScale,
        float fLightShaftLength,
        float fLightShaftGlareRatio,
        float fLightShaftMaskDepthThereshold,
        float fLightShaftAngleAttenuation,
        float fLightShaftNoiseMask,
        float fLightShaftNoiseFrequency);
#endif

        public static void SetLightShaftParameters(
            float fLightShaftScale,
            float fLightShaftLength,
            float fLightShaftGlareRatio,
            float fLightShaftMaskDepthThereshold,
            float fLightShaftAngleAttenuation,
            float fLightShaftNoiseMask,
            float fLightShaftNoiseFrequency)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLightShaftParameters_ES2(fLightShaftScale, fLightShaftLength, fLightShaftGlareRatio, fLightShaftMaskDepthThereshold, fLightShaftAngleAttenuation, fLightShaftNoiseMask, fLightShaftNoiseFrequency);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLightShaftParameters(fLightShaftScale, fLightShaftLength, fLightShaftGlareRatio, fLightShaftMaskDepthThereshold, fLightShaftAngleAttenuation, fLightShaftNoiseMask, fLightShaftNoiseFrequency);
            }
        }

        //// SetLightShaftDiffractionRing
        [DllImport(dll)]
        private static extern void _SetLightShaftDiffractionRing(
            float fDiffractionRing,
            float fDiffractionRingRadius,
            float fDiffractionRingAttenuation,
            float fDiffractionRingSpectrumOrder,
            float[] vDiffractionRingOuterColor);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLightShaftDiffractionRing_ES2(
        float fDiffractionRing,
        float fDiffractionRingRadius,
        float fDiffractionRingAttenuation,
        float fDiffractionRingSpectrumOrder,
        float[] vDiffractionRingOuterColor);
#endif

        public static void SetLightShaftDiffractionRing(
            float fDiffractionRing,
            float fDiffractionRingRadius,
            float fDiffractionRingAttenuation,
            float fDiffractionRingSpectrumOrder,
            Color vDiffractionRingOuterColor)
        {
            float[] pSrcDiffractionRingOuterColor = { vDiffractionRingOuterColor.r, vDiffractionRingOuterColor.g, vDiffractionRingOuterColor.b, vDiffractionRingOuterColor.a };
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLightShaftDiffractionRing_ES2(fDiffractionRing, fDiffractionRingRadius, fDiffractionRingAttenuation, fDiffractionRingSpectrumOrder, pSrcDiffractionRingOuterColor);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLightShaftDiffractionRing(fDiffractionRing, fDiffractionRingRadius, fDiffractionRingAttenuation, fDiffractionRingSpectrumOrder, pSrcDiffractionRingOuterColor);
            }
        }


        //// SetDepthOfFieldEnable
        [DllImport(dll)]
        private static extern void _SetDepthOfFieldEnable(bool bDepthOfFieldEnable = true);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetDepthOfFieldEnable_ES2(bool bDepthOfFieldEnable = true);
#endif

        public static void SetDepthOfFieldEnable(bool bDepthOfFieldEnable = true)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetDepthOfFieldEnable_ES2(bDepthOfFieldEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetDepthOfFieldEnable(bDepthOfFieldEnable);
            }
        }


        //// SetApertureFilterLevelVisualize
        [DllImport(dll)]
        private static extern void _SetApertureFilterLevelVisualize(bool bEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetApertureFilterLevelVisualize_ES2(bool bEnable = false);
#endif

        public static void SetApertureFilterLevelVisualize(bool bEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetApertureFilterLevelVisualize_ES2(bEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetApertureFilterLevelVisualize(bEnable);
            }
        }

        //// SetAutoFocusEnable
        [DllImport(dll)]
        private static extern void _SetAutoFocusEnable(bool bAutoFocusEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAutoFocusEnable_ES2(bool bAutoFocusEnable = false);
#endif

        public static void SetAutoFocusEnable(bool bAutoFocusEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAutoFocusEnable_ES2(bAutoFocusEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAutoFocusEnable(bAutoFocusEnable);
            }
        }

        //// GetAutoFocusDistanceAdjusted
        [DllImport(dll)]
        private static extern float _GetAutoFocusDistanceAdjusted();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern float _GetAutoFocusDistanceAdjusted_ES2();
#endif

        public static float GetAutoFocusDistanceAdjusted()
        {
            float ret = 0.0f;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _GetAutoFocusDistanceAdjusted_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _GetAutoFocusDistanceAdjusted();
            }
            return ret;
        }


        //// SetDepthOfFieldByFnumberAdaptive
        [DllImport(dll)]
        private static extern void _SetDepthOfFieldByFnumberAdaptive(
            float fFocusDistance = 0.0f,
            float fApertureFnumber = 1.0f,
            float fAdaptiveApertureFactor = 0.5f,
            float fImageSensorHeight = 24.0f * (1.0f / 1000.0f));
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetDepthOfFieldByFnumberAdaptive_ES2(
        float fFocusDistance = 0.0f,
        float fApertureFnumber = 1.0f,
        float fAdaptiveApertureFactor = 0.5f,
        float fImageSensorHeight = 24.0f * (1.0f / 1000.0f));
#endif

        public static void SetDepthOfFieldByFnumberAdaptive(
            float fFocusDistance = 0.0f,
            float fApertureFnumber = 1.0f,
            float fAdaptiveApertureFactor = 0.5f,
            float fImageSensorHeight = 24.0f * (1.0f / 1000.0f))
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetDepthOfFieldByFnumberAdaptive_ES2(fFocusDistance, fApertureFnumber, fAdaptiveApertureFactor, fImageSensorHeight);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetDepthOfFieldByFnumberAdaptive(fFocusDistance, fApertureFnumber, fAdaptiveApertureFactor, fImageSensorHeight);
            }
        }

        //// SetLensDistortionEnable
        [DllImport(dll)]
        private static extern void _SetLensDistortionEnable(bool bLensDistortionEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetLensDistortionEnable_ES2(bool bLensDistortionEnable = false);
#endif

        public static void SetLensDistortionEnable(bool bLensDistortionEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetLensDistortionEnable_ES2(bLensDistortionEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetLensDistortionEnable(bLensDistortionEnable);
            }
        }

        //// SetVignetteParameters
        [DllImport(dll)]
        private static extern void _SetVignetteParameters(float fVignette, float fVignetteFovDependence);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetVignetteParameters_ES2(float fVignette, float fVignetteFovDependence);
#endif

        public static void SetVignetteParameters(float fVignette, float fVignetteFovDependence)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetVignetteParameters_ES2(fVignette, fVignetteFovDependence);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetVignetteParameters(fVignette, fVignetteFovDependence);
            }
        }

        //// SetVignetteByOpticsImageCircle
        [DllImport(dll)]
        private static extern void _SetVignetteByOpticsImageCircle(
            float fPowerOfCosineLow,
            float fImageCircle,
            float fPenumbraWidthScale,
            float fImageCircleFovDependence,
            float fPenumbraFovDependence,
            bool bIgnoreDepthOfFieldEnable,
            bool bApertureOpticalVignette);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetVignetteByOpticsImageCircle_ES2(
        float fPowerOfCosineLow,
        float fImageCircle,
        float fPenumbraWidthScale,
        float fImageCircleFovDependence,
        float fPenumbraFovDependence,
        bool bIgnoreDepthOfFieldEnable,
        bool bApertureOpticalVignette);
#endif

        public static void SetVignetteByOpticsImageCircle(
            float fPowerOfCosineLow,
            float fImageCircle,
            float fPenumbraWidthScale,
            float fImageCircleFovDependence,
            float fPenumbraFovDependence,
            bool bIgnoreDepthOfFieldEnable,
            bool bApertureOpticalVignette)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetVignetteByOpticsImageCircle_ES2(fPowerOfCosineLow, fImageCircle, fPenumbraWidthScale, fImageCircleFovDependence, fPenumbraFovDependence, bIgnoreDepthOfFieldEnable, bApertureOpticalVignette);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetVignetteByOpticsImageCircle(fPowerOfCosineLow, fImageCircle, fPenumbraWidthScale, fImageCircleFovDependence, fPenumbraFovDependence, bIgnoreDepthOfFieldEnable, bApertureOpticalVignette);
            }
        }

        //// SetGlareAnamorphicLensFlareEnable
        [DllImport(dll)]
        private static extern void _SetGlareAnamorphicLensFlareEnable(bool bAnamorphicEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetGlareAnamorphicLensFlareEnable_ES2(bool bAnamorphicEnable = false);
#endif

        public static void SetGlareAnamorphicLensFlareEnable(bool bAnamorphicEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetGlareAnamorphicLensFlareEnable_ES2(bAnamorphicEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetGlareAnamorphicLensFlareEnable(bAnamorphicEnable);
            }
        }

        //// SetVignetteMode
        [DllImport(dll)]
        private static extern void _SetVignetteMode(int eVignetteMode);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetVignetteMode_ES2(int eVignetteMode);
#endif

        public static void SetVignetteMode(int eVignetteMode)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetVignetteMode_ES2(eVignetteMode);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetVignetteMode(eVignetteMode);
            }
        }


        //// SetMotionBlurEnable
        [DllImport(dll)]
        private static extern void _SetMotionBlurEnable(bool bMotionBlurEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetMotionBlurEnable_ES2(bool bMotionBlurEnable = false);
#endif

        public static void SetMotionBlurEnable(bool bMotionBlurEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetMotionBlurEnable_ES2(bMotionBlurEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetMotionBlurEnable(bMotionBlurEnable);
            }
        }


        //// SetMotionBlurParameters
        [DllImport(dll)]
        private static extern void _SetMotionBlurParameters(
            float fBlurTimeRatio = 1.0f,
            float fMaxBlurLength = 0.1f,
            int iNumBaseSamples = -1,
            int iMaxRecurrences = -1,
            float fSampleInterleaved = 1.0f);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetMotionBlurParameters_ES2(
        float fBlurTimeRatio = 1.0f,
        float fMaxBlurLength = 0.1f,
        int iNumBaseSamples = -1,
        int iMaxRecurrences = -1,
        float fSampleInterleaved = 1.0f);
#endif

        public static void SetMotionBlurParameters(
            float fBlurTimeRatio = 1.0f,
            float fMaxBlurLength = 0.1f,
            int iNumBaseSamples = -1,
            int iMaxRecurrences = -1,
            float fSampleInterleaved = 1.0f)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetMotionBlurParameters_ES2(fBlurTimeRatio, fMaxBlurLength, iNumBaseSamples, iMaxRecurrences, fSampleInterleaved);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetMotionBlurParameters(fBlurTimeRatio, fMaxBlurLength, iNumBaseSamples, iMaxRecurrences, fSampleInterleaved);
            }
        }

        //// SetAmbientOcclusionEnable
        [DllImport(dll)]
        private static extern void _SetAmbientOcclusionEnable(bool bAmbientOcclusionEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAmbientOcclusionEnable_ES2(bool bAmbientOcclusionEnable = false);
#endif

        public static void SetAmbientOcclusionEnable(bool bAmbientOcclusionEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAmbientOcclusionEnable_ES2(bAmbientOcclusionEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAmbientOcclusionEnable(bAmbientOcclusionEnable);
            }
        }

        //// SetAmbientOcclusionParameters
        [DllImport(dll)]
        private static extern void _SetAmbientOcclusionParameters(
            int nOcclusionSamples = 16,
            float fOcclusionSampleRadius = 1.0f,
            float fOcclusionScale = 1.0f,
            float fOcclusionContrast = 1.0f,
            float fOcclusionDepthBias = 0.0f,
            float fOcclusionEpsiron = 0.0001f,
            float[] vOcclusionColoring = null);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetAmbientOcclusionParameters_ES2(
        int nOcclusionSamples = 16,
        float fOcclusionSampleRadius = 1.0f,
        float fOcclusionScale = 1.0f,
        float fOcclusionContrast = 1.0f,
        float fOcclusionDepthBias = 0.0f,
        float fOcclusionEpsiron = 0.0001f,
        float[] vOcclusionColoring = null);
#endif

        public static void SetAmbientOcclusionParameters(
            int nOcclusionSamples = 16,
            float fOcclusionSampleRadius = 1.0f,
            float fOcclusionScale = 1.0f,
            float fOcclusionContrast = 1.0f,
            float fOcclusionDepthBias = 0.0f,
            float fOcclusionEpsiron = 0.0001f,
            Color vOcclusionColoring = new Color())
        {
            float[] pSrcOcclusionColoring = { vOcclusionColoring.r, vOcclusionColoring.g, vOcclusionColoring.b, vOcclusionColoring.a };
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetAmbientOcclusionParameters_ES2(nOcclusionSamples,fOcclusionSampleRadius,fOcclusionScale,fOcclusionContrast,fOcclusionDepthBias,fOcclusionEpsiron,pSrcOcclusionColoring);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetAmbientOcclusionParameters(nOcclusionSamples, fOcclusionSampleRadius, fOcclusionScale, fOcclusionContrast, fOcclusionDepthBias, fOcclusionEpsiron, pSrcOcclusionColoring);
            }
        }

        //// SetFeedbackEnable
        [DllImport(dll)]
        private static extern void _SetFeedbackEnable(bool bFeedbackEnable = false);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetFeedbackEnable_ES2(bool bFeedbackEnable = false);
#endif

        public static void SetFeedbackEnable(bool bFeedbackEnable = false)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetFeedbackEnable_ES2(bFeedbackEnable);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetFeedbackEnable(bFeedbackEnable);
            }
        }

        //// SetColorCorrection
        [DllImport(dll)]
        private static extern void _SetColorCorrection(
            float colorHue,
            float colorSaturation,
            float colorContrast,
            float colorBrightness,
            float colorSepiaTone,
            float colorTemperature,
            float colorWhiteBalance);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetColorCorrection_ES2(
        float colorHue,
        float colorSaturation,
        float colorContrast,
        float colorBrightness,
        float colorSepiaTone,
        float colorTemperature,
        float colorWhiteBalance);
#endif

        public static void SetColorCorrection(
            float colorHue,
            float colorSaturation,
            float colorContrast,
            float colorBrightness,
            float colorSepiaTone,
            float colorTemperature,
            float colorWhiteBalance)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetColorCorrection_ES2(colorHue, colorSaturation, colorContrast, colorBrightness, colorSepiaTone, colorTemperature, colorWhiteBalance);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetColorCorrection(colorHue, colorSaturation, colorContrast, colorBrightness, colorSepiaTone, colorTemperature, colorWhiteBalance);
            }
        }

        //// SetFeedbackEffect
        [DllImport(dll)]
        private static extern void _SetFeedbackEffect(
            float feedbackWeight,
            float feedbackRotation,
            float feedbackScaling,
            float colorHue,
            float colorSaturation,
            float colorContrast,
            float colorBrightness
        );
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetFeedbackEffect_ES2(
        float feedbackWeight,
        float feedbackRotation,
        float feedbackScaling,
        float colorHue,
        float colorSaturation,
        float colorContrast,
        float colorBrightness
    );
#endif

        public static void SetFeedbackEffect(
            float feedbackWeight,
            float feedbackRotation,
            float feedbackScaling,
            float colorHue,
            float colorSaturation,
            float colorContrast,
            float colorBrightness)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetFeedbackEffect_ES2(feedbackWeight, feedbackRotation, feedbackScaling, colorHue, colorSaturation, colorContrast, colorBrightness);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetFeedbackEffect(feedbackWeight, feedbackRotation, feedbackScaling, colorHue, colorSaturation, colorContrast, colorBrightness);
            }
        }

        //// GetMaxGlareQuality
        [DllImport(dll)]
        private static extern int _GetMaxGlareQuality();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern int _GetMaxGlareQuality_ES2();
#endif
        public static int GetMaxGlareQuality()
        {
            int ret = 0;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _GetMaxGlareQuality_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _GetMaxGlareQuality();
            }
            return ret;
        }


        //// GetMaxDepthOfFieldQuality
        [DllImport(dll)]
        private static extern int _GetMaxDepthOfFieldQuality();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern int _GetMaxDepthOfFieldQuality_ES2();
#endif
        public static int GetMaxDepthOfFieldQuality()
        {
            int ret = 0;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _GetMaxDepthOfFieldQuality_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _GetMaxDepthOfFieldQuality();
            }
            return ret;
        }

        //// EXPORT_IsMessageEmpty
        [DllImport(dll)]
        private static extern bool _IsMessageEmpty();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern bool _IsMessageEmpty_ES2();
#endif

        public static bool IsMessageEmpty()
        {
            bool ret = true;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _IsMessageEmpty_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _IsMessageEmpty();
            }
            return ret;
        }


        //// EXPORT_PopMessage
        [DllImport(dll)]
        private static extern string _PopMessage();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern string _PopMessage_ES2();
#endif

        public static string PopMessage()
        {
            string ret = "";
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _PopMessage_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _PopMessage();
            }
            return ret;
        }


        //// EXPORT_SetPPFXOutputLogLevel
        [DllImport(dll)]
        private static extern void _SetPPFXOutputLogLevel(int lvl);
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern void _SetPPFXOutputLogLevel_ES2(int lvl);
#endif

        public static void SetPPFXOutputLogLevel(int lvl = 0)
        {
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            _SetPPFXOutputLogLevel_ES2(lvl);
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                _SetPPFXOutputLogLevel(lvl);
            }
        }

        //// EXPORT_IsEvaluationVersion
        [DllImport(dll)]
        private static extern bool _IsEvaluationVersion();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern bool _IsEvaluationVersion_ES2();
#endif

        public static bool IsEvaluationVersion()
        {
            bool ret = true;
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            ret = _IsEvaluationVersion_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                ret = _IsEvaluationVersion();
            }
            return ret;
        }

        //// EXPORT_GetExpirationDate
        [DllImport(dll)]
        private static extern string _GetExpirationDate();
#if YEBIS_LIB_GLES
    [DllImport(dll_ES2)]
    private static extern string _GetExpirationDate_ES2();
#endif

        public static DateTime GetExpirationDate()
        {
            string str = "2016/12/31 23:59:59";
#if YEBIS_LIB_GLES
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2){
            str = _GetExpirationDate_ES2();
        }else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
#endif
            {
                str = _GetExpirationDate();
            }
            return DateTime.Parse(str);
        }
    }

}