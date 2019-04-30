using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSeletion : MonoBehaviour
{
    public static CharacterSeletion instance;

    public static CharacterSeletion Instance { get { return instance; } }

    [HideInInspector] public static Sprite CharacterAsset;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public void ChangeCharacterAsset(Sprite image)
    {
        Debug.Log("Entrei 1");
        CharacterAsset = image;
    }

    public Sprite RetriveCharacterAsset()
    {
        return CharacterAsset;
    }
}
