using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public GameObject Camera;
    public AudioMixerGroup SoundFX;
    public AudioMixerGroup BackGround;

    public AudioSource Create(string name, GameObject obj)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = obj.AddComponent<AudioSource>();
        if (s.soundType == "Sound")
            s.source.outputAudioMixerGroup = SoundFX;
        else if (s.soundType == "Background")
            s.source.outputAudioMixerGroup = BackGround;
        s.source.maxDistance = s.distance;
        s.source.playOnAwake = false;
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        AudioSource soundfx = s.source;

        return soundfx;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = Camera.AddComponent<AudioSource>();
        if (s.soundType == "Sound")
            s.source.outputAudioMixerGroup = SoundFX;
        else if (s.soundType == "Background")
            s.source.outputAudioMixerGroup = BackGround;
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        AudioSource soundfx = s.source;
        if (s == null)
        {
            Debug.LogWarning("null" + name);
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("null" + name);
            return;
        }
        s.source.Stop();
    }
}
