

using UnityEngine;

namespace UnitySlippyMap
{

public class LocationMarker : Marker
{
    private Transform orientationMarker;
	public Transform OrientationMarker
    {
        get { return orientationMarker; }
        set
        {
            if (orientationMarker != null)
            {
                orientationMarker.parent = null;
            }
            
            orientationMarker = value;
            
            if (orientationMarker != null)
            {
                orientationMarker.parent = this.transform;
                orientationMarker.localPosition = Vector3.zero; 
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9
                orientationMarker.gameObject.SetActiveRecursively(this.gameObject.active);
#else
				orientationMarker.gameObject.SetActive(this.gameObject.activeSelf);
#endif
            }
        }
    }
}

}