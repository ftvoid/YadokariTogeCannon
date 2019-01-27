using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DestroyLight))]
public class DestroyLight : MonoBehaviour
{
	public float duration = 1.0f;

	public float delay = 0.0f;

	private float Gr_Intensity;
	
	private float Overlifetime = 0.0f;
	private float Overdelay;
	
	
	void Start()
	{
		Gr_Intensity = GetComponent<Light>().intensity;
	}
	
	void OnEnable()
	{
		Overlifetime = 0.0f;
		Overdelay = delay;
		if(delay > 0) GetComponent<Light>().enabled = false;
	}
	
	void Update ()
	{
		if(Overdelay > 0)
		{
			Overdelay -= Time.deltaTime;
			if(Overdelay <= 0)
			{
				GetComponent<Light>().enabled = true;
			}
			return;
		}
		
		if(Overlifetime/duration < 1.0f)
		{
			GetComponent<Light>().intensity = Mathf.Lerp(Gr_Intensity,0.0f, Overlifetime/duration);
			Overlifetime += Time.deltaTime;
		}
		
	}
}