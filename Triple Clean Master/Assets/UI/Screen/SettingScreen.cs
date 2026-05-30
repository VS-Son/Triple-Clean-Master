using Project.Core.UI;
using Project.Manager;
using Project.Services;
using UnityEngine;
using AudioType = Project.Manager.AudioType;

namespace UI.Screen
{
    public class SettingScreen : UICanvas
    {
        [SerializeField] private GameObject turnOnBgm;
        [SerializeField] private GameObject turnOnSfx;
        public void OnClose()
        {
            Close();
        }

        public void TurnOnVolumeBgm(bool turnOn)
        {
            AudioManager.Instance.ToggleMute(turnOn, AudioType.BGM);
            turnOnBgm.SetActive(!turnOn);
        }

        public void TurnOnVolumeSfx(bool turnOn)
        {
            AudioManager.Instance.ToggleMute(turnOn, AudioType.Sfx);
            turnOnSfx.SetActive(!turnOn);
        }
    }
}
