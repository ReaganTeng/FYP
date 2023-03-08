using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool start = true;
    // Start is called before the first frame update

    private void Update()
    {
        if (start)
        {
            GeneralCardManager.instance.testcards[0] = new TestCard();
            GeneralCardManager.instance.testcards[1] = new ATestCard();
            GeneralCardManager.instance.Execute();
            start = false;
        }
    }
}
