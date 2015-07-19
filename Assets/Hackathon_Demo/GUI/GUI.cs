using UnityEngine;

namespace UnitySlippyMap.GUI
{

	public delegate bool GUIDelegate(Map map);
	
	public static class MapGUI
	{
		public static bool Zoom(Map map)
		{
			GUILayout.BeginVertical();
			
			GUILayout.Label("Zoom: " + map.CurrentZoom);
    		
			bool pressed = false;
    		if (GUILayout.RepeatButton("+"))
    		{
    			map.Zoom(1.0f);
				pressed = true;
    		}
    		if (GUILayout.RepeatButton("-"))
    		{
    			map.Zoom(-1.0f);
				pressed = true;
    		}
			
			GUILayout.EndVertical();
			
			return pressed;
		}
	}
}

