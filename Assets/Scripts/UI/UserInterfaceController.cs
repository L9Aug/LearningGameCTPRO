using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using FSM;
using Condition;

public class UserInterfaceController : MonoBehaviour
{
    public static UserInterfaceController UIC;

    public Image HealthBarImg;
    public Text AmmoCount;

    public Image ReloadUIImg;
    public Text ReloadText;

    public GameObject GameOverUIObj;
    public GameObject PauseMenuObj;
    public SpawnAI MySpawner;

    StateMachine PausedMachine;
    bool isGamePaused = false;

	void Start()
    {
        UIC = this;
        HideReloadUI();
        SetupStateMachine();
    }

    private void Update()
    {
        PausedMachine.SMUpdate();
    }

    public void OnHealthChanged(float Change, float Health, float MaxHealth)
    {
        HealthBarImg.fillAmount = Health / MaxHealth;
        if (Change < 0)
        {
            PlayerMetricsController.PMC.DamageTaken -= Change;
        }

        if (Health <= 0)
        {
            PlayerController.PC.IsDead = true;
        }
    }

    public void OnWeaponUpdate(int Magazine, int MagazineSize)
    {
        AmmoCount.text = Magazine + "/" + MagazineSize;

        if(Magazine <= MagazineSize / 6)
        {
            //display Reaload UI
            ReloadText.gameObject.SetActive(true);
        }
        if(Magazine == 0)
        {
            ReloadUIImg.gameObject.SetActive(true);
        }
    }

    public void AnimateReload()
    {
        ReloadUIImg.gameObject.SetActive(true);
        ReloadText.gameObject.SetActive(false);
        StartCoroutine(ReloadAnimation());
    }

    public void HideReloadUI()
    {
        ReloadUIImg.gameObject.SetActive(false);
        ReloadText.gameObject.SetActive(false);
    }

    IEnumerator ReloadAnimation()
    {        
        while (PlayerController.PC.CurrentWeapon.reloading)
        {
            // animate reload symbol.
            ReloadUIImg.GetComponent<RectTransform>().Rotate(Vector3.forward, 90 * Time.deltaTime, Space.Self);
            yield return null;
        }
        ReloadUIImg.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        HideReloadUI();
    }

    public void DisplayGameOver()
    {
        GameOverUIObj.SetActive(true);
    }

    bool IsGamePaused()
    {
        return isGamePaused;
    }

    void PausedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
        }
    }

    public void ResumeFunc()
    {
        isGamePaused = false;
    }

    public void Restart()
    {
        MySpawner.RemoveAllAI();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void PauseGameFunc()
    {
        Time.timeScale = 0;
        PauseMenuObj.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void UnPauseGameFunc()
    {
        Time.timeScale = 1;
        PauseMenuObj.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetupStateMachine()
    {
        // conditions.
        BoolCondition IsGamePausedCond = new BoolCondition(IsGamePaused);
        NotCondition IsGameNotPausedCond = new NotCondition(IsGamePausedCond);

        // transisiton.
        Transition PauseGame = new Transition("Pause Game", IsGamePausedCond, PauseGameFunc);
        Transition ResumeGame = new Transition("Resume Game", IsGameNotPausedCond, UnPauseGameFunc);

        // states.
        State UnPaused = new State("UnPaused",
            new List<Transition>() { PauseGame },
            new List<Action>() { },
            new List<Action>() { PausedUpdate },
            new List<Action>() { });

        State Paused = new State("Paused",
            new List<Transition>() { ResumeGame },
            new List<Action>() { },
            new List<Action>() { PausedUpdate },
            new List<Action>() { });

        // set target states.
        PauseGame.SetTargetState(Paused);
        ResumeGame.SetTargetState(UnPaused);

        // setup machine.
        PausedMachine = new StateMachine(null, UnPaused, Paused);
        PausedMachine.InitMachine();
    }

}
