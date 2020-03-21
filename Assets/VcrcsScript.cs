using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class VcrcsScript : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;

    public KMSelectable Center;
    public TextMesh Words;

    private string[][] possibleWords = new string[10][]{
        new string[10] {"destiny", "control", "refresh", "grouped", "wedging", "summary", "kitchen", "teacher", "concern", "section"},
        new string[10] {"similar", "western", "dropper", "checker", "xeroses", "sunrise", "abolish", "harvest", "protest", "shallow"},
        new string[10] {"plotted", "deafens", "colored", "aroused", "unsling", "holiday", "dictate", "dribble", "retreat", "episode"},
        new string[10] {"crashed", "crazily", "silvers", "usurped", "witcher", "jealous", "village", "wizards", "prosper", "recycle"},
        new string[10] {"pounced", "nonfood", "imblaze", "dryable", "swiftly", "mention", "rubbish", "realize", "collect", "surgeon"},
        new string[10] {"gearbox", "schnozz", "passion", "freshen", "society", "passive", "archive", "shelter", "harmful", "freedom"},
        new string[10] {"papayas", "thwarts", "railway", "teapots", "ravines", "density", "provide", "diagram", "lighter", "general"},
        new string[10] {"upriver", "editors", "mingled", "ransoms", "prairie", "balance", "applied", "history", "calorie", "realism"},
        new string[10] {"liquids", "validly", "varying", "wickers", "isolate", "falsify", "painter", "mixture", "bedroom", "dilemma"},
        new string[10] {"skylike", "ranging", "simplex", "gallied", "missile", "posture", "highway", "prevent", "bracket", "project"}
    };

    private int[] Indices = { 0, 0 };
    private bool ModuleSolved;

    // Logging
    static int moduleIdCounter = 1;
    int moduleId;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        Center.OnInteract += delegate () { CenterPress(); return false; };
    }

    void Start()
    {
        for (int a = 0; a < 2; a++)
        {
            Indices[a] = UnityEngine.Random.Range(0, 10);
        }
        Words.text = possibleWords[Indices[0]][Indices[1]];
        Debug.LogFormat("[Vcrcs #{0}] The displayed word is {1}. It’s in set {2}.", moduleId, Words.text, Indices[0]);
    }

    void CenterPress()
    {
        Center.AddInteractionPunch(0.2f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
        if (ModuleSolved)
            return;
        Debug.LogFormat("[Vcrcs #{0}] You pressed on a {1}.", moduleId, ((int)Bomb.GetTime()) % 10 );
        if (((int)Bomb.GetTime()) % 10 == Indices[0])
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            Module.HandlePass();
            Debug.LogFormat("[Vcrcs #{0}] That was correct. Module solved!", moduleId);
            Words.text = "SOLVED";
            ModuleSolved = true;
        }
        else
        {
            Module.HandleStrike();
            Debug.LogFormat("[Vcrcs #{0}] That was incorrect. Strike! Resetting...", moduleId);
            Start();
        }
    }
}
