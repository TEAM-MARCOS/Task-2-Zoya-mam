using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public LineRenderer lineRenderer;
    public Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        SpawnCircles(Random.Range(5, 11));
    }

    private void SpawnCircles(int count)
    {
        Vector2 minScreenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float circleRadius = circlePrefab.GetComponent<CircleCollider2D>().radius;

        for (int i = 0; i < count; i++)
        {
            Vector2 position = new Vector2(Random.Range(minScreenBounds.x + circleRadius, maxScreenBounds.x - circleRadius), Random.Range(minScreenBounds.y + circleRadius, maxScreenBounds.y - circleRadius));
            Instantiate(circlePrefab, position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateLine(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CheckIntersection();
            lineRenderer.positionCount = 0; // Clear the line renderer
        }
    }

    private void UpdateLine(Vector2 newPosition)
    {
        if (lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, newPosition);
        }
        else
        {
            int currentPosition = lineRenderer.positionCount;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(currentPosition, newPosition);
        }
    }

    private void CheckIntersection()
    {
        foreach (GameObject circleObject in GameObject.FindGameObjectsWithTag("Circle"))
        {
            CircleCollider2D circleCollider = circleObject.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    if (circleCollider.OverlapPoint(lineRenderer.GetPosition(i)))
                    {
                        Destroy(circleObject);
                        break; 
                    }
                }
            }
        }
    }

    public void RestartGame()
    {
        foreach (GameObject circle in GameObject.FindGameObjectsWithTag("Circle"))
        {
            Destroy(circle);
        }
        SpawnCircles(Random.Range(5, 11));
    }
}
