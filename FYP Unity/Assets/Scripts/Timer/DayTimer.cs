using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    [SerializeField] GameObject HourHand;
    [SerializeField] GameObject MinuteHand;
    [SerializeField] EndOfDay eod;
    [SerializeField] float TimeFortheDayInSeconds;
    private float thedaytimer;
    private float RotateHourBy;
    private float RotateMinuteBy;
    bool TimeStop;

    // Start is called before the first frame update
    void Start()
    {
        thedaytimer = TimeFortheDayInSeconds;
        RotateHourBy = 360 / thedaytimer;
        RotateMinuteBy = 360 * 12 / thedaytimer;
        TimeStop = false;
    }

    private void Update()
    {
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
}
