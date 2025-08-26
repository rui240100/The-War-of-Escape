using UnityEngine;
using DG.Tweening;	//DOTween‚ðŽg‚¤‚Æ‚«‚Í‚±‚Ìusing‚ð“ü‚ê‚é

public class TextMove : MonoBehaviour
{
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.DOLocalMove(new Vector3(-128f, 0f, 0f), 1f).SetDelay(1.0f);
        this.transform.DOLocalMove(new Vector3(-1434f, 0f, 0f), 2f).SetDelay(2.5f);



    }

    // Update is called once per frame
    void Update()
    {


    }




}

