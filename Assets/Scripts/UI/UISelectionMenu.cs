using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UISelectionMenu : UIPanel
{
    public override void Awake()
    {
        base.Awake();
        Type = ePanelType.SELECTIONMENU;
    }

    public override void Display(bool display, bool force = false)
    {
        base.Display(display, force);

    }

    public void Level1()
    {
        LoadLevel(1);
    }
    
    public void Level2()
    {
        LoadLevel(2);
    }
    public void Level3()
    {
        LoadLevel(3);
    }
    
    public void Level4()
    {
        LoadLevel(4);
    } 
    
    public void Level5()
    {
        LoadLevel(5);
    }
    
    public void Level6()
    {
        LoadLevel(6);
    }

    public void LoadLevel(int value)
    {
        GameManager.Get.level = value;
        GameManager.Get.Continue();
        if (mUIManager)
            mUIManager.SwitchPanel(ePanelType.HUD);
        mUIManager.eventSystem.SetSelectedGameObject(null);
        this.gameObject.SetActive(false);
        GameManager.Get.isPlaying = true;   
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.started && this.gameObject.activeSelf)
        {
            mUIManager.eventSystem.SetSelectedGameObject(null);
            DisplayMainMenu_Click();

        }
    }

    public void DisplayMainMenu_Click()
    {
        this.gameObject.SetActive(false);
        mUIManager.SwitchPanel(ePanelType.MAINMENU);
    }
}
