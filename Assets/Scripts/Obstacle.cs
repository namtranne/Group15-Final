using UnityEngine;
public enum Lane {
    Left,
    Middle,
    Right
}
public abstract class Obstacle
{
    public GameObject obstacleTiles;
    /// <summary>
    /// Middle of one lane (left and right)
    /// </summary>
    protected const float MidLane = 6.63f;
    public abstract Vector3 Position(Lane lane);
}
public class Cone : Obstacle
{
    public override Vector3 Position(Lane lane)
    {
        switch (lane)
        {
            case Lane.Left:
                return obstacleTiles.transform.position = new Vector3(-MidLane, 1.6f, 0);
            case Lane.Middle:
                return obstacleTiles.transform.position = new Vector3(0, 1.6f, 0);
            case Lane.Right:
                return obstacleTiles.transform.position = new Vector3(MidLane, 1.6f, 0);
            default:
                return new Vector3(0,0,0);
        }
    }
}
public class Wall : Obstacle
{
    public override Vector3 Position(Lane lane)
    {
        return new Vector3(-8.0f, 0, 0);
    }
}
public class FenceLow: Obstacle
{
    public override Vector3 Position(Lane lane=0)
    {
        switch (lane)
        {
            case Lane.Left:
                return obstacleTiles.transform.position = new Vector3(-MidLane, 0.74f, 0);
            case Lane.Middle:
                return obstacleTiles.transform.position = new Vector3(0, 0.74f, 0);
            case Lane.Right:
                return obstacleTiles.transform.position = new Vector3(MidLane, 0.74f, 0);
            default:
                return new Vector3(0,0,0);
        }
    }
}
public class TrainZ : Obstacle
{
    public override Vector3 Position(Lane lane)
    {
        switch (lane)
        {
            case Lane.Left:
                return obstacleTiles.transform.position = new Vector3(-MidLane, 0.74f, 0);
            case Lane.Middle:
                return obstacleTiles.transform.position = new Vector3(0, 0.74f, 0);
            case Lane.Right:
                return obstacleTiles.transform.position = new Vector3(MidLane, 0.74f, 0);
            default: 
                return new Vector3(0,0,0);
        }
    }
}
public class TrainX : Obstacle
{
    public override Vector3 Position(Lane lane)
    {
        return new Vector3(0, 1.6f, 0);
    }
}
public class Coin
{
    private GameObject coinObject;
    protected const float MidLane = 6.63f;
    /// <summary>
    /// Get and set the position before the center of the tile
    /// </summary>
    /// <param name="lane"></param>
    /// <returns></returns>
    public Vector3 PositionBefore(Lane lane)
    {
        switch (lane)
        {
            case Lane.Left:
                return coinObject.transform.position = new Vector3(-MidLane,0.5f, -11.7f);
            case Lane.Middle:
                return coinObject.transform.position = new Vector3(0, 0.5f, -11.7f);
            case Lane.Right:
                return coinObject.transform.position = new Vector3(MidLane, 0.5f, - 11.7f);
            default:
                return new Vector3(0,0,0);
        }
    }
    /// <summary>
    /// Get and set the position before the after of the tile
    /// </summary>
    /// <param name="lane"></param>
    /// <returns></returns>
    public Vector3 PositionAfter(Lane lane)
    {
        switch (lane)
        {
            case Lane.Left:
                return coinObject.transform.position = new Vector3(-MidLane, 0.5f, 11.7f);
            case Lane.Middle:
                return coinObject.transform.position = new Vector3(0, 0.5f, 11.7f);
            case Lane.Right:
                return coinObject.transform.position = new Vector3(MidLane, 0.5f, 11.7f);
            default:
                return new Vector3(0, 0, 0);
        }
    }
    public Coin(GameObject coin)
    {
        coinObject= coin;
    }
}