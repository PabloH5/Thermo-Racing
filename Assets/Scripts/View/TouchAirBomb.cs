using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TouchAirBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject handleBar;
    [SerializeField]
    private Vector3 newPosHB;
    private Vector3 initPosHB;
    // Start is called before the first frame update
    void Start()
    {
        initPosHB = handleBar.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
        }

    }
    IEnumerator MoveHandleBar()
    {
        handleBar.transform.position = newPosHB;
        
        yield return new WaitForSeconds(.5f);

        handleBar.transform.position = initPosHB;
    }

}
