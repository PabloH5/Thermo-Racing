using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckList : MonoBehaviour
{
     [SerializeField]
    private List<GameObject> objectList = new List<GameObject>();
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Método para obtener la lista de objetos
    public List<GameObject> GetObjectList()
    {
        return objectList;
    }

    public void ToggleObjects()
    {
        // Desactivar el objeto en la posición actual
        objectList[currentIndex].SetActive(false);

        // Incrementar el índice para apuntar al siguiente objeto
        currentIndex = (currentIndex + 1) % objectList.Count;

        // Activar el siguiente objeto en la lista
        objectList[currentIndex].SetActive(true);

        Debug.Log("Objeto activado: " + objectList[currentIndex].name);
    }
    // Método para mostrar los objetos en la lista en la consola
    public void PrintObjectList()
    {
        foreach (GameObject obj in objectList)
        {
            Debug.Log("Objeto en la lista: " + obj.name);
        }
    }
}
