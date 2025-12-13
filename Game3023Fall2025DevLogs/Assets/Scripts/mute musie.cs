using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusic : MonoBehaviour
{
    [Header("Music")]
    public List<AudioSource> musicSources;
    public Button musicButton;
    public Color musicOnColor = Color.green;
    public Color musicOffColor = Color.red;

    [Header("Sound Effects")]
    public List<AudioSource> sfxSources;
    public Button sfxButton;
    public Color sfxOnColor = Color.green;
    public Color sfxOffColor = Color.red;

    private bool musicOn = true;
    private bool sfxOn = true;

    void Start()
    {
        // Set default button colors
        UpdateMusicButton();
        UpdateSfxButton();

        // Hook up buttons
        musicButton.onClick.AddListener(ToggleMusic);
        sfxButton.onClick.AddListener(ToggleSfx);
    }

    // -------- MUSIC --------
    public void ToggleMusic()
    {
        musicOn = !musicOn;

        foreach (AudioSource a in musicSources)
        {
            if (a != null)
                a.mute = !musicOn;
        }

        UpdateMusicButton();
    }

    void UpdateMusicButton()
    {
        musicButton.image.color = musicOn ? musicOnColor : musicOffColor;
    }

    // -------- SFX --------
    public void ToggleSfx()
    {
        sfxOn = !sfxOn;

        foreach (AudioSource a in sfxSources)
        {
            if (a != null)
                a.mute = !sfxOn;
        }

        UpdateSfxButton();
    }

    void UpdateSfxButton()
    {
        sfxButton.image.color = sfxOn ? sfxOnColor : sfxOffColor;
    }
}
