using UnityEngine;

public class MostKeyPlayer : MonoBehaviour
{
    public DemonAI demon;
    public Player[] players;

    private Player holderPlayer = null; // このアイテムを取ったプレイヤー

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
            Debug.Log("鍵持ちがいたぞ！"); // ←★ 追加
        }

        // 一度使ったらアイテム効果消滅
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
        if (player != null && holderPlayer == null) // 誰も持っていない時に限り取得可能
        {
            holderPlayer = player;
            Debug.Log($"Player {player.playerID} picked up MostKeyPlayerItem.");
            gameObject.SetActive(false); // アイテムを非表示に（または Destroy(gameObject);）
        }
    }
}
