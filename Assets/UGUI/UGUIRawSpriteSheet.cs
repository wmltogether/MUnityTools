using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace UnityEditor.UI
{
    public class UGUIRawSpriteSheet : MaskableGraphic
    {
        [FormerlySerializedAs("_MainTex")]
        [SerializeField]
        Texture m_Texture;
        [SerializeField]
        Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

        public float SpriteLeft;
        public float SpriteTop;
        public int SpriteWidth;
        public int SpriteHeight;
        


        public void UpdateUVRect(float left, float top, int width, int height)
        {
            m_UVRect = new Rect(left / (float)mainTexture.width,
                                1f - ((float)height + top) / (float)mainTexture.height,
                                (float)width / (float)mainTexture.width, 
                                (float)height / (float)mainTexture.height);
            SetVerticesDirty();
        }


        private float pre_SpriteTop = 0f;
        private float pre_SpriteLeft = 0f;
        private int pre_SpriteWidth = 1;
        private int pre_SpriteHeight = 1;

        private void Update()
        {
            if (!pre_SpriteTop.Equals(SpriteTop) || 
                !pre_SpriteLeft.Equals(SpriteLeft) || 
                !pre_SpriteWidth.Equals(SpriteWidth) || 
                !pre_SpriteHeight.Equals(SpriteHeight))
            {
                UpdateUVRect(SpriteLeft, SpriteTop, SpriteWidth, SpriteHeight);
                pre_SpriteTop = SpriteTop;
                pre_SpriteLeft = SpriteLeft;
                pre_SpriteWidth = SpriteWidth;
                pre_SpriteHeight = SpriteHeight;
            }
        }



        protected UGUIRawSpriteSheet()
        {
            useLegacyMeshGeneration = false;
        }

        public override Texture mainTexture
        {
            get
            {
                if (m_Texture == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return m_Texture;
            }
        }

        public Texture texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                if (m_Texture == value)
                    return;

                m_Texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public Rect uvRect
        {
            get
            {
                return m_UVRect;
            }
            set
            {
                if (m_UVRect == value)
                    return;
                m_UVRect = value;
                SetVerticesDirty();
            }
        }

        public override void SetNativeSize()
        {
            Texture tex = mainTexture;
            if (tex != null)
            {
                int w = Mathf.RoundToInt(tex.width * uvRect.width);
                int h = Mathf.RoundToInt(tex.height * uvRect.height);
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Texture tex = mainTexture;
            vh.Clear();
            if (tex != null)
            {
                var r = GetPixelAdjustedRect();
                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

                {
                    var color32 = color;
                    vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(m_UVRect.xMin, m_UVRect.yMin));
                    vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(m_UVRect.xMin, m_UVRect.yMax));
                    vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(m_UVRect.xMax, m_UVRect.yMax));
                    vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(m_UVRect.xMax, m_UVRect.yMin));

                    vh.AddTriangle(0, 1, 2);
                    vh.AddTriangle(2, 3, 0);
                }
            }
        }
    }
}
