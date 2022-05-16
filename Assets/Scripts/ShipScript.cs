using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    #region  PUBLIC VARIABLES

    public float rotationSpeed = 10f;          //Rotaion of ship in degrees per second
    public float movementSpeed = 20f;          //The movement of ship by force applied in per second

    #endregion

    #region PRIVATE VARIABLES

    private bool isRotating = false;
    public Transform launcher;
    private const string TURN_COROUTINE_FUNCTION = "TurnRotateOnTap";
    private GameManager gameManager;
    #endregion

    #region MONOBEHAVIOUR METHODS

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()                //When gameobject is active ,then we are subscribing 
    {
        MyMobileGameSpace.UserInputHandler.onTapAction += TowardsTouch;
    }

    private void OnDisable()                 //When gameobject is inactive ,then we are desubscribing 
    {
        MyMobileGameSpace.UserInputHandler.onTapAction -= TowardsTouch;
    }

    #region MY PUBLIC METHODS
    #endregion

    public void TowardsTouch(Touch touch)
    {
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);    //It converts screen from pixels coordinates  to world coordinates
        StopCoroutine(TURN_COROUTINE_FUNCTION);
        StartCoroutine(TURN_COROUTINE_FUNCTION, worldTouchPosition);
    }

    /* IEnumerator TurnRotateMoveTowardsTap(Vector3 tempPoint)
     {
         isRotating=true;
         //tempPoint=tempPoint-this.transform.position;   //To find the difference between touch position and current position
        // tempPoint.z=transform.position.z;                //assigning ship position to touch position
         Quaternion startRotation=this.transform.rotation;                              //The rotation start point
         Quaternion endRotation=Quaternion.LookRotation(tempPoint,Vector3.up);         //This rotation will look at touchpoint in up direction 
         float time=Quaternion.Angle(startRotation,endRotation)/rotationSpeed;                  //Angle between two rotations 
         for(float i=0;i<time; i=i+Time.deltaTime)
         {
            transform.rotation= Quaternion.Slerp(startRotation,endRotation,i);
         }
         transform.rotation = endRotation; //we need to put shoot functionality here
         isRotating = false;
         yield return null;
     }*/

    IEnumerator TurnRotateOnTap(Vector3 tempPoint)
    {
        isRotating = true;
        tempPoint = tempPoint - this.transform.position;           //To find the difference between touch position and current
        tempPoint.z = transform.position.z;                //assigning the touch point of z to ship position of z
        Quaternion startRotation = this.transform.rotation;                              //touch the value of ship rotation 
        Quaternion endRotation = Quaternion.LookRotation(tempPoint, Vector3.forward); //This rotation will look at touchpoint in up direction
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, 1);    //Slerp is used for smooth rotation
            yield return null;
        }

        transform.rotation = endRotation;

        Shoot();
        isRotating = false;

    }

    private void Shoot()
    {
        BulletScript bullet = PoolManager.Instance.Spawn(Constants.BULLET_PREFAB_NAME).GetComponent<BulletScript>();
        bullet.SetPosition(launcher.position);
        bullet.SetTrajectory(bullet.transform.position + transform.forward);
    }

    public void OnHit()
    {
        gameManager.LoseLife();
        StartCoroutine(StartInvincibilityTimer(2.5f));
    }

    // Make the ship invincible for a time.
    private IEnumerator StartInvincibilityTimer(float timeLimit)
    {
        GetComponent<Collider2D>().enabled = false;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        float timer = 0;
        float blinkSpeed = 0.25f;

        while (timer < timeLimit)
        {
            yield return new WaitForSeconds(blinkSpeed);

            spriteRenderer.enabled = !spriteRenderer.enabled;
            timer += blinkSpeed;
        }

        spriteRenderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }


    #region MY PRIVATE METHODS
    #endregion
}
