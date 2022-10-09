using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemUI : MonoBehaviour
{
    #region Public Properties

    public GameObject MainMenu, Settings;

    public GameObject SettingsFirstButton, SettingsCloseButton, LoadFirstButton, LoadCloseButton;

    #endregion

    public void OpenSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsFirstButton);
    }

    public void CloseSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsCloseButton);
    }

    public void LoadGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LoadFirstButton);
    }

    public void CancelLoad()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LoadCloseButton);
    }
}
