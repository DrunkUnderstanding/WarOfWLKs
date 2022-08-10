using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

       
    }
	public void Init()
	{
        LoadVolume();
    }
    public void AddSliderLisener(Slider musicSlider, Slider sfxSlider)
	{
        this.musicSlider = musicSlider;
        this.sfxSlider = sfxSlider;
        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }
	public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    /// <summary>
    /// 更改游戏音量
    /// </summary>
    public void UpdateVolume()
    {

        musicSource.volume = musicSlider.value;

        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);

        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.1f);

        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.2f);

/*        musicSlider.value = musicSource.volume;

        sfxSlider.value = sfxSource.volume;*/
    }
}
