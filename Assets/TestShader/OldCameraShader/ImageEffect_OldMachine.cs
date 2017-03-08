using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/自定义效果/OldMachineShader")]
public class ImageEffect_OldMachine : MonoBehaviour
{
    public Texture textureRamp;

    [Range(-1.0f, 1.0f)]
    public float rampOffset;
    public Color color;
    private float _mr;
    private float _mg;
    private float _mb;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _mr = color.r * (float)255.0;
        _mg = color.g * (float)255.0;
        _mb = color.b * (float)255.0;
        material.SetTexture("_RampTex", textureRamp);
        material.SetFloat("_RampOffset", rampOffset);
        material.SetFloat("_mr", _mr);
        material.SetFloat("_mg", _mg);
        material.SetFloat("_mb", _mb);
        Graphics.Blit(source, destination, material);
    }

    public Shader shader;

    private Material m_Material;


    protected virtual void Start()
    {
        if (shader == null)
        {
            shader = Shader.Find("CameraEffect/OldMachineShader");
        }
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
            enabled = false;
    }


    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }


    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }
}
