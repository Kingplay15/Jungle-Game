using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    Slider SoundVolume;

    void Awake()
    {
        SoundVolume = GetComponent<Slider>();
        SoundVolume.value = AudioListener.volume;
    }

    public void ChangeSoundVolume()
    {
        AudioListener.volume = SoundVolume.value;
    }
}
