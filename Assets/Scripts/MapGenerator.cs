using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] int numOfStops;
    [SerializeField] float xPosOffset = 1.5f;
    [SerializeField] float yPosOffset = 2f;
    [Space]
    [SerializeField] Stop stopPrefab;
    public Stop firstStop;
    [SerializeField] List<Stop> stops;
    [SerializeField] Transform treeParent;

    public int rowIndex = 0;
    public Stop lastToMakeNew;
    [Space]
    [SerializeField] List<GameObject> lifts;
    void Start()
    {
        
    }

    public void StartNewMap() {
        GenerateMap(0, numOfStops);
        StartCoroutine(SpawnLifts());
    }

    void GenerateMap(int from, int to) {
        if (from == 0) {
            Stop currStop = firstStop;
            stops.Add(currStop);
        }
        for (int i = from; i < to; i++) {
            stops[i].index = i;
            Stop right = null;
            Vector3 pos = stops[i].transform.position;
            Vector3 leftPos = new(pos.x - xPosOffset, pos.y + yPosOffset); 
            Vector3 rightPos = new(pos.x  + xPosOffset, pos.y + yPosOffset);
            if (!stops[i].leftStop) {
                Stop left = Instantiate(stopPrefab, leftPos, Quaternion.identity, treeParent);
                stops.Add(left);
                stops[i].leftStop = left;
                left.parents.Add(stops[i]);
            }
            if (!stops[i].rightStop) {
                right = Instantiate(stopPrefab, rightPos, Quaternion.identity, treeParent);
                stops.Add(right);
                stops[i].rightStop = right;
                right.parents.Add(stops[i]);
            }
            stops[i].myRow = rowIndex;
            if(i > 0) {
                stops[i].Setup();
            }
            // jesli maja wspolnego rodzica
            if (stops[i + 1].IsParent(stops[i].parents)) {
                stops[i + 1].leftStop = right;
                right.parents.Add(stops[i + 1]);
            } else {
                //nie ma wspolnego rodzica wiec idziemy rz¹d do góry
                rowIndex++;
            }
        }
        //wywal nadprogramowe dzieci ostatniego rzedu
       if (from == 0) {
            for (int i = stops.Count - 1; i > 0; i--) {
                if (stops[i].myRow != rowIndex - 1) {
                    if (stops[i].parents[0]) {
                        lastToMakeNew = stops[i].parents[0];
                    }
                    Destroy(stops[i].gameObject);
                    stops.Remove(stops[i]);
                } else if (stops[i].myRow == rowIndex - 1) break;
            }
            rowIndex--;
        } else {
            lastToMakeNew = lastToMakeNew.leftStop;
        }
        CleanStopsList();
    }

    public void MakeAnotherRow() {
        DeleteStops();
        //depends how many first stops are there
        int from = stops.IndexOf(Controller.Instance.CurrentPlayerStop.leftStop.leftStop.leftStop);
        //stops.Count bo przelatuje caly najwyzszy rzad
        int to = stops.Count;
        GenerateMap(from, to);
        CheckForDoubleYeti();
    }

    void CheckForDoubleYeti() {
        List<Stop> lastLine = stops.FindAll(x => x.myRow == rowIndex-1);
        for (int i = 0; i < lastLine.Count-1; i++) {
            if (lastLine[i].myType == Stop.StopType.yeti) {
                if (lastLine[i + 1].myType == Stop.StopType.yeti) {
                    lastLine[i + 1].ChangeFromYeti();
                }
            }
        }
    }
    
    public void CleanMap() {
        List<Transform> stopsToDelete = new List<Transform>();
        foreach (Transform child in treeParent) {
            stopsToDelete.Add(child);
        }
        for (int i = 1; i < stopsToDelete.Count; i++) {
            Destroy(stopsToDelete[i].gameObject);
        }
        stops.Clear();
        rowIndex = 0;
    }
    

    public void CleanStopsList() {
        stops.RemoveAll(x => x == null);
    }

    public void DeleteStops() {
        CleanStopsList();
        Stop playerStop = Controller.Instance.CurrentPlayerStop;
        Stop left = stops.Find(x => (x.myRow == playerStop.myRow) && (x.index == playerStop.index -1));
        Stop right = stops.Find(x => (x.myRow == playerStop.myRow) && (x.index == playerStop.index +1));
        if (left) {
            DeleteFromList(left.leftStop.leftStop.leftStop);
            DeleteFromList(left.leftStop.leftStop);
            DeleteFromList(left.leftStop);
            DeleteFromList(left);
        } 

        if (right) {
            DeleteFromList(right.rightStop.rightStop.rightStop);
            DeleteFromList(right.rightStop.rightStop);
            DeleteFromList(right.rightStop);
            DeleteFromList(right);
        }
        lastToMakeNew = playerStop.leftStop.leftStop.leftStop;
    }

    void DeleteFromList(Stop stop) {
        if(stop != null) stops.Remove(stop);
        stop.SlowlyDelete();
    }

    public void AddChildrenToList(List<int> indexes, Stop stop) {
        if (stop.leftStop) {
            if(!indexes.Contains(stop.leftStop.index)) indexes.Add(stop.leftStop.index);
        }
        if (stop.rightStop) {
            if (!indexes.Contains(stop.rightStop.index)) indexes.Add(stop.rightStop.index);
        }
    }

    IEnumerator SpawnLifts() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(3, 5));
            int num = Random.Range(0, 2);
            float yPos = Controller.Instance.player.transform.position.y - 15;
            float xPos = Controller.Instance.player.transform.position.x;
            if (Controller.Instance.CanMove) {
                GameObject a = Instantiate(lifts[num]);
                a.transform.position = new Vector3(xPos, yPos, 0);
            }
         }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //MakeAnotherRow();
        }
    }
}
