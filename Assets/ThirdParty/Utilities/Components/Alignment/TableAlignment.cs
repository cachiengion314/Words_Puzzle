using System.Collections.Generic;
/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using UnityEngine;

namespace Utilities.Components
{
    public enum TableLayoutType
    {
        HORIZONTAL,
        VERTICAL
    }

    public class TableAlignment : MyAlignment
    {
        public TableLayoutType layoutType;
        public HorizontalType horizontalType;
        public VerticalType verticalType;

        public int maxColumn;
        public int maxRow;

        public float columnDistance;
        public float rowDistance;

        public float xOffset;
        public float yOffset;

        private Transform[] _children;
        private int _countRow;
        private int _countColumn;

        private List<MyAlignment> _groupAligners = new List<MyAlignment>();
        private MyAlignment _aligner = new MyAlignment();

        private List<GameObject> _pool = new List<GameObject>();

        public override void Initialize()
        {
            ClearAll();

            _children = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                _children[i] = transform.GetChild(i);
            }

            if (layoutType == TableLayoutType.HORIZONTAL)
            {
                if (maxColumn <= 0)
                {
                    Debug.LogError("maxColumn must be greater than 0");
                    return;
                }

                _countRow = Mathf.CeilToInt((float)_children.Length / (float)maxColumn);
                VerticalAlignment aligner = gameObject.AddComponent<VerticalAlignment>();
                aligner.rowDistance = rowDistance;
                aligner.verticalType = verticalType;

                _aligner = aligner;

                for (int i = 0; i < _countRow; i++)
                {
                    GameObject rootOfRow = GetObjectFromPool();
                    if (rootOfRow == null)
                    {
                        rootOfRow = new GameObject();
                        _pool.Add(rootOfRow);
                    }
                    rootOfRow.transform.parent = transform;
                    rootOfRow.name = "Group_" + i;
                    rootOfRow.SetActive(true);

                    HorizontalAlignment alignment = rootOfRow.AddComponent<HorizontalAlignment>();
                    alignment.cellDistance = columnDistance;
                    alignment.horizontalType = horizontalType;
                    alignment.yOffset = yOffset;

                    _groupAligners.Add(alignment);
                }
            }

            if (layoutType == TableLayoutType.VERTICAL)
            {
                if (maxRow <= 0)
                {
                    Debug.LogError("maxRow must be greater than 0");
                    return;
                }

                _countColumn = Mathf.CeilToInt((float)_children.Length / (float)maxRow);
                HorizontalAlignment aligner = gameObject.AddComponent<HorizontalAlignment>();
                aligner.cellDistance = columnDistance;
                aligner.horizontalType = horizontalType;

                _aligner = aligner;

                for (int i = 0; i < _countColumn; i++)
                {
                    GameObject rootOfCol = GetObjectFromPool();
                    if (rootOfCol == null)
                    {
                        rootOfCol = new GameObject();
                        _pool.Add(rootOfCol);
                    }
                    rootOfCol.transform.parent = transform;
                    rootOfCol.name = "Group_" + i;
                    rootOfCol.SetActive(true);

                    VerticalAlignment alignment = rootOfCol.AddComponent<VerticalAlignment>();
                    alignment.rowDistance = rowDistance;
                    alignment.verticalType = verticalType;
                    alignment.xOffset = xOffset;

                    _groupAligners.Add(alignment);
                }
            }

            if (layoutType == TableLayoutType.HORIZONTAL)
            {
                int j = 0;
                bool stop = false;
                while (j < _children.Length)
                {
                    for (int k = 0; k < _groupAligners.Count; k++)
                    {
                        for (int m = 0; m < maxColumn; m++)
                        {
                            _children[j].parent = _groupAligners[k].transform;
                            j++;

                            if (j >= _children.Length)
                            {
                                stop = true;
                                break;
                            }
                        }

                        if (stop)
                            break;
                    }
                }
            }
            else if (layoutType == TableLayoutType.VERTICAL)
            {
                int j = 0;
                bool stop = false;
                while (j < _children.Length)
                {
                    for (int k = 0; k < _groupAligners.Count; k++)
                    {
                        for (int m = 0; m < maxRow; m++)
                        {
                            _children[j].parent = _groupAligners[k].transform;
                            j++;

                            if (j >= _children.Length)
                            {
                                stop = true;
                                break;
                            }
                        }

                        if (stop)
                            break;
                    }
                }
            }
        }

        public override void Align()
        {
            _aligner.Align();

            for (int i = 0; i < _groupAligners.Count; i++)
            {
                _groupAligners[i].Align();
            }
        }

        private GameObject GetObjectFromPool()
        {
            if (_pool.Count > 0)
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    if (!_pool[i].activeSelf)
                        return _pool[i];
                }
            }

            return null;
        }

        public void ClearAll()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                while (_pool[i].transform.childCount > 0)
                {
                    _pool[i].transform.GetChild(0).parent = transform;
                }

                _pool[i].SetActive(false);
                _pool[i].transform.parent = null;
            }

            while (_groupAligners.Count > 0)
            {
                Destroy(_groupAligners[0]);
                _groupAligners.RemoveAt(0);
            }

            Destroy(_aligner);
        }
    }
}