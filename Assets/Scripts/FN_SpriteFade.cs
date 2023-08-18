using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FN_SpriteFade : MonoBehaviour
{
    SpriteRenderer sprite;
    public enum State
    {
        Faded,
        Unfaded,
    };

    public State state { get; private set; }
    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = State.Unfaded;
    }
    public void Subscribe()
    {
        GM.playerMove.OnStep += CheckFade;
    }

    public void Unsubscribe()
    {

        GM.playerMove.OnStep -= CheckFade;
    }
    private void CheckFade()
    {

        Vector2Int pivot = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

        List<STR_Walls> cnt = SO_PlayerWalls.FindContinuousWall((Vector3Int)(pivot));

        if (GM.playerMove.pivotPosition == pivot + new Vector2Int(0, 1))
        {
            Fade();

            foreach (STR_Walls wall in cnt)
            {
                wall.wall.GetComponent<FN_SpriteFade>().Fade();
            }
        }
        else
        {
            foreach(STR_Walls wall in cnt)
            {
                Vector2Int curr = new Vector2Int(Mathf.FloorToInt(wall.wall.transform.position.x), Mathf.FloorToInt(wall.wall.transform.position.y));
                if (GM.playerMove.pivotPosition == curr + new Vector2Int(0, 1))
                    return;
            }
            Unfade();

            foreach (STR_Walls wall in cnt)
            {
                wall.wall.GetComponent<FN_SpriteFade>().Unfade();
            }
        }
    }
    void Fade()
    {
        if (state == State.Faded) return;

        foreach(Transform t in GetComponentsInChildren<Transform>(includeInactive:true))
        {
            if (t.GetComponent<SpriteRenderer>() == null) continue;
            t.GetComponent<SpriteRenderer>().color = (Color)(new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.30f));
        }
        sprite.color = (Color)(new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.30f));
        state = State.Faded;
        
    }
    void Unfade()
    {
        if (state == State.Unfaded) return; 

        foreach(Transform t in GetComponentsInChildren<Transform>(includeInactive: true))
        {
            if (t.GetComponent<SpriteRenderer>() == null) continue;

            t.GetComponent<SpriteRenderer>().color = (Color)(new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 1.00f));
        }

        sprite.color = (Color)(new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 1.00f));
        state = State.Unfaded;
    }
}
