using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UGUIRawSpriteSheet), true)]
    public class UGUIRawSpriteSheetEditor : GraphicEditor
    {
        GUIContent m_UVRectContent;
        void getProperty()
        {
            m_UVRectContent = new GUIContent("UV Rect");
            obj = new SerializedObject(target);
            m_Texture = obj.FindProperty("m_Texture");
            m_UVRect = obj.FindProperty("m_UVRect");
            m_Material = obj.FindProperty("m_Material");
            m_SpriteLeft = obj.FindProperty("SpriteLeft");
            m_SpriteTop = obj.FindProperty("SpriteTop");
            m_SpriteWidth = obj.FindProperty("SpriteWidth");
            m_SpriteHeight = obj.FindProperty("SpriteHeight");

        }

        void Awake()
        {
            
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            getProperty();
            SetShowNativeSize(true);
        }

        public override void OnInspectorGUI()
        {
            obj.Update();
            GUILayout.Label("RawImage 图集精灵");
            EditorGUILayout.PropertyField(m_Texture);
            GUILayout.Label("RawImage 图集材质球");
            EditorGUILayout.PropertyField(m_Material);
            EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
            GUILayout.Label(" 精灵参数（基于图片左上角）");
            EditorGUILayout.PropertyField(m_SpriteTop);
            EditorGUILayout.PropertyField(m_SpriteLeft);
            EditorGUILayout.PropertyField(m_SpriteWidth);
            EditorGUILayout.PropertyField(m_SpriteHeight);
            SetShowNativeSize(false);
            NativeSizeButtonGUI();
            obj.ApplyModifiedProperties();

        }

        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            UGUIRawSpriteSheet rawImage = target as UGUIRawSpriteSheet;
            Texture tex = rawImage.mainTexture;

            if (tex == null)
                return;

            Rect outer = rawImage.uvRect;
            outer.xMin *= rawImage.rectTransform.rect.width;
            outer.xMax *= rawImage.rectTransform.rect.width;
            outer.yMin *= rawImage.rectTransform.rect.height;
            outer.yMax *= rawImage.rectTransform.rect.height;
            UGUILibHax.DrawSprite(tex, rect, outer, rawImage.uvRect, rawImage.canvasRenderer.GetColor());
        }

        void SetShowNativeSize(bool instant)
        {
            base.SetShowNativeSize(m_Texture.objectReferenceValue != null, instant);
        }

        private SerializedObject obj;
        public SerializedProperty m_UVRect;
        public SerializedProperty m_SpriteTop;
        public SerializedProperty m_SpriteLeft;
        public SerializedProperty m_SpriteWidth;
        public SerializedProperty m_SpriteHeight;
        public SerializedProperty m_Texture;
        public new SerializedProperty m_Material;
    }


}
