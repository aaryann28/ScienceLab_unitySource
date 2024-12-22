using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource _sfxAudioSource;
    public AudioSource _musicAudioSource;
    public List<ClipData> allSounds;
    private List<string> playingSounds = new List<string>();

    private void Awake()
    {
         instance = this;
        _musicAudioSource.volume = PlayerPrefs.GetFloat("musicValue", 0.3f);
    }

    private void Start()
    {
        _sfxAudioSource.volume = PlayerPrefs.GetFloat("sfxValue", 1f);
        
    }

    public void playSound(string soundName)
    {
        //Sound is already playing
        if (playingSounds.Contains(soundName))
            return;
    

        foreach (ClipData data in allSounds)
        {
            if (data.name == soundName)
            {
                // Play the sound
                _sfxAudioSource.PlayOneShot(data.clip);
                _sfxAudioSource.volume = data.clipVolume;

                //Sound is playing so adding in the list to keep track
                playingSounds.Add(soundName);

                StartCoroutine(RemovePlayingSound(soundName, data.clip.length));
                break;
            }
        }
    }

    private IEnumerator RemovePlayingSound(string soundName, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        playingSounds.Remove(soundName);
    }
}



[System.Serializable]
public class ClipData
{
    public string name;
    public AudioClip clip;
    [Range(0,1)]public float clipVolume = 1f;
}
