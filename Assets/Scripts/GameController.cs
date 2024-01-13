using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : BaseController
{
	[HideInInspector]
	public DialogController DialogController;

	[HideInInspector]
	public BoostController BoostController;

	[HideInInspector]
	public MineController MineController;

	[HideInInspector]
	public WorldMapController WorldMapController;

	[HideInInspector]
	public TutorialController TutorialController;

	[HideInInspector]
	public AudioController AudioController;

	[HideInInspector]
	public AdsManager AdsManager;

	[HideInInspector]
	public FacebookHelper FacebookHelper;

	[HideInInspector]
	public APIHelper APIHelper;

	[HideInInspector]
	//public AnalyticController AnalyticController;

	//[HideInInspector]
	public LoadingController LoadingController;

	[HideInInspector]
	public ToastController ToastController;

	[HideInInspector]
	public SkillController SkillController;

	[HideInInspector]
	public ChestController ChestController;

	[HideInInspector]
	public ExpeditionController ExpeditionController;

	[HideInInspector]
	public CashFlyEffect CashFlyEffect;

	[HideInInspector]
	public LocalNotificationController LocalNotificationController;

	[HideInInspector]
	public List<Action> OnGlobalCashChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnMineCashChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnGlobalSuperCashChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnMineSuperCashChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnItemChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnSkillpointChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnWorkshopChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnMineWorkshopChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnChestChangeCallback = new List<Action>();

	[HideInInspector]
	public List<Action> OnMineChestChangeCallback = new List<Action>();

	public List<MineModel> MineModels = new List<MineModel>();

	public static GameController Instance => BaseController.GameController;

	public override void Awake()
	{
		base.Awake();
		DataManager.Instance.Init();
		Application.targetFrameRate = 60;
		TutorialController = GetComponentInChildren<TutorialController>(includeInactive: true);
		AudioController = GetComponentInChildren<AudioController>(includeInactive: true);
		DialogController = GetComponentInChildren<DialogController>(includeInactive: true);
		BoostController = GetComponentInChildren<BoostController>(includeInactive: true);
		AdsManager = GetComponentInChildren<AdsManager>(includeInactive: true);
		ToastController = GetComponentInChildren<ToastController>(includeInactive: true);
		APIHelper = GetComponentInChildren<APIHelper>(includeInactive: true);
		FacebookHelper = GetComponentInChildren<FacebookHelper>(includeInactive: true);
		//AnalyticController = GetComponentInChildren<AnalyticController>(includeInactive: true);
		LocalNotificationController = GetComponentInChildren<LocalNotificationController>(includeInactive: true);
		LoadingController = GetComponentInChildren<LoadingController>(includeInactive: true);
		SkillController = GetComponentInChildren<SkillController>(includeInactive: true);
		ChestController = GetComponentInChildren<ChestController>(includeInactive: true);
		CashFlyEffect = GetComponentInChildren<CashFlyEffect>(includeInactive: true);
		ExpeditionController = GetComponentInChildren<ExpeditionController>(includeInactive: true);
		CreateMineModel();
		UpdateStatIncreaseModel();
		BaseController.GameController.OnWorkshopChangeCallback.Add(UpdateStatIncreaseModel);
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		StartCoroutine(Savegame());
	}

	private void CreateMineModel()
	{
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			AddMineModel(i);
		}
	}

	public void AddMineModel(int index)
	{
		MineModels.Add(new MineModel(index));
	}

	public void ResetMineModel(int index)
	{
		MineModels[index] = new MineModel(index);
	}

	public void UpdateStatIncreaseModel()
	{
		foreach (MineModel mineModel in MineModels)
		{
			mineModel.StatsIncreaseModel.MineSkillIncreaseFactor = 1.0;
			mineModel.StatsIncreaseModel.MineCollectibleIncreaseFactor = 1.0;
			mineModel.StatsIncreaseModel.CorridorWorkerIncreaseFactor = 1.0;
			mineModel.StatsIncreaseModel.ElevatorWorkerIncreaseFactor = 1.0;
			mineModel.StatsIncreaseModel.GroundWorkerIncreaseFactor = 1.0;
		}
		foreach (KeyValuePair<int, int> item in DataManager.Instance.SavegameData.SkillSaveGame)
		{
			SkillEntity.Param param = DataManager.Instance.SkillDictionary[item.Key];
			if (param.EffectId == 2)
			{
				double num = param.ParamX[item.Value - 1];
				foreach (MineModel mineModel2 in MineModels)
				{
					mineModel2.StatsIncreaseModel.MineSkillIncreaseFactor *= 1.0 + num / 100.0;
				}
			}
			if (param.EffectId == 1)
			{
				double num2 = param.ParamX[item.Value - 1];
				if (param.ParamY < MineModels.Count)
				{
					MineModels[param.ParamY].StatsIncreaseModel.MineSkillIncreaseFactor *= 1.0 + num2 / 100.0;
				}
			}
		}
		IEnumerator enumerator4 = Enum.GetValues(typeof(ManagerArea)).GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				ManagerArea managerArea = (ManagerArea)enumerator4.Current;
				int key = DataManager.Instance.SavegameData.CurrentActiveWorkerCard(managerArea);
				if (DataManager.Instance.SavegameData.CollectibleSavegames.ContainsKey(key))
				{
					CollectibleSavegame collectibleSavegame = DataManager.Instance.SavegameData.CollectibleSavegames[key];
					CollectiblesEntity.Param param2 = DataManager.Instance.CollectiblesDictionary[collectibleSavegame.CollectibleId];
					if (collectibleSavegame.Level != 0)
					{
						CollectibleProductionFactorsEntity.Param param3 = DataManager.Instance.CollectibleLevelsDictionary[collectibleSavegame.CollectibleId][collectibleSavegame.Level];
						double productionFactor = param3.ProductionFactor;
						double secondaryEffectFactor = param3.SecondaryEffectFactor;
						foreach (MineModel mineModel3 in MineModels)
						{
							switch (managerArea)
							{
							case ManagerArea.Corridor:
								mineModel3.StatsIncreaseModel.CorridorWorkerIncreaseFactor = productionFactor;
								break;
							case ManagerArea.Ground:
								mineModel3.StatsIncreaseModel.GroundWorkerIncreaseFactor = productionFactor;
								break;
							case ManagerArea.Elevator:
								mineModel3.StatsIncreaseModel.ElevatorWorkerIncreaseFactor = productionFactor;
								break;
							}
							if (param2.SecondaryEffectId == 3)
							{
								mineModel3.StatsIncreaseModel.MineCollectibleIncreaseFactor *= secondaryEffectFactor;
							}
							if (param2.SecondaryEffectId == 2 && mineModel3.MineSavegame.ContinentIndex == param2.SecondaryEffectTargetId)
							{
								mineModel3.StatsIncreaseModel.MineCollectibleIncreaseFactor *= secondaryEffectFactor;
							}
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator4 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (MineController != null)
		{
			MineController.UpdateIdleCash();
		}
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Mine")
		{
			MineController.MainCamera.gameObject.SetActive(value: true);
		}
		else if (scene.name != "Init" && scene.name != "Load")
		{
			MineController.MainCamera.gameObject.SetActive(value: false);
		}
	}

	public void OnSceneUnloaded(Scene scene)
	{
		if (scene.name == "WorldMap" && MineController != null)
		{
			MineController.MainCamera.gameObject.SetActive(value: true);
		}
	}

	public override void Start()
	{
		base.Start();
		Screen.sleepTimeout = -1;
		UnityEngine.Object.DontDestroyOnLoad(this);
		SceneManager.LoadScene("Load");
	}

	public override void Update()
	{
		base.Update();
		HandleBackButton();
	}

	public void DeleteDataAndReset()
	{
		PlayerPrefs.DeleteAll();
		DataManager.Instance.SavegameData = null;
		DataManager.Instance.InitSettingData();
		DataManager.Instance.InitSaveGameData();
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		UnityEngine.Object.Destroy(base.gameObject);
		SceneManager.LoadSceneAsync("Init");
	}

	public void StartLoading()
	{
		LoadingController.ToggleLoading(loading: true);
	}

	public void StopLoading()
	{
		LoadingController.ToggleLoading(loading: false);
	}

	public void CreateNewMine()
	{
		DataManager.Instance.SavegameData.CurrentMineUnlocked++;
		MineSavegame mineSavegame = new MineSavegame();
		mineSavegame.ContinentIndex = DataManager.Instance.SavegameData.Mines.Count / 5;
		mineSavegame.MineIndex = DataManager.Instance.SavegameData.Mines.Count % 5;
		MineSavegame item = mineSavegame;
		DataManager.Instance.SavegameData.Mines.Add(item);
		AddMineModel(DataManager.Instance.SavegameData.Mines.Count - 1);
		UpdateStatIncreaseModel();
	}

	public void PrestigeMine(int index)
	{
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[index];
		MineSavegame mineSavegame2 = new MineSavegame();
		mineSavegame2.ContinentIndex = mineSavegame.ContinentIndex;
		mineSavegame2.MineIndex = mineSavegame.MineIndex;
		mineSavegame2.PrestigeCount = mineSavegame.PrestigeCount + 1;
		MineSavegame mineSavegame3 = mineSavegame2;
		DataManager.Instance.SavegameData.Mines[index] = mineSavegame3;
		ResetMineModel(index);
		UpdateStatIncreaseModel();
		//BaseController.GameController.AnalyticController.LogEvent("prestige_mine", "mine", index + "-" + mineSavegame3.PrestigeCount);
	}

	public void GoToMine(int mineIndex)
	{
		DataManager.Instance.SavegameData.CurrentMine = mineIndex;
		SceneManager.LoadSceneAsync("Load");
		DialogController.DialogManagerCorridor.ResetData();
		DialogController.DialogManagerElevator.ResetData();
		DialogController.DialogManagerGround.ResetData();
	}

	private IEnumerator Savegame()
	{
		yield return new WaitForSeconds(10f);
		DataManager.Instance.Savegame();
		StartCoroutine(Savegame());
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			if (SceneManager.GetActiveScene().name == "Mine")
			{
				OnAppInactive();
			}
		}
		else if (SceneManager.GetActiveScene().name == "Mine" && MineController.CheckIdleTime())
		{
			DialogController.HideAllDialog();
			MineController.ShowIdleCashGain();
		}
	}

	private void OnApplicationQuit()
	{
		OnAppInactive();
	}

	public void OnAppInactive()
	{
		DataManager.Instance.SavegameData.LastLoginTime = DateTime.Now.Ticks;
		DataManager.Instance.Savegame();
		LocalNotificationController.ScheduleLocalNotification(LocalNotificationType.DailyNotification, DateTime.Today.AddDays(1.0).AddHours(UnityEngine.Random.Range(9, 9)).AddSeconds(UnityEngine.Random.Range(0, 0)), DataManager.Instance.DailyRewardList[DataManager.Instance.SavegameData.DailyRewardCycleCount].ItemDescription);
		if (MathUtils.CompareDoubleToZero(DataManager.Instance.IdleCash))
		{
			return;
		}
		double cash;
		if (MineController.MileStoneLockController.gameObject.activeSelf)
		{
			if (MineController.MileStoneLockController.State != 0)
			{
				return;
			}
			cash = MineController.MileStoneLockController.UnlockCost.Cash;
		}
		else
		{
			if (MineController.CorridorController.CorridorLevelControllers.Last().IsActiveMine)
			{
				return;
			}
			cash = MineController.CorridorController.CorridorLevelControllers.Last().ButtonNewShaft.Cash;
		}
		if (!(cash < DataManager.Instance.Cash))
		{
			double num = (cash - DataManager.Instance.Cash) / DataManager.Instance.IdleCash;
			if (!(num <= 60.0) && !(num > TimeSpan.FromDays(30.0).TotalSeconds))
			{
				LocalNotificationController.ScheduleLocalNotification(LocalNotificationType.EnoughOpenNewMineShaft, DateTime.Now.AddSeconds(num));
			}
		}
	}

	public void HandleBackButton()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !DialogController.HandleBackButton())
		{
			DialogController.HandleBackButton();
		}
	}

	public void InvokeOnCashChangeCallback()
	{
		for (int i = 0; i < OnMineCashChangeCallback.Count; i++)
		{
			OnMineCashChangeCallback[i]();
		}
		for (int j = 0; j < OnGlobalCashChangeCallback.Count; j++)
		{
			OnGlobalCashChangeCallback[j]();
		}
	}

	public void InvokeOnSuperCashChangeCallback()
	{
		for (int i = 0; i < OnMineSuperCashChangeCallback.Count; i++)
		{
			OnMineSuperCashChangeCallback[i]();
		}
		for (int j = 0; j < OnGlobalSuperCashChangeCallback.Count; j++)
		{
			OnGlobalSuperCashChangeCallback[j]();
		}
	}

	public void InvokeOnItemChangeCallback()
	{
		for (int i = 0; i < OnItemChangeCallback.Count; i++)
		{
			OnItemChangeCallback[i]();
		}
	}

	public void InvokeOnSkillpointChangeCallback()
	{
		for (int i = 0; i < OnSkillpointChangeCallback.Count; i++)
		{
			OnSkillpointChangeCallback[i]();
		}
	}

	public void InvokeOnWorkshopChangeCallback()
	{
		for (int i = 0; i < OnWorkshopChangeCallback.Count; i++)
		{
			OnWorkshopChangeCallback[i]();
		}
		for (int j = 0; j < OnMineWorkshopChangeCallback.Count; j++)
		{
			OnMineWorkshopChangeCallback[j]();
		}
	}

	public void InvokeOnChestChangeCallback()
	{
		for (int i = 0; i < OnChestChangeCallback.Count; i++)
		{
			OnChestChangeCallback[i]();
		}
		for (int j = 0; j < OnMineChestChangeCallback.Count; j++)
		{
			OnMineChestChangeCallback[j]();
		}
	}

	public void UpdateMyInfo()
	{
		int score = (int)DataManager.Instance.SuperCashNetWorth * 30;
		int currentContinentUnlocked = DataManager.Instance.SavegameData.CurrentContinentUnlocked;
		int currentMineUnlockedIndex = DataManager.Instance.SavegameData.CurrentMineUnlockedIndex;
		int currentShaftUnlocked = DataManager.Instance.SavegameData.CurrentShaftUnlocked;
		FacebookHelper.UpdateMyInfo(score, currentContinentUnlocked, currentMineUnlockedIndex, currentShaftUnlocked);
	}
}
