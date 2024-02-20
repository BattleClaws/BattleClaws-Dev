using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreEntry : MonoBehaviour
{
    public TMP_Text textBox;
    public GameObject CurrentSelectionIcon;
    public float[] IconXPositions = new float[3];
    public int currentHoverPositionIndex;
    private string currentString = "___";
    private char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private int selectedCharacterIndex = 0;
    public float scrollSpeed = 0.5f;
    private bool canCycle = true;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput > 0.5f && canCycle)
        {
            StartCoroutine(CycleCharacterCoroutine(1));
        }
        else if (horizontalInput < -0.5f && canCycle)
        {
            StartCoroutine(CycleCharacterCoroutine(-1));
        }

        if (verticalInput > 0.5f && canCycle)
        {
            StartCoroutine(SwapLetterCoroutine(1));
        }
        else if (verticalInput < -0.5f && canCycle)
        {
            StartCoroutine(SwapLetterCoroutine(-1));
        }


       
    }
    System.Collections.IEnumerator CycleCharacterCoroutine(int direction)
    {
        canCycle = false;
        yield return new WaitForSeconds(scrollSpeed);

        // Save the previous selectedCharacterIndex for debugging
        int previousSelectedCharacterIndex = selectedCharacterIndex;

        selectedCharacterIndex += direction;

        // Ensure the index stays within bounds
        selectedCharacterIndex = (selectedCharacterIndex + currentString.Length) % currentString.Length;

        // Check if selectedCharacterIndex is within bounds of IconXPositions array
        float newXPosition = 0f;

        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < IconXPositions.Length)
        {
            newXPosition = IconXPositions[selectedCharacterIndex];
        }
        else
        {
            // Handle the out-of-bounds case, for example, by setting newXPosition to a default value.
            // You might also want to log an error to help with debugging.
            Debug.LogError("Index out of bounds: " + selectedCharacterIndex);
            newXPosition = 0f; // Set a default value or handle it according to your logic.
        }

        CurrentSelectionIcon.transform.localPosition = new Vector3(
            newXPosition,
            CurrentSelectionIcon.transform.localPosition.y,
            CurrentSelectionIcon.transform.localPosition.z);

        UpdateTextBox();


        yield return new WaitForSeconds(scrollSpeed);
        canCycle = true;
    }


    System.Collections.IEnumerator SwapLetterCoroutine(int direction)
    {
        canCycle = false;
        yield return new WaitForSeconds(scrollSpeed);

        characters[selectedCharacterIndex] += (char)direction;
        if (characters[selectedCharacterIndex] > 'Z')
        {
            characters[selectedCharacterIndex] = 'A';
        }
        else if (characters[selectedCharacterIndex] < 'A')
        {
            characters[selectedCharacterIndex] = 'Z';
        }

        UpdateTextBox();

        yield return new WaitForSeconds(scrollSpeed);
        canCycle = true;
    }

    void UpdateTextBox()
    {
        char[] updatedString = currentString.ToCharArray();
        updatedString[selectedCharacterIndex] = characters[selectedCharacterIndex];
        currentString = new string(updatedString);

        textBox.text = currentString;
    }


}