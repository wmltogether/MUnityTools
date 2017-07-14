using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UGUIRawAnimation), true)]
    public class UGUIRawAnimationEditor : GraphicEditor
    {
        GUIContent m_UVRectContent;
        void getProperty()
        {
            m_UVRectContent = new GUIContent("UV Rect");
            obj = new SerializedObject(target);
            m_Texture = obj.FindProperty("m_Texture");
            m_UVRect = obj.FindProperty("m_UVRect");
            m_Material = obj.FindProperty("m_Material");
            m_CellRow = obj.FindProperty("CellRow");
            m_CellNums = obj.FindProperty("CellNums");
            m_CellWidth = obj.FindProperty("CellWidth");
            m_CellHeight = obj.FindProperty("CellHeight");
            FPS = obj.FindProperty("FPS");
            IsPlaying = obj.FindProperty("IsPlaying");
            Foward = obj.FindProperty("Foward");
            AutoPlay = obj.FindProperty("AutoPlay");
            antype = obj.FindProperty("antype");
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
            GUILayout.Label("RawImage图集精灵");
            EditorGUILayout.PropertyField(m_Texture);
            GUILayout.Label("RawImage材质球");
            EditorGUILayout.PropertyField(m_Material);
            EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
            GUILayout.Label(" 精灵动画行列参数");
            EditorGUILayout.PropertyField(m_CellRow);
            EditorGUILayout.PropertyField(m_CellNums);
            EditorGUILayout.PropertyField(m_CellWidth);
            EditorGUILayout.PropertyField(m_CellHeight);
            GUILayout.Label(" 精灵动画播放参数");
            EditorGUILayout.PropertyField(FPS);
            EditorGUILayout.PropertyField(IsPlaying);
            EditorGUILayout.PropertyField(Foward);
            EditorGUILayout.PropertyField(AutoPlay);
            EditorGUILayout.PropertyField(antype);
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
        public SerializedProperty m_Texture;
        public new SerializedProperty m_Material;
        public SerializedProperty m_UVRect;
        public SerializedProperty m_CellRow;
        public SerializedProperty m_CellNums;
        public SerializedProperty m_CellWidth;
        public SerializedProperty m_CellHeight;
        
        public SerializedProperty FPS;
        public SerializedProperty IsPlaying;
        public SerializedProperty Foward;
        public SerializedProperty AutoPlay;
        public SerializedProperty antype;
    }

}
