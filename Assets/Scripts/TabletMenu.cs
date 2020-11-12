using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI[] stats;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Transform curriculums;
    [SerializeField] private float speed = 15;
    private int menuPos = 1;
    private bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveRight()
    {
        if (!moving)
        {
            menuPos -= 1;
            moving = true;
            StartCoroutine(MovementCoroutine(-6.8f));
        }

    }

    public void MoveLeft()
    {

        if (!moving)
        {
            menuPos += 1;
            moving = true;
            StartCoroutine(MovementCoroutine(6.8f));
        }

    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public void UpdateButtons()
    {
        if (menuPos == 2)
        {
            left.interactable = false;
        }
        if (menuPos == 1)
        {
            left.interactable = true;
            right.interactable = true;
        }
        if (menuPos == 0)
        {
            right.interactable = false;
        }
    }

    public IEnumerator MovementCoroutine(float xPos)
    {
        bool arrived = false;
        Vector3 newPos = new Vector3(curriculums.position.x + xPos, curriculums.position.y, 0);
        while (!arrived)
        {
            curriculums.position =  Vector3.MoveTowards(curriculums.position, newPos, speed * Time.deltaTime);
            if (Vector3.Distance(curriculums.position, newPos) == 0) 
                arrived = true;
            yield return null;
        }
        if (arrived)
        {
            moving = false;
        }
    }

    }
