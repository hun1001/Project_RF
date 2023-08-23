using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace InfinityPBR.BOTD
{
    [CustomEditor(typeof(WindController))]
    public class WindControllerEditor : Editor
    {
        private WindController m_windController;

        private void OnEnable()
        {
            if (m_windController == null)
            {
                m_windController = (WindController) target;
            }

            if (m_windController.WindNoiseTexture == null)
            {
                m_windController.WindNoiseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetAssetPath(BOTDShaderID.m_windNoiseTextureName));
            }
        }

        public override void OnInspectorGUI()
        {
            if (m_windController == null)
            {
                m_windController = (WindController) target;
            }

            if (m_windController != null)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                m_windController.gameObject.name = "BOTD Wind Controller";
                if (m_windController.m_windZone == null)
                {
                    EditorGUILayout.HelpBox("You are missing a wind zone in your scene please add one to apply wind.",
                        MessageType.Error);
                }

                m_windController.m_windZone = (WindZone) EditorGUILayout.ObjectField(new GUIContent("Wind Zone", "Set your scene wind zone here so the global shader paramaters can update the global wind based on the wind zone setup."), m_windController.m_windZone, typeof(WindZone), true);
                if (m_windController.WindNoiseTexture == null)
                {
                    EditorGUILayout.HelpBox("You are missing a wind noise texture. You need a noise texture to generate wind patterns.", MessageType.Error);
                }

                m_windController.WindNoiseTexture = (Texture2D) EditorGUILayout.ObjectField(new GUIContent("Wind Noise Texture", "Sets the noise texture used to generate random wind effect on vegetation."), m_windController.WindNoiseTexture, typeof(Texture2D), false, GUILayout.MaxHeight(16f));
                EditorGUI.indentLevel--;

                if (m_windController.m_windZone)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Wind Settings", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    m_windController.m_windZone.windMain = EditorGUILayout.Slider(new GUIContent("Wind Speed", "Sets the wind speed"), m_windController.m_windZone.windMain, 0f, 1f);
                    m_windController.m_windZone.windTurbulence = EditorGUILayout.Slider(new GUIContent("Wind Power", "Sets the wind power"), m_windController.m_windZone.windTurbulence, 0f, 1f);
                    m_windController.m_windZone.windPulseMagnitude = EditorGUILayout.Slider(new GUIContent("Wind Scale", "Sets the wind scale"), m_windController.m_windZone.windPulseMagnitude, 0f, 1f);
                    if (GUILayout.Button(new GUIContent("Revert To Defaults", "This will revert the wind values to the defaults")))
                    {
                        m_windController.RevertToDefaults();
                    }
                    EditorGUI.indentLevel--;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(m_windController);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }

        public static string GetAssetPath(string fileName)
        {
            string fName = Path.GetFileNameWithoutExtension(fileName);
            string[] assets = AssetDatabase.FindAssets(fName, null);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (Path.GetFileName(path) == fileName)
                {
                    return path;
                }
            }
            return "";
        }
    }
}