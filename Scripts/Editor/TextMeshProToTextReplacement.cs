using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class TextMeshProToTextReplacement : EditorWindow
{
    [MenuItem("My Tools/Replace TextMesh Pro with Text")]
    static void ReplaceTextMeshProWithText()
    {
        // 将所有场景物体的 TextMeshPro 组件替换为 Text 组件
        foreach (var textMeshPro in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            var gameObject = textMeshPro.gameObject;

            if (gameObject.GetComponent<Text>() == null)
            {
                Undo.RecordObject(gameObject, "Replace TextMeshPro with Text");

                // 保存 TextMeshPro 参数到中间变量
                var text = textMeshPro.text;
               // var font = new Font(textMeshPro.font.material);
                var fontSize = Mathf.RoundToInt(textMeshPro.fontSize);
                var color = textMeshPro.color;
                var alignment =textMeshPro.alignment;
                // 获取 TextMeshPro 字体文件名
                var fontFileName = textMeshPro.font.name;

                // 根据字体文件名查找对应的 Text 字体文件
                var textFont = FindMatchingFont(fontFileName);
                // 移除 TextMeshPro 组件
                Object.DestroyImmediate(textMeshPro);

                // 添加 Text 组件并设置参数
                var textComponent = gameObject.AddComponent<Text>();
                textComponent.text = text;
               
                textComponent.font = textFont;
                textComponent.fontSize = fontSize;
                textComponent.color = color;
                textComponent.resizeTextForBestFit = true;
                switch (alignment)
                {
                    case TextAlignmentOptions.TopLeft:
                        textComponent.alignment = TextAnchor.UpperLeft;
                        break;
                    case TextAlignmentOptions.Top:
                        textComponent.alignment = TextAnchor.UpperCenter;
                        break;
                    case TextAlignmentOptions.TopRight:
                        textComponent.alignment = TextAnchor.UpperRight;
                        break;
                    case TextAlignmentOptions.Left:
                        textComponent.alignment = TextAnchor.MiddleLeft;
                        break;
                    case TextAlignmentOptions.Center:
                        textComponent.alignment = TextAnchor.MiddleCenter;
                        break;
                    case TextAlignmentOptions.Right:
                        textComponent.alignment = TextAnchor.MiddleRight;
                        break;
                    case TextAlignmentOptions.BottomLeft:
                        textComponent.alignment = TextAnchor.LowerLeft;
                        break;
                    case TextAlignmentOptions.Bottom:
                        textComponent.alignment = TextAnchor.LowerCenter;
                        break;
                    case TextAlignmentOptions.BottomRight:
                        textComponent.alignment = TextAnchor.LowerRight;
                        break;
                }
            }
        }
        static Font FindMatchingFont(string fontFileName)
        {
            string fontfile= fontFileName.Substring(0, fontFileName.Length - 4);

            // 在同一文件夹下查找相似的字体文件
            var fontFolder = "Assets/Figma/Fonts/"; // 替换为你的字体文件夹路径
            var textFont = AssetDatabase.LoadAssetAtPath<Font>(fontFolder + fontfile + ".ttf");
            if (textFont == null)
            {
                Debug.LogError("Matching font not found for " + fontFolder + fontfile + ".ttf");
                // 如果找不到匹配的字体文件，可以根据需要进行其他处理
            }
            else
            {
                return null;
            }
            return textFont;
        }
        Debug.Log("Replaced all TextMeshPro components with Text components.");
    }
}
