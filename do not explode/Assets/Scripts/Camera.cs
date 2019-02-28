using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    GameObject player;
    public Vector3 needPos;
    public float rotation;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        needPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }
    public void FixedUpdate()
    {
        InterpolateToNeeded();
    }
    public void InterpolateToNeeded()
    {
        if ((needPos - transform.position).magnitude < 0.00011f)
        {
            transform.position = needPos;
        }
        else
        {
            transform.position += 0.08f * (needPos - transform.position);
        }
        if (rotation - transform.rotation.z < 0.2f)
        {
            transform.rotation.eulerAngles.Set(transform.rotation.x, transform.rotation.y, rotation);
        }
        else
        {
            transform.rotation.eulerAngles.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z + 0.7f * (rotation - transform.rotation.z));
        }
    }
}
