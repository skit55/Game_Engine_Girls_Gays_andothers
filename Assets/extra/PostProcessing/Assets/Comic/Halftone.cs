using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halftone : MonoBehaviour {
    public Shader halfToneShader;

    [Header("Cyan")]
    public bool printCyan = true;

    [Range(0.0f, 3.0f)]
    public float cyanDotSize = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float cyanBias = 0.0f;
    public Vector2 cyanOffset = new Vector2(0.0f, 0.0f);
    
    [Header("Magenta")]
    public bool printMagenta = true;
    
    [Range(0.0f, 3.0f)]
    public float magentaDotSize = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float magentaBias = 0.0f;
    public Vector2 magentaOffset = new Vector2(0.0f, 0.0f);
    
    [Header("Yellow")]
    public bool printYellow = true;
    
    [Range(0.0f, 3.0f)]
    public float yellowDotSize = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float yellowBias = 0.0f;
    public Vector2 yellowOffset = new Vector2(0.0f, 0.0f);
    
    [Header("Black")]
    public bool printBlack = true;
    
    [Range(0.0f, 3.0f)]
    public float blackDotSize = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float blackBias = 0.0f;
    public Vector2 blackOffset = new Vector2(0.0f, 0.0f);

    private Material halftoneMat;
    
    void OnEnable() {
        halftoneMat = new Material(halfToneShader);
        halftoneMat.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnDisable() {
        halftoneMat = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        halftoneMat.SetFloat("_CyanDotSize", cyanDotSize);
        halftoneMat.SetFloat("_MagentaDotSize", magentaDotSize);
        halftoneMat.SetFloat("_YellowDotSize", yellowDotSize);
        halftoneMat.SetFloat("_BlackDotSize", blackDotSize);
        halftoneMat.SetInt("_PrintCyan", printCyan ? 1 : 0);
        halftoneMat.SetFloat("_CyanBias", cyanBias);
        halftoneMat.SetVector("_CyanOffset", cyanOffset);
        halftoneMat.SetInt("_PrintMagenta", printMagenta ? 1 : 0);
        halftoneMat.SetFloat("_MagentaBias", magentaBias);
        halftoneMat.SetVector("_MagentaOffset", magentaOffset);
        halftoneMat.SetInt("_PrintYellow", printYellow ? 1 : 0);
        halftoneMat.SetFloat("_YellowBias", yellowBias);
        halftoneMat.SetVector("_YellowOffset", yellowOffset);
        halftoneMat.SetInt("_PrintBlack", printBlack ? 1 : 0);
        halftoneMat.SetFloat("_BlackBias", blackBias);
        halftoneMat.SetVector("_BlackOffset", blackOffset);

        var cmyk = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        Graphics.Blit(source, cmyk, halftoneMat, 0);
        Graphics.Blit(cmyk, destination, halftoneMat, 1);
        RenderTexture.ReleaseTemporary(cmyk);
    }
}
