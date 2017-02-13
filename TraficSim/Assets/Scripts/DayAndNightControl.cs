//2016 Spyblood Games

using UnityEngine;
using System.Collections;

public class DayAndNightControl : MonoBehaviour {
	Mesh mesh;
	public GameObject StarDome;
	public int currentDay = 0; //day 8287... still stuck in this grass prison... no esacape... no freedom...
	public string DayState;
	public Light directionalLight; //the directional light in the scene we're going to work with
	public float SecondsInAFullDay = 120f; //in realtime, this is about two minutes by default. (every 1 minute/60 seconds is day in game)
	[Range(0,1)]
	public float currentTime = 0; //at default when you press play, it will be nightTime. (0 = night, 1 = day)
	[HideInInspector]
	public float timeMultiplier = 1f; //how fast the day goes by regardless of the secondsInAFullDay var. lower values will make the days go by longer, while higher values make it go faster. This may be useful if you're siumulating seasons where daylight and night times are altered.

	float lightIntensity; //static variable to see what the current light's insensity is in the inspector
	Material starMat;

	// Use this for initialization
	void Start () {
		lightIntensity = directionalLight.intensity; //what's the current intensity of the light
		starMat = StarDome.GetComponent<MeshRenderer> ().material;
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateLight();
		CheckTimeOfDay ();
		currentTime += (Time.deltaTime / SecondsInAFullDay) * timeMultiplier;
		Config.FLT_TIME_OF_DAY = currentTime;
		if (currentTime >= 1) {
			currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
			currentDay++; //make the day counter go up
		}
	}

	void UpdateLight()
	{
		StarDome.transform.Rotate (new Vector3 (0, 0, 2f * Time.deltaTime));

		directionalLight.transform.localRotation = Quaternion.Euler ((currentTime * 360f) - 90, 170, 0);
		//^^ we rotate the sun 360 degrees around the x axis, or one full rotation times the current time variable. we subtract 90 from this to make it go up
		//in increments of 0.25.

		//the 170 is where the sun will sit on the horizon line. if it were at 180, or completely flat, it would be hard to see. Tweak this value to what you find comfortable.

		float intensityMultiplier = 1;

		if (currentTime <= 0.23f || currentTime >= 0.75f) 
		{
			intensityMultiplier = 0; //when the sun is below the horizon, or setting, the intensity needs to be 0 or else it'll look weird
			starMat.color = new Color(1,1,1,Mathf.Lerp(1,0,Time.deltaTime));
		}
		else if (currentTime <= 0.25f) 
		{
			intensityMultiplier = Mathf.Clamp01((currentTime - 0.23f) * (1 / 0.02f));
			starMat.color = new Color(1,1,1,Mathf.Lerp(0,1,Time.deltaTime));
		}
		else if (currentTime <= 0.73f) 
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1 / 0.02f)));
		}

		directionalLight.intensity = lightIntensity * intensityMultiplier;
	}

	void CheckTimeOfDay ()
	{
	if (currentTime < 0.25f || currentTime > 1f) {
			DayState = "Midnight";
		}
		if (currentTime > 0.25f)
		{
			DayState = "Morning";

		}
		if (currentTime > 0.25f && currentTime < 0.5f)
		{
			DayState = "Mid Noon";
		}
		if (currentTime > 0.5f && currentTime < 0.75f)
		{
			DayState = "Evening";

		}
		if (currentTime > 0.75f && currentTime < 1f)
		{
			DayState = "Night";
		}
	}
}
