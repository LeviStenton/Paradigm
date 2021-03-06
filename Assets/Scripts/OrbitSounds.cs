﻿using UnityEngine;
using System.Collections;

public class OrbitSounds : MonoBehaviour
{

    public Transform target;
    public float orbitDistance = 10.0f;
    public float orbitDegreesPerSec = 180.0f;
    public Vector3 relativeDistance = Vector3.zero;

    public AudioClip[] clips;
    private int clipIndex;
    private AudioSource audioClips;
    private bool audioPlaying = false;

    // Use this for initialization
    void Start()
    {

        if (target != null)
        {
            relativeDistance = transform.position - target.position;
        }

        audioClips = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioClips.isPlaying)
        {
            clipIndex = Random.Range(0, clips.Length - 1);
            audioClips.clip = clips[clipIndex];
            audioClips.PlayDelayed(Random.Range(20f, 50f));
        }
    }

    void Orbit()
    {
        if (target != null)
        {
            // Keep us at the last known relative position
            transform.position = target.position + relativeDistance;
            transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);
            // Reset relative position after rotate
            relativeDistance = transform.position - target.position;
        }
    }

    void LateUpdate()
    {

        Orbit();

    }    
}