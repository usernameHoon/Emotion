using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVsGiant : MonoBehaviour
{
    bool isHide;
    public bool isStart;

    [SerializeField]
    Image noiseImg;
    float noiseCurTime;
    float noiseMaxTime;

    public bool IsStart { get { return isStart; } }

    public bool IsHide { get { return isHide; } }

    PlayerRenewal player;
    public void Init() {
        isStart = true;
        isHide = true;
    }
    void Awake() {
        player = GetComponent<PlayerRenewal>();
        isHide = false;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EventStart")) {
            isHide = true;
            isStart = true;
        }

        if (!isStart)
            return;
        if(collision.CompareTag("Pillar"))
            isHide = true;
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (!isStart)
            return;
        if(collision.CompareTag("Pillar"))
            isHide = false;
    }
    public void GiantApproaching() {
        noiseCurTime += Time.deltaTime;
        if(noiseCurTime > noiseMaxTime) {
            noiseImg.gameObject.SetActive(true);
            noiseCurTime = 0;
            noiseMaxTime = Random.Range(1, 2.5f);
        }
        player.giantDebuffSpeed = -3f;
    }
    public void NotGiantApproaching() {
        player.giantDebuffSpeed = 0;
    }
    public void PlayerDie() {
        isHide = true;
        StartCoroutine(player.Die());
    }
}
