//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	#region Script Parameters
	public ePanelType DefaultPanelType = ePanelType.NOT_DEFINE;
	public bool AutoRefreshListPanel = false;
	public bool DisplayHudOnStart = false;
	public List<UIPanel> Panels = new List<UIPanel>();
	[SerializeField] public EventSystem eventSystem;
    [SerializeField] public UnityEngine.UI.Button Control;
    [SerializeField] public UnityEngine.UI.Button Level;
    [SerializeField] public UnityEngine.UI.Button StartButton;
	public bool cancel = false;	

    #endregion

    #region Fields
    private Dictionary<ePanelType, UIPanel> mPanels = new Dictionary<ePanelType, UIPanel>();
	private UIPanel mCurrentPanel = null;
	private UIPanel mHud = null;
	#endregion

	#region Properties
	#endregion

	
	#region Static
	public static UIManager Instance
	{
		get
		{
			if (mInstance == null)
				Debug.LogWarning("UIManager not initializated");
			return mInstance;
		}
	}
	private static UIManager mInstance;
	#endregion

	#region Unity Methods
	private void Awake()
	{
		if (mInstance != null && mInstance != this)
		{
			Destroy(gameObject);
			return;
		}
		if (mInstance == null)
			mInstance = this;
		if (transform.parent == null)
			DontDestroyOnLoad(gameObject);
		if (AutoRefreshListPanel)
			GetAllPanel();
	}

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		InitDictionnaryOfPanel();
		if(DefaultPanelType != ePanelType.NOT_DEFINE)
			DisplayDefaultPanel();
		if (DisplayHudOnStart)
			DisplayHUD(true);
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(eventSystem.currentSelectedGameObject);
	}
	#endregion

	#region Methods
	public void SwitchPanel(ePanelType type)
	{
		if (mCurrentPanel && mCurrentPanel.Type == type)
			return;
		UIPanel nextPanel;
		if (mPanels.TryGetValue(type, out nextPanel))
		{
			if(mCurrentPanel)
				mCurrentPanel.Display(false);
			mCurrentPanel = nextPanel;
			mCurrentPanel.Display(true);
		}
		else
			Debug.LogWarning("No panel of type : " + nameof(type) + " is defined");
	}

	public void DisplayHUD(bool display)
	{
		if(mHud)
		{
			mHud.Display(display);
		}
	}
	public void DisplayDefaultPanel()
	{
		SwitchPanel(DefaultPanelType);
	}
	#endregion

	#region Implementation
	private void GetAllPanel()
	{
		UIPanel tmpPanel;
		foreach(Transform child in transform)
		{
			if(child.TryGetComponent<UIPanel>(out tmpPanel) && !Panels.Contains(tmpPanel))
			{
				Panels.Add(tmpPanel);
			}
		}
	}
	private void InitDictionnaryOfPanel()
	{
		if (Panels == null || Panels.Count == 0)
			return;
		foreach(var panel in Panels)
		{
			if(panel.Type == ePanelType.NOT_DEFINE)
			{
				Debug.LogWarning("Panel type for " + panel.name + " is not defined");
				continue;
			}
			if(mPanels.ContainsKey(panel.Type))
			{
				Debug.LogWarning("More than one panel (type : " + nameof(panel.Type) + ") is defined");
				continue;
			}
			else
			{
				mPanels.Add(panel.Type, panel);
				panel.SetUIManager(this);
				if (panel.Type == ePanelType.HUD)
					mHud = panel;
			}
		}
	}
	#endregion
}
