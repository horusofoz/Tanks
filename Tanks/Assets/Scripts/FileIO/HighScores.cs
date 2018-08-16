using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class HighScores : MonoBehaviour {

    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    private void Start()
    {
        // We need to know where we're reading from and writing to.
        // To help us with that, we'll print the current directory to the console.
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);

        // Load the scores by default.
        LoadScoresFromFile();
    }

    private void Update()
    {
        ManuallyLoadSaveHighScores();
    }

    public void LoadScoresFromFile()
    {
#if UNITY_STANDALONE
        // Before we try to read a file, we should check that it exists. If it doesn't exist, we'll log a mesage and abort.
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("the file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
        }

        // Make a  new array of default values. This ensures that no old values stick around if we've loaded a scores file in the past.
        scores = new int[scores.Length];

        // Now we read the file in. We do this using a "StreamReader" which we give our fill file path to. 
        // Don't forget the directory seperatior between the directory and the filename!

        StreamReader fileReader;

        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }

        // A counter to make sure we don't go past the end of our scores
        int scoreCount = 0;

        // A while loop, which runs as long as there is data to be read AND we haven't reached the end of our scores array.
        while(fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();

            // Try to parse that variable into an int.
            // First make a variable to put it in.
            int readScore = -1;
            // Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if(didParse == true)
            {
                scores[scoreCount] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then we probably had junk in our file.
                // Let's print an error, and then use a default value
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);
                scores[scoreCount] = 0;
            }
            // Don't forget to increment the counter!
            scoreCount++;
        }
        // Make sure to close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
#endif
    }

    public void SaveScoresToFile()
    {
#if UNITY_STANDALONE
        // Create a StreamWriter for our file path.

        StreamWriter fileWriter;

        try
        {
            fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

            // Write the lines to the file
            for (int i = 0; i < scores.Length; i++)
            {
                fileWriter.WriteLine(scores[i]);
                Debug.Log(i + ": " + scores[i]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }       

        

        fileWriter.Close();

        // Write a log message.
        Debug.Log("High scores written to " + scoreFileName);
#endif
    }

    public void AddScore(int newScore)
    {
        // First up we find out what index it belongs at.
        // This will be the first index with a score lower than the new score
        int desiredIndex = -1;
        for(int i = 0; i < scores.Length; i++)
        {
            // Instead of checking the value of desiredIndex we could use a 'break' to stop the loop
            if(scores[i] > newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        // If no desired index was found then the score isn't high enough to get on the table, so we just abort
        if(desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);
            return;
        }

        // Then we move all of the scores after that index back by one position.
        // We'll so this by looping from the back of the array to our desired index.
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        // Insert our new score in its place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into high scores at position " + desiredIndex, this);
    }

    private void ManuallyLoadSaveHighScores()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            SaveScoresToFile();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            LoadScoresFromFile();
            Debug.Log("High Scores Loaded...");
            for (int i = 0; i < scores.Length; i++)
            {
                Debug.Log((i + 1) + ": " + scores[i]);
            }
        }
    }
}
