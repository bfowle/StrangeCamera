using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    private float accum = 0;
    private int frames  = 0;
    private string sFPS = "";

    private Color greenColor = new Color(0.72f, 1f, 0);
    private Color yellowColor = new Color(0.97f, 1f, 0);
    private Color redColor = new Color(1f, 0.18f, 0);

    public bool updateColor = true;
    public float frequency = 0.5f;
    public int numDecimal = 2;
	
	private string labelText;
	private GUIStyle labelStyle;

    void Start() {
		labelStyle = new GUIStyle();
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 14;
			
        StartCoroutine(FPS());
    }

    void Update() {
        accum += Time.timeScale/ Time.deltaTime;
        ++frames;
    }
	
	void OnGUI() {
		GUI.Label(new Rect(10, Screen.height - 25, 100, 50), labelText, labelStyle);
	}

    private IEnumerator FPS() {
        while (true) {
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(numDecimal, 0, 10));
            accum = 0;
            frames = 0;

            if (sFPS != "NaN") {
                labelStyle.normal.textColor = (fps >= 30) ? greenColor : ((fps > 10) ? yellowColor : redColor);
                labelText = sFPS + " fps";
            }

            yield return new WaitForSeconds(frequency);
        }
    }

}
