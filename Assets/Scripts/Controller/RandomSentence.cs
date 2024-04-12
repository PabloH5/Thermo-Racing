using System.Collections.Generic;
using UnityEngine;

public class RandomSentence : MonoBehaviour
{
    [SerializeField]
    private List<SentenseSO> sentenceList = new();
    public List<SentenseSO> sentence2Active = new();

    [SerializeField]
    private int sentenceAmount;
    public int SentenceAmount
    {
        get { return sentenceAmount; }
        set { sentenceAmount = value; }
    }
    // public
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
        return new List<int>(uniqueNumbers);//Genera una lista de las posiciones de las oraciones aletorias que se se seleccionaron
    }

    private void SelectSentences()
    {
        List<int> sentence = GenerateRandomNumbers();
        for (int i = 0; i <= sentence.Count - 1; i++)
        {
            sentence2Active.Add(sentenceList[sentence[i]]);// agrego a la lista de SO los SO seleccionados en el metodo de GenerateRandomNumbers
        }
    }

    public SentenseSO ActiveSentence(int sentenceIndex)
    {
        // sentence2Active[sentenceIndex].gameObject.SetActive(true);
        // Instantiate(sentence2Active[sentenceIndex].SentenceSO);
        // Debug.Log("Yes, Im Active");
        return sentence2Active[sentenceIndex];
    }
}
