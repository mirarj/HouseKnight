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
    public Cinemachine.CinemachineTargetGroup lookGroup;
    public Cinemachine.CinemachineTargetGroup followGroup;

    public Text dialogueText;
    public Slider swingSlide;
    int enemyIndex;
    bool attacking = false;
    bool choosing = false;
    bool turn = false;
    int choice = 0;
    int prevChoice = 0;
    public GameObject[] buttons;
    public float[] buttonPos;
    public GameObject bars;

    public BattleState state;
    
    //START OF BATTLE LOGIC
    void Update()
    {
        if(!attacking && !choosing && turn)
        {
            if(Input.GetKeyDown(KeyCode.E))
                StartCoroutine(AttackButton());
            else if(Input.GetKeyDown(KeyCode.R))
                RunButton();
        }

        //PLAYER ATTACK
        if(attacking)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                int dmg;
                swingSlide.gameObject.GetComponent<SliderBar>().StopSlide();
                dmg = (int)(playerUnit.atkDamagehigh*(1.0f - Mathf.Abs(swingSlide.value)));
                swingSlide.gameObject.SetActive(false);
                attacking = false;
                LeanTween.scale(bars, new Vector3(1f, 1.67f, 1f), 0.5f).setEase(LeanTweenType.easeInOutCubic);
                StartCoroutine(AttackEnemy(dmg, choice));
            }
        }

        //PLAYER SELECT
        if(choosing)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                choosing = false;
                EnemySelect(choice, true);
                choice = 0; prevChoice = 0;
                PlayerTurn();
            }

            //Left/Right logic
            if(Input.GetButtonDown("Right"))
                ChangeChoice(true);
            else if(Input.GetButtonDown("Left"))
                ChangeChoice(false);


            EnemySelect(choice, false);
            EnemySelect(prevChoice, true);

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

    void EnemySelect(int index, bool isBig)
    {
        if(!isBig)
        {
            enemyUnit[index].transform.GetChild(0).GetComponent<SpriteRenderer>().material = enemyMats[1];
            enemyUnit[index].transform.GetChild(1).localScale = new Vector3(0.53f,0.52f,1f);
            enemyUnit[index].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            enemyUnit[index].transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            enemyUnit[index].transform.GetChild(0).GetComponent<SpriteRenderer>().material = enemyMats[0];
            enemyUnit[index].transform.GetChild(1).localScale = new Vector3(0.34f,0.27f,1f);
            enemyUnit[index].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            enemyUnit[index].transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
    }


    void ChangeChoice(bool increase)
    {
        if(increase)
        {
            prevChoice = choice;
            choice++;
            if(choice > numEnemies - 1)
                choice = 0;
        }
        else
        {
            prevChoice = choice;
            choice--;
            if(choice < 0)
                choice = numEnemies - 1;
        }
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        playerUnit = playerGO.GetComponent<Unit>();

        numEnemies = Random.Range(1,4);

        for(int i = 0; i < numEnemies; i++)
        {
            enemyGO[i] = Instantiate(enemyPrefab, enemySpawn[i]);
            enemyUnit[i] = enemyGO[i].GetComponent<Unit>();

            lookGroup.m_Targets[i + 1].target = enemySpawn[i];
            followGroup.m_Targets[i + 1].target = enemySpawn[i];

            yield return new WaitForSeconds(0.03f);
            enemyGO[i].GetComponent<SetHUD>().Setup(enemyUnit[i], false);
            prevChoice = i;
        }

        playerGO.GetComponent<SetHUD>().Setup(playerUnit, true);

        if (numEnemies > 1)
            dialogueText.text = "A group of enemies approaches...";
        else
            dialogueText.text = "A wild " + enemyUnit[0].unitName + " approaches...";

        yield return new WaitForSeconds(1.5f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }



    //PLAYER TURN
    void PlayerTurn()
    {
        turn = true;
        LeanTween.scale(bars, new Vector3(1f, 1.13f, 1f), 0.7f).setEase(LeanTweenType.easeOutExpo);
        for(int i = 0; i < buttons.Length; i++)
        {
            LeanTween.move(buttons[i], new Vector2(buttons[i].transform.position.x, buttonPos[i]), 1.1f).setEase(LeanTweenType.easeOutExpo).setDelay(i/10f);
        }
        dialogueText.text = "Choose an action";
    }

    IEnumerator AttackEnemy(int dmg, int index)
    {
        int randomcrit = Random.Range(0, playerUnit.critChance + 1);
        if(randomcrit == playerUnit.critChance)
        {
            dmg = (int)(dmg * 1.5);
            dialogueText.text = "CRITICAL HIT!!";
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(AttackAnimation(index));
        bool isDead = enemyUnit[index].TakeDamage(dmg);
        enemyGO[index].GetComponent<SetHUD>().SetHP(enemyUnit[index].curHP, 2.5f, dmg);
        dialogueText.text = "You hit " + enemyUnit[index].unitName + " for " + dmg + " damage.";

        yield return new WaitForSeconds(0.9f);
        CineMachineShake.Instance.ShakeCamera(dmg/10f, 0.3f);
        yield return new WaitForSeconds(0.6f);
        EnemySelect(choice, true);

        if(isDead)
        {
            enemyUnit[index].Die();
            yield return new WaitForSeconds(1.5f);
            Destroy(enemyUnit[index].gameObject);
            numEnemies--;
            lookGroup.m_Targets[index + 1].target = null;
            followGroup.m_Targets[index + 1].target = null;

            //GAIN EXP
            playerUnit.GainEXP(enemyUnit[index].dropEXP);
            dialogueText.text = "You Gained " + enemyUnit[index].dropEXP + "EXP.";
            


            for(int j = index; j < numEnemies; j++)
            {
                enemySpawn[j] = enemySpawn[j + 1];
                enemyUnit[j] = enemyUnit[j + 1];
                enemyGO[j] = enemyGO[j + 1];
                enemySpawn[j + 1] = null;
                enemyUnit[j + 1] = null;
                enemyGO[j + 1] = null;
            }
            ChangeChoice(true);

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

    IEnumerator AttackAnimation(int index)
    {
        GameObject player = playerUnit.transform.GetChild(0).gameObject;
        Vector3 initialPos = player.transform.position;
        LeanTween.move(player, new Vector3(enemySpawn[index].position.x, 10.1f, enemySpawn[index].position.z), 1f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.move(player, initialPos, 1f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.5f);

        LeanTween.move(playerSpawn.gameObject, new Vector3(enemySpawn[index].position.x, 10.1f, enemySpawn[index].position.z), 1f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.move(playerSpawn.gameObject, initialPos, 1f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.5f);
        yield return new WaitForSeconds(2f);
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

        yield return new WaitForSeconds(1.25f);

        bool isDead = playerUnit.TakeDamage(damage);
        CineMachineShake.Instance.ShakeCamera(1.5f, 0.3f);
        playerGO.GetComponent<SetHUD>().SetHP(playerUnit.curHP, 0.1f, damage);

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
    public IEnumerator AttackButton()
    {
        yield return new WaitForSeconds(0.05f);
        turn = false;
        if(state != BattleState.PLAYERTURN) {}
        else
        {
            if(numEnemies > 1)
                ChooseEnemy();
            else
            {
                EnemySelect(0, false);
                Swing(0);
            }
            for(int i = buttons.Length - 1; i >= 0; i--)
                LeanTween.move(buttons[i], new Vector2(buttons[i].transform.position.x, -150f), 1f).setEase(LeanTweenType.easeOutExpo).setDelay(i/20f);
        }
    }

    public void RunButton()
    {
        turn = false;
        for(int i = buttons.Length - 1; i >= 0; i--)
            LeanTween.move(buttons[i], new Vector2(buttons[i].transform.position.x, -150f), 1f).setEase(LeanTweenType.easeOutExpo).setDelay(i/20f);
        if(state != BattleState.PLAYERTURN)
            return;
        state = BattleState.RAN;
        StartCoroutine(EndBattle());
    }

}