using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float blinkTime;
    float timer;
    bool toggle;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        toggle = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkTime)
        {
            text.enabled = toggle;

            toggle = !toggle;
            timer = 0;
        }
    }
}
