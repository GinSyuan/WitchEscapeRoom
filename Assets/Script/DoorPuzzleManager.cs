using UnityEngine;

public class DoorPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PuzzleSocket[] allSockets;
    public GameObject doorToHide;

    private bool isSolved = false;

    public void CheckIfPuzzleSolved()
    {
        if (isSolved) return;

        int correctCount = 0;

        // Count how many sockets currently have the right item
        foreach (PuzzleSocket socket in allSockets)
        {
            if (socket.isCorrectItemPlaced)
            {
                correctCount++;
            }
        }

        // Print the score to the Console!
        UnityEngine.Debug.Log("Puzzle Check: " + correctCount + " out of " + allSockets.Length + " are correct.");

        // If the score matches the total number of sockets, YOU WIN!
        if (correctCount == allSockets.Length)
        {
            UnityEngine.Debug.Log("Puzzle Solved! The door is opening.");
            isSolved = true;

            if (doorToHide != null)
            {
                doorToHide.SetActive(false);
            }
            else
            {
                UnityEngine.Debug.LogError("The puzzle is solved, but you forgot to assign the Door in the Inspector!");
            }
        }
    }
}