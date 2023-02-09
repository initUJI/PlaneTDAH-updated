using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public float range;

    private PipesGameManager gameManager;
    public GameObject hitingObject;
    private GameObject lastDirtyPoint = null, lastPipePoint = null;
    public AimRotationAnimation aimAnimationScript;
    public GameObject laserObject;
    public GameObject laserPrefab;

    public UI_Manager_PipeGame UI_Manager;

    public bool recentlyDirtyDestroy = false;

    public float playerDamage;


    // Start is called before the first frame update
    void Start()
    {
        UI_Manager = GameObject.Find("/GameManager").GetComponent<UI_Manager_PipeGame>();
        gameManager = GameObject.Find("/GameManager").GetComponent<PipesGameManager>();
        //laserObject.DisablePrepare();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.hasStarted)
        {
            RaycastHit hit;
            if (!recentlyDirtyDestroy)
            {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
                {
                    aimAnimationScript.ChangeState(1);

                    if (hit.collider.tag == "pipe")
                    {
                        //Está apuntando a una tubería
                        if (hitingObject != null && hitingObject.tag == "dirtyPoint")
                        {
                            stopDoingDirtyPointDamage();
                        }

                        if (Input.touchCount > 0 || Input.GetMouseButton(0))
                        {
                            CrearRayoLaser();

                            if (hit.collider.gameObject != lastPipePoint)
                            {
                                changePipeInShot(hit.collider.gameObject);
                            }
                            
                            interactionWithPipe(hit.collider.gameObject);
                            aimAnimationScript.ChangeState(2);
                            return;
                        }
                        else if (lastPipePoint != null)
                        {
                            changePipeInShot();
                        }

                        DesactivarRayoLaser();
                    }
                    else if (hit.collider.tag == "dirtyPoint")
                    {
                        //Está apuntando a un punto sucio
                        if (lastPipePoint != null)
                        {
                            changePipeInShot();
                        }

                        if (Input.touchCount > 0 || Input.GetMouseButton(0))
                        {
                            CrearRayoLaser();

                            interactionWithDirtyPoint(hit.collider.gameObject);
                            aimAnimationScript.ChangeState(2);
                            return;
                        }else
                        {
                            if (hitingObject != null && hitingObject.tag == "dirtyPoint")
                            {
                                stopDoingDirtyPointDamage();
                            }
                            DesactivarRayoLaser();
                        }

                        aimAnimationScript.ChangeState(1);
                    }
                }
                else
                {
                    //Se ha dejado de pulsar mientras se apunta
                    if (hitingObject != null && hitingObject.tag == "dirtyPoint")
                    {
                        stopDoingDirtyPointDamage();
                    }

                    if (lastPipePoint != null)
                        changePipeInShot();

                    if (Input.touchCount > 0 || Input.GetMouseButton(0))
                    {
                        CrearRayoLaser();

                        aimAnimationScript.ChangeState(2);
                        return;
                    }
                    DesactivarRayoLaser();
                    aimAnimationScript.ChangeState(0);
                }
            }
            else
            {
                if (hitingObject != null && hitingObject.tag == "dirtyPoint")
                {
                    stopDoingDirtyPointDamage();
                }
                
                if (Input.touchCount == 0 || !Input.GetMouseButton(0))
                    recentlyDirtyDestroy = false;
            }
        }else
        {
            if (hitingObject != null && hitingObject.tag == "dirtyPoint")
            {
                stopDoingDirtyPointDamage();
            }

            //No apunta a nada
            changePipeInShot();
            aimAnimationScript.ChangeState(0);
            DesactivarRayoLaser();
        }
    }
   
    void CrearRayoLaser()
    {
        //Se crea el rayo
        if (laserObject == null)
            laserObject = (GameObject)Instantiate(laserPrefab, this.transform.GetChild(0));
        
    }

    void DesactivarRayoLaser()
    {
        if(laserObject != null)
        {
            laserObject.GetComponent<EGA_Laser>().DisablePrepare();
            laserObject = null;

            if(UI_Manager.userAccuarcyError)
            {
                UI_Manager.userAccuarcyError = false;
            }
        }
    }

    void changePipeInShot(GameObject newPipe = null)
    {
        if(lastPipePoint != null && lastPipePoint.GetComponent<PipeColliderManager>() != null)
            lastPipePoint.GetComponent<PipeColliderManager>().FinishDamageToPipe();
        
        lastPipePoint = newPipe;
    }

    void stopDoingDirtyPointDamage()
    {
        if (hitingObject != null && hitingObject.tag == "dirtyPoint")
            hitingObject.GetComponent<DirtyPointObject>().gettingDamage = false;
    }
    void interactionWithPipe(GameObject pipeSelected)
    {
        if(pipeSelected.GetComponent<PipeColliderManager>() != null)
        {
            hitingObject = pipeSelected.GetComponent<PipeColliderManager>().modelReferenceToCollider;
            pipeSelected.GetComponent<PipeColliderManager>().StartDamageToPipe();
        }
        UI_Manager.increasingHeatOfPipe(/*playerDamage*/);
    }

    void interactionWithDirtyPoint(GameObject dirtyPointSelected)
    {
        hitingObject = dirtyPointSelected;
        if(!dirtyPointSelected.GetComponent<DirtyPointObject>().gettingDamage)
        {
            dirtyPointSelected.GetComponent<DirtyPointObject>().gettingDamage = true;
        }

        if (dirtyPointSelected.GetComponent<DirtyPointObject>().restLife(playerDamage * Time.deltaTime))
        {
            recentlyDirtyDestroy = true;
            aimAnimationScript.ChangeState(0);
            UI_Manager.restDirtyPoints(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range);
    }
}
