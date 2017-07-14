using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public class UGUIRawAnimation : MaskableGraphic
    {

        [FormerlySerializedAs("_MainTex")]

        [SerializeField]
        Texture m_Texture;

        [SerializeField]
        Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);



        private int mCurFrame = 0;
        private float mDelta = 0;

        public int CellWidth = 0;
        public int CellHeight = 0;
        public int CellRow = 0;
        public int CellNums = 0;

        public float FPS = 5;
        public bool IsPlaying = false;
        public bool Foward = true;
        public bool AutoPlay = false;
        public bool Loop = false;
        public bool StopIt = false;
        public int time;
        public int antype;
        public int FrameCount
        {
            get
            {
                return CellNums;
            }
        }

        private int prevCellWidth;
        private int prevCellHeight;
        private int prevCellRow;
        private int prevCellNums;

        protected override void Start()
        {
            base.Start();
            if (AutoPlay)
            {
                Play();
            }
            else
            {
                IsPlaying = false;
            }
        }



        public void UpdateUVRect(float left, float top, int width, int height)
        {
            m_UVRect = new Rect(left / (float)mainTexture.width,
                                1f - ((float)height + top) / (float)mainTexture.height,
                                (float)width / (float)mainTexture.width,
                                (float)height / (float)mainTexture.height);
            SetVerticesDirty();
        }

        void PrevSet()
        {
            prevCellWidth = CellWidth;
            prevCellHeight = CellHeight;
            prevCellRow = CellRow;
            prevCellNums = CellNums;
        }

        void UpdateComponents()
        {
            if (this.texture != null)
            {
                if ((CellWidth != prevCellWidth)
                                              || (CellHeight != prevCellHeight)
                                              || (CellRow != prevCellRow)
                                              || (CellNums != prevCellNums))
                {
                    PrevSet();
                    SetSprite(mCurFrame);
                }
            }
        }

        public void SetSprite(int curFrame)
        {
            int frames = CellNums;
            int frame = fmod(curFrame, frames);
            int _CellCol = ((CellNums + CellRow - 1) & ~(CellRow - 1)) / CellRow;
            int left = fmod(frame, CellRow) * CellWidth;
            int top = (frame / CellRow) * CellHeight;
            this.UpdateUVRect(left, top, CellWidth, CellHeight);
        }


        float fmod(float x, float y)
        {
            return x % y;
        }

        int fmod(int x, int y)
        {
            return x % y;
        }


        #region 播放动画
        public void Play()
        {
            AutoPlay = true;
            IsPlaying = true;
            Foward = true;
        }


        public void PlayReverse()
        {
            IsPlaying = true;
            Foward = false;
        }
        Action callback = null;

        void Update()
        {
            UpdateComponents();
            if (!IsPlaying || 0 == FrameCount)
            {
                return;
            }

            mDelta += Time.deltaTime;
            if (mDelta > 1 / FPS)
            {
                mDelta = 0;
                if (Foward)
                {
                    mCurFrame++;
                }
                else
                {
                    mCurFrame--;
                }

                if (mCurFrame >= FrameCount)
                {
                    if (Loop)
                    {
                        mCurFrame = 0;
                        if (!StopIt)
                        {
                            FPS = 20;
                            if (time > 0)
                            {
                                mDelta = 0;
                                FPS = 30;
                                time--;
                            }
                            else
                            {
                                if (antype == 4)
                                {
                                    time = 20;
                                    mDelta = 0;
                                }
                                else if (antype == 5)
                                {
                                    mDelta = -UnityEngine.Random.Range(50, 250) / 100f;
                                    time = 20;
                                }
                                else
                                {
                                    mDelta = -UnityEngine.Random.Range(100, 500) / 100f;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (callback != null) callback.Invoke();
                        IsPlaying = false;
                        return;
                    }
                }
                else if (mCurFrame < 0)
                {
                    if (Loop)
                    {
                        mCurFrame = FrameCount - 1;
                    }
                    else
                    {
                        IsPlaying = false;
                        return;
                    }
                }

                SetSprite(mCurFrame);
            }
        }

        internal void SetCallBack(Action p)
        {
            callback = p;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            if (!IsPlaying)
            {
                IsPlaying = true;
            }
        }

        public void Stop()
        {
            mCurFrame = 0;
            SetSprite(mCurFrame);
            IsPlaying = false;
        }

        public void Rewind()
        {
            mCurFrame = 0;
            SetSprite(mCurFrame);
            Play();
        }


        internal void changeStatue(int earan)
        {
            if (earan == 3)
            {
                time = 3; antype = 1;
            }
            else if (earan == 4)
            {
                antype = 4;
            }
            else if (earan == 5)
            {
                antype = 5;
            }
            else
            {
                antype = 1;
            }
        }

        #endregion
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #region 原始rawimage参数


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

        #endregion

    }

}
