using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using System.IO;


public class AlgLayeredMaterialEditor : MaterialEditor
{
    static class Utils
    {
        [System.Diagnostics.Conditional("ALG_VERBOSE")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
            ErrorMessages.Add(message);
        }

        static Dictionary<string, List<string>> s_ErrorMessages = new Dictionary<string, List<string>>();
        public static string s_CurrentLogScope = "";

        public static List<string> ErrorMessages
        {
            get
            {
                if (!s_ErrorMessages.ContainsKey(s_CurrentLogScope))
                {
                    s_ErrorMessages[s_CurrentLogScope] = new List<string>();
                }
                return s_ErrorMessages[s_CurrentLogScope];
            }
        }
    }

    #region Data
    public class MaterialLayeringData
    {
        public class Shader
        {
            public class Material
            {
                public string m_Name;
                public string m_ResourcePath;

                public ProceduralMaterial RetrieveProceduralMaterial()
                {
                    ProceduralMaterial result = null;

                    string[] pathParts = m_ResourcePath.Split('/');

                    if (pathParts.Length >= 2)
                    {
                        string sbsarRawName = pathParts[pathParts.Length - 2].Replace(".sbsar", "");
                        string pmRawName = pathParts[pathParts.Length - 1];

                        string[] foundAssets = AssetDatabase.FindAssets("t:SubstanceArchive " + sbsarRawName);

                        foreach (string sbsarGUID in foundAssets)
                        {
                            string sbsarPath = AssetDatabase.GUIDToAssetPath(sbsarGUID);

                            SubstanceImporter si = AssetImporter.GetAtPath(sbsarPath) as SubstanceImporter;

                            foreach (ProceduralMaterial pm in si.GetMaterials())
                            {
                                Utils.Log(pm.name);
                                if (result == null)
                                    result = pm;
                                else if (pm.name == pmRawName)
                                    result = pm;
                            }
                        }
                        Utils.Log("Trying to find ProceduralMaterial named " + sbsarRawName + "/" + pmRawName + "... " + (result == null ? " not found!" : "found: " + result.name));
                    }

                    return result;
                }
            }

            public string m_Name;
            public string m_PainterShaderName;
            public List<Material> m_Materials = new List<Material>();
            public JSONNode m_Parameters;
        }

        public class TextureSet
        {
            public string m_Name;
            public string m_Shader;
            public List<string> m_Masks = new List<string>();
            public List<string> m_PackedMasks = new List<string>();

            public void ParseMasks(JSONNode masksJson)
            {
                foreach (JSONNode maskStackJson in masksJson.Childs)
                {
                    string opacity = maskStackJson["Opacity"];
                    if(opacity == null)
                    {
                        opacity = maskStackJson["•s“§–¾“x"];
                    }

                    if (opacity != null)
                    {
                        m_Masks.Add(opacity);
                        Utils.Log("Added mask " + opacity);
                    }
                    else
                    {
                        Debug.LogError("nomask");
                    }
                }
            }
        }

        public List<Shader> m_Shaders = new List<Shader>();
        public List<TextureSet> m_TextureSets = new List<TextureSet>();
        public string m_MeshNormalsPath;

        public void ParseFormat(JSONNode formatJson)
        {
            if (!Mathf.Approximately(formatJson["version"].AsFloat, 1.0f))
            {
                throw new System.Exception("Unsupported version");
            }
        }

        public void ParseShaders(JSONNode shadersJson)
        {
            foreach (KeyValuePair<string, JSONNode> kvp in shadersJson.AsObject)
            {
                ParseShader(kvp.Key, kvp.Value);
            }
        }
        void ParseShader(string name, JSONNode shaderJson)
        {
            Shader shader = new Shader();
            shader.m_Name = name;
            shader.m_PainterShaderName = shaderJson["shader"];
            shader.m_Parameters = shaderJson["parameters"];

            foreach (KeyValuePair<string, JSONNode> kvp in shaderJson["materials"].AsObject)
            {
                Utils.Log(kvp.Key + " = " + kvp.Value);
                Shader.Material mat = new Shader.Material();
                mat.m_Name = kvp.Key;
                mat.m_ResourcePath = kvp.Value;
                shader.m_Materials.Add(mat);
            }

            m_Shaders.Add(shader);
            Utils.Log("Parsed shader " + shader.m_Name);

            if (shader.m_PainterShaderName != "pbr-material-layering")
            {
                Utils.LogError("Substance Painter shader \"" + shader.m_PainterShaderName + "\" is not supported by this tool. Pleas use shader \"pbr-material-layering\" instead and re-export from Substance Painter.");
            }
        }

        public void ParseTextureSets(JSONNode textureSetsJson)
        {
            foreach (KeyValuePair<string, JSONNode> kvp in textureSetsJson.AsObject)
            {
                ParseTextureSet(kvp.Key, kvp.Value);
            }
        }

        void ParseTextureSet(string name, JSONNode textureSetJson)
        {
            TextureSet textureSet = new TextureSet();
            textureSet.m_Name = name;
            textureSet.m_Shader = textureSetJson["shader"];
            textureSet.ParseMasks(textureSetJson["stacks"]);
            m_TextureSets.Add(textureSet);
            Utils.Log("Parsed texture set " + textureSet.m_Name);
        }

        public void Load(string jsonPath, string targetPath)
        {
            Debug.Log(m_TextureSets.Count());
            foreach (TextureSet textureSet in m_TextureSets)
            {
                EditorUtility.DisplayProgressBar("Importing .json", "Loading masks for TextureSet " + textureSet.m_Name, 0.2f);
                List<Texture2D> rawMasks = new List<Texture2D>();
                Debug.Log(textureSet.m_Masks.Count());
                foreach (string maskName in textureSet.m_Masks)
                {
                    string filePath = jsonPath + "/" + maskName;
                    Debug.Log(filePath);
                    if (File.Exists(filePath))
                    {

                        byte[] fileData = File.ReadAllBytes(filePath);
                        Texture2D texture = new Texture2D(2, 2);
                        texture.LoadImage(fileData);

                        rawMasks.Add(texture);
                    }
                    else
                    {
                        Utils.LogError(filePath + " does not exist.");
                    }
                }

                int currentPackedTexture = 0;
                while (rawMasks.Count >= currentPackedTexture * 4)
                {
                    EditorUtility.DisplayProgressBar("Importing .json", "Packing masks for TextureSet " + textureSet.m_Name, 0.6f);

                    Texture2D packedMasksTexture = PackMasks(rawMasks.GetRange(currentPackedTexture * 4, Mathf.Min(4, rawMasks.Count - (currentPackedTexture * 4))));

                    string filename = targetPath + "/" + textureSet.m_Name + "_masks" + (currentPackedTexture > 0 ? (currentPackedTexture + 1).ToString() : "") + ".png";
                    Utils.Log("Writing packed masks file " + filename);
                    
                    File.WriteAllBytes(filename, packedMasksTexture.EncodeToPNG());


                    EditorUtility.DisplayProgressBar("Importing .json", "Importing packed masks for TextureSet " + textureSet.m_Name, 0.8f);
                    AssetDatabase.Refresh();
                    TextureImporter ti = AssetImporter.GetAtPath(filename) as TextureImporter;
                    ti.linearTexture = true;
                    ti.SaveAndReimport();

                    DestroyImmediate(packedMasksTexture);

                    textureSet.m_PackedMasks.Add(filename);

                    currentPackedTexture++;
                }

                foreach (Texture2D rawMask in rawMasks)
                {
                    DestroyImmediate(rawMask);
                }
            }

            // Guess normal files
            string[] files = Directory.GetFiles(jsonPath, "*Normal*.png");
            foreach (string file in files)
            {
                m_MeshNormalsPath = targetPath + "/meshnormals.png";
                Utils.Log("Found " + file + " - copying to " + m_MeshNormalsPath);
                File.Copy(file, m_MeshNormalsPath, true);

                AssetDatabase.Refresh();
                TextureImporter ti = AssetImporter.GetAtPath(m_MeshNormalsPath) as TextureImporter;
                ti.normalmap = true;
                ti.SaveAndReimport();

                break;
            }

        }

        TextureSet FindTextureSet(string name)
        {
            foreach (TextureSet textureSet in m_TextureSets)
            {
                if (textureSet.m_Name == name)
                    return textureSet;
            }

            if (m_TextureSets.Count == 1)
            {
                // If we have only one texture set, bypass name check.
                return m_TextureSets[0];
            }

            return null;
        }

        Shader FindShader(string name)
        {
            foreach (Shader shader in m_Shaders)
            {
                if (shader.m_Name == name)
                    return shader;
            }
            return null;
        }

        public Shader FindTextureSetShader(string name)
        {
            TextureSet textureSet = FindTextureSet(name);
            if (textureSet == null)
            {
                return null;
            }

            return FindShader(textureSet.m_Shader);
        }

        public void AssignToMaterial(Material target, out List<Material> materials, out List<Texture2D> masks, ref Texture2D normal)
        {
            materials = null;
            masks = null;

            // Find textureset & get masks
            TextureSet textureSet = FindTextureSet(target.name);
            if (textureSet == null)
                return;

            masks = new List<Texture2D>();
            foreach (string maskPath in textureSet.m_PackedMasks)
            {
                masks.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(maskPath));
            }


            // Find textureset shader
            Shader shader = FindShader(textureSet.m_Shader);

            materials = new List<Material>(shader.m_Materials.Count);
            for (int i = 0; i < shader.m_Materials.Count; i++)
                materials.Add(null);

            foreach (Shader.Material material in shader.m_Materials)
            {
                ProceduralMaterial loadedMaterial = material.RetrieveProceduralMaterial();
                if (loadedMaterial == null)
                {
                    Utils.LogError("Could not find referenced .sbsar file: " + material.m_ResourcePath + ". Make sure the file is in your Unity project and load json file again.");
                }
                MaterialConfigurationData data = new MaterialConfigurationData();
                data.m_Material = loadedMaterial;
                ApplyMaterial(target, data, material.m_Name);

                int matIndex;
                if (int.TryParse(material.m_Name.Replace("Material", ""), out matIndex))
                {
                    matIndex--;
                    materials[matIndex] = loadedMaterial;
                }
            }

            normal = AssetDatabase.LoadAssetAtPath<Texture2D>(m_MeshNormalsPath);
        }

        Texture2D PackMasks(List<Texture2D> masks)
        {
            if (masks.Count == 0)
                return null;

            Texture2D result = new Texture2D(masks[0].width, masks[0].height, TextureFormat.ARGB32, false, true);

            for (int i = 0; i < result.width; i++)
            {
                for (int j = 0; j < result.height; j++)
                {
                    result.SetPixel(i, j, new Color(
                        masks[0].GetPixel(i, j).grayscale,
                        masks.Count > 1 ? masks[1].GetPixel(i, j).grayscale : 0f,
                        masks.Count > 2 ? masks[2].GetPixel(i, j).grayscale : 0f,
                        masks.Count > 3 ? masks[3].GetPixel(i, j).grayscale : 1f));
                }
            }

            result.Apply();
            return result;
        }
    }

    // MATERIALS BLENDED
    class MaterialConfigurationData
    {
        public Material m_Material;
    }
    #endregion

    List<MaterialConfigurationData> m_MaterialConfigurations = new List<MaterialConfigurationData>();

    public Texture2D m_MaskTexture;
    public Texture2D m_NormalTexture;

    bool m_ProgressBarCleared = true;

    // Cached material parameters
    bool m_UseNormalFromMask;

    Material Target
    {
        get { return target as Material; }
    }

    GUIStyle m_TitlesStyle;
    GUIStyle TitlesStyle
    {
        get
        {
            if (m_TitlesStyle == null)
            {
                m_TitlesStyle = new GUIStyle();
                m_TitlesStyle.fontStyle = FontStyle.Bold;
                m_TitlesStyle.normal.textColor = Color.grey;
                m_TitlesStyle.alignment = TextAnchor.MiddleCenter;
            }
            return m_TitlesStyle;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Utils.s_CurrentLogScope = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Target));
        // Create 4 sub-materials
        m_MaterialConfigurations = new List<MaterialConfigurationData>();
        for (int i = 0; i < 4; i++)
            m_MaterialConfigurations.Add(new MaterialConfigurationData());

        // Check material asset user data and load previous saved configuration
        AssetImporter ai = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(target));
        if (ai != null && ai.userData != null)
        {
            JSONNode userData = JSON.Parse(ai.userData);
            if (userData != null)
            {
                for (int i = 0; i < m_MaterialConfigurations.Count; i++)
                {
                    if (userData["mat" + (i + 1).ToString()] != null)
                    {
                        m_MaterialConfigurations[i].m_Material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(userData["mat" + (i + 1).ToString()].Value));
                    }
                }
            }
        }

        // Retrieve initial material configuration
        if (Target != null)
            m_UseNormalFromMask = Target.shaderKeywords.Contains("USE_NORMAL_FROM_MASK");
    }

    public override void OnInspectorGUI()
    {
        if (!m_ProgressBarCleared)
        {
            m_ProgressBarCleared = true;
            EditorUtility.ClearProgressBar();
        }

        EditorGUI.BeginChangeCheck();

        // Warn if colorspace is not linear
        if (PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            GUILayout.Label("Color space mismatch", TitlesStyle);
            GUILayout.BeginVertical("box");
            EditorGUILayout.HelpBox("You need to set the project color space to *linear* for correct rendering. (Go to edit -> project settings -> player)", MessageType.Error);
            GUILayout.EndVertical();
        }

        // Warn if colorspace is not linear
        if (PlayerSettings.renderingPath != RenderingPath.DeferredShading)
        {
            GUILayout.Label("Rendering path", TitlesStyle);
            GUILayout.BeginVertical("box");
            EditorGUILayout.HelpBox("Depending on your hardware, the shader might not work. Try to set rendering path to *Deferred*. (Go to edit -> project settings -> player)", MessageType.Warning);
            GUILayout.EndVertical();
        }

        if (Utils.ErrorMessages.Count > 0)
        {
            GUILayout.Label("Errors", TitlesStyle);
            GUILayout.BeginVertical("box");
            foreach (string error in Utils.ErrorMessages)
            {
                EditorGUILayout.HelpBox(error, MessageType.Error);
            }
            GUILayout.EndVertical();
        }

        //LOAD LAYERED MATERIAL
        if (GUILayout.Button("Load .json file", GUILayout.Height(50)))
        {
            Utils.ErrorMessages.Clear();
            LoadJson(Target);
        }

        //Materials Slots
        GUILayout.Label("Materials", TitlesStyle);
        GUILayout.BeginVertical("box");

        for (int i = 0; i < m_MaterialConfigurations.Count; i++)
        {
            OnInspectorMaterial(ref m_MaterialConfigurations[i].m_Material, i + 1);
        }

        GUILayout.EndVertical();

        //Mask Slots
        GUILayout.Label("Masks", TitlesStyle);
        GUILayout.BeginVertical("box");
        m_MaskTexture = (Texture2D)EditorGUILayout.ObjectField("Composite Mask", Target.GetTexture("_Mask"), typeof(Texture2D), false);
        Target.SetTexture("_Mask", m_MaskTexture);
        GUILayout.EndVertical();

        //Normal Slots
        GUILayout.Label("Mesh", TitlesStyle);
        GUILayout.BeginVertical("box");
        m_NormalTexture = (Texture2D)EditorGUILayout.ObjectField("Mesh normal", Target.GetTexture("BaseNormal"), typeof(Texture2D), false);
        Target.SetTexture("BaseNormal", m_NormalTexture);
        GUILayout.EndVertical();

        //Shader Parameters
        GUILayout.Label("Parameters", TitlesStyle);
        GUILayout.BeginVertical("box");


        bool tmp = EditorGUILayout.Toggle("Generate normals from masks", m_UseNormalFromMask);
        if (tmp != m_UseNormalFromMask)
        {
            m_UseNormalFromMask = tmp;
            SetupUseNormalFromMask();
        }


        Object[] materialList = new Object[1];
        materialList[0] = Target;
        for (int i = 0; i < m_MaterialConfigurations.Count; i++)
        {
            OnInspectorMaterialParameters(materialList, i + 1);
        }

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            PropertiesChanged();
            SaveUserData();

            for (int i = 0; i < m_MaterialConfigurations.Count; i++)
            {
                MaterialConfigurationData data = m_MaterialConfigurations[i];
                if (data.m_Material != null)
                {
                    ApplyMaterial(Target, data, "Material" + (i + 1).ToString());
                }
            }
        }
    }

    void OnInspectorMaterial(ref Material material, int number)
    {
        GUILayout.BeginHorizontal();
        material = (Material)EditorGUILayout.ObjectField("Material " + number.ToString(), material, typeof(Material), false);
        if (material == null)
        {
            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            Texture basecolor = (Texture)EditorGUILayout.ObjectField("Material " + number.ToString() + " basecolor", Target.GetTexture("_Material" + number.ToString() + "_Color") as Texture, typeof(Texture), false);
            Texture metallic = (Texture)EditorGUILayout.ObjectField("Material " + number.ToString() + " metallic", Target.GetTexture("_Material" + number.ToString() + "_RM") as Texture, typeof(Texture), false);
            Texture normal = (Texture)EditorGUILayout.ObjectField("Material " + number.ToString() + " normal", Target.GetTexture("_Material" + number.ToString() + "_Normal") as Texture, typeof(Texture), false);
            if (EditorGUI.EndChangeCheck())
            {
                Target.SetTexture("_Material" + number.ToString() + "_Color", basecolor);
                Target.SetTexture("_Material" + number.ToString() + "_RM", metallic);
                Target.SetTexture("_Material" + number.ToString() + "_Normal", normal);
            }
        }
        else
        {
            if (GUILayout.Button("Set maps manually"))
            {
                material = null;
            }
            GUILayout.EndHorizontal();
        }
    }

    void OnInspectorMaterialParameters(Object[] materialList, int number)
    {
        GUILayout.Label("Material " + number.ToString(), TitlesStyle);
        GUILayout.BeginVertical("box");
        DrawProperty(MaterialEditor.GetMaterialProperty(materialList, "Material" + number.ToString() + "_Scale"), "Material " + number.ToString() + " coords");
        DrawProperty(MaterialEditor.GetMaterialProperty(materialList, "Material" + number.ToString() + "_NormalIntensity"), "Normal intensity " + number.ToString());

        if (m_UseNormalFromMask && number > 1)
        {
            DrawProperty(MaterialEditor.GetMaterialProperty(materialList, "Material" + number.ToString() + "_NormalFromMaskIntensity"), "Normal from Mask Intensity " + number.ToString());
            DrawProperty(MaterialEditor.GetMaterialProperty(materialList, "Material" + number.ToString() + "_NormalFromMaskOffset"), "Normal from Mask " + (number - 1).ToString() + " Offset");
        }
        GUILayout.EndVertical();
    }

    public void DrawProperty(MaterialProperty prop, string displayName)
    {
        prop.floatValue = EditorGUILayout.Slider(displayName, prop.floatValue, prop.rangeLimits.x, prop.rangeLimits.y);
    }

    void SaveUserData()
    {
        JSONClass userData = new JSONClass();

        for (int i = 0; i < m_MaterialConfigurations.Count; i++)
        {
            MaterialConfigurationData data = m_MaterialConfigurations[i];
            if (data.m_Material != null)
            {
                userData["mat" + (i + 1).ToString()] = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(data.m_Material));
            }
        }

        AssetImporter ai = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(target));
        ai.userData = userData.ToString();
    }

    static void ApplyMaterial(Material material, MaterialConfigurationData materialData, string matName)
    {
        ProceduralMaterial proceduralMaterial = materialData.m_Material as ProceduralMaterial;

        if (proceduralMaterial != null)
        {
            Utils.Log("Setting procmat " + proceduralMaterial + " on property " + matName);
            material.SetTexture("_" + matName + "_Color", proceduralMaterial.GetGeneratedTexture(proceduralMaterial.name + "_basecolor"));
            material.SetTexture("_" + matName + "_RM", proceduralMaterial.GetGeneratedTexture(proceduralMaterial.name + "_metallic"));
            material.SetTexture("_" + matName + "_Normal", proceduralMaterial.GetGeneratedTexture(proceduralMaterial.name + "_normal"));
        }
        else if (materialData.m_Material != null)
        {
            material.SetTexture("_" + matName + "_Color", materialData.m_Material.GetTexture("_MainTex"));
            material.SetTexture("_" + matName + "_RM", materialData.m_Material.GetTexture("_MetallicGlossMap"));
            material.SetTexture("_" + matName + "_Normal", materialData.m_Material.GetTexture("_BumpMap"));
        }
    }

    #region Loading
    public string LoadJson(Material targetMaterial)
    {
        string path = EditorUtility.OpenFilePanel("Open Layered Material Definition", "", "json");
        if (path.Length != 0)
        {
            m_ProgressBarCleared = false;
            EditorUtility.DisplayProgressBar("Importing .json", "Please wait...", 0f);
            //Load JSON file
            StreamReader sr = new StreamReader(path);
            string fileContents = sr.ReadToEnd();
            sr.Close();
            var N = JSON.Parse(fileContents);

            // Generate MaterialLayeringData from json file
            MaterialLayeringData data = new MaterialLayeringData();
            data.ParseFormat(N["format"]);
            data.ParseShaders(N["shaders"]);
            data.ParseTextureSets(N["texturesets"]);
            
            string mainMaterialPath = AssetDatabase.GetAssetPath(target);
            Debug.Log(mainMaterialPath);
            mainMaterialPath = mainMaterialPath.Substring(0, mainMaterialPath.LastIndexOf('/'));
            Debug.Log(mainMaterialPath);
            Debug.Log(path.Substring(0, path.LastIndexOf('/')));
            // Load the parsed data
            data.Load(path.Substring(0, path.LastIndexOf('/')), mainMaterialPath);

            // Assign generated data
            List<Material> mats;
            List<Texture2D> masks;
            data.AssignToMaterial(Target, out mats, out masks, ref m_NormalTexture);

            if (mats != null)
            {
                for (int i = 0; i < Mathf.Min(m_MaterialConfigurations.Count, mats.Count); i++)
                {
                    m_MaterialConfigurations[i].m_Material = mats[i];
                }
            }

            if (masks != null)
            {
                m_MaskTexture = masks[0];
            }

            for (int i = 0; i < m_MaterialConfigurations.Count; i++)
            {
                ApplyMaterial(targetMaterial, m_MaterialConfigurations[i], "Material" + (i + 1).ToString());
            }

            targetMaterial.SetTexture("_Mask", m_MaskTexture);
            targetMaterial.SetTexture("BaseNormal", m_NormalTexture);

            // Get Shader Parameters from json parameters
            MaterialLayeringData.Shader dataShader = data.FindTextureSetShader(target.name);

            if (dataShader != null)
            {
                JSONNode parameters = dataShader.m_Parameters;

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        ParseMaterialParameters(targetMaterial, i + 1, parameters["Material " + (i + 1).ToString()]);
                    }
                    catch (System.Exception ex)
                    {
                        Utils.LogError("Could not parse material " + (i + 1) + " data: " + ex.Message);
                    }
                }

                try
                {
                    m_UseNormalFromMask = parameters["UseNormalFromMask"].AsBool;
                    SetupUseNormalFromMask();
                }
                catch (System.Exception ex)
                {
                    Utils.LogError("Could not parse parameters: " + ex.Message);
                }
            }
            else
            {
                Utils.LogError("Could not find textureset named \"" + target.name + "\" in .json file. Please make sure the material has the same name as the textureset in Substance Painter.");
            }

            SaveUserData();
            EditorUtility.ClearProgressBar();
            m_ProgressBarCleared = true;
        }
        return path.Substring(path.LastIndexOf('/') + 1);
    }

    void SetupUseNormalFromMask()
    {
        List<string> keywords = new List<string>(Target.shaderKeywords);
        if (m_UseNormalFromMask)
        {
            if (!keywords.Contains("USE_NORMAL_FROM_MASK"))
                keywords.Add("USE_NORMAL_FROM_MASK");
            if (keywords.Contains("IGNORE_NORMAL_FROM_MASK"))
                keywords.Remove("IGNORE_NORMAL_FROM_MASK");
        }
        else
        {
            if (keywords.Contains("USE_NORMAL_FROM_MASK"))
                keywords.Remove("USE_NORMAL_FROM_MASK");
            if (!keywords.Contains("IGNORE_NORMAL_FROM_MASK"))
                keywords.Add("IGNORE_NORMAL_FROM_MASK");
        }
        Target.shaderKeywords = keywords.ToArray();
    }

    void ParseMaterialParameters(Material material, int matIndex, JSONNode jsonData)
    {
        material.SetFloat("Material" + matIndex.ToString() + "_NormalIntensity", jsonData["normal_intensity" + matIndex.ToString()].AsFloat);
        material.SetFloat("Material" + matIndex.ToString() + "_Scale", jsonData["u_coords" + matIndex.ToString()].AsFloat);
        if (matIndex > 1)
        {
            material.SetFloat("Material" + matIndex.ToString() + "_NormalFromMaskIntensity", jsonData["mask_normal_intensity" + (matIndex - 1).ToString()].AsFloat);
            material.SetFloat("Material" + matIndex.ToString() + "_NormalFromMaskOffset", jsonData["mask_normal_offset" + (matIndex - 1).ToString()].AsFloat);
        }
    }
    #endregion

}
