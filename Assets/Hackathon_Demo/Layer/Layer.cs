
using UnityEngine;

namespace UnitySlippyMap
{


public abstract class Layer : MonoBehaviour
{
	public Map	Map;
	
	#region Protected members & properties
	
	protected float				minZoom;
	public float				MinZoom { get { return minZoom; } set { minZoom = value; } }
	
	protected float				maxZoom;
	public float				MaxZoom { get { return maxZoom; } set { maxZoom = value; } }
	
	#endregion
	
	#region Layer interface

	public abstract void UpdateContent();
	
	#endregion
}

}