﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeControl : MonoBehaviour
{
    public int lifes = 99;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillPlayer() {
        Debug.Log("Player is dead!");

        //ativa anim de morte
        StartCoroutine(deathAnim());

        lifes -= 1;

        this.GetComponent<PlayerMovimentation>().RespawnPlayerAfterDeath();
    }
    private IEnumerator deathAnim()
    {
        gameObject.GetComponent<PlayerMovimentation>().enabled = false;
        gameObject.GetComponentInChildren<Animator>().SetTrigger("death");
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<PlayerMovimentation>().enabled = true;

    }

}
