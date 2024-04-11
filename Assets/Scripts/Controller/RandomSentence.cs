using System.Collections.Generic;
using UnityEngine;

public class RandomSentence : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> sentenceList = new();
    private List<GameObject> sentence2Active = new();
    [SerializeField]
    private int sentenceAmount;
    public int SentenceAmount
    {
        get { return sentenceAmount; }
        set { sentenceAmount = value; }
    }
    private void Start()
    {
        if (sentenceList.Count > 0)
        {
            SelectSentences();
        }
        else { Debug.LogWarning("The Sentence List is Empty, for solved fill in the inspector."); }
    }

    private List<int> GenerateRandomNumbers()
    {
        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < sentenceAmount)
        {
            int num = Random.Range(0, sentenceList.Count - 1);
            uniqueNumbers.Add(num);
        }
        return new List<int>(uniqueNumbers);
    }

    private void SelectSentences()
    {
        List<int> sentence = GenerateRandomNumbers();
        for (int i = 0; i <= sentence.Count - 1; i++)
        {
            sentence2Active.Add(sentenceList[sentence[i]]);
        }
    }

    public void ActiveSentence(int sentenceIndex)
    {
        sentence2Active[sentenceIndex].gameObject.SetActive(true);
    }
}
