using System.Collections.Generic;
using UnityEngine;

public class MagatamaUIManager : MonoBehaviour
{
    //public GameObject magatamaIconPrefab;
    //public Transform container;

    //private List<GameObject> icons = new List<GameObject>();

    [Header("表示する勾玉UI（順番通りに並べる）")]
    public List<GameObject> magatamaIcons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void UpdateMagatamaUI(int count)
    {
        // 現在の表示を全て削除
        foreach (Transform child in magatamaUIParent)
        {
            Destroy(child.gameObject);
        }

        // 新しく count 分アイコンを生成
        for (int i = 0; i < count; i++)
        {
            Instantiate(magatamaIconPrefab, magatamaUIParent);
        }
    }*/



    public void UpdateMagatamaUI(int count)
    {
        for (int i = 0; i < magatamaIcons.Count; i++)
        {
            magatamaIcons[i].SetActive(i < count);
        }
    }

    /*public void ResetMagatamaUI()
    {
        UpdateMagatamaUI(0);
    }*/

    public void ResetMagatamaUI()
    {
        for (int i = 0; i < magatamaIcons.Count; i++)
        {
            magatamaIcons[i].SetActive(false);
        }
    }



}
