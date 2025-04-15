using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class postProcess : MonoBehaviour
{
    [SerializeField] PostProcessVolume postProcessVolume;
    ColorGrading colorGrading;
    Bloom bloom;
    AmbientOcclusion ambientOcclusion;
    AutoExposure autoExposure;
    ChromaticAberration chromaticAberration;

    [SerializeField] GameObject visualSettings;
    [SerializeField] GameObject settingsBtn;

    [SerializeField] Slider colorGradingSlider;
    [SerializeField] Slider bloomSlider;
    [SerializeField] Slider ambientOcclusionSlider;
    [SerializeField] Slider autoExposureSlider;
    [SerializeField] Slider chromaticAberrationSlider;


    public void initializeValuesPostProcess()
    {
        if (postProcessVolume == null || postProcessVolume.profile == null)
        {
            return;
        }

        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            colorGradingSlider.onValueChanged.AddListener(changeColorGrading);
        }

        if (postProcessVolume.profile.TryGetSettings(out bloom))
        {
            bloomSlider.onValueChanged.AddListener(changeBloom);
        }

        if (postProcessVolume.profile.TryGetSettings(out ambientOcclusion))
        {
            ambientOcclusionSlider.onValueChanged.AddListener(changeAmbientOcclusion);
        }

        if (postProcessVolume.profile.TryGetSettings(out autoExposure))
        {
            autoExposureSlider.onValueChanged.AddListener(changeAutoExposure);
        }

        if (postProcessVolume.profile.TryGetSettings(out chromaticAberration))
        {
            chromaticAberrationSlider.onValueChanged.AddListener(changeChromaticAberration);
        }
    }

    public void enableSettings()
    {
        visualSettings.SetActive(true);
        settingsBtn.SetActive(false);

        autoExposureSlider.value = 0.5f;

    }

    public void disableSetting()
    {
        visualSettings.SetActive(false);
        settingsBtn.SetActive(true);
    }

    public void changeColorGrading(float value)
    {
        if (colorGrading != null)
        {
            colorGrading.saturation.value = value * 25;
            colorGrading.contrast.value = value * 25;

            postProcessManager.postProcessInstance.colorGradingSaturation = colorGrading.saturation.value;
            postProcessManager.postProcessInstance.colorGradingContrast = colorGrading.contrast.value;
        }
    }

    public void changeBloom(float value)
    {
        if (bloom != null)
        {
            bloom.intensity.value = value * 2.5f;
            postProcessManager.postProcessInstance.bloomIntensity = bloom.intensity.value;
        }
    }

    public void changeAmbientOcclusion(float value)
    {
        if (ambientOcclusion != null)
        {
            ambientOcclusion.intensity.value = value * 1.5f;
            postProcessManager.postProcessInstance.ambientOcclusionIntensity = ambientOcclusion.intensity.value;
        }
    }

    public void changeAutoExposure(float value)
    {
        if (autoExposure != null)
        {
            float exposureValue = Mathf.Lerp(0.5f, 1.5f, value);
            autoExposure.keyValue.value = exposureValue;
            postProcessManager.postProcessInstance.autoExposureValue = autoExposure.keyValue.value;
        }
    }

    public void changeChromaticAberration(float value)
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = value * 0.5f;
            postProcessManager.postProcessInstance.chromaticAberrationIntensity = chromaticAberration.intensity.value;
        }
    }
}
