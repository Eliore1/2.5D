//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour, IDeselectHandler, ISelectHandler
{
	#region Script Parameters
	public Image Background;
	#endregion
	
	#region Fields
	#endregion
	
	#region Properties
	#endregion
	
	#region Unity Methods
	// Start is called before the first frame update
	void Start()
	{
		
	}

	#endregion

	#region Methods
	public void SetEnable(bool value)
	{
		if (!Background)
			Background = GetComponent<Image>();
		if(Background)
			Background.enabled = value;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		SetEnable(false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		SetEnable(true);
	}
	#endregion

	#region Implementation

	#endregion
}
