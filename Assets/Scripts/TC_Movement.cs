using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TC_Movement : BC_TouchControls
{
    public S_HotBar hotBar;
    public GameObject queueMarker;
    public AStar_Pathfinding astar;
    readonly Queue<IEnumerator> commands = new Queue<IEnumerator>();
    Queue<GameObject> queueMarkers = new Queue<GameObject>();
    public Coroutine currentCommand;

    public int queueCount;
    public void Initialize()
    {
        if (GM.playerMove.tC_Movement == null)
            GM.playerMove.tC_Movement = this;
    }
    public void ClearCommands()
    {
        if(Camera.main.GetComponent<FN_FollowPlayer>().type == FN_FollowPlayer.Type.Object)
        {
            Camera.main.GetComponent<FN_FollowPlayer>().PlayerFollow(GM.playerObj.transform);
        }
        if (currentCommand != null)
        {
            StopCoroutine(currentCommand);
            BC_Items.stcRunning = false;
            currentCommand = null;
        }
        if (commands != null)
        {
            ClearQueue();
        }
    }

    void SpawnMarker(Vector2Int block)
    {
        GameObject g = Instantiate(queueMarker);
        g.transform.position = Positions.BlockToTransformCenter(block);
        queueMarkers.Enqueue(g);

    }
    void SpawnMarker(Vector3 pos,Vector3 scale)
    {
        GameObject g = Instantiate(queueMarker);
        g.transform.position = pos;
        g.transform.localScale = scale;
        queueMarkers.Enqueue(g);

    }
    void RemoveMarker()
    {
        print(queueMarkers.Count + " & " + commands.Count);
        if((queueMarkers.Count>0)&&(queueMarkers.Count-1==commands.Count))
            Destroy(queueMarkers.Dequeue());
    }

    internal override void SingleTouch()
    {
        mode = Mode.SingleTouch;

        Vector2Int targetBlock = Positions.TouchToBlockPos(firstTouch);


        if (GM.playerMove == null|| hotBar.activeSlot.item == null) return;

        //IF NO COROUTINE IS RUNNING && QUEUE COUNT IS ZERO, START THE FIRST ITEM IN THE QUEUE
        if (commands.Count == 0 && BC_Items.stcRunning == false)
        {
            Camera.main.GetComponent<FN_FollowPlayer>().ObjectFollow(GM.playerObj.transform);

            Enqueue(targetBlock);
            Dequeue();
        }
        else
        {
            Enqueue(targetBlock);
        }
    }
    internal void SingleTouchA()
    {
        mode = Mode.SingleTouch;

        Vector2Int targetBlock = Positions.TouchToBlockPos(firstTouch);


        if (GM.playerMove == null) return;

        if (hotBar.activeSlot.item == null) return;

        //IF NO COROUTINE IS RUNNING && QUEUE COUNT IS ZERO, START THE FIRST ITEM IN THE QUEUE
        if (commands.Count == 0 && BC_Items.stcRunning == false)
        {
            Camera.main.GetComponent<FN_FollowPlayer>().ObjectFollow(GM.playerObj.transform);

            commands.Enqueue(hotBar.activeSlot.item.SingleTouchCoroutine(targetBlock, hotBar.activeSlot, Try));
            currentCommand = StartCoroutine(commands.Dequeue());
        }
        else
        {
            commands.Enqueue(hotBar.activeSlot.item.SingleTouchCoroutine(targetBlock, hotBar.activeSlot, Try));
            SpawnMarker(targetBlock);
        }
    }

    void Enqueue(Vector2Int targetBlock)
    {
        commands.Enqueue(hotBar.activeSlot.item.SingleTouchCoroutine(targetBlock, hotBar.activeSlot, Try));

        if(hotBar.activeSlot.item.GetComponent<ITEM_Wallpaper>()!=null)
        {
            Vector2Int tile = Positions.BlockToTile(targetBlock);

            STR_Walls actualWall = ITEM_Wallpaper.FindNearestWall(tile);

            if (actualWall == null || actualWall.wallPaperIndex == hotBar.activeSlot.item.GetComponent<ITEM_Wallpaper>().wallset.index)
            {
                return;
            }

            Vector3 pos = actualWall.pos + new Vector3(0.50f, 1.00f, 0.00f);
            SpawnMarker(pos, Vector3.one * 0.15f);

        }
        if(hotBar.activeSlot.item.GetComponent<ITEM_Carpets>() != null)
        {
            Vector2Int tile = Positions.BlockToTile(targetBlock);


            STR_Floors actualFloor = FloorsManager.GetFloor(tile);

            //NO APPLIABLE CARPET
            if (actualFloor == null || actualFloor.carpet.index == hotBar.activeSlot.item.GetComponent<ITEM_Carpets>().carpet.index)
            {
                return;
            }

            Vector3 pos = Positions.TileToTransform(tile);
            SpawnMarker(pos, Vector3.one * 0.15f);
        }
    }

    void Dequeue()
    {
        currentCommand = StartCoroutine(commands.Dequeue());
    }

    void ClearQueue()
    {
        commands.Clear();
        foreach (GameObject g in queueMarkers)
        {
            Destroy(g);
        }
        queueMarkers.Clear();
    }

    private void Try()
    {
        if (commands.Count > 0 && BC_Items.stcRunning == false)
        {
            RemoveMarker();
            Dequeue();
        }
        if(commands.Count == 0&& BC_Items.stcRunning == false)
        {
            RemoveMarker();
        }
    }
}
