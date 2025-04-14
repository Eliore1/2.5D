//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndGamePanel : UIPanel
{
	#region Script Parameters
	#endregion

	#region Fields
	#endregion

	#region Properties
	#endregion

	#region Unity Methods
	public override void Awake()
	{
		base.Awake();
		Type = ePanelType.ENDGAME;
	}

	#endregion

	#region Methods
	public override void Display(bool display, bool force = false)
	{
		base.Display(display, force);

	}
	#endregion

	#region Implementation

	#endregion
}
