using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    FairyAbility fairyAbility;

    public static GameManager manager;

    //������ �÷��̾� �ɷ� ���� bool��
    public bool playerAbilityOn;
    //y������ ���� ������?
    public bool thisHighMap;
    public int abilityCnt;

    //�ɷ� ������ ������ �ֳ�?
    //�� ���� �����ͼ� ���� ���� ���� ���� �⺻�� true�� �ٲ�� �Ѵ�.
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
