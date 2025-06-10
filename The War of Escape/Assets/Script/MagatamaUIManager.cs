using System.Collections.Generic;
using UnityEngine;

public class MagatamaUIManager : MonoBehaviour
{
    public GameObject magatamaIconPrefab;
    public Transform container;

    private List<GameObject> icons = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMagatamaCount(int count)
    {
        while (icons.Count < count)
        {
            GameObject icon = Instantiate(magatamaIconPrefab, container);
            icons.Add(icon);
        }

        while (icons.Count > count)
        {
            GameObject icon = icons[icons.Count - 1];
            icons.RemoveAt(icons.Count - 1);
            Destroy(icon);
        }
    }


}
