﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exodrifter.Rumor.Engine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BarrelText : MonoBehaviour
{
    private Rumor rumor;
    public Text text;

    private void Awake()
    {
        rumor = new Rumor(File.ReadAllText("Assets/Writing/Barrel.txt"));
        rumor.Scope.DefaultSpeaker = "Manager";

        StartCoroutine(rumor.Start());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rumor == null)
        {
            text.text = "";
            return;
        }

        if (rumor.Finished)
            SceneManager.LoadScene("BarrelScene");

        text.text = rumor.State.Dialog["Manager"];

        rumor.Update(Time.deltaTime);
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            rumor.Advance();
    }
}
