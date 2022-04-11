using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageOverlay : MonoBehaviour
{
    [Range(0f, 1f)] public float m_intensity = 0f;
    public Color m_damageColour = new Color {};
    public Texture m_overlayTexture;

    [Range(0f, 1f)] public float m_roundness = 0f;
    [Range(0f, 1f)] public float m_smoothness = 0f;
    [Range(0f, 1f)] public float m_opacity = 0f;

    [SerializeField] private Material m_shaderMaterial;
    
    const string ShaderPath = "Hidden/PostProcessing/DamageOverlay";
    private  Shader propertySheet;

    private void Awake()
    {
        var shader = Shader.Find(ShaderPath);
        if (shader == null)
        {
            Debug.LogError("Failed to find shader: " + ShaderPath);
            return;
        }

        // attempt to retrieve the property sheet
        propertySheet = m_shaderMaterial.shader;
        if (propertySheet == null)
        {
            Debug.LogError("Failed to retrieve property sheet for shader: " + ShaderPath);
            return;
        }
    }

    // attempt to retrieve the shader

    private void Update()
    {
        UpdateValues();
    }

    public void UpdateValues()
    {
        if (m_overlayTexture != null) {
            m_shaderMaterial.SetTexture("_OverlayTexture", m_overlayTexture);
            m_shaderMaterial.SetColor("_DamageColour", m_damageColour);
            m_shaderMaterial.SetFloat("_Intensity", Mathf.Lerp(0f, 5f, m_intensity));
            m_shaderMaterial.SetFloat("_Smoothness", Mathf.Lerp(0.1f, 5f, m_smoothness));
            m_shaderMaterial.SetFloat("_Roundness", Mathf.Lerp(5f, 1f, m_roundness));
            m_shaderMaterial.SetFloat("_Opacity", m_opacity);
        }
    }
    
}

