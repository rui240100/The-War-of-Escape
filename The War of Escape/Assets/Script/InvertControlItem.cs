using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControlItem : Item
{
    public float duration = 10f; // ”½“]‚ª‘±‚­ŽžŠÔ

    public override void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(InvertControl(user.otherPlayer));
        }
    }

    private IEnumerator InvertControl(Player targetPlayer)
    {
        InvertedInput inverted = targetPlayer.gameObject.GetComponent<InvertedInput>();
        if (inverted == null)
        {
            inverted = targetPlayer.gameObject.AddComponent<InvertedInput>();
        }

        inverted.EnableInversion();

        yield return new WaitForSeconds(duration);

        inverted.DisableInversion();
    }
}
