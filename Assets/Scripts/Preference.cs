using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Preference
{
	private string DATA = "BaiDaiGia";

	public DataGame dataGame = new DataGame();

	private static Preference instance;

	private Preference()
	{
		loadData();
	}

	public static Preference getInstance()
	{
		if (instance == null)
		{
			instance = new Preference();
		}
		return instance;
	}

	private void loadData()
	{
		if (PlayerPrefs.HasKey(DATA))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(dataGame.GetType());
			StringReader textReader = new StringReader(PlayerPrefs.GetString(DATA));
			dataGame = (DataGame)xmlSerializer.Deserialize(textReader);
		}
		else
		{
			saveData();
		}
	}

	public void saveData()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(dataGame.GetType());
		StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, dataGame);
		PlayerPrefs.SetString(DATA, stringWriter.ToString());
	}
}
