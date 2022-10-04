using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Giant : MonoBehaviour
{
    enum GiantState { Stop, Move, Search}
    [SerializeField]
    GiantState state;
    /* 
     * 1순위. 공간Space 변할 시 그곳으로 이동 
     * (공간으로 할지? 플레이어 시야 내에서 할지?)
     * 2순위. 왼쪽2~3s 이동 - 0.5s후 돌아보기 - 2초 유지 - 오른쪽으로 2~3s 이동 - 0.5s후 돌아보기
     */

    int moveSpace;

    [SerializeField]
    float maxSpeed;
    float curSpeed;

    float patternTime;

    // 움직임
    float[] maxPosX = new float[2];
    float walkTime;
    bool isLeft = true;
    Transform eyeLight;
    int moveCount;
    // 이동 중 플레이어 발견
    bool isDiscovery;
    float lastDiscoveryPos;

    // 벽 뒤 확인
    float searchingTime;
    int StopCnt;

    [SerializeField]
    Text tempTxt;

    // 컴포넌트
    Rigidbody2D rigid;
    SpriteRenderer renderer;
    Animator anim;
    AudioSource audio;

    // 플레이어 관련 변수
    PlayerVsGiant player;
    Transform playerTr;

    void Awake() {
        state = GiantState.Move;
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        maxPosX[0] = 568;
        maxPosX[1] = 660;
        patternTime = 0;
        walkTime = 4;
        moveCount = 0;
        searchingTime = 2f;

        eyeLight = transform.GetChild(0);

        player = GameObject.Find("player").GetComponent<PlayerVsGiant>();
        playerTr = GameObject.Find("player").transform;
    }
    void Update() {
        if (!player.IsStart)
            return;
        renderer.flipX = !isLeft;
        patternTime += Time.deltaTime;

        StateUpdate();
        PosXLimit();
        
        eyeLight.localEulerAngles = isLeft ? Vector3.forward * 90 : Vector3.forward * -90;

        // 가속
        if (1 > curSpeed && state == GiantState.Move) {
            curSpeed += Time.deltaTime * 3;
        }
        if(audio.time > 3.2f) {
            audio.Stop();
        }
    }
    void FixedUpdate() {
        if (!player.IsStart)
            return;
        FixedStateUpdate();
    }
    void PosXLimit() {
        if (transform.position.x <= maxPosX[0]) {
            isLeft = false;
        }
        else if (transform.position.x >= maxPosX[1]) {
            isLeft = true;
        }
    }
    void StateUpdate() {
        switch (state) {
            case GiantState.Stop:
                anim.SetBool("isMove", false);
                if (1 < patternTime) {
                    if (transform.position.x > playerTr.position.x)
                        isLeft = true;
                    else
                        isLeft = false;
                    patternTime = 0;
                    StopCnt++;
                    //int rand = Random.Range(1, 4);
                    state = GiantState.Move;
                }
                break;
            case GiantState.Move:
                anim.SetBool("isMove", true);
                Move();
                if (isDiscovery) {
                    // 발견했다면? 마지막 발견 위치까지 걸어가지
                    float dis = lastDiscoveryPos - transform.position.x > 0 ? lastDiscoveryPos - transform.position.x : (lastDiscoveryPos - transform.position.x) * -1;
                    Debug.Log("거리 ? " + dis);
                    if (dis < 15) {
                        // 플레이어가 기둥 뒤에 없다면 공격
                        if (!player.IsHide) {
                            anim.SetTrigger("AtkTrigger");
                            patternTime = -4;
                            state = GiantState.Stop;
                            // 0.9166666666초 후에 플레이어 사망 처리
                            StartCoroutine(PlayerDie());
                            isDiscovery = false;
                        }
                        // 플레이어가 기둥 뒤에 있어서 숨었다면 기둥 뒤 확인
                        else {
                            // 기둥 뒤 확인
                            isDiscovery = false;
                        }
                    }
                }
                //
                if (isLeft) {
                    if (transform.position.x > playerTr.position.x && !player.IsHide) {
                        Debug.Log("플레이어 걸림");
                        player.GiantApproaching();
                        isDiscovery = true;
                        lastDiscoveryPos = player.transform.position.x;
                    }
                    else {
                        player.NotGiantApproaching();
                    }
                }
                else {
                    if (transform.position.x < playerTr.position.x && !player.IsHide) {
                        Debug.Log("플레이어 걸림");
                        player.GiantApproaching();
                        isDiscovery = true;
                        lastDiscoveryPos = player.transform.position.x;
                    }
                    else {
                        player.NotGiantApproaching();
                    }
                }
                // State.Move 끄는거 MoveLerp()에 있음
                break;
            case GiantState.Search:
                if (searchingTime < patternTime) {
                    patternTime = 0;
                    StopCnt = 0;
                    state = GiantState.Move;
                }
                break;
        }
    }
    IEnumerator PlayerDie() {
        // @@ 테스트용 빼기
        yield return new WaitForSeconds(0.9166f);
        //player.PlayerDie();
        tempTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        // 플레이어가 죽으면, 거인의 위치는 초기 위치로 초기화
        transform.position = new Vector2(594, -9);
    }
    void FixedStateUpdate() {
        switch (state) {
            case GiantState.Stop:
                Debug.Log("정지");
                break;
            case GiantState.Move:
                break;
            case GiantState.Search:
                Debug.Log("플레이어 서칭");
                break;
        }
    }
    void Move() {
        if (isLeft) {
            Movement();
            //rigid.velocity = Vector3.left * curSpeed * maxSpeed;
        }
        else {
            Movement();
            //rigid.velocity = Vector3.right * curSpeed * maxSpeed;
        }
    }
    void Movement() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.25f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.49f)) {
            if (moveCount == 0) {
                moveCount = 1;
                StartCoroutine(MoveLerp(true));
                //transform.position = transform.position + (Vector3.left * 1);
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.74f)) {
            if (moveCount == 1) {
                moveCount = 0;
                StartCoroutine(MoveLerp(false));
                //transform.position = transform.position + (Vector3.left * 1);
            }
        }
    }
    IEnumerator MoveLerp(bool up) {
        Vector2 originPos = transform.position;
        float progress = 0;
        bool shake = false;
        while(progress < 1) {
            progress += 0.2f;
            if (isLeft) {
                if (up)
                    transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 1) /*+ (Vector2.up * 0.5f)*/, progress);
                else {
                    transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 1)/* + (Vector2.down * 0.5f)*/, progress);
                    if (progress >= 0.7f && !shake) {
                        shake = true;
                        CameraShake.instance.ShakeCoroutine();
                        audio.Play();
                        audio.time = 2.5f;
                    }
                }
            }
            else {
                if (up)
                    transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 1)/* + (Vector2.up * 0.5f)*/, progress);
                else {
                    transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 1)/* + (Vector2.down * 0.5f)*/, progress);
                    if (progress >= 0.7f && !shake) {
                        shake = true;
                        CameraShake.instance.ShakeCoroutine();
                        audio.Play();
                        audio.time = 2.5f;
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        if (!up) {
            if (walkTime < patternTime && !isDiscovery) {
                patternTime = 0;
                rigid.velocity = Vector2.zero;
                state = GiantState.Stop;
                curSpeed = 0;
            }
        }
    }
}
