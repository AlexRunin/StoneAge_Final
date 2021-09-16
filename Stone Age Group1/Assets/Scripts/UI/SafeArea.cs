
using UnityEngine;

[RequireComponent(typeof(RectTransform))] // Запрещяет удалять компонент или создает автоматически

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position; // Получем кординату SafeArea
        var anchorMax = anchorMin + safeArea.size;

        // Ищем центр экрана
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

    }
}
