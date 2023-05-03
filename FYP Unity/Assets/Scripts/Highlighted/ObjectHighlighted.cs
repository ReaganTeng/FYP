using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighted : MonoBehaviour
{
    [SerializeField] GameObject HighlightCircle;
    [SerializeField] float maxScale;
    [SerializeField] float minScale;
    [SerializeField] float HighlightLoopDuration;
    float highlightduration;
    bool Highlighted = false;
    // Start is called before the first frame update
    void Start()
    {
        HighlightCircle.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        HighlightCircle.SetActive(false);
        highlightduration = HighlightLoopDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Highlighted)
            return;

        highlightduration -= Time.deltaTime;
        float highlightratio = highlightduration/HighlightLoopDuration;
        float scaleAmt = Mathf.Lerp(minScale, maxScale, highlightratio);
        HighlightCircle.transform.localScale = new Vector3(scaleAmt, scaleAmt, scaleAmt);

        if (highlightduration <= 0)
        {
            HighlightCircle.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
            highlightduration = HighlightLoopDuration;
        }
    }

    public void ToggleHighlight(bool highlightOrNot)
    {
        Highlighted = highlightOrNot;
        HighlightCircle.SetActive(Highlighted);
    }
}
