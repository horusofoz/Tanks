using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuitApplication : MonoBehaviour {

	public void OnQuitButton()
	{
		#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
		Debug.Log (this.name + " : " + this.GetType () + " : " + System.Reflection.MethodBase.GetCurrentMethod ().Name); 
		#endif
		#if (UNITY_EDITOR)
		UnityEditor.EditorApplication.isPlaying = false;
		#elif (UNITY_STANDALONE) 
		Application.Quit();
		#elif (UNITY_WEBGL)
		Application.OpenURL("about:blank");
		#endif
	}
}
