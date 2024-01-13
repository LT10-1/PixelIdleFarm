using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
	public class ETFXButtonScript : MonoBehaviour
	{
		public GameObject Button;

		private Text MyButtonText;

		private string projectileParticleName;

		private ETFXFireProjectile effectScript;

		private ETFXProjectileScript projectileScript;

		public float buttonsX;

		public float buttonsY;

		public float buttonsSizeX;

		public float buttonsSizeY;

		public float buttonsDistance;

		private void Start()
		{
			effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
			getProjectileNames();
			MyButtonText = Button.transform.Find("Text").GetComponent<Text>();
			MyButtonText.text = projectileParticleName;
		}

		private void Update()
		{
			MyButtonText.text = projectileParticleName;
		}

		public void getProjectileNames()
		{
			projectileScript = effectScript.projectiles[effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
			projectileParticleName = projectileScript.projectileParticle.name;
		}

		public bool overButton()
		{
			Rect rect = new Rect(buttonsX, buttonsY, buttonsSizeX, buttonsSizeY);
			Rect rect2 = new Rect(buttonsX + buttonsDistance, buttonsY, buttonsSizeX, buttonsSizeY);
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			float x = mousePosition.x;
			float num = Screen.height;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			if (!rect.Contains(new Vector2(x, num - mousePosition2.y)))
			{
				Vector3 mousePosition3 = UnityEngine.Input.mousePosition;
				float x2 = mousePosition3.x;
				float num2 = Screen.height;
				Vector3 mousePosition4 = UnityEngine.Input.mousePosition;
				if (!rect2.Contains(new Vector2(x2, num2 - mousePosition4.y)))
				{
					return false;
				}
			}
			return true;
		}
	}
}
