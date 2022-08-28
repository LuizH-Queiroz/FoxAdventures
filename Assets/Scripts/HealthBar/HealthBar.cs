using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerStateManager player;


    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = player.maxHealth;
        slider.value = player.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.currentHealth;
    }
}
