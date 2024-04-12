using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SentenseSO : ScriptableObject
{
    [SerializeField] private int idSentence;
    [SerializeField] private GameObject sentenceGO;

    public GameObject SentenceSO
    {
        get { return sentenceGO; }
        set { sentenceGO = value; }
    }
    public int IdSentence
    {
        get { return idSentence; }
        set { idSentence = value; }
    }
}
