using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FollowersController : MonoBehaviour
{
    private int pleaseLvl = 0;
    private int pleaseCost = 10;
    private int pleaseReward = 100;
    private int serveLvl = 0;
    private int serveCost = 100;
    private int serveReward = 1;
    private int serveXPRate = 0;
    public static int fighters = 0;
    public static int fighterAS = 20;

    public TextMeshProUGUI pleaseText;
    public TextMeshProUGUI pleaseLvlText;
    public TextMeshProUGUI serveText;
    public TextMeshProUGUI serveLvlText;
    public TextMeshProUGUI fightersText;

    public AudioController AC;

    public static List<float> fighterTimers = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        pleaseText.text = "Please Gods\nCost: 10 Followers\nReward: 100 XP";
        pleaseLvlText.text = "Level: 0";
        serveText.text = "Serve Gods\nCost: "+((int)Mathf.Pow(serveLvl + 1, 2) * 20) +" Followers\nReward: +1 XP / s";
        serveLvlText.text = "Level: 0\n(0 XP / s)";
    }

    // Update is called once per frame
    void Update()
    {
        GameController.XP += serveXPRate * Time.deltaTime;
        if (GameController.Followers < fighters)
        {
            for (int i = 0; i < fighters - GameController.Followers; i++)
            {
                DecreaseFighters();
            }
        }
    }

    public void PleaseGods()
    {
        if (GameController.Followers >= pleaseCost)
        {
            GameController.Followers -= pleaseCost;
            GameController.XP += pleaseReward;
            pleaseLvl++;
            pleaseCost = 10;
            pleaseReward = 100;
            pleaseText.text = "Please Gods\nCost: " + pleaseCost + " Followers\nReward: " + pleaseReward + " XP";
            pleaseLvlText.text = "Level: " + pleaseLvl;
            AC.PlayNoise(12);
        }
        else
        {
            AC.PlayNoise(11);
        }
    }

    public void ServeGods()
    {
        if (GameController.Followers >= serveCost)
        {
            GameController.Followers -= serveCost;
            serveXPRate += serveReward;
            serveLvl++;
            serveCost = (int)Mathf.Pow(serveLvl + 1, 2) * 20;
            serveReward = (int)Mathf.Pow(serveLvl + 1, 2) +1;
            serveText.text = "Serve Gods\nCost: " + serveCost + " Followers\nReward: +" + serveReward + " XP / s";
            serveLvlText.text = "Level: " + serveLvl + "\n(" + serveXPRate + " XP / s)";
            AC.PlayNoise(12);
        }
        else
        {
            AC.PlayNoise(11);
        }
    }

    public void IncreaseFighters()
    {
        if (GameController.Followers > fighters)
        {
            fighters++;
            //if (fighters == 1)
            //{
            //    GameController.fightersAS = fighterAS;
            //    GameController.fightersAttackIn = fighterAS;
            //    fightersText.text = fighters.ToString();
            //}
            //else
            //{
            //    GameController.fightersAS = fighterAS / fighters;
            //    GameController.fightersAttackIn = fighterAS / fighters;
            //    fightersText.text = fighters.ToString();
            //}

            fightersText.text = fighters.ToString();
            fighterTimers.Add(fighterAS);
        }
    }
    public void IncreaseFightersHundred()
    {
        for (int counter = 0; counter < 100; counter++)
        {
            if (GameController.Followers > fighters)
            {
                fighters++;
                //if (fighters == 1)
                //{
                //    GameController.fightersAS = fighterAS;
                //    GameController.fightersAttackIn = fighterAS;
                //    fightersText.text = fighters.ToString();
                //}
                //else
                //{
                //    GameController.fightersAS = fighterAS / fighters;
                //    GameController.fightersAttackIn = fighterAS / fighters;
                //    fightersText.text = fighters.ToString();
                //}

                fightersText.text = fighters.ToString();
                fighterTimers.Add(fighterAS - 0.001f * counter);
            }
        }
    }

    public void DecreaseFighters()
    {
        if (fighters > 0)
        {
            fighters--;
            //if (fighters == 0)
            //{
            //    GameController.fightersAS = 0;
            //    GameController.fightersAttackIn = 0;
            //    fightersText.text = fighters.ToString();
            //}
            //else
            //{
            //    GameController.fightersAS = fighterAS / fighters;
            //    GameController.fightersAttackIn = fighterAS / fighters;
            //    fightersText.text = fighters.ToString();
            //}

            fightersText.text = fighters.ToString();
            fighterTimers.RemoveAt(fighterTimers.Count - 1);
        }
    }
    public void DecreaseFightersHundred()
    {
        for (int counter = 0; counter < 100; counter++)
        {
            if (fighters > 0)
            {
                fighters--;
                //if (fighters == 0)
                //{
                //    GameController.fightersAS = 0;
                //    GameController.fightersAttackIn = 0;
                //    fightersText.text = fighters.ToString();
                //}
                //else
                //{
                //    GameController.fightersAS = fighterAS / fighters;
                //    GameController.fightersAttackIn = fighterAS / fighters;
                //    fightersText.text = fighters.ToString();
                //}

                fightersText.text = fighters.ToString();
                fighterTimers.RemoveAt(fighterTimers.Count - 1);
            }
        }
    }
}
