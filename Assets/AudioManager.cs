using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    
    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }
    
    private void Start()
    {
        Play("Dash");
        Debug.Log(sounds[1].source.clip.name);
        sounds[1].source.Play();
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null){
            Debug.Log("Sound: " + soundName + " not found.");
            return;
        }
        s.source.Play();
        
    }
}
