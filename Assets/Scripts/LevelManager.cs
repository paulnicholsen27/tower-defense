using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
public class LevelManager : MonoBehaviour
{

    public static LevelManager main;
    public Transform[] path; 
    public Transform startPoint;
    
    public int currency;

    private void Awake() {
        main = this;
    }

    private void Start() {
        currency = 100;
    }

    public void IncreaseCurrency(int amt) {
        currency += amt;
    }

    public bool SpendCurrency(int amt) {
        if (amt <= currency) {
            currency -= amt;
            return true;
        } else {
            Debug.Log("Not enough money");
            return false;
        }
    }
}
