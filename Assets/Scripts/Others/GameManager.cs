using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public PlayerManager playerManager;
    Movement movement;
    public FxManager fxmanager;
    public Animator TransitionAnimator;
    public int level = 0;

    

    #region Sound   
    public float musicVolume = 1f;
    public float effectsVolume = 1f;
    public float environmentVolume = 1f;
    #endregion


    public static GameManager Get
    {
        get
        {
            return Instance;
        }
    }

    IEnumerator LoadLevel()
    {
        TransitionAnimator.SetTrigger("close");
        yield return new WaitForSeconds(.85f);

	    AudioManager.Get.StopAmbient();

        AudioManager.Get.StopMusic();

        DontDestroyOnLoad(playerManager.GetComponentInParent<Transform>().gameObject);

        SceneManager.LoadScene(level);
        while (SceneManager.GetActiveScene().buildIndex != level)
        {
            yield return null;
        }
        movement.controller.enabled = false;
        playerManager.transform.position = GameObject.Find("LevelStart").transform.position;
        movement.controller.enabled = true;

        yield return new WaitForSeconds(.85f);
        TransitionAnimator.SetTrigger("open");


    }

    public void NextLevel()
    {
        level += 1;
	if (level == 7) 
	{
		level = 0;
	}    
        StartCoroutine(LoadLevel());
        
    }

    public void Continue()
    {
        StartCoroutine(LoadLevel());
    }

    #region settings
    public bool isPlaying = false;
    #endregion
    private void Awake()
    {
        level = 0;
        DontDestroyOnLoad(gameObject);
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        movement = playerManager.gameObject.GetComponent<Movement>();
    }
}
