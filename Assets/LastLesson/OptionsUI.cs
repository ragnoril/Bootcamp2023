using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{

    public class OptionsUI : MonoBehaviour
    {
        public Toggle FullScreen;
        public Toggle Vsync;

        public TMP_Dropdown ResolutionDropDown;
        public TMP_Dropdown QualityDropdown;

        Resolution[] resolutions;

        private void Start()
        {
            #region Resolution Setup

            resolutions = Screen.resolutions;
            ResolutionDropDown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for(int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            ResolutionDropDown.AddOptions(options);
            ResolutionDropDown.RefreshShownValue();
            ResolutionDropDown.value = currentResolutionIndex;
            #endregion

            int qualityLevel = QualitySettings.GetQualityLevel();
            QualityDropdown.value = qualityLevel;

            FullScreen.isOn = Screen.fullScreen;
            if (QualitySettings.vSyncCount == 0)
            {
                Vsync.isOn = false;
            }
            else
            {
                Vsync.isOn = true;
            }
        }

        public void SetQualityLevel(int qualityLevel)
        {
            QualitySettings.SetQualityLevel(qualityLevel);
            Debug.Log(QualitySettings.GetQualityLevel());
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Debug.Log(resolution);
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool fullScreen)
        {
            Screen.fullScreen = fullScreen;
        }

        public void SetVsync(bool vsync)
        {
            if (vsync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }

            Debug.Log(QualitySettings.vSyncCount);
        }


    }
}
