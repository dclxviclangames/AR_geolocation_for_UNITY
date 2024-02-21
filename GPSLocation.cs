using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPSLocation : MonoBehaviour
{
    public Text GPSStatus;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timestampValue;

    public Transform player;
    public GameObject testSphere;
    private bool isState;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
        // InstantEiwithPlayer();
        isState = true;
    }

    // Update is called once per frame
    void Update()
    {
        InstantEiwithPlayer();
    }

    IEnumerator GPSLoc()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            GPSStatus.text = "Time out";
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }
    }

    private void UpdateGPSData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            GPSStatus.text = "Running";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timestampValue.text = Input.location.lastData.timestamp.ToString();
        }
        else
        {
            GPSStatus.text = "Stop";
        }
    }

    private void InstantEiwithPlayer()
    {
        if (Input.location.lastData.latitude > 6666 && Input.location.lastData.longitude > 6666 && isState)
        {
            Instantiate(testSphere, player.position, Quaternion.identity);
            isState = false;
        }
        else if(!isState)
        {
            return;
        }
    }
}
