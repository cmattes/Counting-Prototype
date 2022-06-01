using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text CounterText;
    public int Count = 0;

    private ParticleSystem goal;

    private void Start()
    {
        Count = 0;
        goal = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Count += 1;
            CounterText.text = "Count : " + Count;
            goal.Play();
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
