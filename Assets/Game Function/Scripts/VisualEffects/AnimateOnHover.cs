using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimateOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
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

    private void Focus()
    {
        targetPosition = new Vector3(originalPosition.x, originalPosition.y + raiseAmount, originalPosition.z);
        isHovered = true;
    }

    private void Unfocus()
    {
        targetPosition = originalPosition;
        isHovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the target position to the raised position
        Focus();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        Focus();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        Unfocus();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Set the target position back to the original position
        Unfocus();
    }
}
