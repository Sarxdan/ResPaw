//Created By Mehmet 
//Peer-Reviewed by Daniel
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LibraPlatforms : MonoBehaviour
{
    //initializing all platforms and adding their original positions.
    GameObject[] Platform;
    List<Vector3> orgPos = new List<Vector3>();
    public LayerMask mask;
    Transform MediumDistance;

    //initializing the list, containing all objects that can interact with the scale.
    List<GameObject> gameObjects = new List<GameObject>();
    public float maxDistance = 20f;
    [SerializeField] float minDistance = 20f;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        //initializing masks, platforms and all interactable with the scale.
        mask = LayerMask.GetMask("Player", "Enemy");
        Platform = GameObject.FindGameObjectsWithTag("Platforms");
        MediumDistance = GameObject.Find("MediumDistance").transform;

        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));




        //checking how many platforms we have and adding the original position of the platforms.
        foreach (GameObject plats in Platform)
        {
            orgPos.Add(plats.transform.position);
            if (plats.transform.position.y <= minDistance)
                minDistance = plats.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Grabbing how many objects are on the platforms and segmenting them based on the most objects per platform.
        List<int> platformCount = getEntityCount();
        List<int> mostDensePlatform = getDensePlatform(platformCount);
        bool blocked = false;
        //both scales aren't on a stalemate.
        if (mostDensePlatform.Count != platformCount.Count)
        {
            //starts moving the heaviest platform downwards and checking that it doesn't bottom out.
            foreach (int index in mostDensePlatform)
            {

                Platform[index].transform.position = Vector3.MoveTowards(Platform[index].transform.position, orgPos[index], speed * Time.deltaTime);

                if (Platform[index].transform.position.y <= orgPos[index].y && !blocked)
                {
                    blocked = true;
                }
            }
            //if the heaviest platform didn't bottom out, move the lighter platforms upwards :)
            if (!blocked)
            {
                for (int i = 0; i < platformCount.Count; i++)
                {

                    if (!mostDensePlatform.Contains(i))
                    {
                        Vector3 platformPosition = Platform[i].transform.position;
                        platformPosition.y = orgPos[i].y + maxDistance;
                        Platform[i].transform.position = Vector3.MoveTowards(Platform[i].transform.position, platformPosition, speed * Time.deltaTime);
                    }
                }
            }
        }
        else
        //if the positions are at a stale mate, reset the scales to the average min and max distance
        {
            for (int i = 0; i < platformCount.Count; i++)
            {
                Vector3 destinationPos = orgPos[i];

                destinationPos.y = MediumDistance.position.y;

                Platform[i].transform.position = Vector3.MoveTowards(Platform[i].transform.position, destinationPos, speed * Time.deltaTime);
            }
        }
    }

    private List<int> getEntityCount()
    {
        //initializing a list for the amount of objects on each scale.
        List<int> platformCount = new List<int>();

        //Sets colliders both above and below the scales, the above collider checks how many interactable objects are sitting on the scale
        //the below collider makes sure you don't get squished.
        for (int i = 0; i < Platform.Length; i++)
        {
            GameObject platforms = Platform[i];

            Vector3 hitboxSize = new Vector3(0.9f, 0.5f, 1f);
            Vector3 boxSize = platforms.transform.position;
            boxSize.y += hitboxSize.y;
            Collider[] AboveCollider = Physics.OverlapBox(boxSize, hitboxSize, Quaternion.identity, mask);

            boxSize = platforms.transform.position;
            boxSize.y -= 0.4f;
            Collider[] BelowCollider = Physics.OverlapBox(boxSize, new Vector3(0.9f, 0.1f, 1), Quaternion.identity, mask);
            if (BelowCollider.Length != 0)
            {
                platformCount.Clear();
                break;
            }

            platformCount.Add(AboveCollider.Length);

        }

        return platformCount;
    }
    //We are getting the platforms with the most interactable objects
    private List<int> getDensePlatform(List<int> platformCount)
    {
        if (platformCount.Count == 0)
            return platformCount;

        int maxObjectsCount = platformCount.Max();
        List<int> mostDensePlatform = new List<int>();

        for (int i = 0; i < platformCount.Count; i++)
            if (platformCount[i] == maxObjectsCount)
                mostDensePlatform.Add(i);

        return mostDensePlatform;
    }



}
