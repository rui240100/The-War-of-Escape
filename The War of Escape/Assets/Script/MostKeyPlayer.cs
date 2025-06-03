using UnityEngine;

public class MostKeyPlayer : MonoBehaviour
{
    public DemonAI demon;
    public Player[] players;

    private Player holderPlayer = null; // ���̃A�C�e����������v���C���[

    // Update is called once per frame
    void Update()
    {
        if (holderPlayer != null)
        {
            if (holderPlayer.playerID == 1 && Input.GetButtonDown("Fire3"))
            {
                TriggerDemonChase();
            }
            else if (holderPlayer.playerID == 2 && Input.GetButtonDown("Fire3_2"))
            {
                TriggerDemonChase();
            }
        }
    }

    void TriggerDemonChase()
    {
        Player target = GetPlayerWithMostKeys();
        if (target != null)
        {
            demon.StartChase(target.transform);
            Debug.Log($"Demon is now chasing Player {target.playerID} with {target.keyCount} keys!");
            Debug.Log("���������������I"); // ���� �ǉ�
        }

        // ��x�g������A�C�e�����ʏ���
        holderPlayer = null;
    }

    Player GetPlayerWithMostKeys()
    {
        if (players == null || players.Length == 0) return null;

        Player maxPlayer = players[0];
        foreach (Player p in players)
        {
            if (p != null && p.keyCount > maxPlayer.keyCount)
            {
                maxPlayer = p;
            }
        }
        return maxPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && holderPlayer == null) // �N�������Ă��Ȃ����Ɍ���擾�\
        {
            holderPlayer = player;
            Debug.Log($"Player {player.playerID} picked up MostKeyPlayerItem.");
            gameObject.SetActive(false); // �A�C�e�����\���Ɂi�܂��� Destroy(gameObject);�j
        }
    }
}
