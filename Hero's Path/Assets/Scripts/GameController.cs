using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// To DO:
    /// ADD cultist system to the right.
    /// ADD Attck sliders that show how long until attack.
    /// ADD Sprites.
    /// ADD Dialogue.
    /// SUBMIT.
    /// </summary>

    //All Texts that change on screen
    public Text PlayerXPtext;
    public Text PlayerHPtext;
    public Text EnemyHPtext;
    public Text EnemyNametext;
    public Text UnallocatedStatPointstext;
    public Text Damagetext;
    public Text AStext;
    public Text FollowerGaintext;
    public Text STRtext;
    public Text AGItext;
    public Text CONSTtext;
    public Text CHARtext;
    public Text OutText;
    public TMP_Text Followerstext;
    public Text QuestTracker;
    public Text EnemiesNeededText;

    //Sliders
    public Slider PlayerXPSlider;
    public Slider PlayerHPSlider;
    public Slider EnemySlider;

    public Slider PlayerAtkSlider;
    public Slider EnemyAtkSlider;

    //GameObjects
    public GameObject EnemyPrefab;
    public GameObject EnemySpawnLocation;
    public GameObject TitleScreen;
    public GameObject OutGO;
    public GameObject tagOne;
    public GameObject tagTwo;
    public GameObject tagThree;
    public GameObject LevelUpDisplay;
    public GameObject QuestCompleteDisplay;
    public GameObject FailedDisplay;


    //Game Logic
    private bool Start;
    private bool PreStart;
    public int EnemyPointer;

    //Stats raw:
    public int Str;
    public int Agi;
    public int Const;
    public int Chari;

    //Stats InPlay:
    private int Damage;
    private float AttackSpeed;
    private float PoPGrowthBonus;
    private int MaxHp;
    private int Hp;

    //XP, Leveling and Followers:
    public static float XP;
    private int MaxXP;
    public static int Followers;
    public int UnallocatedStatPoints;

    //Enemy kill count and kill count required to pass:
    private int EnemiesKilled;
    private int EnemiesNeeded;
    private string CurrentQuest;

    //Enemy in play:
    private Enemy enemy;
    public int EnemyHp;
    private bool EnemyActive;
    private bool EnemySpawning;
    private bool FirstRun;

    //Time trickery:
    public float PlayerAttackIn;
    public float EnemyAttackIn;
    public float clickAmount;

    //Background Image:
    public Image Background;
    public Sprite AreaOneBack;
    public Sprite AreaTwoBack;
    public Sprite AreaThreeBack;

    public static float fightersAS;
    public static float fightersAttackIn;
    public TextMeshProUGUI fightersText;

    //Audio Controller:
    public AudioController AC;
    //PlayDialogue() from 0 to 5
    //MusicFromTo(string MusicName) Name: "Title" or "Boss"
    //PlayNoise() from 0 to 16

    //Upgrades the InPlay Stats with their raw values.
    public void SetInPlay()
    {
        Damage = Str + 1;
        AttackSpeed = 10f * (Mathf.Exp(-0.15f * Agi));
        PoPGrowthBonus = 1f + (Chari * 0.1f);
        MaxHp = 20 + (Const * 30);
        clickAmount = AttackSpeed / 10;
    }

    public void PlayGame()
    {
        //Initialising Stats:
        UnallocatedStatPoints = 0;
        Str = 0;
        Agi = 0;
        Const = 0;
        Chari = 0;

        SetInPlay();
        //First XP CAP:
        MaxXP = 25;
        XP = 0;

        FirstRun = true;
        PreStart = false;
        Start = false;
        //Changing Scene:
        TitleScreen.SetActive(false);
        SelectArea1();
        //Starting on first quest:
        StartQuest(CurrentQuest);

        AC.PlayDialogue(0);
        StartCoroutine(PlayNext());
    }

    IEnumerator PlayNext()
    {
        yield return new WaitForSeconds(AC.TutorialBoiExplainsSelf.clip.length + 1f);
        if (AC.TutorialBoiExplainsSelf.isPlaying)
        {
            
        }
        else
        {
            AC.PlayDialogue(3);
            yield return new WaitForSeconds(AC.TutorialBoiExplainsPurpose.clip.length + 1f);
            if (AC.TutorialBoiExplainsPurpose.isPlaying)
            {
                
            }
            else
            {
                AC.PlayDialogue(1);
                yield return new WaitForSeconds(AC.TutorialBoiExplainsRage.clip.length + 1f);
                if (AC.TutorialBoiExplainsRage.isPlaying)
                {
                    
                }
                else
                {
                    AC.PlayDialogue(2);
                    yield return new WaitForSeconds(AC.TutorialBoiExplainsQuests.clip.length + 1f);
                    if (AC.TutorialBoiExplainsQuests.isPlaying)
                    {

                    }
                    else
                    {
                        AC.PlayDialogue(4);
                    }
                }
            }
            
        }
    }

    public void StartQuest(string Quest)
    {
        
        //For When the Quest Ends Check if its already Started:
        if (!PreStart)
        {
            GameObject[] Mobs;
            Mobs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject Mob in Mobs)
            {
                Destroy(Mob);
            }
            EnemyActive = false;
            Debug.Log("Quest Started: "+Quest);
            //descides the mobs to spawn depending on quest name.
            //Also descides the number of mobs needed before quest ends.
            if(Quest == "Area 1")
            {
                EnemyPointer = 0;
                EnemiesNeeded = 10;
                CurrentQuest = "Area 1";
            }
            if(Quest == "Area 2")
            {
                EnemyPointer = 3;
                EnemiesNeeded = 15;
                CurrentQuest = "Area 2";
            }
            if (Quest == "Area 3")
            {
                EnemyPointer = 6;
                EnemiesNeeded = 30;
                CurrentQuest = "Area 3";
            }

            if(Quest == "Boss")
            {
                EnemyPointer = 9;
                EnemiesNeeded = 1;
                CurrentQuest = "Boss";
                AC.MusicFromTo("Boss");
            }
            //Setup:
            //First Enemy:
            GameObject Enemy = Instantiate(EnemyPrefab, EnemySpawnLocation.transform);
            Enemy.transform.parent = EnemySpawnLocation.transform;
            enemy = Enemy.GetComponent<Enemy>();
            EnemyActive = true;

            //Stat Setting:
            EnemyAttackIn = enemy.AS;
            PlayerAttackIn = AttackSpeed;

            Hp = MaxHp;
            EnemyHp = enemy.MaxHp;

            //Game Logic:
            EnemiesKilled = 0;
            PreStart = true;
        }
        else
        {
            bool DoNextQuest = true;
            //Ends The Action:
            PreStart = false;
            Debug.Log("Quest Ended.");
            //Outcomes
            if (EnemiesKilled >= EnemiesNeeded)
            {
                if(Quest == "Area 1")
                {
                    Debug.Log("Bonus XP");
                    XP += 100;
                    Followers += (int)Mathf.Round(10f * (PoPGrowthBonus));
                    AC.PlayNoise(14);
                    QuestCompleteDisplay.SetActive(true);
                    StartCoroutine(HideQuestCompleteDisplay());
                }
                else if(Quest == "Area 2")
                {
                    Debug.Log("Bonus XP");
                    XP += 750;
                    Followers += (int)Mathf.Round(100f * (PoPGrowthBonus));
                    AC.PlayNoise(14);
                    QuestCompleteDisplay.SetActive(true);
                    StartCoroutine(HideQuestCompleteDisplay());
                }
                else if(Quest == "Area 3")
                {
                    XP += 5000;
                    Debug.Log("Start Boss fight");
                    CurrentQuest = "Boss";
                }
                else if(Quest == "Boss")
                {
                    DoNextQuest = false;
                }
            }
            else
            {
                if (!FirstRun)
                {
                    FailedDisplay.SetActive(true);
                    AC.PlayNoise(9);
                    StartCoroutine(HideFailedDisplay());
                }
                else
                {
                    FirstRun = false;
                }
            }
            //DeSetup:
            GameObject[] Mobs;
            Mobs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject Mob in Mobs)
            {
                Destroy(Mob);
            }
            EnemyActive = false;
            if (DoNextQuest == true)
            {
                StartQuest(CurrentQuest);
            }
            else
            {
                Debug.Log("Game Won!");
                EndGame(); 
            }
        }
    }
    IEnumerator HideFailedDisplay()
    {
        yield return new WaitForSeconds(2);
        FailedDisplay.SetActive(false);
    }

    IEnumerator HideQuestCompleteDisplay()
    {
        yield return new WaitForSeconds(2);
        QuestCompleteDisplay.SetActive(false);
    }

    //End game Variables:
    public bool End = false;
    public GameObject EndScreen;

    public void EndGame()
    {
        //Display End Screen music naration ect.
        End = true;
        GameObject[] Mobs;
        Mobs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Mob in Mobs)
        {
            Destroy(Mob);
        }
        EndScreen.SetActive(true);
        AC.PlayDialogue(5);
        AC.MusicFromTo("End");
    }

    //Enemy Attack Noise:
    public int EANpointer;
    public int EDNpointer;
    public bool DoEAanimation;
    public bool DoPAanimation;

    // FixedUpdate is called before each internal physics update ( deafualt 0.02 timestep ) 
    void FixedUpdate()
    {
        if (!End)
        {

            if (PreStart && GameObject.FindGameObjectWithTag("Enemy") != null)
            {
                int EnemyDamage = enemy.Dmg;
                EnemyAttackIn = enemy.AS;
                EnemyHp = enemy.MaxHp;
                switch(enemy.name)
                {
                    case "Rat":
                        EANpointer = 19;
                        EDNpointer = 7;
                        break;
                    case "Goblin":
                        EANpointer = 4;
                        EDNpointer = 6;
                        break;
                    case "Wolf":
                        EANpointer = 2;
                        EDNpointer = 5;
                        break;
                    case "Forest Shadow":
                        EANpointer = 2;
                        EDNpointer = 5;
                        break;
                    case "Beholder":
                        EANpointer = 20;
                        EDNpointer = 6;
                        break;
                    case "Dire Rat":
                        EANpointer = 19;
                        EDNpointer = 7;
                        break;
                    case "Hobgolblin":
                        EANpointer = 1;
                        EDNpointer = 6;
                        break;
                    case "Shadow of Death":
                        EANpointer = 2;
                        EDNpointer = 6;
                        break;
                    case "Gelatinous Cube":
                        EANpointer = 3;
                        EDNpointer = 6;
                        break;
                    case "God":
                        EANpointer = 17;
                        EDNpointer = 18;
                        break;
                    default:
                        Debug.Log("Audio out of bounds");
                        break;
                }
                if (EnemyDamage > 0 && EnemyAttackIn > 0f && EnemyHp > 0)
                {
                    Start = true;
                    PreStart = false;
                }
            }
            if (Start)
            {
                if (!(EnemiesKilled >= EnemiesNeeded))
                {
                    Debug.Log("Do'in Quest");
                    if (EnemyActive)
                    {
                        if (DoEAanimation)
                        {
                            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
                            Vector3 target = new Vector3(EnemySpawnLocation.transform.position.x - 100, EnemySpawnLocation.transform.position.y, EnemySpawnLocation.transform.position.z);
                            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, target, 500f * Time.deltaTime);
                        }
                        else
                        {
                            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
                            Vector3 target = new Vector3(EnemySpawnLocation.transform.position.x, EnemySpawnLocation.transform.position.y, EnemySpawnLocation.transform.position.z);

                            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, target, 500f * Time.deltaTime);
                        }
                        if (DoPAanimation)
                        {
                            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
                            Vector3 target = new Vector3(EnemySpawnLocation.transform.position.x, EnemySpawnLocation.transform.position.y +100, EnemySpawnLocation.transform.position.z);
                            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, target, 500f * Time.deltaTime);
                        }
                        else
                        {
                            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
                            Vector3 target = new Vector3(EnemySpawnLocation.transform.position.x, EnemySpawnLocation.transform.position.y, EnemySpawnLocation.transform.position.z);

                        }
                            Debug.Log("Enemy Active EnemyHP: " + EnemyHp.ToString());
                        //Combat
                        if (EnemyHp >= 1 && Hp >= 1)
                        {
                            Debug.Log("Combat");
                            if (EnemyAttackIn <= 0f)
                            {
                                Debug.Log("Enemy Attack");
                                AC.PlayNoise(EANpointer);
                                DoEAanimation = true;
                                EnemyAttackIn = enemy.AS;
                                Hp = Hp - enemy.Dmg;
                                StartCoroutine(StopEAin());
                            }
                            else
                            {
                                EnemyAttackIn = EnemyAttackIn - (Time.deltaTime);

                            }

                            if (PlayerAttackIn <= 0f)
                            {
                                Debug.Log("Player Attack");
                                AC.PlayNoise(1);
                                DoPAanimation = true;
                                EnemyHp = EnemyHp - Damage;
                                PlayerAttackIn = AttackSpeed;
                                StartCoroutine(StopPAin());
                            }
                            else
                            {
                                PlayerAttackIn = PlayerAttackIn - (Time.deltaTime);
                            }
                            if (FollowersController.fighters > 0)
                            {
                                //if (fightersAttackIn <= 0f)
                                //{
                                //    EnemyHp -= 1;
                                //    fightersAttackIn = fightersAS;
                                //    if (Random.Range(0f, 1f) < 0.5)
                                //    {
                                //        FollowersController.fighters--;
                                //        if (FollowersController.fighters == 0)
                                //        {
                                //            fightersAS = 0;
                                //            fightersAttackIn = 0;
                                //            fightersText.text = FollowersController.fighters.ToString();
                                //        }
                                //        else
                                //        {
                                //            fightersAS = FollowersController.fighterAS / FollowersController.fighters;
                                //            fightersText.text = FollowersController.fighters.ToString();
                                //        }
                                //        Followers--;
                                //    }
                                //}
                                //else
                                //{
                                //    fightersAttackIn -= Time.deltaTime;
                                //}

                                for (int i = 0; i < FollowersController.fighterTimers.Count; i++)
                                {
                                    if (FollowersController.fighterTimers[i] <= 0)
                                    {
                                        EnemyHp -= 1;
                                        AC.PlayNoise(4);
                                        FollowersController.fighterTimers[i] = 20;
                                        if (Random.Range(0f, 1f) < 0.2)
                                        {
                                            FollowersController.fighterTimers.RemoveAt(i);
                                            FollowersController.fighters--;
                                            Followers--;
                                            fightersText.text = FollowersController.fighters.ToString();
                                            AC.PlayNoise(10);
                                            i--;
                                        }
                                    }
                                    else
                                    {
                                        FollowersController.fighterTimers[i] -= Time.deltaTime;
                                    }
                                }
                            }
                        }
                        else //Death(player or mob):
                        {
                            if (EnemyHp <= 0)
                            {
                                Debug.Log("Regular XP");
                                XP = XP + enemy.XPGiven;
                                EnemiesKilled++;
                                EnemyAttackIn = enemy.AS;
                                AC.PlayNoise(EDNpointer);
                                enemy.EnemyImage = null;
                                EnemyActive = false;
                            }
                            else
                            {
                                if(enemy.name == "Boss")
                                {
                                    AC.MusicFromTo("Title");
                                }
                                PreStart = true;
                                StartQuest(CurrentQuest);
                            }
                        }
                    }
                    else
                    {
                        if (GameObject.FindGameObjectWithTag("Enemy") == null)
                        {
                            GameObject Enemy = Instantiate(EnemyPrefab, EnemySpawnLocation.transform);
                            Enemy.transform.parent = EnemySpawnLocation.transform;
                            enemy = Enemy.GetComponent<Enemy>();
                            EnemyActive = true;
                            PreStart = true;
                        }
                        else
                        {
                            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
                            Destroy(Enemy);
                        }
                    }

                }
                else
                {
                    if(CurrentQuest == "Boss")
                    {
                        EndGame();
                    }

                    PreStart = true;
                    StartQuest(CurrentQuest);
                }
                Display();
                if (XP >= MaxXP)
                {
                    AC.PlayNoise(14);
                    LevelUpDisplay.SetActive(true);
                    StartCoroutine(HideLevelUpDisplay());
                    UnallocatedStatPoints++;
                    XP = XP - MaxXP;
                    MaxXP = (int)Mathf.Round(MaxXP * 1.15f);
                }
            }
        }
    }

    IEnumerator StopEAin()
    {
        yield return new WaitForSeconds(0.1f);
        DoEAanimation = false;
    }
    IEnumerator StopPAin()
    {
        yield return new WaitForSeconds(0.1f);
        DoPAanimation = false;
    }

    IEnumerator HideLevelUpDisplay()
    {
        yield return new WaitForSeconds(2);
        LevelUpDisplay.SetActive(false);
    }

    public void Display()
    {
        //All Texts that change on screen
        PlayerXPtext.text = "XP: "+ Mathf.Round(XP).ToString() + "/" + MaxXP.ToString();
        PlayerHPtext.text = "HP: " + Hp.ToString() + "/" + MaxHp.ToString();
        EnemyHPtext.text = "HP: " + EnemyHp.ToString() + "/" + (enemy.MaxHp).ToString();
        EnemyNametext.text = enemy.name;
        UnallocatedStatPointstext.text = "Points: "+UnallocatedStatPoints.ToString();
        Damagetext.text = "Damage: "+Damage.ToString();
        AStext.text = "Atk Speed: "+AttackSpeed.ToString();
        FollowerGaintext.text = "Popularity: "+PoPGrowthBonus.ToString()+"%";
        STRtext.text = "STR: " + Str.ToString();
        AGItext.text = "AGI: " + Agi.ToString();
        CONSTtext.text = "CONST: " + Const.ToString();
        CHARtext.text = "CHAR: " + Chari.ToString();
        EnemiesNeededText.text = "Mobs Killed: " + EnemiesKilled.ToString() + "/" + EnemiesNeeded.ToString();

        //Sliders
        PlayerXPSlider.maxValue = MaxXP;
        PlayerXPSlider.value = XP;

        PlayerHPSlider.maxValue = MaxHp;
        PlayerHPSlider.value = Hp;

        EnemySlider.maxValue = enemy.MaxHp;
        EnemySlider.value = EnemyHp;

        EnemyAtkSlider.maxValue = enemy.AS;
        PlayerAtkSlider.maxValue = AttackSpeed;

        EnemyAtkSlider.value = enemy.AS - EnemyAttackIn;
        PlayerAtkSlider.value = AttackSpeed - PlayerAttackIn;

        Followerstext.text = string.Format("{0:D7}", Followers);
    }

    //Increment Stats / Buttons!
    public void IncrementSTR()
    {
        if(UnallocatedStatPoints > 0)
        {
            UnallocatedStatPoints--;
            Str++;
            AC.PlayNoise(12);
            SetInPlay();
        }
        else
        {
            AC.PlayNoise(11);
            OutPutToPlayer(0);
        }
    }

    public void IncrementAGI()
    {
        if (UnallocatedStatPoints > 0)
        {
            UnallocatedStatPoints--;
            Agi++;
            AC.PlayNoise(12);
            SetInPlay();
        }
        else
        {
            AC.PlayNoise(11);
            OutPutToPlayer(0);
        }
    }

    public void IncrementCON()
    {
        if (UnallocatedStatPoints > 0)
        {
            UnallocatedStatPoints--;
            Const++;
            AC.PlayNoise(12);
            SetInPlay();
        }
        else
        {
            AC.PlayNoise(11);
            OutPutToPlayer(0);
        }
    }

    public void IncrementCHAR()
    {
        if (UnallocatedStatPoints > 0)
        {
            UnallocatedStatPoints--;
            Chari++;
            AC.PlayNoise(12);
            SetInPlay();
        }
        else
        {
            AC.PlayNoise(11);
            OutPutToPlayer(0);
        }
    }


    public bool LockArea2 = true;
    public bool LockArea3 = true;

    public TMP_Text Area2text;
    public TMP_Text Area3text;

    public int Cost;
    //Select Area 1
    public void SelectArea1()
    {
        if(CurrentQuest == "Area 1")
        {
            OutPutToPlayer(2);
        }
        else
        {
            tagOne.SetActive(true);
            tagTwo.SetActive(false);
            tagThree.SetActive(false);
            Background.sprite = AreaOneBack;
            PreStart = false;
            StartQuest("Area 1");
        }
    }
    //Select Area 2
    public void SelectArea2()
    {
        if (CurrentQuest == "Area 2")
        {
            OutPutToPlayer(2);
        }
        else
        {
            if (LockArea2 )
            {
                if(Followers >= 200)
                {
                    Followers = Followers - 200;
                    LockArea2 = false;
                    GameObject LockImage2 = GameObject.Find("LockedImage2");
                    LockImage2.SetActive(false);
                    Area2text.text = "Jungle";
                }
                else
                {
                    Cost = 200;
                    OutPutToPlayer(3);
                }
            }
            else
            {
                tagOne.SetActive(false);
                tagTwo.SetActive(true);
                tagThree.SetActive(false);
                Background.sprite = AreaTwoBack;
                PreStart = false;
                StartQuest("Area 2");
            }
        }
    }
    //Select Area 3
    public void SelectArea3()
    {
        if (CurrentQuest == "Area 3")
        {
            OutPutToPlayer(2);
        }
        else
        {
            if (LockArea3)
            {
                if (Followers >= 1000)
                {
                    Followers = Followers - 1000;
                    LockArea3 = false;
                    GameObject LockImage3 = GameObject.Find("LockedImage3");
                    LockImage3.SetActive(false);
                    Area3text.text = "Dungeon";
                }
                else
                {
                    Cost = 1000;
                    OutPutToPlayer(3);
                }
            }
            else
            {
                tagOne.SetActive(false);
                tagTwo.SetActive(false);
                tagThree.SetActive(true);
                Background.sprite = AreaThreeBack;
                PreStart = false;
                StartQuest("Area 3");
            }
        }
    }

    //Clicky Clicky Bit:
    public void Slice()
    {
        if (Start)
        {
            if (EnemyActive)
            {
                if (EnemyHp >= 0 && Hp >= 0)
                {
                    int choice = Random.Range(0, 3);
                    switch (choice)
                    {
                        case 0:
                            choice = 0;
                            break;
                        case 1:
                            choice = 15;
                            break;
                        case 2:
                            choice = 16;
                            break;
                        default:
                            choice = 0;
                            break;
                    }
                    AC.PlayNoise(choice);
                    PlayerAttackIn = PlayerAttackIn - clickAmount;
                }
            }
        }
    }

    public void OutPutToPlayer(int choice)
    {
        if(choice == 0)
        {
            //if Player doesn't have enough Points:
            OutGO.SetActive(true);
            OutText.text = "Not Enough Points to upgrade.\nSlay more mobs.";
        }
        if(choice == 1)
        {
            //if Player doesn't have enough Followers:
            OutGO.SetActive(true);
            OutText.text = "Not Enough followers to upgrade.\nDo more quests.";
        }
        if(choice == 2)
        {
            //if Player has already selected that quest:
            OutGO.SetActive(true);
            OutText.text = "That quest is already selected.\nSee there's a red dot.";
        }
        if(choice == 3)
        {
            //if Player doesn't have enough followers to unlock a quest:
            OutGO.SetActive(true);
            OutText.text = "Not Enough followers to Unlock.\nComplete more Quests.\nYou need atleast:"+Cost.ToString()+" followers.";
        }
        StartCoroutine(HideBox());
    }

    IEnumerator HideBox()
    {
        yield return new WaitForSeconds(3);
        OutGO.SetActive(false);
    }

}
