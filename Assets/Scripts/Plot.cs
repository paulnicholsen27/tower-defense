using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;
    
    private void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        Debug.Log("Entered");
        sr.color = hoverColor;
    }

    private void OnMouseExit() {
        sr.color = startColor;
    }

    private void OnMouseDown() {
        Debug.Log("Build Tower Here: " + name);
        if (tower != null) {
            return;
        }
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        Debug.Log(towerToBuild);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
