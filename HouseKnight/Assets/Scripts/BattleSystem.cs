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
    public Transform[] enemySpawn;
    public int numEnemies;

    Unit playerUnit;
    public Unit[] enemyUnit;
    public GameObject[] enemyGO;
    GameObject playerGO;
    public Material[] enemyMats;

    public Text dialogueText;
    public Slider swingSlide;
    int enemyIndex;
    bool attacking;
    bool choosing;
    int choice = 0;
    public GameObject[] buttons;

    public BattleState state;
    
    //START OF BATTLE LOGIC
    void Update()
    {
        //PLAYER ATTACK
        if(Input.GetKeyDown(KeyCode.E) && attacking)
        {
            int dmg;
            swingSlide.gameObject.GetComponent<SliderBar>().StopSlide();
            dmg = (int)(playerUnit.atkDamagehigh*(1.0f - Mathf.Abs(swingSlide.value)));
            swingSlide.gameObject.SetActive(false);
            attacking = false;
            StartCoroutine(AttackEnemy(dmg, choice));
        }

        //PLAYER SELECT
        if(choosing)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                choosing = false;
                PlayerTurn();
            }

            //Left/Right logic
            if(Input.GetButtonDown("Right"))
                ChangeChoice(true);
            else if(Input.GetButtonDown("Left"))
                ChangeChoice(false);


            enemyUnit[choice].transform.GetChild(0).GetComponent<SpriteRenderer>().material = enemyMats[1];
            for(int i = 0; i < numEnemies; i++)
            {
                if(i != choice && enemyUnit[choice] != null)
                    enemyUnit[i].transform.GetChild(0).GetComponent<SpriteRenderer>().material = enemyMats[0];
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                Swing(choice);
                choosing = false;
            }
        }
    }
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void ChangeChoice(bool increase)
    {
        if(increase)
        {
            choice++;
            if(choice > numEnemies - 1)
                choice = 0;
        }
        else
        {
            choice--;
            if(choice < 0)
                choice = numEnemies - 1;
        }
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<Unit>();

        numEnemies = Random.Range(1,4);

        for(int i = 0; i < numEnemies; i++)
        {
            enemyGO[i] = Instantiate(enemyPrefab, enemySpawn[i]);
            enemyUnit[i] = enemyGO[i].GetComponent<Unit>();
            yield return new WaitForSeconds(0.03f);
            enemyGO[i].GetComponent<SetHUD>().Setup(enemyUnit[i]);
        }

        playerGO.GetComponent<SetHUD>().Setup(playerUnit);

        if (numEnemies > 1)
            dialogueText.text = "A group of enemies approaches...";
        else
            dialogueText.text = "A wild " + enemyUnit[0].unitName + " approaches...";

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

    IEnumerator AttackEnemy(int dmg, int index)
    {
        bool isDead = enemyUnit[index].TakeDamage(dmg);
        enemyGO[index].GetComponent<SetHUD>().SetHP(enemyUnit[index].curHP);
        dialogueText.text = "You hit " + enemyUnit[index].unitName + " for " + dmg + " damage.";

        yield return new WaitForSeconds(3f);


        if(isDead)
        {
            Destroy(enemyUnit[index].gameObject);
            numEnemies--;

            for(int j = 0; j < numEnemies - index; j++)
            {
                enemyUnit[index] = enemyUnit[index + 1];
                enemyGO[index] = enemyGO[index + 1];
                enemyUnit[index + 1] = null;
                enemyGO[index + 1] = null;
            }

            if(numEnemies <= 0)
            {
                state = BattleState.WIN;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn(0));
            }
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn(0));
        }
    }

    void Swing(int swingChoice)
    {
        swingSlide.gameObject.SetActive(true);
        swingSlide.gameObject.GetComponent<SliderBar>().StartSlider(playerUnit.swingSpeed);
        attacking = true;
        choice = swingChoice;
    }

    void ChooseEnemy()
    {
        choosing = true;
        dialogueText.text = "Choose which enemy to attack";
    }


    //ENEMY TURN (Currently only attacks)
    IEnumerator EnemyTurn(int cur)
    {
        int damage = Random.Range(enemyUnit[cur].atkDamagelow, enemyUnit[cur].atkDamagehigh);
        dialogueText.text = enemyUnit[cur].unitName + "attacks for " + damage + " damage.";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(damage);
        playerGO.GetComponent<SetHUD>().SetHP(playerUnit.curHP);

        yield return new WaitForSeconds(0.2f);

        if(isDead)
        {
            state = BattleState.LOSE;
            StartCoroutine(EndBattle());
        }
        else
        {
            if(cur == numEnemies - 1)
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
            else
                StartCoroutine(EnemyTurn(cur + 1));
        }
    }

    //END OF BATTLE
    IEnumerator EndBattle()
    {
        if(state == BattleState.WIN)
            dialogueText.text = "You won the battle";
        else if (state == BattleState.LOSE)
            dialogueText.text = "You lost.";
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
        
        if(numEnemies > 1)
            ChooseEnemy();
        else
            Swing(0);
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
