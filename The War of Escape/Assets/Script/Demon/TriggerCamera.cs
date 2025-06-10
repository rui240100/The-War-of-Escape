using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TriggerCamera : MonoBehaviour
{
    public GameObject targetObject;
    private Player target;
    private Player player;
    private TriggerCamera targetCamera;
    public bool demonHave;

    private ProtectingDemon protectingDemon; 
    private DemonAI demon;

    public float stun = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        demonHave = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (demonHave)  //���������S�������Ă�����
        {
            if (other.gameObject == targetObject)   //���肪�v���C���[���m���߂�
            {
                target = other.GetComponent<Player>();
                targetCamera = other.GetComponentInChildren<TriggerCamera>();

                if (!targetCamera.demonHave)    //���肪���S�������Ă��Ȃ�������
                {
                    StartCoroutine(StunCoroutine(target));
                }
                else
                {
                    Destroy(target.pd);
                }

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
            }
            else if (other.CompareTag("Demon"))
            {
                demon = other.GetComponent<DemonAI>();

                StartCoroutine(ProtectCoroutine(demon));

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
            }
        }
    }

    private IEnumerator StunCoroutine(Player target)
    {
        if(target != this.transform.parent.GetComponent<Player>())
        {
            float playerSpeed = target.Speed;
            target.Speed = stun;

            yield return new WaitForSeconds(5.0f);

            target.Speed = playerSpeed;
        }
        else
        {
            Debug.Log("������GetComponent���Ă�");
        }
    }

    private IEnumerator ProtectCoroutine(DemonAI demon)
    {
        float demonSpeed = demon.chaseSpeed;

        demon.chaseSpeed = 0.0f;
        demon.agent.speed = demon.chaseSpeed;

        Debug.Log("SlowDemon");

        yield return new WaitForSeconds(5.0f);

        demon.chaseSpeed = 6.0f;
        demon.agent.speed = demon.chaseSpeed;

        Debug.Log("�S�ĊJ");

        demon.StopChase();
    }
}
