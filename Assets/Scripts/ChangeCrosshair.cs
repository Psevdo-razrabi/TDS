using UnityEngine;

public class ChangeCrosshair : MonoBehaviour
{
    [SerializeField] private Crosshair _crosshair;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform[] crosshairParts;
    [SerializeField] private float _forceChanges;
    [SerializeField] private float maxExpandDistance = 100.0f;
    private Vector2[] initialPositions;
    private float expandMultiplier;
    private float _additionalCoeficent;

    public float AdditionalCoeficent
    {
        get { return _additionalCoeficent; }
        set { _additionalCoeficent = value; }
    }
    void Start()
    {
        initialPositions = new Vector2[crosshairParts.Length];
        for (int i = 0; i < crosshairParts.Length; i++)
        {
            initialPositions[i] = crosshairParts[i].anchoredPosition;
        }
    }

    void Update()
    {
        Vector2 playerScreenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, player.position);
        Vector2 crosshairScreenPosition = _crosshair.CrossHair.position;
        float distance = Vector2.Distance(playerScreenPosition, crosshairScreenPosition);
        expandMultiplier = Mathf.Clamp(distance / maxExpandDistance, 0, 1);

        ChangeSize();
    }

    public void ChangeSize()
    {
        for (int i = 0; i < crosshairParts.Length; i++)
        {
            Vector2 direction = initialPositions[i].normalized;
            
            crosshairParts[i].anchoredPosition = initialPositions[i] + direction * expandMultiplier * _forceChanges * _additionalCoeficent;
        }

        _additionalCoeficent = 1;
    }
}
