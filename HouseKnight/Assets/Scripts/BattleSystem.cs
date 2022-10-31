using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE, RAN };

public class BattleSystem : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerSpawn;
    public Transform enemySpawn;

    Unit playerUnit;
    Unit enemyUnit;
    GameObject enemyGO;
    GameObject playerGO;

    public Text dialogueText;
    public GameObject[] buttons;

    public BattleState state;
    
    //START OF BATTLE LOGIC
    void Update()
    {
        Debug.Log(SystemManager.curHP);
    }
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<Unit>();
        enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<Unit>();
        
        yield return new WaitForSeconds(0.2f);

        playerGO.GetComponent<SetHUD>().Setup(playerUnit);
        enemyGO.GetComponent<SetHUD>().Setup(enemyUnit);

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    //PLAYER TURN
    void PlayerTurn()
    {
        for(int i = 0; i < buttons.Length; i++)
            buttons[i].SetActive(true);
        dialogueText.text = "Choose an action";
    }

    IEnumerator AttackEnemy()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.atkDamage);
        enemyGO.GetComponent<SetHUD>().SetHP(enemyUnit.curHP);
        dialogueText.text = "You hit " + enemyUnit.unitName + " for " + playerUnit.atkDamage + " damage.";

        yield return new WaitForSeconds(3f);

        if(isDead)
        {
            state = BattleState.WIN;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    //ENEMY TURN (Currently only attacks)
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "attacks for " + enemyUnit.atkDamage + " damage.";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.atkDamage);
        playerGO.GetComponent<SetHUD>().SetHP(playerUnit.curHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOSE;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    //END OF BATTLE
    IEnumerator EndBattle()
    {
        if(state == BattleState.WIN)
            dialogueText.text = "You won the battle";
        else if (state == BattleState.LOSE)
            dialogueText.text = "You lost to " + enemyUnit.unitName + ".";
        else if (state == BattleState.RAN)
            dialogueText.text = "You got away successfully";

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Test");
    }

    //UI BUTTONS
    public void AttackButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(AttackEnemy());
        for(int i = 0; i < buttons.Length; i++)
            buttons[i].SetActive(false);
    }

    public void RunButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        state = BattleState.RAN;
        StartCoroutine(EndBattle());
    }

}
