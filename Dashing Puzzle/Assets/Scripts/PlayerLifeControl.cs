using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeControl : MonoBehaviour
{
    public int lifes = 99;

    private void Awake()
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = CharacterSeletion.Instance.RetriveCharacterAsset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillPlayer() {
        Debug.Log("Player is dead!");

        //ativa anim de morte
        StartCoroutine(deathAnim());
    }
    private IEnumerator deathAnim()
    {
        gameObject.GetComponent<PlayerMovimentation>().enabled = false;
        gameObject.GetComponentInChildren<Animator>().SetTrigger("death");
        yield return new WaitForSeconds(0.8f);
        gameObject.GetComponent<PlayerMovimentation>().enabled = true;

        lifes -= 1;

        this.GetComponent<PlayerMovimentation>().RespawnPlayerAfterDeath();

    }

}
