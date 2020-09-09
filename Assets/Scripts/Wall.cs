﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour{

    /* Visual for player to see that they hit the wall */
    public Sprite dmgSprite;
    public int hp = 4;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake(){

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss){

        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;

        /* If hp <=0, deactivate the component */
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
