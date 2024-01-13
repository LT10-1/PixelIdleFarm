using System;
using System.Collections.Generic;

[Serializable]
public class ChargeIAPEntity
{
	[Serializable]
	public class Param
	{
		public string UserId;

		public string DeviceId;

		public string DevicePlatform;

		public string PackageName;

		public int PackageValue;
	}

	public List<Param> Params;

	public ChargeIAPEntity()
	{
		Params = new List<Param>();
	}
}
