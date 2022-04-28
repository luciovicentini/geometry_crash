using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static Dictionary<Sound, float> soundTimerDictionary = new Dictionary<Sound, float>();
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;
    
    public enum Sound
    {
        ChipSelected,
        ChipSwitching,
        ChipSwitchingBack,
        ChipDestroying,
        ChipCreating,
        ChipFalling,
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }


    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null) {
                oneShotGameObject = new GameObject("One Shot Sound");;
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.ChipCreating:
                return CheckSoundTimer(sound, .5f);
        }
    }

    private static bool CheckSoundTimer(Sound sound, float paddingTime)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDictionary[sound];
            if (lastTimePlayed + paddingTime < Time.time)
            {
                soundTimerDictionary[sound] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            soundTimerDictionary[sound] = Time.time;
            return true;
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClips[Random.Range(0, soundAudioClip.audioClips.Length)];
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
