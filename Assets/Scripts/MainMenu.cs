using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    SceneController sceneController;

    AudioSource keyTap;
    GameObject mainMenu;
    GameObject controls;
    GameObject options;    

    public Slider camSensX;
    public Slider camSensY;
    public Toggle invertYToggle;
    public Text endGameSecretText;
    public Button playButton;
    public Button controlsButton;
    public Button optionsButton;

   // public float minX;
    //public float maxX;
    //public float minY;
   // public float maxY;
    //Vector3 mousePos;

    private const string Y_SENSITIVITY = "Y-Sensitivity";
    private const string X_SENSITIVITY = "X-Sensitivity";
    private const string INVERT_CAMERA = "Invert-Camera";
    private const string ENDGAME_SECRET = "Endgame-Secret";

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        mainMenu = GameObject.FindGameObjectWithTag("MenuMain");
        controls = GameObject.FindGameObjectWithTag("MenuControls");
        options = GameObject.FindGameObjectWithTag("MenuOptions");
        keyTap = GetComponent<AudioSource>();        
        mainMenu.SetActive(true);
        controls.SetActive(false);
        options.SetActive(false);
        camSensX.value = PlayerPrefs.GetFloat(X_SENSITIVITY);
        camSensY.value = PlayerPrefs.GetFloat(Y_SENSITIVITY);
        if (sceneController.endGameSecretBool)
        {
            SetEndGameSecret(sceneController.endGameSecretBool);
        }        
        invertYToggle.isOn = sceneController.GetBoolPref(INVERT_CAMERA, false);
        sceneController.endGameSecretBool = sceneController.GetBoolPref(ENDGAME_SECRET, false);
        endGameSecretText.enabled = sceneController.endGameSecretBool;  
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void FixedUpdate()
    {
        if (Input.GetButtonDown("Back") && !mainMenu.activeInHierarchy)
        {
            BackToMM();
        }
    }

    public void Controls()
    {
        keyTap.Play();
        options.SetActive(false);
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void Options()
    {
        keyTap.Play();        
        mainMenu.SetActive(false);
        controls.SetActive(false);
        options.SetActive(true);
    }

    public void BackToMM()
    {
        keyTap.Play();
        if (controls.activeInHierarchy)
        {
            controls.SetActive(false);
            options.SetActive(false);
            mainMenu.SetActive(true);
            controlsButton.Select();
        }

        if (options.activeInHierarchy)
        {
            controls.SetActive(false);
            options.SetActive(false);
            mainMenu.SetActive(true);
            optionsButton.Select();
        }                
    }

    public void SetEndGameSecret(bool state)
    {
        sceneController.SetPref(ENDGAME_SECRET, state);
    }

    #region Pref Sensitivity

    public void SetXSensitivity(Single value)
    {
        sceneController.SetPref(X_SENSITIVITY, value);
    }
    public void SetYSensitivity(Single value)
    {
        sceneController.SetPref(Y_SENSITIVITY, value);
    }

    #endregion

    #region Pref InvertY

    public void SetInvertY(bool state)
    {
        sceneController.SetPref(INVERT_CAMERA, state);
    }

    #endregion   
}
