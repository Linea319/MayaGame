using UnityEngine;
using System.Collections;
using System;

namespace YEBIS
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("YEBIS/YebisPostEffects")]
    public class YebisPostEffects : MonoBehaviour
    {

        public bool enableYebis = true;
        public bool enableFXAA = true;
        public Tonemap tonemap;
        public Glare glare;
        public DepthOfField depthOfField;
        public LensSimulation lensSimulation;
        public ColorCorrection colorCorrection;
        public MotionBlur motionBlur;
        public SSAO sSAO;
        public FeedbackEffect feedbackEffect;


        private Camera cachedCamera;
        private Transform cachedTransform;

        private Material yebisInputRenderMaterial = null;
        private RenderTexture yebisSourceTexture = null;
        private RenderTexture yebisDestinationTexture = null;

        private float[] beforeFrameViewMatrix = new float[16];
        private float beforeFrameCameraFov;
        private float beforeFrameCameraNear;
        private float beforeFrameCameraFar;

        private bool IsUseFloatTextureMode()
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
            {
                return false;
            }
            else
            {
                if (cachedCamera == null) cachedCamera = GetComponent<Camera>();
                return cachedCamera.hdr && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR);
            }
        }

        public static bool IsSupportDevice()
        {
            return
                SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLCore ||
                SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 ||
                SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3
                ;
        }

        bool IsEnableYebis()
        {
            return enableYebis && IsSupportDevice();
        }

        void BindBeforeFrameViewMatrix()
        {
            if (cachedTransform != null)
            {
                Matrix4x4 m = cachedTransform.worldToLocalMatrix;
                for (int i = 0; i < 16; ++i)
                {
                    beforeFrameViewMatrix[i] = m[i];
                }
            }
        }

        void YebisInputRender(RenderTexture source)
        {
            if (yebisInputRenderMaterial == null)
            {
                yebisInputRenderMaterial = Resources.Load<Material>("YebisInputRender");
            }
            yebisInputRenderMaterial.SetVector("vDofFactorScaleOffset", YebisLib.GetDepthOfFieldFactorScaleOffset());
            yebisInputRenderMaterial.SetFloat("fDofFocusDistance", YebisLib.GetFocusDistance());
            yebisInputRenderMaterial.SetTexture("_MainTex", source);
            Graphics.Blit(source, yebisSourceTexture, yebisInputRenderMaterial);
        }

        void Awake()
        {
#if UNITY_EDITOR
            if (YebisLib.IsEvaluationVersion())
            {
                var expireationDate = YebisLib.GetExpirationDate();
                Debug.LogWarning("This YEBIS plugin is evaluation version. (expireation date=" + expireationDate.ToString("yyyy/MM/dd") + ")");
            }
#endif
        }


        void Start()
        {
            cachedCamera = GetComponent<Camera>();
            cachedCamera.depthTextureMode |= DepthTextureMode.Depth;

            cachedTransform = transform;
            BindBeforeFrameViewMatrix();
            beforeFrameCameraFov = cachedCamera.fieldOfView;
            beforeFrameCameraNear = cachedCamera.nearClipPlane;
            beforeFrameCameraFar = cachedCamera.farClipPlane;
        }

        private void CreateInputBuffer()
        {
            var sourceFormat = IsUseFloatTextureMode() ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.ARGB32;
            // YEBIS入力バッファの作成
            if (yebisSourceTexture == null
                || yebisSourceTexture.width != Screen.width
                || yebisSourceTexture.height != Screen.height
                || yebisSourceTexture.format != sourceFormat
                )
            {
                if (yebisSourceTexture != null) yebisSourceTexture.Release();
                yebisSourceTexture = new RenderTexture((int)Screen.width, (int)Screen.height, 0, sourceFormat);
                yebisSourceTexture.Create();
                YebisLib.SetSourceBuffer(yebisSourceTexture.GetNativeTexturePtr(), yebisSourceTexture.width, yebisSourceTexture.height);
            }

            // YEBIS出力バッファの作成
            if (yebisDestinationTexture == null
                || yebisDestinationTexture.width != Screen.width
                || yebisDestinationTexture.height != Screen.height
                )
            {
                if (yebisDestinationTexture != null) yebisDestinationTexture.Release();
                yebisDestinationTexture = new RenderTexture((int)Screen.width, (int)Screen.height, 0, RenderTextureFormat.ARGB32);
                yebisDestinationTexture.Create();
                YebisLib.SetDestinationBuffer(yebisDestinationTexture.GetNativeTexturePtr(), yebisDestinationTexture.width, yebisDestinationTexture.height);
            }
        }


        void OnPreRender()
        {
            if (IsEnableYebis())
            {
                // Initialize Camera Buffer
                CreateInputBuffer();

                //// YEBIS Setting
                {
                    // Initialize
                    YebisLib.Initialize(
                        yebisDestinationTexture.width,
                        yebisDestinationTexture.height,
                        glare.quality,
                        depthOfField.quality,
                        IsUseFloatTextureMode(),
                        (int)depthOfField.apertureShape);

                    // カメラ行列のセット
                    YebisLib.SetRenderScenePreviousViewMatrix(beforeFrameViewMatrix);
                    BindBeforeFrameViewMatrix();
                    YebisLib.SetRenderSceneViewMatrix(beforeFrameViewMatrix);

                    YebisLib.SetRenderScenePreviousPerspective(beforeFrameCameraNear, beforeFrameCameraFar, beforeFrameCameraFov);
                    beforeFrameCameraFov = cachedCamera.fieldOfView;
                    beforeFrameCameraNear = cachedCamera.nearClipPlane;
                    beforeFrameCameraFar = cachedCamera.farClipPlane;
                    YebisLib.SetRenderScenePerspective(cachedCamera.nearClipPlane, cachedCamera.farClipPlane, cachedCamera.fieldOfView);

                    // その他全体的な設定
                    YebisLib.SetRenderSceneElapsedTime(Time.deltaTime);
                    YebisLib.SetRenderSceneLuminanceScale(1.0f);
                    YebisLib.SetRemapCompressionLuminanceScale(1.0f);

                    // アンチエイリアス
                    YebisLib.SetAntialiasEnable(enableFXAA);

                    //// 露光
                    YebisLib.SetAutoExposureEnable(tonemap.autoExposure);
                    if (tonemap.autoExposure)
                    {
                        // 自動露光系機能はプラグイン内部で正確に調整される
                        YebisLib.SetTonemapParameters(tonemap.exposure, colorCorrection.colorGamma); // 現在の値を渡すにとどめる
                        tonemap.exposure = YebisLib.GetAutoExposureAdjusted(); // GetAutoExposureAdjusted()はUI表示用なので1フレーム遅れる
                        YebisLib.SetAutoExposureMiddleGray(tonemap.middleGray);
                    }
                    else
                    {
                        YebisLib.SetTonemapParameters(tonemap.exposure, colorCorrection.colorGamma);
                    }

                    // グレア
                    YebisLib.SetGlareShape(((glare.enableGlare) ? ((int)glare.shape) : (-1)));
                    if (glare.enableGlare)
                    {
                        YebisLib.SetGlareParameters(glare.luminance, glare.threshold, glare.remapFactor);

                        YebisLib.SetLightShaftEnable(glare.lightShaft.enableLightShaft);
                        if (glare.lightShaft.enableLightShaft)
                        {

                            Vector3 screenPosition = new Vector3(0, 0, 0);
                            if (glare.lightShaft.lookAt != null)
                            {
                                screenPosition = cachedCamera.WorldToViewportPoint(glare.lightShaft.lookAt.position);
                                screenPosition.y = 1.0f - screenPosition.y;
                                //screenPosition.z = (((cachedCamera.farClipPlane - screenPosition.z) / (cachedCamera.farClipPlane + cachedCamera.nearClipPlane)));
                            }
                            YebisLib.SetLightShaftPosition(screenPosition.x, screenPosition.y);

                            YebisLib.SetLightShaftLightColor(glare.lightShaft.color);

                            float maskThreshold = screenPosition.z - glare.lightShaft.maskBias;
                            YebisLib.SetLightShaftParameters(
                                glare.lightShaft.scale,
                                glare.lightShaft.length,
                                glare.lightShaft.glareRatio,
                                maskThreshold,
                                glare.lightShaft.angleAttenuation,
                                glare.lightShaft.noiseMask,
                                glare.lightShaft.noiseFrequency);

                            YebisLib.SetLightShaftDiffractionRing(
                                glare.lightShaft.diffractionRing,
                                glare.lightShaft.diffractionRingRadius * Mathf.Deg2Rad,
                                glare.lightShaft.diffractionRingAttenuation,
                                glare.lightShaft.diffractionRingSpectrumOrder,
                                glare.lightShaft.diffractionRingOuterColor);
                        }
                    }

                    //// DoF
                    YebisLib.SetDepthOfFieldEnable(depthOfField.enableDof);
                    if (depthOfField.enableDof)
                    {
                        YebisLib.SetApertureFilterLevelVisualize(depthOfField.visualizeApertureFilterLevel);
                        YebisLib.SetAutoFocusEnable(depthOfField.autoFocus);
                        if (depthOfField.autoFocus)
                        {
                            //オートフォーカス機能はプラグイン内部で正確に調整される
                            // 現在の値を渡すにとどめる
                            YebisLib.SetDepthOfFieldByFnumberAdaptive(
                                depthOfField.focusDistance,
                                depthOfField.apertureFnumber,
                                0.5f,
                                depthOfField.filmSize);
                            // UI表示
                            depthOfField.focusDistance = YebisLib.GetAutoFocusDistanceAdjusted();
                        }
                        else
                        {
                            YebisLib.SetDepthOfFieldByFnumberAdaptive(
                                depthOfField.focusDistance,
                                depthOfField.apertureFnumber,
                                0.5f,
                                depthOfField.filmSize);
                        }
                    }

                    // レンズシミュレーション
                    YebisLib.SetLensDistortionEnable(lensSimulation.lensDistortion);
                    YebisLib.SetVignetteMode((int)lensSimulation.vignetteMode);
                    if (lensSimulation.vignetteMode == YebisPostEffects.VIGNETTE_MODE.VIGNETTE_OFF)
                    {
                        YebisLib.SetVignetteParameters(0.0f, lensSimulation.vignetteFovDependence);
                    }
                    else if (lensSimulation.vignetteMode == YebisPostEffects.VIGNETTE_MODE.VIGNETTE_EFFECT)
                    {
                        YebisLib.SetVignetteParameters(lensSimulation.vignetteIntensity, lensSimulation.vignetteFovDependence);
                    }
                    else if (lensSimulation.vignetteMode == YebisPostEffects.VIGNETTE_MODE.VIGNETTE_SIMULATION)
                    {
                        YebisLib.SetVignetteByOpticsImageCircle(
                            lensSimulation.powerOfCosine,
                            lensSimulation.imageCircle,
                            lensSimulation.penumbraWidth,
                            lensSimulation.imageCircleFovDependence,
                            lensSimulation.penumbraFovDependence,
                            lensSimulation.ignoreDepthOfFieldEnable,
                            lensSimulation.opticalVignetting);
                    }
                    YebisLib.SetGlareAnamorphicLensFlareEnable(lensSimulation.anamorphicLens);

                    // 線形カラー変換
                    YebisLib.SetColorCorrection(
                        colorCorrection.colorHue * Mathf.Deg2Rad,
                        colorCorrection.colorSaturation,
                        colorCorrection.colorContrast,
                        colorCorrection.colorBrightness,
                        colorCorrection.colorSepia,
                        colorCorrection.colorTemperature,
                        colorCorrection.colorWhiteBalance);

                    // モーションブラー
                    YebisLib.SetMotionBlurEnable(motionBlur.enableMotionBlur);
                    if (motionBlur.enableMotionBlur)
                    {
                        YebisLib.SetMotionBlurParameters(motionBlur.blurTimeRatio, motionBlur.maxBlurLength);
                    }
                    // SSAO
                    YebisLib.SetAmbientOcclusionEnable(sSAO.enableSSAO);
                    if (sSAO.enableSSAO)
                    {
                        YebisLib.SetAmbientOcclusionParameters(
                            sSAO.samples,
                            sSAO.sampleRadius,
                            sSAO.scale,
                            sSAO.contrast,
                            sSAO.bias,
                            sSAO.epsilon,
                            sSAO.color
                         );
                    }

                    // フィードバックエフェクト
                    YebisLib.SetFeedbackEnable(feedbackEffect.feedbackEffect);
                    if (feedbackEffect.feedbackEffect)
                    {
                        YebisLib.SetFeedbackEffect(
                            feedbackEffect.feedbackWeight,
                            feedbackEffect.feedbackRotation,
                            feedbackEffect.feedbackScaling,
                            feedbackEffect.feedbackHue,
                            feedbackEffect.feedbackSaturation,
                            feedbackEffect.feedbackContrast,
                            feedbackEffect.feedbackBrightness);
                    }
                }

                // YEBIS Setup
                YebisLib.CallSetupPostEffectEvent();
                // YEBIS Begin
                YebisLib.CallBeginPostEffectEvent();
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (IsEnableYebis())
            {
                // sourceの内容をYEBIS入力テクスチャに書き込み
                YebisInputRender(source);
                YebisLib.CallEndPostEffectEvent();

                // YEBIS LOG
                printNativeLog();

                // YEBIS 適用
                Graphics.Blit(yebisDestinationTexture, destination);
            }
            else
            {
                // YEBISがOFFの場合は何もしない
                Graphics.Blit(source, destination);
            }

        }


        void printNativeLog()
        {
            while (!YebisLib.IsMessageEmpty())
            {
                var logString = YebisLib.PopMessage();
                var splitPos = logString.IndexOf(":");
                var logHeader = logString.Substring(0, splitPos);
                var logBody = "[YebisPostEffects] " + logString.Substring(splitPos + 1);
                if (logHeader.Contains("ERROR"))
                {
                    Debug.LogError(logBody);
                }
                else if (logHeader.Contains("WARNING"))
                {
                    Debug.LogWarning(logBody);
                }
                else
                {
                    Debug.Log(logBody);
                }
            }
        }


        void Reset()
        {
            tonemap.Reset();
            glare.Reset();
            depthOfField.Reset();
            lensSimulation.Reset();
            colorCorrection.Reset();
            motionBlur.Reset();
            sSAO.Reset();
            feedbackEffect.Reset();
        }

        void OnValidate()
        {
            depthOfField.focusDistance = Mathf.Max(0.0f, depthOfField.focusDistance);
        }
        

        public enum VIGNETTE_MODE
        {
            VIGNETTE_OFF = 0,
            VIGNETTE_EFFECT = 1,
            VIGNETTE_SIMULATION = 2
        }

        public enum GLARESHAPE
        {
            BLOOM = 0,
            LENSFLARE = 1,
            STANDARD = 2,
            CHEAPLENS = 3,
            AFTERIMAGE = 4,
            FILTER_CROSSSCREEN = 5,
            FILTER_CROSSSCREEN_SPECTRAL = 6,
            FILTER_SNOWCROSS = 7,
            FILTER_SNOWCROSS_SPECTRAL = 8,
            FILTER_SUNNYCROSS = 9,
            FILTER_SUNNYCROSS_SPECTRAL = 10,
            HORIZONTALSTREAK = 11,
            VERTICALSTREAK = 12,
        }

        public enum APERTURESHAPE
        {
            DISC = 0,
            GAUSSIAN = 1,
            SQUARE = 2,
            HEXAGON = 3,
            HEXAGONFLAT = 4,
            AUTO = 5,
        }


    }



    [System.Serializable]
    public struct Tonemap
    {
        // {Effective Exposure} = 2^{exposure} 
        public bool autoExposure; // = false;
        public float exposure;
        public float middleGray;

        public void Reset()
        {
            exposure = 0.0f;
            autoExposure = false;
            middleGray = 0.25f;
        }

    }

    [System.Serializable]
    public struct Glare
    {
        public bool enableGlare;
        public int quality;
        public YebisPostEffects.GLARESHAPE shape;
        public float luminance; // = 1.0f;
        public float threshold; // = 0.0f;
        public float remapFactor; // = 1.0f;
        public GlareLightShaft lightShaft;

        public void Reset()
        {
            enableGlare = true;
            quality = 3;
            shape = YebisPostEffects.GLARESHAPE.BLOOM;
            luminance = 1.0f;
            threshold = 0.0f;
            remapFactor = 1.0f;
            lightShaft.Reset();
        }

    }

    [System.Serializable]
    public struct GlareLightShaft
    {
        public bool enableLightShaft;
        public Transform lookAt;
        public Vector2 position;
        public Color color;

        public float scale;
        public float length;
        public float glareRatio;
        public float maskBias;
        public float angleAttenuation;
        public float noiseMask;
        public float noiseFrequency;

        public float diffractionRing;
        public float diffractionRingRadius;
        public float diffractionRingAttenuation;
        public float diffractionRingSpectrumOrder;
        public Color diffractionRingOuterColor;

        public void Reset()
        {
            enableLightShaft = false;
            position = Vector2.zero;
            color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            scale = 1.0f;
            maskBias = 1.0f;
            length = 1.0f;
            glareRatio = 0.25f;
            angleAttenuation = 1.0f;
            noiseMask = 1.0f;
            noiseFrequency = 1.0f;

            diffractionRing = 0.5f;
            diffractionRingRadius = 15.0f;
            diffractionRingAttenuation = 0.5f;
            diffractionRingSpectrumOrder = 1.0f;
            diffractionRingOuterColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    [System.Serializable]
    public struct DepthOfField
    {
        public bool enableDof;
        public int quality;
        public YebisPostEffects.APERTURESHAPE apertureShape;
        public float filmSize;
        public float apertureFnumber;
        public bool autoFocus;
        public float focusDistance;
        public bool visualizeApertureFilterLevel;

        public void Reset()
        {
            enableDof = true;
            quality = 5;
            apertureShape = YebisPostEffects.APERTURESHAPE.AUTO;
            filmSize = 0.24f;
            apertureFnumber = 1.0f;
            autoFocus = false;
            focusDistance = 0.0f;
            visualizeApertureFilterLevel = false;
        }

    }

    [System.Serializable]
    public struct LensSimulation
    {
        public bool lensDistortion;
        public bool anamorphicLens;
        public YebisPostEffects.VIGNETTE_MODE vignetteMode;
        public float vignetteIntensity;
        public float vignetteFovDependence;
        public float powerOfCosine;
        public float imageCircle;
        public float penumbraWidth;
        public float imageCircleFovDependence;
        public float penumbraFovDependence;
        public bool ignoreDepthOfFieldEnable;
        public bool opticalVignetting;

        public void Reset()
        {
            lensDistortion = false;
            anamorphicLens = false;
            vignetteMode = YebisPostEffects.VIGNETTE_MODE.VIGNETTE_SIMULATION;
            vignetteIntensity = 0.5f;
            powerOfCosine = 1.0f;
            imageCircle = 2.0f;
            penumbraWidth = 1.0f;
            ignoreDepthOfFieldEnable = true;
            opticalVignetting = false;
        }

    }

    [System.Serializable]
    public struct ColorCorrection
    {
        public float colorHue;
        public float colorSaturation;
        public float colorContrast;
        public float colorBrightness;
        public float colorSepia;
        public float colorTemperature;
        public float colorWhiteBalance;
        public float colorGamma;

        public void Reset()
        {
            colorHue = 0.0f;
            colorSaturation = 1.0f;
            colorContrast = 1.0f;
            colorBrightness = 1.0f;
            colorSepia = 0.0f;
            colorTemperature = 6500.0f;
            colorWhiteBalance = 6500.0f;
            colorGamma = 1.0f;
        }
    }

    [System.Serializable]
    public struct MotionBlur
    {
        public bool enableMotionBlur;
        public float blurTimeRatio;
        public float maxBlurLength;

        public void Reset()
        {
            enableMotionBlur = false;
            blurTimeRatio = 1.0f;
            maxBlurLength = 0.1f;
        }

    }

    [System.Serializable]
    public struct SSAO
    {
        public bool enableSSAO;
        public int samples;
        public float sampleRadius;
        public float scale;
        public float contrast;
        public float bias;
        public float epsilon;
        public Color color;

        public void Reset()
        {
            enableSSAO = false;
            samples = 16;
            sampleRadius = 1.0f;
            scale = 1.0f;
            contrast = 1.0f;
            bias = 0.0f;
            epsilon = 0.0001f;
            color = Color.gray;
        }

    }

    [System.Serializable]
    public struct FeedbackEffect
    {
        public bool feedbackEffect;
        public float feedbackWeight;
        public float feedbackRotation;
        public float feedbackScaling;
        public float feedbackHue;
        public float feedbackSaturation;
        public float feedbackContrast;
        public float feedbackBrightness;

        public void Reset()
        {
            feedbackEffect = false;
            feedbackWeight = 0.0f;
            feedbackRotation = 0.0f;
            feedbackScaling = 1.0f;
            feedbackHue = 0.0f;
            feedbackSaturation = 1.0f;
            feedbackContrast = 1.0f;
            feedbackBrightness = 1.0f;
        }

    }

}