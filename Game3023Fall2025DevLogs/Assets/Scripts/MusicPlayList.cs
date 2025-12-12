using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayList : MonoBehaviour
{
    [Header("=== Playlist Settings ===")]
    public List<AudioClip> musicClips = new List<AudioClip>();   // Drag your music files here
    public AudioSource audioSource;                              // Drag AudioSource here

    private List<AudioClip> _shuffleList;                        // Internal randomized list
    private int _currentIndex = 0;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        ShufflePlaylist();
        PlayNextSong();
    }

    void Update()
    {
        // If current song ended → play next
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    // Shuffle list for random order
    void ShufflePlaylist()
    {
        _shuffleList = new List<AudioClip>(musicClips);

        for (int i = 0; i < _shuffleList.Count; i++)
        {
            int rand = Random.Range(i, _shuffleList.Count);
            AudioClip temp = _shuffleList[i];
            _shuffleList[i] = _shuffleList[rand];
            _shuffleList[rand] = temp;
        }

        _currentIndex = 0;
    }

    void PlayNextSong()
    {
        if (_shuffleList.Count == 0)
        {
            Debug.LogWarning("Playlist is empty!");
            return;
        }

        // If finished all tracks → reshuffle
        if (_currentIndex >= _shuffleList.Count)
        {
            ShufflePlaylist();
        }

        audioSource.clip = _shuffleList[_currentIndex];
        audioSource.Play();

        _currentIndex++;
    }
}