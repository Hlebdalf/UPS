using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonManagment : MonoBehaviour {
    public Text SeedText;
    private Animator GodAnimator;
    private string Seed;
    public GameObject God;

    private void Start() {
        GodAnimator = God.GetComponent <Animator> ();   
    }

    public void StartLoadMenu() {
        //GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("LoadMenuAnimationForward");
    }

    public void StartExitMenu() {
        GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("ConfirmAnimationForward");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void BackFromLoad() {
        //GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("LoadMenuAnimationBack");
        
    }

    public void CancelExit() {
        GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("ConfirmAnimationBack");
    }

    public void StartGame() {
        SceneManager.LoadScene("GameField");
    }

    public void SeedInput() {
        Seed = SeedText.GetComponent<Text>().text;
        print(Seed);
    }
}
