using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: Mehmet
//


public class Bow : MonoBehaviour
{
    
    public GameObject arrowPrefab;
    public int ammunition = -1;    // -1 = infinite ammmunition
    public float arrowSpeed = 4;
    public float arrowLifetime = 4.0f;
    
    private float direction;
    

    public void Fire()
    {
        if (ammunition == -1 || ammunition > 0)
        {
            direction = (transform.rotation.y > 0) ? 1.0f : -1.0f;
        
            // Create arrow and place it in correct position and rotation
            GameObject arrow = Instantiate(arrowPrefab, transform.position + new Vector3(0,0.5f,0), transform.rotation * Quaternion.Euler(90,0,0));
            arrow.GetComponent<Rigidbody>().velocity = new Vector3(arrowSpeed*direction, 0,0);
            arrow.GetComponent<Arrow>().StartCoroutine("DestroyWhenFinished", arrowLifetime);

            ammunition = (ammunition > 0) ? ammunition-1 : -1;
            if (ammunition == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
