using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextForMan : MonoBehaviour
{
    LayerMask mask;
    [SerializeField] Collider[] AboveCollider;
    [SerializeField] Vector3 hitboxSize;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayers();
    }

    private void ShowText()
    {
        Debug.Log("I am being called");
    }

    private void GetPlayers()
    {
        Vector3 boxSize = transform.position;
        boxSize.y += hitboxSize.y;
        AboveCollider = Physics.OverlapBox(boxSize, hitboxSize, Quaternion.identity, mask);
        if(AboveCollider.Length != 0)
        {
            ShowText();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(2f, 1f, 10f));
    }
}
