//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePanelType
{
	NOT_DEFINE,
	MAINMENU,
	SELECTIONMENU,
	HUD,
	SETTINGS,
	ENDGAME
}
public class UIPanel : MonoBehaviour
{
	#region Script Parameters
	public GameObject DefaultBtn;
	#endregion

	#region Fields
	protected UIManager mUIManager = null;
	protected bool mIsDisplay = false;
	#endregion

	#region Properties
	public ePanelType Type { get; protected set; }
	#endregion
	
	#region Unity Methods
	public virtual void Awake()
	{
		Type = ePanelType.NOT_DEFINE;
		Display(false, true);
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	public virtual void Update()
	{
		if (mUIManager.eventSystem && mUIManager.eventSystem.currentSelectedGameObject == null)
			mUIManager.eventSystem.SetSelectedGameObject(DefaultBtn);
	}
	#endregion

	#region Methods
	public virtual void Display(bool display, bool force = false)
	{
		if(!force && mIsDisplay == display)
			return;
		gameObject.SetActive(display);
		mIsDisplay = display;
	}

	public void SetUIManager(UIManager manager)
	{
		mUIManager = manager;
	}
	#endregion

	#region Implementation

	#endregion
}
