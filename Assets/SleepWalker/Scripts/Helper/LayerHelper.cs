using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHelper
{
    private const int OBSTACLES_LAYER = 8;
    private const int GROUND_LAYER = 9;
    private const int PLAYER_LAYER = 10;
    private const int ENEMIES_LAYER = 13;

    public static int obstaclesLayerMask = 1 << OBSTACLES_LAYER;
    public static int groundLayerMask = 1 << GROUND_LAYER;
    public static int playerLayerMask = 1 << PLAYER_LAYER;
    public static int enemiesLayerMask = 1 << ENEMIES_LAYER;
}
