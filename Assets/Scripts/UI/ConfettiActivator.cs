using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiActivator : MonoBehaviour
{
    public ParticleSystem winConfetti;

    public void Win()
    {
        winConfetti.Play();
    }
    
}
