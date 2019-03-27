using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    GameObject listOfValues;
    List<string> listOfNames = new List<string>();
    List<GameObject> list = new List<GameObject>();
    public void Start()
    {
        listOfValues = GameObject.FindGameObjectWithTag("ListOfValues");
        Debug.Log(listOfValues);
        for (int i = 0; i < listOfValues.transform.childCount - 1; i++)
        {
            Debug.Log(listOfValues.transform.GetChild(i).gameObject);
            list.Add(listOfValues.transform.GetChild(i).gameObject);
            listOfNames.Add(listOfValues.transform.GetChild(i).gameObject.GetComponentInChildren<Text>().text);
        }
    }
    public void Update()
    {
        for(int i = 0; i < list.Count; i++)
        {
            float c = list[i].GetComponentInChildren<Slider>().value;
            list[i].GetComponentInChildren<Text>().text = listOfNames[i] + " " + c.ToString();
        }
    }
}
