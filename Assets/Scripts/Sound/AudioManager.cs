using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] music;
    public Sound[] ambient;
    public Sound[] effect;

    public static AudioManager instance;

    public AudioSource ambientAudio;
    public AudioSource musicAudio;

    float ambientVolume = 1f;
    float musicVolume = 1f;
    float effectVolume = 1f;

    public GameObject audioSourcePrefab;
    public static AudioManager Get
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        StartCoroutine(CheckForSoundValueChangeCoroutine());
        SetupSounds();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(effect, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " doesnt exist!");
            return;
        }
        AudioSource audio = audioSourcePrefab.GetComponent<AudioSource>();
        audio.clip = s.clip;
        audio.volume = s.volume;
        audio.pitch = s.pitch;
        audio.loop = s.loop;
        audio.spatialBlend = 0f;
        GameObject soundObject = Instantiate(audioSourcePrefab);
        StartCoroutine(DestroyAfterSoundHasPlayed(audio.clip.length, soundObject));
    }



    //Instanciates a new sound as a child
    public void Play(string name, Transform parent)
    {
        Sound s = Array.Find(effect, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " doesnt exist!");
            return;
        }
        AudioSource audio = audioSourcePrefab.GetComponent<AudioSource>();
        audio.clip = s.clip;
        audio.volume = s.volume;
        audio.pitch = s.pitch;
        audio.loop = s.loop;
        audio.spatialBlend = 1f;
        GameObject soundObject = Instantiate(audioSourcePrefab, parent.transform);
        StartCoroutine(DestroyAfterSoundHasPlayed(audio.clip.length, soundObject));
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(effect, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " doesnt exist!");
            return null;
        }
        return s;
    }

    IEnumerator DestroyAfterSoundHasPlayed(float time, UnityEngine.Object audioClip)
    {
        yield return new WaitForSeconds(time);
        Destroy(audioClip);
    }

    public void PlayAmbientAudio(string name)
    {
        Sound s = Array.Find(ambient, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " doesnt exist!");
            return;
        }
        if (ambientAudio)
            ambientAudio.Stop();
        ambientAudio = s.source;
        ambientAudio.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " doesnt exist!");
            return;
        }
        if (musicAudio)
            musicAudio.Stop();
        musicAudio = s.source;
        musicAudio.Play();
    }

    void SetupSounds()
    {
        foreach (Sound s in music)
        {
            if (s.name == "")
                s.name = s.clip.name;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.originalVolume = s.volume;
        }
        foreach (Sound s in ambient)
        {
            if (s.name == "")
                s.name = s.clip.name;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.originalVolume = s.volume;
        }
        foreach (Sound s in effect)
        {
            if (s.name == "")
                s.name = s.clip.name;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.spatialBlend = 1f;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.originalVolume = s.volume;
        }
    }

    IEnumerator CheckForSoundValueChangeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameManager manager = GameManager.Get;
            if (manager.effectsVolume != effectVolume) { UpdateEffectsVolume(manager.effectsVolume); }
            if (manager.musicVolume != musicVolume) { UpdateMusicVolume(manager.musicVolume); }
            if (manager.environmentVolume != ambientVolume) { UpdateAmbientVolume(manager.environmentVolume); }
            effectVolume = manager.effectsVolume;
            musicVolume = manager.musicVolume;
            ambientVolume = manager.environmentVolume;
        }
    }

    void UpdateEffectsVolume(float value)
    {
        foreach (Sound s in effect)
        {
            s.volume = s.originalVolume*value;
            s.source.volume *= s.volume;
        }
    }

    void UpdateMusicVolume(float value)
    {
        foreach (Sound s in music)
        {
            s.volume = s.originalVolume * value;
            s.source.volume = s.originalVolume * value;
        }

    }

    void UpdateAmbientVolume(float value)
    {
        foreach (Sound s in ambient)
        {
            s.volume = s.originalVolume * value;
            s.source.volume = s.originalVolume * value;
        }

    }

public void StopAmbient()
{
if(ambientAudio)
	ambientAudio.Stop();
}

public void StopMusic()
{
if(musicAudio)
	musicAudio.Stop();
}
}
