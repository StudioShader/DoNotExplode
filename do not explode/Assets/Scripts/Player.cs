using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float velocity = 1;
    public float a = 1, bigA = 5, bigVelocity = 300;
    public int k = 100;
    public float energy = 0, energyIncrement = 1, radiusOfExplosion, energyTime;
    private bool preButton;
    public GameObject radius;
    public void Start()
    {
        radius = transform.GetChild(0).gameObject;
    }
    public void Update()
    {
        if (energy != 0)
            radius.transform.localScale = new Vector3(1, 1, 1) * energy * k / 16;
        else
            radius.transform.localScale = new Vector3(1, 1, 1);
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        //Debug.Log(NCellController.pathLengths[0] + "   " + NCellController.pathLengths[1]);
        if (velocity * Time.deltaTime < (NCellController.pathLengths[1] - myPos).magnitude)
        {
            transform.position += new Vector3((NCellController.pathLengths[1] - myPos).normalized.x, (NCellController.pathLengths[1] - myPos).normalized.y,0) * velocity * Time.deltaTime;
        }
        else
        {
            float deltaLength = velocity * Time.deltaTime - (myPos - NCellController.pathLengths[1]).magnitude;
            NCellController.pathLengths.RemoveAt(0);
            Vector2 pos = NCellController.pathLengths[0] + (deltaLength * (NCellController.pathLengths[1] - NCellController.pathLengths[0]).normalized);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
        Debug.Log(velocity);
        if (velocity >= a * Time.deltaTime) {
            velocity -= a * Time.deltaTime;
        }
        if (velocity >= bigVelocity)
        {
            velocity -= bigA * Time.deltaTime;
        }
        Debug.Log(a * Time.deltaTime);
        if (velocity <= 0)
        {
            die();
        }
        if (Input.GetKey("c"))
        {
            preButton = true;
            energy += energyIncrement;
        }
        else
        {
            if (preButton)
            {
                Blow();
                preButton = false;
            }
            else
            {
                preButton = false;
            }
        }
    }
    public void Blow()
    {
        int c = 30;
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        CalculateRadius();
        if ((NCellController.pathLengths[1] - myPos).magnitude < radiusOfExplosion || (NCellController.pathLengths[0] - myPos).magnitude < radiusOfExplosion)
        {
            velocity += energy * c;
            Debug.Log(velocity + "  " + energy + "  " + c + " vvvvvvvvvvvvvvvvvvvvvvvvvvv");
        }
        else
        {
            die();
        }
        energy = 0;
    }
    public void CalculateRadius()
    {
        radiusOfExplosion = energy * k;
    }
    public void die()
    {
        Debug.Log("ur ded");
    }
    
}
