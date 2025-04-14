//******************************************************************************
// Authors: Frederic SETTAMA  
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UISettingsPanel : UIPanel
{
	#region Script Parameters
    private GameObject currentSubpanel;
	[SerializeField] private List<UIButtons> Btns;
    [SerializeField] List<GameObject> SubPanels = new List<GameObject>();


    #endregion

    #region Sliders
    public Slider music;
    public Slider effects;
    public Slider environment;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();
        Type = ePanelType.SETTINGS;
        currentSubpanel = SubPanels[0];
        foreach (GameObject obj in SubPanels)
        {
            if (obj != currentSubpanel)
                obj.SetActive(false);
        }

    }
    // Start is called before the first frame update
    void Start()
	{
    }

    // Update is called once per frame
    public override void Update()
	{
        base.Update();
        ReadSliderValue();
        foreach (GameObject obj in SubPanels)
        {
            if (obj != currentSubpanel)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
            else if (obj == currentSubpanel)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }

        }
    }
	#endregion

	#region Methods
	public override void Display(bool display, bool force = false)
	{
		base.Display(display, force);
	}

	public void DisplayMainMenu_Click()
	{
		if (mUIManager != null)
			mUIManager.SwitchPanel(ePanelType.MAINMENU);
        this.gameObject.SetActive(false);
        mUIManager.cancel = true;
	}

    public void DisplayCredits_Click()
    {
        Debug.Log("Credits");
        if (currentSubpanel == SubPanels[1])
            return;
        else
        {
            currentSubpanel = SubPanels[1];
        }
    }

    public void DisplayControl_Click()
    {
        Debug.Log("Control");
        if (currentSubpanel == SubPanels[0])
            return;
        else
            currentSubpanel = SubPanels[0];
    }

    public void DisplayAudio_Click()
    {
        Debug.Log("Audio");
        if (currentSubpanel == SubPanels[2])
            return;
        else
            currentSubpanel = SubPanels[2];
    }

    public void ResetVolumeOnClick()
    {
        music.value = 50f;
        effects.value = 50f;
        environment.value = 50f;
    }

    public void cancelInput(InputAction.CallbackContext context)
    {
        if (context.started && this.gameObject.activeSelf)
        {
            mUIManager.eventSystem.SetSelectedGameObject(null);
            DisplayMainMenu_Click();

        }
    }

    public void ReadSliderValue()
    {
        GameManager.Get.musicVolume = music.value / 100f;
        GameManager.Get.effectsVolume = effects.value / 100f;
        GameManager.Get.environmentVolume = environment.value / 100f;
    }
	#endregion

	#region Implementation

	#endregion
}
