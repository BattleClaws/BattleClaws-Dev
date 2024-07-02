using UnityEngine;
using UnityEngine.EventSystems;

public class AnimateOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float raiseAmount = 10f;  // Amount to raise the UI element
    public float animationSpeed = 10f; // suitable Speed of the animation

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isHovered = false;

    void Start()
    {
        // Store the original position of the UI element
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    void Update()
    {
        // Smoothly move the UI element towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the target position to the raised position
        targetPosition = new Vector3(originalPosition.x, originalPosition.y + raiseAmount, originalPosition.z);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Set the target position back to the original position
        targetPosition = originalPosition;
        isHovered = false;
    }
}
