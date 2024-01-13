using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : BaseController
{
	private AudioSource _audioSource;

	[HideInInspector]
	public Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

	private bool isPlaying;

	private float timeWaitClickButton = 0.07f;

	private bool haveOtherSoundPlay;

	public AudioSource AudioSource => _audioSource ?? (_audioSource = GetComponent<AudioSource>());

	public override void Start()
	{
		base.Start();
		DataManager.Instance.OnSettingChangeCallback.Add(OnSettingChangeCallback);
	}

	public void OnSettingChangeCallback()
	{
		if (AudioSource.clip != null)
		{
			if (isPlaying && !DataManager.Instance.SettingData.Music)
			{
				AudioSource.Stop();
			}
			if (!isPlaying && DataManager.Instance.SettingData.Music)
			{
				AudioSource.Play();
			}
			isPlaying = DataManager.Instance.SettingData.Music;
		}
	}

	public void PlayLoop(string audioName)
	{
		if (CheckClipExist(audioName))
		{
			AudioSource.clip = AudioClips[audioName];
			AudioSource.loop = true;
			if (DataManager.Instance.SettingData.Music)
			{
				isPlaying = true;
				AudioSource.Play();
			}
		}
	}

	public void PlayOneShot(string audioName)
	{
		if (!CheckClipExist(audioName) || !DataManager.Instance.SettingData.Sound)
		{
			return;
		}
		if (audioName == "Audios/Effect/click")
		{
			if (!haveOtherSoundPlay)
			{
				StopAllCoroutines();
				StartCoroutine(PlayButtonClick());
			}
		}
		else
		{
			haveOtherSoundPlay = true;
			AudioSource.PlayOneShot(AudioClips[audioName]);
			StopAllCoroutines();
			StartCoroutine(AfterOtherSoundPlay());
		}
	}

	private IEnumerator PlayButtonClick()
	{
		haveOtherSoundPlay = false;
		yield return new WaitForSeconds(timeWaitClickButton);
		if (!haveOtherSoundPlay)
		{
			AudioSource.PlayOneShot(AudioClips["Audios/Effect/click"]);
		}
	}

	private IEnumerator AfterOtherSoundPlay()
	{
		yield return new WaitForSeconds(timeWaitClickButton);
		haveOtherSoundPlay = false;
	}

	public bool CheckClipExist(string audioName)
	{
		if (AudioClips.ContainsKey(audioName))
		{
			return true;
		}
		AudioClip audioClip = Resources.Load<AudioClip>(audioName);
		if (audioClip == null)
		{
			return false;
		}
		AudioClips[audioName] = audioClip;
		return true;
	}
}
