using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{

    public GameObject brush;
    private float brushSize = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray rayoMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(rayoMouse, out hit))
            {
                GameObject go = Instantiate(brush, hit.point, Quaternion.identity, transform);

                go.transform.localScale = Vector3.one * brushSize;
            }
        }
    }
}
