using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollRect : ScrollRect
{
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        for(int i = 0; i < m_parentInitializePotentialDragHandlers.Length; ++i)
        {
            m_parentInitializePotentialDragHandlers[i].OnInitializePotentialDrag(eventData);
        }
        base.OnInitializePotentialDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        m_routeToParent = (!horizontal && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) || (!vertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y));

        if (m_routeToParent)
        {
            for(int i = 0; i < m_parentBeginDragHandlers.Length; ++i)
            {
                m_parentBeginDragHandlers[i].OnBeginDrag(eventData);
            }
        }
        else
        {
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(m_routeToParent)
        {
            for(int i = 0; i < m_parentDragHandlers.Length; ++i)
            {
                m_parentDragHandlers[i].OnDrag(eventData);
            }
        }
        else
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_routeToParent)
        {
            for(int i = 0; i < m_parentEndDragHandlers.Length; ++i)
            {
                m_parentEndDragHandlers[i].OnEndDrag(eventData);
            }
        }
        else
        {
            base.OnEndDrag(eventData);
        }

        m_routeToParent = false;
    } 
    
    protected override void Awake() 
    {
        base.Awake();
    }

    public void InitParentScrollRect()
    {
        if(m_parentScrollRect == null)
        {
            throw new System.NullReferenceException("Parent Scroll Rect is null");
        }

        m_parentInitializePotentialDragHandlers = m_parentScrollRect.GetComponents<IInitializePotentialDragHandler>();
        m_parentBeginDragHandlers = m_parentScrollRect.GetComponents<IBeginDragHandler>();
        m_parentDragHandlers = m_parentScrollRect.GetComponents<IDragHandler>();
        m_parentEndDragHandlers = m_parentScrollRect.GetComponents<IEndDragHandler>();
    }
    
    public ScrollRect m_parentScrollRect = null;

    private bool m_routeToParent = false;

    private IInitializePotentialDragHandler[] m_parentInitializePotentialDragHandlers = null;
    private IBeginDragHandler[] m_parentBeginDragHandlers = null;
    private IDragHandler[] m_parentDragHandlers = null;
    private IEndDragHandler[] m_parentEndDragHandlers = null;
}
