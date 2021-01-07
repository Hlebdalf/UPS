using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonManagment : MonoBehaviour {
    private Animator GodAnimator;
    private string seed;

    public GameObject God;
    public Text SeedText;

    private void Start() {
        GodAnimator = God.GetComponent <Animator> ();
    }

    public void StartSettingsMenu() {
        GodAnimator.Play("LoadMenuAnimationForward");
    }

    public void BackFromSettings() {
        GodAnimator.Play("LoadMenuAnimationBack");
    }

    public void StartExitMenu() {
        GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("ConfirmAnimationForward");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void CancelExit() {
        GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("ConfirmAnimationBack");
    }

    public void StartGame() {
        SceneManager.LoadScene("GameField");
    }

    public void SeedInput() {
        seed = SeedText.GetComponent <Text> ().text;
    }
}
