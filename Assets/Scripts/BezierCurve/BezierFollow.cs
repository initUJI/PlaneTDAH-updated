using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    public int routeToGo;

    private float tParam;

    private Vector3 objectPosition;

    public float speedModifier;

    private bool coroutineAllowed;
    private FluidGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0;
        speedModifier = 0.5f;
        coroutineAllowed = true;
        gameManager = GameObject.Find("GameManager").GetComponent<FluidGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(coroutineAllowed && gameManager.hasStarted)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int actualRoute)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[actualRoute].GetChild(0).position;
        Vector3 p1 = routes[actualRoute].GetChild(1).position;
        Vector3 p2 = routes[actualRoute].GetChild(2).position;
        Vector3 p3 = routes[actualRoute].GetChild(3).position;

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0;

        routeToGo += 1;
        Debug.Log(routes.Length - 1 + "," + routeToGo);

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;

        coroutineAllowed = true;
    }
}
