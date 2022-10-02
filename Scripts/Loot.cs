using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{

    private float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        spinSpeed = Random.Range(5f, 40f);

        int chooseDir = Random.Range(0, 2);
        if (chooseDir == 1)
        {
            spinSpeed = spinSpeed * -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
