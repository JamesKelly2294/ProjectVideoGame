using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGroup : MonoBehaviour
{
    private AudioManager _am;

    public Slider globalVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider environmentVolumeSlider;

    public TMP_Dropdown displayModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    Resolution[] resolutions;

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetGlobalVolume(float volume)
    {
        if(!_am) { return; }
        _am.SetGlobalVol(volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (!_am) { return; }
        _am.SetMusicVol(volume);
    }

    public void SetEnvironmentVolume(float volume)
    {
        if (!_am) { return; }
        _am.SetEnvironmentVol(volume);
    }
    public void SetSFXVolume(float volume)
    {
        if (!_am) { return; }
        _am.SetSFXVol(volume);
    }


    public void SetResolution(int resolutionIndex)
    {
        var resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);

        Debug.Log("Resolution updated - toggling enablement for text mesh pro text.");

        StartCoroutine(RedrawText());
    }

    IEnumerator RedrawText()
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("Redraw text.");
        // Seems to be a bug where TMPro text UGUI elements don't properly redraw for resolution changes?
        // ... this is a hacky workaround
        yield return new WaitForEndOfFrame();
        var textMeshProTexts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (var tmpText in textMeshProTexts)
        {
            tmpText.ForceMeshUpdate();
        }
    }

    public void SetDisplayMode(int index)
    {
        if (index == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (index == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (index == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _am = AudioManager.Instance;
        globalVolumeSlider.value = _am.GlobalVolume;
        musicVolumeSlider.value = _am.MusicVolume;
        sfxVolumeSlider.value = _am.SFXVolume;
        environmentVolumeSlider.value = _am.EnvironmentVolume;

        resolutions = Screen.resolutions.Where(r => r.refreshRate == 60 || r.refreshRate == 59).ToArray();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            var resolution = resolutions[i];
            options.Add(resolution.width + " x " + resolution.height + " @ " + resolution.refreshRate + "hz");
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        int currentQualityIndex = 0;
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            var quality = QualitySettings.names[i];
            if (quality == QualitySettings.names[QualitySettings.GetQualityLevel()])
            {
                currentQualityIndex = i;
            }
        }

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();

        switch(Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                displayModeDropdown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                displayModeDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                displayModeDropdown.value = 2;
                break;
            default:
                displayModeDropdown.value = 2;
                break;
        }
        displayModeDropdown.RefreshShownValue();

#if UNITY_WEBGL || UNITY_EDITOR
        displayModeDropdown.interactable = false;
        resolutionDropdown.interactable = false;

        var placeholderOptions = new List<string>();
        placeholderOptions.Add("Unsupported");

        displayModeDropdown.ClearOptions();
        resolutionDropdown.ClearOptions();

        displayModeDropdown.AddOptions(placeholderOptions);
        resolutionDropdown.AddOptions(placeholderOptions);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
