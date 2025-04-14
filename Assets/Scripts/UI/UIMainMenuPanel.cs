//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMainMenuPanel : UIPanel
{
	#region Script Parameters
	[SerializeField] private GameObject firstMenu;	
	[SerializeField] private GameObject secondMenu;
	[SerializeField] private UIButtons startButton;
	[SerializeField] private List<UIButtons> Buttons;
	private GameObject current;
	#endregion

	#region Fields
	private UIButtons mCurrentBtn;
	public UIButtons CurrentBtn
	{
		get { return mCurrentBtn; }
		private set
		{
			if (mCurrentBtn != null)
				mCurrentBtn.SetEnable(false);
			mCurrentBtn = value;
			if(mCurrentBtn != null)
				mCurrentBtn.SetEnable(true);
		}
	}
    #endregion

    #region Properties

    #endregion

    #region Unity Methods
    public override void Awake()
	{
		base.Awake();
		Type = ePanelType.MAINMENU;
		if (firstMenu == null)
			Debug.LogError("Menu 1 not setupe");
		if (secondMenu == null)
			Debug.LogError("Menu 2 not setupe");
		current = firstMenu;
		mCurrentBtn = null;
        secondMenu.SetActive(false);

	}
	// Update is called once per frame
	public override void Update()
	{
		CheckIfRestart();
		if (mUIManager.eventSystem && mUIManager.eventSystem.currentSelectedGameObject == null)
		{
			if (firstMenu.activeSelf)
				mUIManager.eventSystem.SetSelectedGameObject(DefaultBtn);
			else
				mUIManager.eventSystem.SetSelectedGameObject(startButton.gameObject);
		}
	}
	#endregion

	#region Methods

	public void CheckIfRestart()
	{
		if (mUIManager.cancel)
		{
			LoadSecondMenu_Click();
			mUIManager.cancel = false;
        }
	}
	public override void Display(bool display, bool force = false)
	{
		base.Display(display, force);
	}

	public void LoadSecondMenu_Click()
	{
		Debug.Log("Press");
        firstMenu.SetActive(false);
        secondMenu.SetActive(true);
		if (mUIManager.eventSystem.currentSelectedGameObject != startButton.gameObject || !mUIManager.eventSystem.currentSelectedGameObject)
			mUIManager.eventSystem.SetSelectedGameObject(startButton.gameObject);
		
    }

	public void LoadGame_Click()
	{
		if (mUIManager)
			mUIManager.SwitchPanel(ePanelType.HUD);
		mUIManager.eventSystem.SetSelectedGameObject(null);
        this.gameObject.SetActive(false);
		if(GameManager.Get.level == 0)
		{
			GameManager.Get.level = 1; 
			GameManager.Get.Continue();
            GameManager.Get.isPlaying = true;
        }
        else
		{

            if (GameManager.Get.playerManager.transform.position != GameObject.Find("LevelStart").transform.position)
                GameManager.Get.isPlaying = true;

        }
    }

    public void DisplayTutorial_Click()
	{
		// Display tutorail
	}

	public void ExitGame_Click()
	{
		Application.Quit();
	}

	public void LoadLevelSelection()
	{
        if (mUIManager)
		{
            mUIManager.SwitchPanel(ePanelType.SELECTIONMENU);
            mUIManager.eventSystem.currentSelectedGameObject.GetComponent<UIButtons>().SetEnable(false);
            mUIManager.eventSystem.SetSelectedGameObject(mUIManager.Level.gameObject);
        }
		this.gameObject.SetActive(false);

    }
	public void DisplaySettings_Click()
	{
		if (mUIManager)
		{
            mUIManager.SwitchPanel(ePanelType.SETTINGS);
			mUIManager.eventSystem.currentSelectedGameObject.GetComponent<UIButtons>().SetEnable(false);
            mUIManager.eventSystem.SetSelectedGameObject(mUIManager.Control.gameObject);

        }
        this.gameObject.SetActive(false);

    }
    #endregion

    #region Implementation

    #endregion
}
