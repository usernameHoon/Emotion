using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    FairyAbility fairyAbility;

    public static GameManager manager;

    //요정과 플레이어 능력 스왑 bool값
    public bool playerAbilityOn;
    //y축으로 맵이 넓은가?
    public bool thisHighMap;
    public int abilityCnt;

    //능력 가면을 가지고 있나?
    //씬 정보 가져와서 가면 얻은 이후 맵은 기본값 true로 바꿔야 한다.
    public bool haveSadMask;
    public bool haveHorrorMask;

    void Awake() {
        playerAbilityOn = true;
        if (SceneManager.GetActiveScene().buildIndex == 2) {
            thisHighMap = true;
        }
        manager = this;
    }
    void Update() {
        Swap();
    }
    void Swap() {
        if (fairyAbility == null)
            return;
        if (fairyAbility.Sading)
            return;
        if (Input.GetButtonDown("Swap"))
            playerAbilityOn = !playerAbilityOn; 
    }
}
