using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UITextToTMP : MonoBehaviour
{
    public class TextData
    {
        public string text; // ���ڿ� ����
        public Color color; // �ؽ�Ʈ �÷� ����
        public TMP_FontAsset font; // ��Ʈ ����
        public int fontSize; // ��Ʈ ������ ����
        public int fontMaxSize; // ��Ʈ �ִ� ������ ���� (AutoSize ��)
        public bool autoSize; // AutoSize ����
        public TextAlignmentOptions alignOptions; // ���� �ɼ�
        public FontStyles fontStyles; // ��Ʈ ��Ÿ�� �ɼ�
    }

    [MenuItem("Tools/UITextToTMP")]
    public static void ChangeTextMeshProUGUI()
    {
        GameObject[] rootObj = GetSceneRootObject();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject gbj = (GameObject)rootObj[i] as GameObject;
            Component[] com = gbj.transform.GetComponentsInChildren(typeof(Text), true);
            foreach (Text txt in com)
            {
                TextData data = ConvertTextData(txt);
                GameObject obj = txt.gameObject;
                DestroyImmediate(obj.GetComponent<Text>());
                if (!obj.GetComponent<TextMeshProUGUI>())
                {
                    TextMeshProUGUI newGUIText = obj.AddComponent<TextMeshProUGUI>();
                    newGUIText.text = data.text;
                    //newGUIText.font = data.font;
                    newGUIText.fontSize = data.fontSize;
                    newGUIText.color = data.color;
                    newGUIText.enableAutoSizing = data.autoSize;
                    newGUIText.fontSizeMax = data.fontMaxSize;
                    newGUIText.alignment = data.alignOptions;
                    newGUIText.fontStyle = data.fontStyles;
                }
            }
        }
    }

    private static TextData ConvertTextData(Text text)
    {
        TextData data = new TextData();
        data.text = text.text;
        data.color = text.color;
        //data.font
        data.fontSize = text.fontSize;
        data.fontMaxSize = text.resizeTextMaxSize;
        data.autoSize = text.resizeTextForBestFit;
        switch (text.alignment)
        {
            case TextAnchor.LowerCenter:
                data.alignOptions = TextAlignmentOptions.Bottom;
                break;
            case TextAnchor.LowerLeft:
                data.alignOptions = TextAlignmentOptions.BottomLeft;
                break;
            case TextAnchor.LowerRight:
                data.alignOptions = TextAlignmentOptions.BottomRight;
                break;
            case TextAnchor.MiddleCenter:
                data.alignOptions = TextAlignmentOptions.Midline;
                break;
            case TextAnchor.MiddleLeft:
                data.alignOptions = TextAlignmentOptions.Left;
                break;
            case TextAnchor.MiddleRight:
                data.alignOptions = TextAlignmentOptions.Right;
                break;
            case TextAnchor.UpperCenter:
                data.alignOptions = TextAlignmentOptions.Top;
                break;
            case TextAnchor.UpperLeft:
                data.alignOptions = TextAlignmentOptions.TopLeft;
                break;
            case TextAnchor.UpperRight:
                data.alignOptions = TextAlignmentOptions.TopRight;
                break;
        }
        switch (text.fontStyle)
        {
            case FontStyle.Bold:
                data.fontStyles = (FontStyles)FontStyle.Bold;
                break;
            case FontStyle.BoldAndItalic:
            case FontStyle.Italic:
                data.fontStyles = (FontStyles)FontStyle.Italic;
                break;
            default:
                data.fontStyles = (FontStyles)FontStyle.Normal;
                break;
        }

        return data;
    }

    private static GameObject[] GetSceneRootObject()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        return currentScene.GetRootGameObjects();
    }
}
