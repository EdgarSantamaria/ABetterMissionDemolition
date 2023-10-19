using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //fields set in the Unity pane
    [Header("Inscribed")]
    // Array to store the projectiles
    public GameObject[] projectilePrefabs;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    //fields set dynamically
    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private int currentPrefabIndex = 0; // Index of the current projectile prefab

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }



    private void OnMouseEnter()
    {
        //print("Slingshot:OneMouseEnter");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot:OneMouseExit");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Randomly select a prefab
        currentPrefabIndex = Random.Range(0, projectilePrefabs.Length);
        //Instantiate a Projectile from the array of projectilePrefabs
        projectile = Instantiate(projectilePrefabs[currentPrefabIndex]) as GameObject;
        // Check if the selected prefab is a Missile and adjust velocityMult accordingly
        if (projectile.CompareTag("Missile"))
        {
            velocityMult = 20.0f; // Adjust the value as needed for the Missile speed
        }
        else
        {
            velocityMult = 10.0f; // Set the default velocityMult for other projectiles
        }

        //Start it at the launchPoint
        projectile.transform.position = launchPos;
        //Set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        //If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0) )
        {
            //The mouse has been released
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            //Switch to Slingshot view immediately before setting POI
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);  //prevents button from "breaking"
            FollowCam.POI = projectile;  //Set the _MainCamera POI
            //Add a ProjectileLine to the Projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }
    
}
