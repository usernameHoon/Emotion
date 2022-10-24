using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
    public enum MaskType { Sad, Horror, Angry, Happy }
    [SerializeField]
    Image maskSprite;

    public MaskType mask;

    bool doMaskEvent;

    PlayerRenewal player;

    void Update() {
        if (doMaskEvent) {
            if (Input.anyKeyDown) {
                doMaskEvent = false;
                player.dontInput = false;
                gameObject.SetActive(false);
                if (maskSprite != null)
                    maskSprite.gameObject.SetActive(false);
                else
                    return;
            }
        }
    }
    public void MaskEvent(PlayerRenewal player) {
        this.player = player;
        if (mask == MaskType.Sad) {
            GameManager.manager.haveSadMask = true;
        }
        if (mask == MaskType.Horror)
        {
            GameManager.manager.haveHorrorMask = true;
        }
        if (mask == MaskType.Angry)
        {
            GameManager.manager.haveAngryMask = true;
        }
        if (mask == MaskType.Happy)
        {
            GameManager.manager.haveHappyMask = true;
            Debug.Log("Get Mask!!");
        }
        doMaskEvent = true;
        // player.dontInput = true;
        if (maskSprite != null)
            maskSprite.gameObject.SetActive(true);
        else
            return;
    }
}
