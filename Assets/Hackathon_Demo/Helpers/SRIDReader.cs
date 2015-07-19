
using UnityEngine;

using System.Collections.Generic;

using ProjNet.CoordinateSystems;

namespace UnitySlippyMap
{

	public class SridReader
	{
		private static string filename = "SRID";
	
		public struct WKTstring {

			public int WKID;

			public string WKT;
		}
	

		public static IEnumerable<WKTstring> GetSRIDs()
		{
			TextAsset filecontent = Resources.Load(filename) as TextAsset;
			using (System.IO.StringReader sr = new System.IO.StringReader(filecontent.text))
			{
				string line = sr.ReadLine();
				while (line != null)
				{
					int split = line.IndexOf(';');
					if (split > -1)
					{
						WKTstring wkt = new WKTstring();
						wkt.WKID = int.Parse(line.Substring(0, split));
						wkt.WKT = line.Substring(split + 1);
						yield return wkt;
					}
					line = sr.ReadLine();
				}
				sr.Close();
			}
		}
		public static ICoordinateSystem GetCSbyID(int id)
		{
			foreach (SridReader.WKTstring wkt in SridReader.GetSRIDs())
			{
				if (wkt.WKID == id)
				{
					return ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt.WKT) as ICoordinateSystem;
				}
			}
			return null;
		}
	}
}