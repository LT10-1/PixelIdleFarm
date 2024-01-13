using Newtonsoft.Json;
using System;

[Serializable]
public class SettingData
{
	[JsonIgnore]
	public Action OnValueChanged;

	private bool _hasLikeFanpage;

	private bool _sound = true;

	private bool _music = true;

	private bool _notification = true;

	public bool HasLikeFanpage
	{
		get
		{
			return _hasLikeFanpage;
		}
		set
		{
			_hasLikeFanpage = value;
			if (OnValueChanged != null)
			{
				OnValueChanged();
			}
		}
	}

	public bool Sound
	{
		get
		{
			return _sound;
		}
		set
		{
			_sound = value;
			if (OnValueChanged != null)
			{
				OnValueChanged();
			}
		}
	}

	public bool Music
	{
		get
		{
			return _music;
		}
		set
		{
			_music = value;
			if (OnValueChanged != null)
			{
				OnValueChanged();
			}
		}
	}

	public bool Notification
	{
		get
		{
			return _notification;
		}
		set
		{
			_notification = value;
			if (OnValueChanged != null)
			{
				OnValueChanged();
			}
		}
	}
}
