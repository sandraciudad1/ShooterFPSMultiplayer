using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class postProcessManager : MonoBehaviour
{
    public static postProcessManager postProcessInstance;

    public float colorGradingSaturation = 0;
    public float colorGradingContrast = 0;
    public float bloomIntensity = 0;
    public float ambientOcclusionIntensity = 0;
    public float autoExposureValue = 0.5f;
    public float chromaticAberrationIntensity = 0;

    private void Awake()
    {
        if (postProcessInstance == null)
        {
            postProcessInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyPostProcessSettings(PostProcessVolume volume)
    {
        if (volume == null || volume.profile == null) return;

        if (volume.profile.TryGetSettings(out ColorGrading colorGrading))
        {
            colorGrading.saturation.value = colorGradingSaturation;
            colorGrading.contrast.value = colorGradingContrast;
        }

        if (volume.profile.TryGetSettings(out Bloom bloom))
        {
            bloom.intensity.value = bloomIntensity;
        }

        if (volume.profile.TryGetSettings(out AmbientOcclusion ambientOcclusion))
        {
            ambientOcclusion.intensity.value = ambientOcclusionIntensity;
        }

        if (volume.profile.TryGetSettings(out AutoExposure autoExposure))
        {
            autoExposure.keyValue.value = autoExposureValue;
        }

        if (volume.profile.TryGetSettings(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.value = chromaticAberrationIntensity;
        }
    }
}
