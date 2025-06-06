using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent コンポーネント

    public GameObject player1;
    public GameObject player2;

    private PrisonGate prisonGate;
    private TriggerCamera triggerCamera;
    private Player player;

    private Transform owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch() 
    {
        if (prisonGate.playerID)
        {
            triggerCamera = player1.GetComponent<TriggerCamera>();
            triggerCamera.demonHave = true;

            player = player1.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;

            player1.transform.position = owner.transform.position;
        }
        else if (!prisonGate.playerID) 
        {
            triggerCamera = player2.GetComponent<TriggerCamera>();
            triggerCamera.demonHave= true;

            player = player2.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;

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

