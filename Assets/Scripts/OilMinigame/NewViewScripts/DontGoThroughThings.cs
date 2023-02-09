// Script from Daniel Brauer, Adrian
// http://wiki.unity3d.com/index.php?title=DontGoThroughThings

using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer
    public float skinWidth = 0.1f; //probably doesn't need to be changed

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;

    public GameObject waterParticle;
    public GameObject poatToFill;

    public Transform poatParent;

    public bool activated = true;

    //initialize values
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        previousPosition = myRigidbody.position;
        poatToFill = GameObject.Find("PoatToFill");
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;

        poatParent = GameObject.Find("Main Camera/MainPoat").transform;
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent?
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                if (!hitInfo.collider)
                    return;

                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);

                if (!hitInfo.collider.isTrigger)
                    myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
            }
        }

        previousPosition = myRigidbody.position;
    }

    

    public void DesactivateRigidbody(bool parent)
    {
        activated = false;
        if(parent)
        {
            GameObject particle = (GameObject) Instantiate(waterParticle, transform.position, Quaternion.identity);
            particle.transform.localScale = new Vector3(1, 1, 1);
            particle.transform.parent = poatToFill.transform;
        }
        else
        {
            Instantiate(waterParticle, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

}