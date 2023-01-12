using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string name;

    public float AS;

    public int Dmg;
    public int MaxHp;
    public int XPGiven;

    public Image EnemyImage;

    public Sprite Beholder;
    public Sprite Goblin;
    public Sprite God;
    public Sprite Rat;
    public Sprite Wolf;
    public Sprite GelatinousCube;
    public Sprite Shadow;

    // Start is called before the first frame update
    void Start()
    {

        // gets the desciding variable (from GameController),
        // for what selection of mobs to choose from.
        GameObject GC = GameObject.Find("GameController");
        GameController gameController = GC.GetComponent<GameController>();
        int Adjust = gameController.EnemyPointer;
        // Generates a random number within set parameters descided by the
        // EnemyPointer.
        int num = Random.Range(0 + Adjust, 3 + Adjust);
        switch (num) {
            case 0:
                //place holder names!
                name = "Goblin";
                Dmg = 2;
                AS = 9f;
                MaxHp = 8;
                XPGiven = 10;
                EnemyImage.sprite = Goblin;
                break;
            case 1:
                name = "Rat";
                Dmg = 1;
                AS = 3f;
                MaxHp = 4;
                XPGiven = 7;
                EnemyImage.sprite = Rat;
                break;
            case 2:
                name = "Wolf";
                Dmg = 2;
                AS = 6f;
                MaxHp = 6;
                XPGiven = 12;
                EnemyImage.sprite = Wolf;
                break;
            //Next location
            case 3:
                name = "Hobgoblin";
                Dmg = 6;
                AS = 7f;
                MaxHp = 22;
                XPGiven = 65;
                EnemyImage.sprite = Goblin;
                break;
            case 4:
                name = "Dire Rat";
                Dmg = 4;
                AS = 5f;
                MaxHp = 14;
                XPGiven = 55;
                EnemyImage.sprite = Rat;
                break;
            case 5:
                name = "Forest Shadow";
                Dmg = 5;
                AS = 6f;
                MaxHp = 17;
                XPGiven = 60;
                EnemyImage.sprite = Shadow;
                break;
            //Next location
            case 6:
                name = "Gelatinous Cube";
                Dmg = 20;
                AS = 4f;
                MaxHp = 50;
                XPGiven = 350;
                EnemyImage.sprite = GelatinousCube;
                break;
            case 7:
                name = "Beholder";
                Dmg = 10;
                AS = 1f;
                MaxHp = 40;
                XPGiven = 400;
                EnemyImage.sprite = Beholder;
                break;
            case 8:
                name = "Shadow of Death";
                Dmg = 20;
                AS = 2f;
                MaxHp = 60;
                XPGiven = 400;
                EnemyImage.sprite = Shadow;
                break;
                //Boss 
            case 9:
                name = "God";
                Dmg = 60;
                AS = 3f;
                MaxHp = 1000;
                XPGiven = 0;
                EnemyImage.sprite = God;
                break;
            case 10:
                name = "God";
                Dmg = 60;
                AS = 3f;
                MaxHp = 1000;
                XPGiven = 0;
                EnemyImage.sprite = God;
                break;
            case 11:
                name = "God";
                Dmg = 60;
                AS = 3f;
                MaxHp = 1000;
                XPGiven = 0;
                EnemyImage.sprite = God;
                break;
            //incase of an overflow to EnemyPointer ( programing error ).
            default:
                Debug.Log("Error Enemy Unknown");
                break;
        }
        
    }
}
