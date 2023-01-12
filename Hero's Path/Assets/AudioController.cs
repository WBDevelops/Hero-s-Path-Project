using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    //AudioSources:
    //Music Management:
    private AudioSource CurrentMusic;
    public AudioSource TitleMusic;
    public AudioSource BossMusic;
    public AudioSource EndMusic;

    //Tutorial Boi dialogue:
    public AudioSource TutorialBoiExplainsSelf;
    public AudioSource TutorialBoiExplainsRage;
    public AudioSource TutorialBoiExplainsQuests;
    public AudioSource TutorialBoiExplainsPurpose;
    public AudioSource TutorialBoiFollowers;

    public AudioSource EndGameExplanation;
    public AudioSource CurrentDialogue;

    //Combat noises:
    public AudioSource Rage;
    public AudioSource Rage2;
    public AudioSource Rage3;
    public AudioSource SlashSound;
    public AudioSource AttackSound;
    public AudioSource AttackSoundTwo;
    public AudioSource AttackSoundThree;
    public AudioSource BossAttack;
    public AudioSource BossDeath;
    public AudioSource RatAttack;
    public AudioSource LazerAttack;

    //Combat result noises:
    public AudioSource SlaySoundOne;
    public AudioSource SlaySoundTwo;
    public AudioSource SlaySoundThree;

    public AudioSource PlayerDies;

    //Transactional noises:
    public AudioSource FollowerMumbles;
    public AudioSource Denied;
    public AudioSource Aprroved;

    //Quest and level nosies:
    public AudioSource QuestComplete;
    public AudioSource LevelUp;

    //Logic
    private bool musicChange;
    private bool musicTransition;
    private bool musicSelected;
    private string MusicNameG;

    //Volume Controls:
    public Slider VolumeSlider;
    public TMP_Text DisplayVolume;
    public float VolumeSetting;

    //Volume Controlling variables:
    public float TransitionSpeed = 1f;
    public float MaxVolume = 1f;
    public float MinVolume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentDialogue = TutorialBoiExplainsPurpose;
        CurrentMusic = TitleMusic;
        VolumeSlider.maxValue = MaxVolume;
        VolumeSlider.minValue = MinVolume;
        musicChange = true;
        musicTransition = false;
        musicSelected = false;
    }

    public void MusicFromTo(string MusicName)
    {
        if (MusicName == "Boss")
        {
            MusicNameG = MusicName;
        }
        if(MusicName == "Title")
        {
            MusicNameG = MusicName;
        }
        if(MusicName == "End")
        {
            MusicNameG = MusicName;
        }
        musicTransition = true;
    }
    void FixedUpdate()
    {
        
        if(musicTransition)
        {            
            if(CurrentMusic.volume <= 0f || musicSelected)
            {
                musicChange = true;
                musicSelected = true;
                if (MusicNameG == "Boss")
                {
                    CurrentMusic = BossMusic;
                }
                if (MusicNameG == "Title")
                {
                    CurrentMusic = BossMusic;
                }
                if(MusicNameG == "End")
                {
                    CurrentMusic = EndMusic;
                }
                CurrentMusic.volume += Time.deltaTime * TransitionSpeed;
                if (CurrentMusic.volume >= VolumeSlider.value)
                {
                    musicSelected = false;
                    CurrentMusic.volume = VolumeSlider.value;
                    musicTransition = false;
                }
            }
            else
            {
                CurrentMusic.volume -= Time.deltaTime * TransitionSpeed;
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (musicChange)
        {
            if (!(CurrentMusic.isPlaying))
            {
                CurrentMusic.Play();
                musicChange = false;
            }
        }
        else
        {
            if (!(CurrentMusic.isPlaying))
            {
                CurrentMusic.Play();
            }
        }
        if(!musicTransition)
            CurrentMusic.volume = VolumeSlider.value;

        VolumeSetting = VolumeSlider.value;
        DisplayVolume.text = "Volume Slider: " + ((int)(Mathf.RoundToInt(VolumeSlider.value * 100f))).ToString() + "%";
        if(CurrentDialogue.isPlaying)
            CurrentDialogue.volume = VolumeSetting * 1.5f;
    }

    public void PlayNoise(int choice)
    {
        switch (choice)
        {
            case 0:
                Rage.volume = VolumeSetting;
                Rage.Play();
                break;
            case 1:
                SlashSound.volume = VolumeSetting;
                SlashSound.Play();
                break;
            case 2:
                AttackSound.volume = VolumeSetting;
                AttackSound.Play();
                break;
            case 3:
                AttackSoundTwo.volume = VolumeSetting;
                AttackSoundTwo.Play();
                break;
            case 4:
                AttackSoundThree.volume = VolumeSetting;
                AttackSoundThree.Play();
                break;
            case 5:
                SlaySoundOne.volume = VolumeSetting;
                SlaySoundOne.Play();
                break;
            case 6:
                SlaySoundTwo.volume = VolumeSetting;
                SlaySoundTwo.Play();
                break;
            case 7:
                SlaySoundThree.volume = VolumeSetting;
                SlaySoundThree.Play();
                break;
            case 8:
                PlayerDies.volume = VolumeSetting;
                PlayerDies.Play();
                break;
            case 9:
                PlayerDies.volume = VolumeSetting;
                PlayerDies.Play();
                break;
            case 10:
                FollowerMumbles.volume = VolumeSetting;
                FollowerMumbles.Play();
                break;
            case 11:
                Denied.volume = VolumeSetting;
                Denied.Play();
                break;
            case 12:
                Aprroved.volume = VolumeSetting;
                Aprroved.Play();
                break;
            case 13:
                QuestComplete.volume = VolumeSetting;
                QuestComplete.Play();
                break;
            case 14:
                LevelUp.volume = VolumeSetting;
                LevelUp.Play();
                break;
            case 15:
                Rage2.volume = VolumeSetting;
                Rage2.Play();
                break;
            case 16:
                Rage3.volume = VolumeSetting;
                Rage3.Play();
                break;
            case 17:
                BossAttack.volume = VolumeSetting;
                BossAttack.Play();
                break;
            case 18:
                BossDeath.volume = VolumeSetting;
                BossDeath.Play();
                break;
            case 19:
                RatAttack.volume = VolumeSetting;
                RatAttack.Play();
                break;
            case 20:
                LazerAttack.volume = VolumeSetting;
                LazerAttack.Play();
                break;

            default:
                Debug.Log("OUT OF BOUNDS");
                break;
        }
        
    }
    public void PlayDialogue(int choice)
    {
        switch (choice)
        {
            case 0:
                TutorialBoiExplainsSelf.volume = 1.5f * VolumeSetting;
                CurrentDialogue = TutorialBoiExplainsSelf;
                CurrentDialogue.Play();
                break;
            case 1:
                TutorialBoiExplainsRage.volume = 1.5f * VolumeSetting;
                CurrentDialogue = TutorialBoiExplainsRage;
                CurrentDialogue.Play();
                break;
            case 2:
                TutorialBoiExplainsQuests.volume = 1.5f * VolumeSetting;
                CurrentDialogue = TutorialBoiExplainsQuests;
                CurrentDialogue.Play();
                break;
            case 3:
                TutorialBoiExplainsPurpose.volume = 1.5f * VolumeSetting;
                CurrentDialogue = TutorialBoiExplainsPurpose;
                CurrentDialogue.Play();
                break;
            case 4:
                TutorialBoiFollowers.volume = 1.5f * VolumeSetting;
                CurrentDialogue = TutorialBoiFollowers;
                CurrentDialogue.Play();
                break;
            case 5:
                EndGameExplanation.volume = 1.5f * VolumeSetting;
                CurrentDialogue = EndGameExplanation;
                CurrentDialogue.Play();
                break;
            default:
                Debug.Log("OUT OF BOUNDS 2");
                break;
        }
    }

}
