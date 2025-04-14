//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHud : UIPanel
{
	#region Script Parameters
	public bool SetSortingOrder = true;
	#endregion

	#region Fields
	#endregion

	#region Properties
	#endregion

	#region Unity Methods
	public override void Awake()
	{
		base.Awake();
		Type = ePanelType.HUD;
		if (SetSortingOrder)
			SetSortingOrderFct();
	}
	// Start is called before the first frame update
	void Start()
	{
		
	}

    // Update is called once per frame
    public override void Update()
	{

	}
    #endregion

    #region Methods
    public override void Display(bool display, bool force = false)
	{
		base.Display(display, force);


	}
    #endregion

    public void ClickPauseMenu(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && this.gameObject.activeSelf)
        {
            GameManager.Get.isPlaying = false;
            PauseMenu();
        }
    }
    public void PauseMenu()
    {
        mUIManager.SwitchPanel(ePanelType.MAINMENU);
		mUIManager.eventSystem.SetSelectedGameObject(mUIManager.StartButton.gameObject);
        this.gameObject.SetActive(false);
    }
    #region Implementation
    private void SetSortingOrderFct()
	{
		var canvas = GetComponent<Canvas>();
		if(canvas == null)
		{
			Debug.LogWarning("Hud not contains Canvas, Can't set sorting order");
			return;
		}
		canvas.overrideSorting = true;
		canvas.sortingOrder = 500;
	}
	#endregion
}
