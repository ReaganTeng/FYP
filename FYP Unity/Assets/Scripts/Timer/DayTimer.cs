using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class DayTimer : MonoBehaviour
{
    [SerializeField] LevelManager lm;
    [SerializeField] GameObject HourHand;
    [SerializeField] GameObject MinuteHand;
    [SerializeField] EndOfDay eod;
    float TimeFortheDayInSeconds;
    private float thedaytimer;
    private float RotateHourBy;
    private float RotateMinuteBy;
    bool TimeStop;

    // Start is called before the first frame update
    void Start()
    {
        // Set the time for the day according to what user put for that level
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            if (i == lm.DaySelected - 1)
            {
                TimeFortheDayInSeconds = lm.levelInfo[i].DayDuration;
            }
        }

        thedaytimer = TimeFortheDayInSeconds;
        RotateHourBy = 360 / thedaytimer;
        RotateMinuteBy = 360 * 12 / thedaytimer;
        TimeStop = false;
        ProCamera2DTransitionsFX.Instance.TransitionEnter();
    }

    private void Update()
    {
        // during game startup, do not run the update
        if (FreezeGame.instance.startUpfreeze || Tutorial.instance.InTutorial)
            return;

        if (!TimeStop)
        {
            thedaytimer -= Time.deltaTime;

            if (thedaytimer <= 0)
            {
                TimeStop = true;
                eod.StopOrders();
            }
        }

        // Rotate the hour hand, needed to just do 1 complete circle
        Quaternion currentHourRotation = HourHand.transform.rotation;
        Quaternion newHourRotation = Quaternion.Euler(0, 0, -RotateHourBy * Time.deltaTime) * currentHourRotation;
        HourHand.transform.rotation = newHourRotation;

        // Rotate the minute hand, needed to do 12 complete rounds
        Quaternion currentMinuteRotation = MinuteHand.transform.rotation;
        Quaternion newMinuteRotation = Quaternion.Euler(0, 0, -RotateMinuteBy * Time.deltaTime) * currentMinuteRotation;
        MinuteHand.transform.rotation = newMinuteRotation;
    }

    public void SetTimer(int timer)
    {
        thedaytimer = timer;
    }
}
