using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    public List<Vector3> letterPositions = new List<Vector3>();
    public List<int> currentIndexes = new List<int>();
    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> positions = new List<Vector3>();
    public TextPreview textPreview;
    public GameObject lineParticle;
    public List<GameObject> letterPrefaps;

    private LineRenderer lineRenderer;
    private Vector3 mousePoint;
    public static LineDrawer instance;
    public Pan pan;

    private bool isDragging;
    private float RADIUS = 1.0f;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lineParticle.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "MyLineRenderer";
        //pan = FindObjectOfType<Pan>();
    }

    private void Update()
    {
        if (DialogController.instance.IsDialogShowing() || MainController.instance.IsLevelClear) return;
        //if (SocialRegion.instance.isShowing) return;
        if (Input.GetMouseButtonDown(0))
        {
            textPreview.ClearText();
        }

        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            //textPreview.SetActive(true);

            mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 90;

            //Line Particle
            if (currentIndexes.Count != 0)
            {
                lineParticle.SetActive(true);
                lineParticle.transform.position = new Vector3(mousePoint.x, mousePoint.y, lineParticle.transform.position.z);
            }
            //

            int nearest = GetNearestPosition(mousePoint, letterPositions);

            Vector3 letterPosition = letterPositions[nearest];

            if (Vector3.Distance(letterPosition, mousePoint) < RADIUS)
            {
                pan.ScaleWord(letterPosition);
                if (currentIndexes.Count >= 2 && currentIndexes[currentIndexes.Count - 2] == nearest)
                {
                    currentIndexes.RemoveAt(currentIndexes.Count - 1);
                    textPreview.SetIndexes(currentIndexes);
                }
                else if (!currentIndexes.Contains(nearest))
                {
                    currentIndexes.Add(nearest);
                    textPreview.SetIndexes(currentIndexes);
                }
                //if(currentIndexes!=null)
                //    pan.SendMessageUpwards("ScaleWord", currentIndexes[nearest]);

            }
            BuildPoints();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (textPreview.GetText() != "")
            {
                isDragging = false;
                currentIndexes.Clear();
                lineRenderer.positionCount = 0;
                lineParticle.SetActive(false);

                WordRegion.instance.CheckAnswer(textPreview.GetText());
                pan.ResetScaleWord();
            }
        }

        if (points.Count >= 2 && isDragging)
        {
            positions = iTween.GetSmoothPoints(points.ToArray(), 8);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }

    private int GetNearestPosition(Vector3 point, List<Vector3> letters)
    {
        float min = float.MaxValue;
        int index = -1;
        for (int i = 0; i < letters.Count; i++)
        {
            float distant = Vector3.Distance(point, letters[i]);
            if (distant < min)
            {
                min = distant;
                index = i;
            }
        }
        return index;
    }

    private void BuildPoints()
    {
        points.Clear();
        foreach (var i in currentIndexes) points.Add(letterPositions[i]);

        if (currentIndexes.Count == 1 || points.Count >= 1 && Vector3.Distance(mousePoint, points[points.Count - 1]) >= RADIUS)
        {
            points.Add(mousePoint);
        }
    }


}
