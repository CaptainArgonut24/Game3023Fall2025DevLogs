using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musicplayer : MonoBehaviour
{

    public AudioSource backgroundMusic; // Reference to the background music AudioSource

    public float fadeDuration = 1f;     // Duration for fading in/out music

    private Coroutine currentFade;
    void Start()
    {
        // Ensure background music starts playing if not already
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        // Background music logic can be extended here if needed
    }

    private void StartMusicFadeOut(AudioSource musicToFade)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeOutMusic(musicToFade));
    }

    private void StartMusicFadeIn(AudioSource musicToFade)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeInMusic(musicToFade));
    }

    private IEnumerator FadeOutMusic(AudioSource musicToFade)
    {
        float elapsedTime = 0f;

        // Fade out the music
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (musicToFade != null)
            {
                musicToFade.volume = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            }
            yield return null;
        }

        if (musicToFade != null)
        {
            musicToFade.Pause();
        }

        currentFade = null;
    }

    private IEnumerator FadeInMusic(AudioSource musicToFade)
    {
        float elapsedTime = 0f;

        if (musicToFade != null)
        {
            musicToFade.Play();
            // Fade in the music
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                musicToFade.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                yield return null;
            }
        }

        currentFade = null;
    }
}
