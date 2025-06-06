using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon00 : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent コンポーネント

    public GameObject player1;
    public GameObject player2;
    public GameObject prisonGateObject00;

    private PrisonGate00 prisonGate00;
    private TriggerCamera triggerCamera;
    private Player player;

    private Transform owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        prisonGate00 = prisonGateObject00.GetComponent<PrisonGate00>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch() 
    {
        if (prisonGate00.playerID00)
        {
            triggerCamera = player1.GetComponent<TriggerCamera>();
            triggerCamera.demonHave = true;

            player = player1.GetComponent<Player>();
            player.pd = prisonGate00.protectingDemonObj;

            player1.transform.position = owner.transform.position;
        }
        else if (!prisonGate00.playerID00) 
        {
            triggerCamera = player2.GetComponent<TriggerCamera>();
            triggerCamera.demonHave= true;

            player = player2.GetComponent<Player>();
            player.pd = prisonGate00.protectingDemonObj;

            player2.transform.position = owner.transform.position;
        }

        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
        agent.SetDestination(targetPosition);
    }

    public void Stun()
    {

    }

    //松山ここから下追加しました
    //public void SetOwner(Player newOwner)
    //{
    //    owner = newOwner;
    //    Debug.Log($"鬼がプレイヤー{owner.playerID}のものになった！");
    //    // ここでAIや追従先などを切り替える処理を書く！
    //}
}

