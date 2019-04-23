using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSSUIController : MonoBehaviour {

    #region Varriables

    PlayerController playerController;
    SceneController sceneController;
    public GameObject optionsMenu;
    
    public Slider camSensX;
    public Slider camSensY;
    public Text sensXText;
    public Text sensYText;
    public Toggle invertYToggle;

    public bool isPaused;
    bool optionsInput;
    bool toggleOptions = false;

    private const string Y_SENSITIVITY = "Y-Sensitivity";
    private const string X_SENSITIVITY = "X-Sensitivity";
    private const string INVERT_CAMERA = "Invert-Camera";

    #endregion

    #region Start/Update

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }
    
    // Use this for initialization
    void Start ()
    {
        camSensX.maxValue = playerController.mouseSenseXMax;
        camSensX.minValue = playerController.mouseSenseXStart;
        camSensY.maxValue = playerController.mouseSenseYMax;
        camSensY.minValue = playerController.mouseSenseYStart;
        camSensX.value = PlayerPrefs.GetFloat(X_SENSITIVITY);
        camSensY.value = PlayerPrefs.GetFloat(Y_SENSITIVITY);
        invertYToggle.isOn = GetBoolPref(INVERT_CAMERA);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("o"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleting Saved Data");
        }

        if (Input.GetButtonDown("OptionsMenu"))
        {
            toggleOptions = !toggleOptions;
            isPaused = !isPaused;
        }

        optionsMenu.SetActive(toggleOptions ? true : false);
        Cursor.visible = toggleOptions ? true : false;
        Cursor.lockState = toggleOptions ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        OptionsMenu();
    }

    #endregion

    #region OptionsMenu

    public void OptionsMenu()
    {
        //sensXText.text = "" + (int)camSensX.value;
        //sensYText.text = "" + (int)camSensY.value;
        
        playerController.mouseSensX = camSensX.value;
        playerController.mouseSensY = camSensY.value;
        
    }

    public void QuitToDesktop()
    {
        sceneController.EndGame();
    }

    public void RestartGame()
    {
        sceneController.RestartGame();
    }

    #endregion

    #region Pref InvertY

    public void SetInvertY(bool state)
    {
         SetPref(INVERT_CAMERA, state);     
    }

    #endregion

    #region Pref Sensitivity

    public void SetXSensitivity(Single value)
    {
        SetPref(X_SENSITIVITY, value);
    }
    public void SetYSensitivity(Single value)
    {
        SetPref(Y_SENSITIVITY, value);
    }

    #endregion

    #region Pref Setters

    private void SetPref(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private void SetPref(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    private void SetPref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    private void SetPref(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    private bool GetBoolPref(string key, bool defaultValue = true)
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue)));
    }
    #endregion
}
