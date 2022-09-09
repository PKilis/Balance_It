using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;


    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject lane;

    public static bool runOnce = false;


    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BlockObject") && !runOnce)
        {
            runOnce = true;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FailArea"))
        {
            GameManager.instance.OpenRestart();
        }
    }

}
