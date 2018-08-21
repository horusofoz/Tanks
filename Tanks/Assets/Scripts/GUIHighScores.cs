using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHighScores : MonoBehaviour {

	public Text _highscoresText = null;
	public HighScores _highscores = null;

	private bool _loaded = false;

	void Start()
	{
		if (_highscoresText.text == "SCORES NOT LOADED") 
		{
			_highscoresText.text = string.Empty;
		}
	}

	void Update () 
	{
		if (_loaded == false) 
		{
			_loaded = true;
			_highscores.LoadScoresFromFile ();

			int scorePlace = 0;

			foreach (int score in _highscores.scores) 
			{
				scorePlace++;
				//_highscoresText.text += scorePlace.ToString("D2") + ": " + score.ToString () + "\n";
				_highscoresText.text += string.Format("{0:D2} - {1:D2}:{2:D2}\n", scorePlace, (score/60), (score % 60));

			}
		}
	}
}
