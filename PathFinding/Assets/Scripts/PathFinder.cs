using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder
{
    public static TilePath DiscoverPath(Tilemap map, Vector3Int start, Vector3Int end)
    {
        //you will return this path to the user.  It should be the shortest path between
        //the start and end vertices 
        TilePath discoveredPath = new TilePath();

        //TileFactory is how you get information on tiles that exist at a particular vector's
        //coordinates
        TileFactory tileFactory = TileFactory.GetInstance();

        //This is the priority queue of paths that you will use in your implementation of
        //Dijkstra's algorithm
        PriortyQueue<TilePath> pathQueue = new PriortyQueue<TilePath>();

        //You can slightly speed up your algorithm by remembering previously visited tiles.
        //This isn't strictly necessary.
        Dictionary<Vector3Int, int> discoveredTiles = new Dictionary<Vector3Int, int>();

        //quick sanity check
        if(map == null || start == null || end == null)
        {
            return discoveredPath;
        }

        //This is how you get tile information for a particular map location
        //This gets the Unity tile, which contains a coordinate (.Position)
        var startingMapLocation = map.GetTile(start);

        //And this converts the Unity tile into an object model that tracks the
        //cost to visit the tile.
        var startingTile = tileFactory.GetTile(startingMapLocation.name);
        startingTile.Position = start;

        //Any discovered path must start at the origin!
        discoveredPath.AddTileToPath(startingTile);

        //This adds the starting tile to the PQ and we start off from there...
        pathQueue.Enqueue(discoveredPath);
        bool found = false;

        int count = -1;

        while(found == false && pathQueue.IsEmpty() == false)
        {
            //TODO: Implement Dijkstra's algorithm!

            //pop item off priority queue
            TilePath newPath = pathQueue.Dequeue();
            //Tile nextTile = newPath.GetMostRecentTile();

            //if item contains the final tile in the path, you are done
            if (newPath.GetMostRecentTile().Position == end)
            {
                discoveredPath = newPath;
                found = true;
            }
            else
            {
                Tile nextTile = newPath.GetMostRecentTile();
                //discoveredTiles.Add(new Vector3Int(nextTile.Position.x, nextTile.Position.y, nextTile.Position.z), tileFactory.GetTile(nextTile.name));
                List<Tile> neighborTiles = findNeighbors(map, nextTile, tileFactory);
                //the findneighbors helper funciton only gets the above, the other three are the same

                //for each of the neighbors(4 since using squares)
                for (int i = 0; i < neighborTiles.Count; i++)
                {
                    //create a new path with the additional tile
                    TilePath anotherPath = new TilePath(newPath);
                    anotherPath.AddTileToPath(neighborTiles[i]);

                    //for(int j = 0; j < discoveredTiles.Count; j++)
                    //{
                    //    if(neighborTiles[i].Position != discoveredTiles[j] && anotherPath.GetMostRecentTile().Position != end)
                    //    {
                    //        pathQueue.Enqueue(anotherPath);
                    //    }
                    //    else
                    //    {
                    //        discoveredPath = anotherPath;
                    //        found = true;
                    //        break;
                    //    }

                    //}


                    //if that path contains the final tile, done
                    if (anotherPath.GetMostRecentTile().Position == end)
                    {
                        discoveredPath = anotherPath;
                        found = true;
                        break;
                    }
                    else
                    {
                        //add that back into priority Queue
                        pathQueue.Enqueue(anotherPath);
                    }
                }
            }
            // else; for each of the neighbors(4 since using squares)
                //create a new path with the additional tile
                //if that path contains the final tile, done
                //else add that back into priority Queue
            //return the path discovered back to caller

            //This line ensures that we don't get an infinite loop in Unity.
            //You will need to remove it in order for your pathfinding algorithm to work.
            //found = true;
            //if(pathQueue.Count >10000)
            //{
            //    break;
            //}
        }
        return discoveredPath;
    }

    static List<Tile> findNeighbors(Tilemap map, Tile tile, TileFactory tileFactory)
    {
        List<Tile> neighbor = new List<Tile>();

        TileBase above = map.GetTile(new Vector3Int(tile.Position.x, tile.Position.y + 1, tile.Position.z));
        if(above != null)
        {
            Tile aboveTile = tileFactory.GetTile(above.name);
            aboveTile.Position = new Vector3Int(tile.Position.x, tile.Position.y + 1, tile.Position.z);
            if (aboveTile != null)
            {
                neighbor.Add(new Tile(aboveTile));
            }
        }
       
        TileBase below = map.GetTile(new Vector3Int(tile.Position.x, tile.Position.y - 1, tile.Position.z));
        if (below != null)
        {
            Tile belowTile = tileFactory.GetTile(below.name);
            belowTile.Position = new Vector3Int(tile.Position.x, tile.Position.y - 1, tile.Position.z);
            if (belowTile != null)
            {
                neighbor.Add(new Tile(belowTile));
            }
        }

        TileBase right = map.GetTile(new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z));
        if (right != null)
        {
            Tile rightTile = tileFactory.GetTile(right.name);
            rightTile.Position = new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z);
            if (rightTile != null)
            {
                neighbor.Add(new Tile(rightTile));
            }
        }
       

        TileBase left = map.GetTile(new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z));
        if (left != null)
        {
            Tile leftTile = tileFactory.GetTile(left.name);
            leftTile.Position = new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z);
            if (leftTile != null)
            {
                neighbor.Add(new Tile(leftTile));
            }
        }


        return neighbor;
    }




}
