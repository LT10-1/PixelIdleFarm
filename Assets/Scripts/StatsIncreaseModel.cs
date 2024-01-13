public class StatsIncreaseModel : IStatsIncreaseModel
{
	public double PrestigeIncreaseFactor = 1.0;

	public double GroundWorkerIncreaseFactor = 1.0;

	public double ElevatorWorkerIncreaseFactor = 1.0;

	public double CorridorWorkerIncreaseFactor = 1.0;

	public double MineCollectibleIncreaseFactor = 1.0;

	public double MineSkillIncreaseFactor = 1.0;

	public static double FacebookFriendIncreaseFactor => 1.0 + (double)GameController.Instance.FacebookHelper.FriendsInfo.Params.Count * 5.0 / 100.0;

	public double MineIncreaseFactor => FacebookFriendIncreaseFactor;

	public double MineTotalFactor => PrestigeIncreaseFactor * MineIncreaseFactor * MineSkillIncreaseFactor * MineCollectibleIncreaseFactor;

	public double GroundTotalFactor => MineTotalFactor * GroundWorkerIncreaseFactor;

	public double ElevatorTotalFactor => MineTotalFactor * ElevatorWorkerIncreaseFactor;

	public double CorridorTotalFactor => MineTotalFactor * CorridorWorkerIncreaseFactor;
}
