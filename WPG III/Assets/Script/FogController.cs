using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
    [Header("Fog Settings")]
    public bool enableFog = true;
    public Color fogColor = new Color(0.65f, 0.65f, 0.65f, 1f);
    public float targetFogDensity = 0.05f;
    public FogMode fogMode = FogMode.Exponential;

    [Header("Skybox Override")]
    public bool useSolidColorSky = true;
    public Color skyColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    [Header("Transition Settings")]
    public bool fadeInFog = true;
    public float fadeDuration = 3f; // waktu transisi fog muncul

    void Start()
    {
        // Atur mode fog dan warna
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogColor = fogColor;

        if (useSolidColorSky)
        {
            RenderSettings.skybox = null; // hapus skybox agar kabut menyatu
            if (Camera.main != null)
            {
                Camera.main.clearFlags = CameraClearFlags.SolidColor;
                Camera.main.backgroundColor = skyColor;
            }
        }

        // Kalau fade aktif, jalankan coroutine
        if (fadeInFog && enableFog)
            StartCoroutine(FadeInFog());
        else
            RenderSettings.fog = enableFog;
    }

    IEnumerator FadeInFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(0f, targetFogDensity, elapsed / fadeDuration);
            yield return null;
        }

        RenderSettings.fogDensity = targetFogDensity;
    }
}
