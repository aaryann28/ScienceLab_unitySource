using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider _musicSlder;
    public Slider _sfxSlder;

    [SerializeField]float musicValue;
    [SerializeField] float sfxValue;
    public AudioSource musicSource;

    private void Awake()
    {
        musicValue = PlayerPrefs.GetFloat("musicValue",.3f);
        sfxValue = PlayerPrefs.GetFloat("sfxValue",1f);

        _musicSlder.value = musicValue;
        _sfxSlder.value = sfxValue;

        musicSource.volume = musicValue;
    }

    public void OnMusicSliderValueChange()
    {
        musicValue = _musicSlder.value;
        PlayerPrefs.SetFloat("musicValue", musicValue);
        musicSource.volume = musicValue;
    }

    public void OnSFXSliderValueChange()
    {
        sfxValue = _sfxSlder.value;
        PlayerPrefs.SetFloat("sfxValue", sfxValue);
    }

    public void OnSceneChangebtnClick(int Sceneid)
    {
        SceneManager.LoadScene(Sceneid);
    }

    public void OnExitBtnClick()
    {
        Application.Quit();
    }

}
