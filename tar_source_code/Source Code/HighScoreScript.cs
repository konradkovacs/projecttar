using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HighScoreScript : MonoBehaviour {

    public GameObject score; // A pontokért felelős változó, objektum
    public GameObject scoreName; // A játékos nevéért felelős változó, objektum
    public GameObject rank; // A helyezésért felelős változó, objektum

    // A pontokért felelős metódus, a változók értékét helyezi be a szövegbe
    public void SetScore(string name, string score, string rank)
    {
        this.score.GetComponent<TMP_Text>().text = score;
        this.scoreName.GetComponent<TMP_Text>().text = name;
        this.rank.GetComponent<TMP_Text>().text = rank;
    }
}
