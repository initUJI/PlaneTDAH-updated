using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform UI_transform;
    float oscillation, maximumSize;
    float timer;

    void Start()
    {
        UI_transform = GetComponent<RectTransform>();
        timer = 0;

        oscillation = 500;
        maximumSize = 3000;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UI_transform.sizeDelta = new Vector2(maximumSize + Mathf.PingPong(Time.time * 200 , oscillation), UI_transform.sizeDelta.y);
    }

    private float Oscillate(float min, float max, float value)
    {
        float range = max - min;

        float multiple = value / range;

        bool ascending = multiple % 2 == 0;
        float modulus = value % range;

        return ascending ? modulus + min : max - modulus;
    }
}
