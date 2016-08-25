using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(YebisPostEffects))]
public class YebisPostEffectsEditor : Editor
{
    SerializedObject serObj;
    SerializedProperty enableYebis;
    SerializedProperty enableFXAA;
    
    SerializedProperty tonemap;
    AnimBool showTonemapAutoExposure = new AnimBool();
    
    SerializedProperty glare;
    AnimBool showEnableGlare = new AnimBool();
    AnimBool showLightShaft = new AnimBool();
    SerializedProperty lightShaft;
    
    SerializedProperty dof;
    AnimBool showEnableDoF = new AnimBool();
    
    SerializedProperty lensSimulation;
    AnimBool showEnableVignette_Effect = new AnimBool();
    AnimBool showEnableVignette_Simulation = new AnimBool();
    
    SerializedProperty colorCorrection;
    
    SerializedProperty motionBlur;
    AnimBool showEnableMotionBlur = new AnimBool();
    
    SerializedProperty sSAO;
    AnimBool showEnableSSAO = new AnimBool();
    
    SerializedProperty feedbackEffect;
    AnimBool showEnableFeedbackEffect = new AnimBool();

    bool autoExposure = false;
    bool autoFocus = false;



    void OnEnable()
    {
        serObj = new SerializedObject(target);

        enableYebis = serObj.FindProperty("enableYebis");
        enableFXAA = serObj.FindProperty("enableFXAA");
        tonemap = serObj.FindProperty("tonemap");
        glare = serObj.FindProperty("glare");
        lightShaft = glare.FindPropertyRelative("lightShaft");
        dof = serObj.FindProperty("depthOfField");
        lensSimulation = serObj.FindProperty("lensSimulation");
        colorCorrection = serObj.FindProperty("colorCorrection");
        motionBlur = serObj.FindProperty("motionBlur");
        sSAO = serObj.FindProperty("sSAO");
        feedbackEffect = serObj.FindProperty("feedbackEffect");

        InitializedAnimBools();
    }



    void InitializedAnimBools()
    {
        showTonemapAutoExposure.valueChanged.AddListener(Repaint);
        showTonemapAutoExposure.value = tonemap.FindPropertyRelative("autoExposure").boolValue;

        showEnableGlare.valueChanged.AddListener(Repaint);
        showEnableGlare.value = glare.FindPropertyRelative("enableGlare").boolValue;

        showLightShaft.valueChanged.AddListener(Repaint);
        showLightShaft.value = lightShaft.FindPropertyRelative("enableLightShaft").boolValue;

        showEnableDoF.valueChanged.AddListener(Repaint);
        showEnableDoF.value = dof.FindPropertyRelative("enableDof").boolValue;

        showEnableMotionBlur.valueChanged.AddListener(Repaint);
        showEnableMotionBlur.value = motionBlur.FindPropertyRelative("enableMotionBlur").boolValue;

        showEnableSSAO.valueChanged.AddListener(Repaint);
        showEnableSSAO.value = sSAO.FindPropertyRelative("enableSSAO").boolValue;

        showEnableFeedbackEffect.valueChanged.AddListener(Repaint);
        showEnableFeedbackEffect.value = feedbackEffect.FindPropertyRelative("feedbackEffect").boolValue;

        showEnableVignette_Effect.valueChanged.AddListener(Repaint);
        showEnableVignette_Simulation.valueChanged.AddListener(Repaint);
        switch (lensSimulation.FindPropertyRelative("vignetteMode").intValue)
        {
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_OFF:
                showEnableVignette_Effect.value = false;
                showEnableVignette_Simulation.value = false;
                break;
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_EFFECT:
                showEnableVignette_Effect.value = true;
                showEnableVignette_Simulation.value = false;
                break;
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_SIMULATION:
                showEnableVignette_Effect.value = false;
                showEnableVignette_Simulation.value = true;
                break;
        }
    }



    void UpdateAnimBoolTargets()
    {
        autoExposure = tonemap.FindPropertyRelative("autoExposure").boolValue;
        autoFocus = dof.FindPropertyRelative("autoFocus").boolValue;
        showTonemapAutoExposure.target = autoExposure;
        showEnableGlare.target = glare.FindPropertyRelative("enableGlare").boolValue;
        showLightShaft.target = lightShaft.FindPropertyRelative("enableLightShaft").boolValue;
        showEnableDoF.target = dof.FindPropertyRelative("enableDof").boolValue;
        showEnableMotionBlur.target = motionBlur.FindPropertyRelative("enableMotionBlur").boolValue;
        showEnableFeedbackEffect.target = feedbackEffect.FindPropertyRelative("feedbackEffect").boolValue;
        showEnableSSAO.target = sSAO.FindPropertyRelative("enableSSAO").boolValue;
        switch (lensSimulation.FindPropertyRelative("vignetteMode").intValue)
        {
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_OFF:
                showEnableVignette_Effect.target = false;
                showEnableVignette_Simulation.target = false;
                break;
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_EFFECT:
                showEnableVignette_Effect.target = true;
                showEnableVignette_Simulation.target = false;
                break;
            case (int)YebisPostEffects.VIGNETTE_MODE.VIGNETTE_SIMULATION:
                showEnableVignette_Effect.target = false;
                showEnableVignette_Simulation.target = true;
                break;
        }
    }

    private bool _Foldout(string content)
    {
        string prefsKey = "_YEBIS_UI_Foldout_" + content;
        bool temp = EditorGUILayout.Foldout(EditorPrefs.GetBool(prefsKey,false), content);
        EditorPrefs.SetBool(prefsKey, temp);
        return temp;
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();
        UpdateAnimBoolTargets();

        if (YebisLib.IsEvaluationVersion())
        {
            EditorGUILayout.HelpBox("This YEBIS plugin is evaluation version.", MessageType.Warning, true);
        }

        EditorGUILayout.PropertyField(enableYebis, new GUIContent("Enable YEBIS"));
        EditorGUILayout.PropertyField(enableFXAA, new GUIContent("Enable FXAA"));

        //// Tonemap
        if (_Foldout("Tonemap"))
        {
            EditorGUI.BeginDisabledGroup(autoExposure);
            EditorGUILayout.Slider(tonemap.FindPropertyRelative("exposure"), -10.0f, 10.0f, new GUIContent(" Exposure"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(tonemap.FindPropertyRelative("autoExposure"), new GUIContent(" Auto Exposure"));
            if (EditorGUILayout.BeginFadeGroup(showTonemapAutoExposure.faded))
            {
                EditorGUILayout.Slider(tonemap.FindPropertyRelative("middleGray"), 0.01f, 1.0f, new GUIContent("  MiddleGray"));
            }
            EditorGUILayout.EndFadeGroup();
        }

        //// Glare
        if (_Foldout("Glare"))
        {
            EditorGUILayout.PropertyField(glare.FindPropertyRelative("enableGlare"), new GUIContent(" Enable Glare"));
            if (EditorGUILayout.BeginFadeGroup(showEnableGlare.faded))
            {
                EditorGUILayout.IntSlider(glare.FindPropertyRelative("quality"), 0, YebisLib.GetMaxGlareQuality(), new GUIContent(" Quality"));
                EditorGUILayout.PropertyField(glare.FindPropertyRelative("shape"), new GUIContent(" Shape"));

                EditorGUILayout.Slider(glare.FindPropertyRelative("luminance"), 0.0f, 16.0f, new GUIContent(" Luminance"));
                EditorGUILayout.Slider(glare.FindPropertyRelative("threshold"), 0.0f, 2.0f, new GUIContent(" Threshold"));
                EditorGUILayout.Slider(glare.FindPropertyRelative("remapFactor"), 0.0f, 2.0f, new GUIContent(" RemapFactor"));

                //// LightShaft
                EditorGUILayout.PropertyField(lightShaft.FindPropertyRelative("enableLightShaft"), new GUIContent(" LightShaft"));
                if (EditorGUILayout.BeginFadeGroup(showLightShaft.faded))
                {
                    EditorGUILayout.PropertyField(lightShaft.FindPropertyRelative("lookAt"), new GUIContent("  Position"));
                    EditorGUILayout.PropertyField(lightShaft.FindPropertyRelative("color"), new GUIContent("  Color"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("scale"), 0.0f, 100.0f, new GUIContent("  Scale"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("length"), 0.0f, 1.0f, new GUIContent("  Length"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("glareRatio"), 0.0f, 1.0f, new GUIContent("  GlareRatio"));
                    EditorGUILayout.PropertyField(lightShaft.FindPropertyRelative("maskBias"), new GUIContent("  MaskBias"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("angleAttenuation"), 0.0f, 1.0f, new GUIContent("  AngleAttenuation"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("noiseMask"), 0.0f, 1.0f, new GUIContent("  NoiseMask"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("noiseFrequency"), 0.0f, 1.0f, new GUIContent("  NoiseFrequency"));

                    EditorGUILayout.PropertyField(lightShaft.FindPropertyRelative("diffractionRingOuterColor"), new GUIContent("  DiffractionRingOuterColor"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("diffractionRing"), 0.0f, 1.0f, new GUIContent("  DiffractionRing"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("diffractionRingRadius"), 0.0f, 50.0f, new GUIContent("  DiffractionRingRadius"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("diffractionRingAttenuation"), 0.0f, 1.0f, new GUIContent("  DiffractionRingAttenuation"));
                    EditorGUILayout.Slider(lightShaft.FindPropertyRelative("diffractionRingSpectrumOrder"), 0.0f, 1.0f, new GUIContent("  DiffractionRingSpectrumOrder"));
                }
                EditorGUILayout.EndFadeGroup();
            }
        }

        //// DepthOfField
        if (_Foldout("Depth of Field"))
        {
            EditorGUILayout.PropertyField(dof.FindPropertyRelative("enableDof"), new GUIContent(" Enable DoF"));
            if (EditorGUILayout.BeginFadeGroup(showEnableDoF.faded))
            {
                EditorGUILayout.IntSlider(dof.FindPropertyRelative("quality"), 0, YebisLib.GetMaxDepthOfFieldQuality(), new GUIContent(" Quality"));
                EditorGUILayout.PropertyField(dof.FindPropertyRelative("apertureShape"), new GUIContent(" ApertureShape"));
                EditorGUILayout.Slider(dof.FindPropertyRelative("filmSize"), 0.001f, 10.0f, new GUIContent(" FilmSize"));
                EditorGUILayout.Slider(dof.FindPropertyRelative("apertureFnumber"), 0.12f, 16.0f, new GUIContent(" ApertureFnumber"));
                EditorGUILayout.PropertyField(dof.FindPropertyRelative("autoFocus"), new GUIContent(" AutoFocus"));
                EditorGUI.BeginDisabledGroup(autoFocus);
                EditorGUILayout.PropertyField(dof.FindPropertyRelative("focusDistance"), new GUIContent(" FocusDistance"));
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.PropertyField(dof.FindPropertyRelative("visualizeApertureFilterLevel"), new GUIContent(" VisualizeApertureFilterLevel"));

            }
            EditorGUILayout.EndFadeGroup();
        }

        //// LensSimulation
        if (_Foldout("Lens Simulation"))
        {
            EditorGUILayout.PropertyField(lensSimulation.FindPropertyRelative("lensDistortion"), new GUIContent(" LensDistortion"));
            EditorGUILayout.PropertyField(lensSimulation.FindPropertyRelative("anamorphicLens"), new GUIContent(" AnamorphicLens"));
            EditorGUILayout.PropertyField(lensSimulation.FindPropertyRelative("vignetteMode"), new GUIContent(" VignetteMode"));
            if (EditorGUILayout.BeginFadeGroup(showEnableVignette_Effect.faded))
            {
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("vignetteIntensity"), 0.0f, 2.0f, new GUIContent("  VignetteIntensity"));
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("vignetteFovDependence"), 0.0f, 4.0f, new GUIContent("  VignetteFovDependence"));
            }
            EditorGUILayout.EndFadeGroup();
            if (EditorGUILayout.BeginFadeGroup(showEnableVignette_Simulation.faded))
            {
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("powerOfCosine"), 0.0f, 4.0f, new GUIContent("  PowerOfCosine"));
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("imageCircle"), 0.0f, 4.0f, new GUIContent("  ImageCircle"));
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("penumbraWidth"), 0.0f, 10.0f, new GUIContent("  PenumbraWidth"));
                EditorGUILayout.Slider(lensSimulation.FindPropertyRelative("imageCircleFovDependence"), -4.0f, 4.0f, new GUIContent("  ImageCircleFovDependence"));
                EditorGUILayout.PropertyField(lensSimulation.FindPropertyRelative("opticalVignetting"), new GUIContent("  OpticalVignetting"));
            }
            EditorGUILayout.EndFadeGroup();
        }

        //// ColorCorrection
        if (_Foldout("Color Correction"))
        {
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorHue"), -180.0f, 180.0f, new GUIContent(" Hue"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorSaturation"), -1.0f, 3.0f, new GUIContent(" Saturation"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorContrast"), -1.0f, 2.0f, new GUIContent(" Contrast"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorBrightness"), 0.25f, 4.0f, new GUIContent(" Brightness"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorSepia"), 0.0f, 1.0f, new GUIContent(" Sepia"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorTemperature"), 2019.0f, 25000.0f, new GUIContent(" ColorTemperature"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorWhiteBalance"), 2019.0f, 25000.0f, new GUIContent(" ColorWhiteBalance"));
            EditorGUILayout.Slider(colorCorrection.FindPropertyRelative("colorGamma"), 0.31f, 7.18f, new GUIContent(" Gamma"));
        }

        //// Motion Blur
        if (_Foldout("MotionBlur"))
        {
            EditorGUILayout.PropertyField(motionBlur.FindPropertyRelative("enableMotionBlur"), new GUIContent(" Enable MotionBlur"));
            if (EditorGUILayout.BeginFadeGroup(showEnableMotionBlur.faded))
            {
                EditorGUILayout.Slider(motionBlur.FindPropertyRelative("blurTimeRatio"), 0.0f, 1.0f, new GUIContent("  BlurTimeRatio"));
                EditorGUILayout.Slider(motionBlur.FindPropertyRelative("maxBlurLength"), 0.0f, 1.0f, new GUIContent("  MaxBlurLength"));
            }
            EditorGUILayout.EndFadeGroup();
        }

        //// SSAO
        if (_Foldout("SSAO"))
        {
            EditorGUILayout.PropertyField(sSAO.FindPropertyRelative("enableSSAO"), new GUIContent(" Enable SSAO"));
            if (EditorGUILayout.BeginFadeGroup(showEnableSSAO.faded))
            {
                EditorGUILayout.PropertyField(sSAO.FindPropertyRelative("color"), new GUIContent("  Color"));
                EditorGUILayout.IntSlider(sSAO.FindPropertyRelative("samples"), 9, 64, new GUIContent("  Samples"));
                EditorGUILayout.Slider(sSAO.FindPropertyRelative("sampleRadius"), 0.001f, 100.0f, new GUIContent("  SampleRadius"));
                EditorGUILayout.Slider(sSAO.FindPropertyRelative("scale"), 0.0f, 2.0f, new GUIContent("  Scale"));
                EditorGUILayout.Slider(sSAO.FindPropertyRelative("contrast"), 0.0f, 1.0f, new GUIContent("  Contrast"));
                EditorGUILayout.Slider(sSAO.FindPropertyRelative("bias"), 0.0f, 1.0f, new GUIContent("  Bias"));
                EditorGUILayout.Slider(sSAO.FindPropertyRelative("epsilon"), 0.0f, 0.1f, new GUIContent("  Epsilon"));
            }
            EditorGUILayout.EndFadeGroup();
        }


        //// showFeedbackEffect
        if (_Foldout("Feedback Effect"))
        {
            EditorGUILayout.PropertyField(feedbackEffect.FindPropertyRelative("feedbackEffect"), new GUIContent(" Enable Feedback Effect"));
            if (EditorGUILayout.BeginFadeGroup(showEnableFeedbackEffect.faded))
            {
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackWeight"), 0.0f, 0.99f, new GUIContent("  FeedbackWeight"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackRotation"), -3.0f, 3.0f, new GUIContent("  FeedbackRotation"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackScaling"), 0.95f, 1.05f, new GUIContent("  FeedbackScaling"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackHue"), -10.0f, 10.0f, new GUIContent("  FeedbackHue"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackSaturation"), 0.9f, 1.1f, new GUIContent("  FeedbackSaturation"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackContrast"), 0.9f, 1.1f, new GUIContent("  FeedbackContrast"));
                EditorGUILayout.Slider(feedbackEffect.FindPropertyRelative("feedbackBrightness"), 0.9f, 1.1f, new GUIContent("  FeedbackBrightness"));
            }
            EditorGUILayout.EndFadeGroup();
        }


        serObj.ApplyModifiedProperties();
    }
}