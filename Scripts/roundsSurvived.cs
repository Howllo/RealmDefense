using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class roundsSurvived : MonoBehaviour
{
    public Text roundsText;


    private void OnEnable()
    {
        StartCoroutine(animateText());
    }

    IEnumerator animateText()
    {
        roundsText.text = "0";
        int round = 0;

        while(round < PlayerStats.rounds)
        {
            round++;
            roundsText.text = round.ToString();

            yield return new WaitForSeconds(0.5f);
        }
    }

}
