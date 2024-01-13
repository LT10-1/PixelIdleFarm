using System;
using System.Collections.Generic;

public class MineModel
{
	public int MineOrder;

	public CorridorTotalExtractionCalculator CorridorTotalExtractionCalculator;

	public ElevatorTotalExtractionCalculator ElevatorTotalExtractionCalculator;

	public GroundTotalExtractionCalculator GroundTotalExtractionCalculator;

	public ElevatorModel ElevatorModel;

	public GroundModel GroundModel;

	public Dictionary<int, CorridorModel> CorridorModels = new Dictionary<int, CorridorModel>();

	public StatsIncreaseModel StatsIncreaseModel;

	public MineSavegame MineSavegame => DataManager.Instance.SavegameData.Mines[MineOrder];

	public int NumberActiveCorridor
	{
		get
		{
			int num = 0;
			MineSavegame mineSavegame = MineSavegame;
			for (int i = 0; i < mineSavegame.CorridorLevel.Count; i++)
			{
				if (mineSavegame.CorridorLevel[i] > 0)
				{
					num++;
				}
			}
			return num;
		}
	}

	public MineModel(int index)
	{
		MineOrder = index;
		MineSavegame mineSavegame = MineSavegame;
		MineFactorsEntity.Param param = DataManager.Instance.MineFactorsEntityParams[mineSavegame.MineIndex][mineSavegame.PrestigeCount];
		StatsIncreaseModel = new StatsIncreaseModel
		{
			PrestigeIncreaseFactor = param.Factor
		};
		ElevatorModel = new ElevatorModel(StatsIncreaseModel, ElevatorImporter.Instance);
		GroundModel = new GroundModel(StatsIncreaseModel, WarehouseImporter.Instance, WarehouseImporter.Instance);
		ElevatorModel.Level = Math.Max(mineSavegame.ElevatorLevel, 1);
		GroundModel.Level = Math.Max(mineSavegame.GroundLevel, 1);
		ElevatorModel.isManagerActive = (mineSavegame.ElevatorManagerSavegame != null);
		GroundModel.isManagerActive = (mineSavegame.GroundManagerSavegame != null);
		for (int i = 0; i < mineSavegame.CorridorLevel.Count; i++)
		{
			CorridorModel corridorModel = GetCorridorModel(i + 1);
			if (mineSavegame.CorridorLevel[i] > 0)
			{
				corridorModel.Level = mineSavegame.CorridorLevel[i];
				corridorModel.isManagerActive = mineSavegame.CorridorCurrentManager.ContainsKey(corridorModel.Tier);
			}
		}
		CorridorTotalExtractionCalculator = new CorridorTotalExtractionCalculator(CorridorModels, CorridorImporter.Instance, CorridorImporter.Instance);
		ElevatorTotalExtractionCalculator = new ElevatorTotalExtractionCalculator(this, ElevatorModel, ElevatorImporter.Instance, StatsIncreaseModel);
		GroundTotalExtractionCalculator = new GroundTotalExtractionCalculator(GroundModel, WarehouseImporter.Instance, WarehouseImporter.Instance, StatsIncreaseModel);
	}

	public void UpdateIdleCash()
	{
		MineSavegame mineSavegame = MineSavegame;
		mineSavegame.IdleCash = 0.0;
		if (ElevatorModel.Level != 0 && GroundModel.Level != 0)
		{
			mineSavegame.IdleCash = 0.1 * Math.Min(CorridorTotalExtractionCalculator.GetTotalExtration(), Math.Min(ElevatorTotalExtractionCalculator.GetTotalExtration(), GroundTotalExtractionCalculator.GetTotalExtration()));
		}
	}

	public CorridorModel GetCorridorModel(int tier)
	{
		if (!CorridorModels.ContainsKey(tier))
		{
			CorridorModels.Add(tier, new CorridorModel(tier, StatsIncreaseModel, CorridorImporter.Instance, CorridorImporter.Instance));
		}
		return CorridorModels[tier];
	}
}
