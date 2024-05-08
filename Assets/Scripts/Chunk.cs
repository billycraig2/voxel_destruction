using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private static int[] edgeTable = new int[256]
    {
        0x0, 0x109, 0x203, 0x30a, 0x80c, 0x905, 0xa0f, 0xb06,
        0x406, 0x50f, 0x605, 0x70c, 0xc0a, 0xd03, 0xe09, 0xf00,
        0x190, 0x99, 0x393, 0x29a, 0x99c, 0x895, 0xb9f, 0xa96,
        0x596, 0x49f, 0x795, 0x69c, 0xd9a, 0xc93, 0xf99, 0xe90,
        0x230, 0x339, 0x33, 0x13a, 0xa3c, 0xb35, 0x83f, 0x936,
        0x636, 0x73f, 0x435, 0x53c, 0xe3a, 0xf33, 0xc39, 0xd30,
        0x3a0, 0x2a9, 0x1a3, 0xaa, 0xbac, 0xaa5, 0x9af, 0x8a6,
        0x7a6, 0x6af, 0x5a5, 0x4ac, 0xfaa, 0xea3, 0xda9, 0xca0,
        0x8c0, 0x9c9, 0xac3, 0xbca, 0xcc, 0x1c5, 0x2cf, 0x3c6,
        0xcc6, 0xdcf, 0xec5, 0xfcc, 0x4ca, 0x5c3, 0x6c9, 0x7c0,
        0x950, 0x859, 0xb53, 0xa5a, 0x15c, 0x55, 0x35f, 0x256,
        0xd56, 0xc5f, 0xf55, 0xe5c, 0x55a, 0x453, 0x759, 0x650,
        0xaf0, 0xbf9, 0x8f3, 0x9fa, 0x2fc, 0x3f5, 0xff, 0x1f6,
        0xef6, 0xfff, 0xcf5, 0xdfc, 0x6fa, 0x7f3, 0x4f9, 0x5f0,
        0xb60, 0xa69, 0x963, 0x86a, 0x36c, 0x265, 0x16f, 0x66,
        0xf66, 0xe6f, 0xd65, 0xc6c, 0x76a, 0x663, 0x569, 0x460,
        0x460, 0x569, 0x663, 0x76a, 0xc6c, 0xd65, 0xe6f, 0xf66,
        0x66, 0x16f, 0x265, 0x36c, 0x86a, 0x963, 0xa69, 0xb60,
        0x5f0, 0x4f9, 0x7f3, 0x6fa, 0xdfc, 0xcf5, 0xfff, 0xef6,
        0x1f6, 0xff, 0x3f5, 0x2fc, 0x9fa, 0x8f3, 0xbf9, 0xaf0,
        0x650, 0x759, 0x453, 0x55a, 0xe5c, 0xf55, 0xc5f, 0xd56,
        0x256, 0x35f, 0x55, 0x15c, 0xa5a, 0xb53, 0x859, 0x950,
        0x7c0, 0x6c9, 0x5c3, 0x4ca, 0xfcc, 0xec5, 0xdcf, 0xcc6,
        0x3c6, 0x2cf, 0x1c5, 0xcc, 0xbca, 0xac3, 0x9c9, 0x8c0,
        0xca0, 0xda9, 0xea3, 0xfaa, 0x4ac, 0x5a5, 0x6af, 0x7a6,
        0x8a6, 0x9af, 0xaa5, 0xbac, 0xaa, 0x1a3, 0x2a9, 0x3a0,
        0xd30, 0xc39, 0xf33, 0xe3a, 0x53c, 0x435, 0x73f, 0x636,
        0x936, 0x83f, 0xb35, 0xa3c, 0x13a, 0x33, 0x339, 0x230,
        0xe90, 0xf99, 0xc93, 0xd9a, 0x69c, 0x795, 0x49f, 0x596,
        0xa96, 0xb9f, 0x895, 0x99c, 0x29a, 0x393, 0x99, 0x190,
        0xf00, 0xe09, 0xd03, 0xc0a, 0x70c, 0x605, 0x50f, 0x406,
        0xb06, 0xa0f, 0x905, 0x80c, 0x30a, 0x203, 0x109, 0x0,
    };

    private static int[,] triTable = new int[256, 16]
    {
		{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 9, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 8, 1, 1, 8, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 11, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 11, 11, 0, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 11, 1, 0, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 1, 2, 11, 9, 1, 11, 8, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 10, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 8, 2, 1, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 9, 9, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 2, 3, 8, 10, 2, 8, 9, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 3, 10, 10, 3, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 0, 1, 10, 8, 0, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 3, 0, 9, 11, 3, 9, 10, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 9, 11, 11, 9, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 4, 3, 3, 4, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 7, 0, 9, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 4, 9, 1, 7, 4, 1, 3, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 7, 4, 11, 3, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 11, 7, 4, 2, 11, 4, 0, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 9, 1, 8, 7, 4, 11, 3, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 4, 11, 11, 4, 2, 2, 4, 9, 2, 9, 1, -1, -1, -1, -1 },
		{ 4, 8, 7, 2, 1, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 4, 3, 3, 4, 0, 10, 2, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 9, 9, 2, 0, 7, 4, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 3, 10, 3, 4, 3, 7, 4, 9, 10, 4, -1, -1, -1, -1 },
		{ 1, 10, 3, 3, 10, 11, 4, 8, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 11, 1, 11, 7, 4, 1, 11, 4, 1, 4, 0, -1, -1, -1, -1 },
		{ 7, 4, 8, 9, 3, 0, 9, 11, 3, 9, 10, 11, -1, -1, -1, -1 },
		{ 7, 4, 11, 4, 9, 11, 9, 10, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 5, 8, 0, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 5, 0, 0, 5, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 8, 4, 5, 3, 8, 5, 1, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 5, 11, 3, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 11, 0, 0, 11, 8, 5, 9, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 5, 0, 0, 5, 1, 11, 3, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 1, 4, 1, 2, 11, 4, 1, 11, 4, 11, 8, -1, -1, -1, -1 },
		{ 1, 10, 2, 5, 9, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 5, 0, 3, 8, 2, 1, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 5, 10, 2, 4, 5, 2, 0, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 5, 5, 2, 4, 4, 2, 3, 4, 3, 8, -1, -1, -1, -1 },
		{ 11, 3, 10, 10, 3, 1, 4, 5, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 5, 9, 10, 0, 1, 10, 8, 0, 10, 11, 8, -1, -1, -1, -1 },
		{ 11, 3, 0, 11, 0, 5, 0, 4, 5, 10, 11, 5, -1, -1, -1, -1 },
		{ 4, 5, 8, 5, 10, 8, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 7, 9, 9, 7, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 9, 0, 3, 5, 9, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 0, 8, 7, 1, 0, 7, 5, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 5, 3, 3, 5, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 9, 7, 7, 9, 8, 2, 11, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 11, 7, 2, 7, 9, 7, 5, 9, 0, 2, 9, -1, -1, -1, -1 },
		{ 2, 11, 3, 7, 0, 8, 7, 1, 0, 7, 5, 1, -1, -1, -1, -1 },
		{ 2, 11, 1, 11, 7, 1, 7, 5, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 7, 9, 9, 7, 5, 2, 1, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 1, 3, 9, 0, 3, 5, 9, 3, 7, 5, -1, -1, -1, -1 },
		{ 7, 5, 8, 5, 10, 2, 8, 5, 2, 8, 2, 0, -1, -1, -1, -1 },
		{ 10, 2, 5, 2, 3, 5, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 7, 5, 8, 5, 9, 11, 3, 10, 3, 1, 10, -1, -1, -1, -1 },
		{ 5, 11, 7, 10, 11, 5, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 5, 10, 7, 5, 11, 8, 3, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 11, 7, 10, 11, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 6, 3, 8, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 11, 0, 9, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 1, 8, 8, 1, 3, 6, 7, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 7, 7, 2, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 7, 8, 0, 6, 7, 0, 2, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 2, 2, 7, 3, 9, 1, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 8, 6, 8, 1, 8, 9, 1, 2, 6, 1, -1, -1, -1, -1 },
		{ 11, 6, 7, 10, 2, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 8, 0, 11, 6, 7, 10, 2, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 9, 2, 2, 9, 10, 7, 11, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 11, 8, 2, 3, 8, 10, 2, 8, 9, 10, -1, -1, -1, -1 },
		{ 7, 10, 6, 7, 1, 10, 7, 3, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 7, 7, 0, 6, 6, 0, 1, 6, 1, 10, -1, -1, -1, -1 },
		{ 7, 3, 6, 3, 0, 9, 6, 3, 9, 6, 9, 10, -1, -1, -1, -1 },
		{ 6, 7, 10, 7, 8, 10, 8, 9, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 6, 8, 8, 6, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 3, 11, 6, 0, 3, 6, 4, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 6, 8, 8, 6, 4, 1, 0, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 3, 9, 3, 11, 6, 9, 3, 6, 9, 6, 4, -1, -1, -1, -1 },
		{ 2, 8, 3, 2, 4, 8, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 0, 6, 6, 0, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 1, 0, 2, 8, 3, 2, 4, 8, 2, 6, 4, -1, -1, -1, -1 },
		{ 9, 1, 4, 1, 2, 4, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 6, 6, 8, 11, 1, 10, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 10, 2, 6, 3, 11, 6, 0, 3, 6, 4, 0, -1, -1, -1, -1 },
		{ 11, 6, 4, 11, 4, 8, 10, 2, 9, 2, 0, 9, -1, -1, -1, -1 },
		{ 10, 4, 9, 6, 4, 10, 11, 2, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 3, 4, 3, 10, 3, 1, 10, 6, 4, 10, -1, -1, -1, -1 },
		{ 1, 10, 0, 10, 6, 0, 6, 4, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 10, 6, 9, 10, 4, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 10, 6, 9, 10, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 7, 11, 4, 5, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 5, 9, 7, 11, 6, 3, 8, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 0, 5, 5, 0, 4, 11, 6, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 6, 7, 5, 8, 4, 5, 3, 8, 5, 1, 3, -1, -1, -1, -1 },
		{ 3, 2, 7, 7, 2, 6, 9, 4, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 9, 4, 0, 7, 8, 0, 6, 7, 0, 2, 6, -1, -1, -1, -1 },
		{ 3, 2, 6, 3, 6, 7, 1, 0, 5, 0, 4, 5, -1, -1, -1, -1 },
		{ 6, 1, 2, 5, 1, 6, 4, 7, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 2, 1, 6, 7, 11, 4, 5, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 8, 4, 5, 9, 11, 6, 7, 10, 2, 1, -1, -1, -1, -1 },
		{ 7, 11, 6, 2, 5, 10, 2, 4, 5, 2, 0, 4, -1, -1, -1, -1 },
		{ 8, 4, 7, 5, 10, 6, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 5, 7, 10, 6, 7, 1, 10, 7, 3, 1, -1, -1, -1, -1 },
		{ 10, 6, 5, 7, 8, 4, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 3, 0, 7, 3, 4, 6, 5, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 6, 5, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 6, 5, 9, 11, 6, 9, 8, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 6, 3, 3, 6, 0, 0, 6, 5, 0, 5, 9, -1, -1, -1, -1 },
		{ 11, 6, 5, 11, 5, 0, 5, 1, 0, 8, 11, 0, -1, -1, -1, -1 },
		{ 11, 6, 3, 6, 5, 3, 5, 1, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 8, 5, 8, 3, 2, 5, 8, 2, 5, 2, 6, -1, -1, -1, -1 },
		{ 5, 9, 6, 9, 0, 6, 0, 2, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 6, 5, 2, 6, 1, 3, 0, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 6, 5, 2, 6, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 1, 10, 9, 6, 5, 9, 11, 6, 9, 8, 11, -1, -1, -1, -1 },
		{ 9, 0, 1, 3, 11, 2, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 0, 8, 2, 0, 11, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 11, 2, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 8, 3, 9, 8, 1, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 5, 10, 0, 1, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 3, 0, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 5, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 5, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 8, 6, 10, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 5, 6, 9, 1, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 8, 1, 1, 8, 9, 6, 10, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 11, 3, 6, 10, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 11, 11, 0, 2, 5, 6, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 0, 9, 2, 11, 3, 6, 10, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 6, 10, 11, 1, 2, 11, 9, 1, 11, 8, 9, -1, -1, -1, -1 },
		{ 5, 6, 1, 1, 6, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 6, 1, 1, 6, 2, 8, 0, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 9, 5, 6, 0, 9, 6, 2, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 2, 5, 2, 3, 8, 5, 2, 8, 5, 8, 9, -1, -1, -1, -1 },
		{ 3, 6, 11, 3, 5, 6, 3, 1, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 1, 8, 1, 6, 1, 5, 6, 11, 8, 6, -1, -1, -1, -1 },
		{ 11, 3, 6, 6, 3, 5, 5, 3, 0, 5, 0, 9, -1, -1, -1, -1 },
		{ 5, 6, 9, 6, 11, 9, 11, 8, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 6, 10, 7, 4, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 4, 4, 3, 7, 10, 5, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 6, 10, 4, 8, 7, 0, 9, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 10, 5, 1, 4, 9, 1, 7, 4, 1, 3, 7, -1, -1, -1, -1 },
		{ 7, 4, 8, 6, 10, 5, 2, 11, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 5, 6, 4, 11, 7, 4, 2, 11, 4, 0, 2, -1, -1, -1, -1 },
		{ 4, 8, 7, 6, 10, 5, 3, 2, 11, 1, 0, 9, -1, -1, -1, -1 },
		{ 1, 2, 10, 11, 7, 6, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 1, 6, 6, 1, 5, 8, 7, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 7, 0, 7, 4, 2, 1, 6, 1, 5, 6, -1, -1, -1, -1 },
		{ 8, 7, 4, 6, 9, 5, 6, 0, 9, 6, 2, 0, -1, -1, -1, -1 },
		{ 7, 2, 3, 6, 2, 7, 5, 4, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 7, 3, 6, 11, 3, 5, 6, 3, 1, 5, -1, -1, -1, -1 },
		{ 5, 0, 1, 4, 0, 5, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 5, 4, 6, 11, 7, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 7, 6, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 10, 4, 4, 10, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 10, 4, 4, 10, 9, 3, 8, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 10, 1, 0, 6, 10, 0, 4, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 10, 1, 6, 1, 8, 1, 3, 8, 4, 6, 8, -1, -1, -1, -1 },
		{ 9, 4, 10, 10, 4, 6, 3, 2, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 11, 8, 2, 8, 0, 6, 10, 4, 10, 9, 4, -1, -1, -1, -1 },
		{ 11, 3, 2, 0, 10, 1, 0, 6, 10, 0, 4, 6, -1, -1, -1, -1 },
		{ 6, 8, 4, 11, 8, 6, 2, 10, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 1, 9, 4, 2, 1, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 8, 0, 4, 1, 9, 4, 2, 1, 4, 6, 2, -1, -1, -1, -1 },
		{ 6, 2, 4, 4, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 8, 2, 8, 4, 2, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 6, 9, 6, 11, 3, 9, 6, 3, 9, 3, 1, -1, -1, -1, -1 },
		{ 8, 6, 11, 4, 6, 8, 9, 0, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 3, 6, 3, 0, 6, 0, 4, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 6, 11, 4, 6, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 7, 6, 10, 8, 7, 10, 9, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 7, 0, 7, 6, 10, 0, 7, 10, 0, 10, 9, -1, -1, -1, -1 },
		{ 6, 10, 7, 7, 10, 8, 8, 10, 1, 8, 1, 0, -1, -1, -1, -1 },
		{ 6, 10, 7, 10, 1, 7, 1, 3, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 11, 10, 7, 6, 10, 8, 7, 10, 9, 8, -1, -1, -1, -1 },
		{ 2, 9, 0, 10, 9, 2, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 8, 3, 7, 6, 11, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 6, 11, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 1, 9, 2, 9, 7, 9, 8, 7, 6, 2, 7, -1, -1, -1, -1 },
		{ 2, 7, 6, 3, 7, 2, 0, 1, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 7, 0, 7, 6, 0, 6, 2, 0, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 2, 3, 6, 2, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 1, 9, 3, 1, 8, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 7, 6, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 6, 11, 7, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 5, 5, 11, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 5, 11, 11, 5, 7, 0, 3, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 5, 5, 11, 10, 0, 9, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 10, 7, 10, 5, 3, 8, 1, 8, 9, 1, -1, -1, -1, -1 },
		{ 5, 2, 10, 5, 3, 2, 5, 7, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 7, 10, 7, 8, 0, 10, 7, 0, 10, 0, 2, -1, -1, -1, -1 },
		{ 0, 9, 1, 5, 2, 10, 5, 3, 2, 5, 7, 3, -1, -1, -1, -1 },
		{ 9, 7, 8, 5, 7, 9, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 11, 2, 1, 7, 11, 1, 5, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 3, 1, 11, 2, 1, 7, 11, 1, 5, 7, -1, -1, -1, -1 },
		{ 7, 11, 2, 7, 2, 9, 2, 0, 9, 5, 7, 9, -1, -1, -1, -1 },
		{ 7, 9, 5, 8, 9, 7, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 1, 7, 7, 1, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 0, 7, 0, 1, 7, 1, 5, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 9, 3, 9, 5, 3, 5, 7, 3, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 7, 8, 5, 7, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 5, 4, 8, 10, 5, 8, 11, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 11, 0, 11, 5, 11, 10, 5, 4, 0, 5, -1, -1, -1, -1 },
		{ 1, 0, 9, 8, 5, 4, 8, 10, 5, 8, 11, 10, -1, -1, -1, -1 },
		{ 10, 3, 11, 1, 3, 10, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 8, 8, 2, 4, 4, 2, 10, 4, 10, 5, -1, -1, -1, -1 },
		{ 10, 5, 2, 5, 4, 2, 4, 0, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 4, 9, 8, 3, 0, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 10, 1, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 11, 4, 11, 2, 1, 4, 11, 1, 4, 1, 5, -1, -1, -1, -1 },
		{ 0, 5, 4, 1, 5, 0, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 11, 2, 8, 11, 0, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 4, 9, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 8, 5, 8, 3, 5, 3, 1, 5, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 5, 4, 1, 5, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 4, 9, 3, 0, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 5, 4, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 4, 7, 11, 9, 4, 11, 10, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 8, 11, 4, 7, 11, 9, 4, 11, 10, 9, -1, -1, -1, -1 },
		{ 11, 10, 7, 10, 1, 0, 7, 10, 0, 7, 0, 4, -1, -1, -1, -1 },
		{ 3, 10, 1, 11, 10, 3, 7, 8, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 10, 3, 10, 4, 10, 9, 4, 7, 3, 4, -1, -1, -1, -1 },
		{ 9, 2, 10, 0, 2, 9, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 4, 7, 0, 4, 3, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 8, 4, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 4, 4, 11, 9, 9, 11, 2, 9, 2, 1, -1, -1, -1, -1 },
		{ 1, 9, 0, 4, 7, 8, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 11, 4, 11, 2, 4, 2, 0, 4, -1, -1, -1, -1, -1, -1, -1 },
		{ 4, 7, 8, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 4, 1, 4, 7, 1, 7, 3, 1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 8, 4, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 4, 7, 0, 4, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 7, 8, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 10, 8, 8, 10, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 0, 3, 9, 3, 11, 9, 11, 10, 9, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 0, 10, 0, 8, 10, 8, 11, 10, -1, -1, -1, -1, -1, -1, -1 },
		{ 10, 3, 11, 1, 3, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 2, 8, 2, 10, 8, 10, 9, 8, -1, -1, -1, -1, -1, -1, -1 },
		{ 9, 2, 10, 0, 2, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 3, 0, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 10, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 2, 1, 11, 1, 9, 11, 9, 8, 11, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 2, 3, 9, 0, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 11, 0, 8, 2, 0, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 3, 11, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 8, 3, 9, 8, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 1, 9, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ 8, 3, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
		{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
	};

    private float isoLevel = 0.5f;
    
    public GameObject voxelHighlight;
    private GameObject voxelHighlightInstance;
    private Quaternion lastCameraRotation;

    private Voxel[,,] voxels;
    private int chunkSize = 16;
    private Color gizmoColor;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    [SerializeField]
    private GameObject voxelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (voxelHighlight != null)
        {
            voxelHighlightInstance = Instantiate(voxelHighlight, Vector3.zero, Quaternion.identity);
            voxelHighlightInstance.SetActive(false);
        }
        
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        InitializeVoxels();
        GenerateMesh();
    }

    void Update()
    {
        HighlightVoxel();
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPointInsideVoxel = hit.point - hit.normal * 0.01f; 
                Vector3 hitVoxelPosition = hitPointInsideVoxel - transform.position;

                int x = Mathf.FloorToInt(hitVoxelPosition.x);
                int y = Mathf.FloorToInt(hitVoxelPosition.y);
                int z = Mathf.FloorToInt(hitVoxelPosition.z);
                
                if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
                {
                    voxels[x, y, z].isActive = false;
                    GenerateMesh();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                Vector3 neighbourPosition = hitPoint - transform.position + hit.normal * 0.5f;

                int x = Mathf.FloorToInt(neighbourPosition.x);
                int y = Mathf.FloorToInt(neighbourPosition.y);
                int z = Mathf.FloorToInt(neighbourPosition.z);
                
                if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
                {
                    voxels[x, y, z].isActive = true;
                    GenerateMesh();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerExplosion();
        }
        
    }

    private void InitializeVoxels()
    {
        float worldX, worldZ, height;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                worldX = x + transform.position.x;
                worldZ = z + transform.position.z;
                
                height = Mathf.PerlinNoise(worldX * World.Instance.noiseScale, worldZ * World.Instance.noiseScale) * World.Instance.heightScale;

                for (int y = 0; y < chunkSize; y++)
                {
                    float worldY = y + transform.position.y;
                    float density = height - worldY;
                    bool isActive = density >= 0;

                    voxels[x, y, z] = new Voxel(transform.position + new Vector3(x, y, z), Color.white, isActive);
                    if (isActive)
                    {
                        gizmoColor = new Color(Random.value, Random.value, Random.value, 0.4f);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (voxels != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position + new Vector3(chunkSize / 2, chunkSize / 2, chunkSize / 2),
                new Vector3(chunkSize, chunkSize, chunkSize));
        }
    }

    public void Initialize(int size)
    {
        this.chunkSize = size;
        voxels = new Voxel[size, size, size];

        InitializeVoxels();

    }

    public void IterateVoxels()
    {
        if (voxels == null)
        {
            return;
        }
        
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (voxels[x, y, z].isActive)
                    {
                        ProcessVoxel(x, y, z);
                    }
                }
            }
        }
    }

    private void ProcessVoxel(int x, int y, int z)
    {
        if (voxels == null || x < 0 || x >= voxels.GetLength(0) ||
            y < 0 || y >= voxels.GetLength(1) || z < 0 || z >= voxels.GetLength(2))
        {
            return; 
        }

        Voxel voxel = voxels[x, y, z];
        if (voxel.isActive)
        {
            bool[] facesVisible = new bool[6];

            // Check visibility for each face
            facesVisible[0] = IsFaceVisible(x, y + 1, z); // Top
            facesVisible[1] = IsFaceVisible(x, y - 1, z); // Bottom
            facesVisible[2] = IsFaceVisible(x - 1, y, z); // Left
            facesVisible[3] = IsFaceVisible(x + 1, y, z); // Right
            facesVisible[4] = IsFaceVisible(x, y, z + 1); // Front
            facesVisible[5] = IsFaceVisible(x, y, z - 1); // Back

            for (int i = 0; i < facesVisible.Length; i++)
            {
                if (facesVisible[i])
                    AddFaceData(x, y, z, i); 
            }
        }
    }

    private bool IsFaceVisible(int x, int y, int z)
    {
        Vector3 globalPos = transform.position + new Vector3(x, y, z);
        
        return IsVoxelHiddenInChunk(x, y, z) && IsVoxelHiddenInWorld(globalPos);
    }

    private void AddFaceData(int x, int y, int z, int faceIndex)
    {

        if (faceIndex == 0) // Top Face
        {
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 1) // Bottom Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x, y, z + 1));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 2) // Left Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 3) // Right Face
        {
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 4) // Front Face
        {
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
        }

        if (faceIndex == 5) // Back Face
        {
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));

        }

        AddTriangleIndices();
    }

    private void AddTriangleIndices()
    {
        int vertCount = vertices.Count;

        // First triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 3);
        triangles.Add(vertCount - 2);

        // Second triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 2);
        triangles.Add(vertCount - 1);
    }

    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        
        for (int x = 0; x < chunkSize - 1; x++)
		{
			for (int y = 0; y < chunkSize - 1; y++)
			{
				for (int z = 0; z < chunkSize - 1; z++)
				{
					MarchCube(x, y, z);
				}
			}
		}
        
        Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
    }

    private void MarchCube(int x, int y, int z)
    {
	    int cubeIndex = CalculateCubeIndex(x, y, z);
	    if (edgeTable[cubeIndex] == 0)
		    return;
	    
	    Vector3[] edgeVertices = new Vector3[12];
	    if ((edgeTable[cubeIndex] & 1) != 0)
		    edgeVertices[0] = InterpolateEdge(voxels[x, y, z], voxels[x + 1, y, z]);
	    if ((edgeTable[cubeIndex] & 2) != 0)
		    edgeVertices[1] = InterpolateEdge(voxels[x + 1, y, z], voxels[x + 1, y, z + 1]);
	    if ((edgeTable[cubeIndex] & 4) != 0)
		    edgeVertices[2] = InterpolateEdge(voxels[x, y, z + 1], voxels[x + 1, y, z + 1]);
	    if ((edgeTable[cubeIndex] & 8) != 0)
		    edgeVertices[3] = InterpolateEdge(voxels[x, y, z], voxels[x, y, z + 1]);
	    if ((edgeTable[cubeIndex] & 16) != 0)
		    edgeVertices[4] = InterpolateEdge(voxels[x, y + 1, z], voxels[x + 1, y + 1, z]);
	    if ((edgeTable[cubeIndex] & 32) != 0)
		    edgeVertices[5] = InterpolateEdge(voxels[x + 1, y + 1, z], voxels[x + 1, y + 1, z + 1]);
	    if ((edgeTable[cubeIndex] & 64) != 0)
		    edgeVertices[6] = InterpolateEdge(voxels[x, y + 1, z + 1], voxels[x + 1, y + 1, z + 1]);
	    if ((edgeTable[cubeIndex] & 128) != 0)
		    edgeVertices[7] = InterpolateEdge(voxels[x, y + 1, z], voxels[x, y + 1, z + 1]);
	    if ((edgeTable[cubeIndex] & 256) != 0)
		    edgeVertices[8] = InterpolateEdge(voxels[x, y, z], voxels[x, y + 1, z]);
	    if ((edgeTable[cubeIndex] & 512) != 0)
		    edgeVertices[9] = InterpolateEdge(voxels[x + 1, y, z], voxels[x + 1, y + 1, z]);
	    if ((edgeTable[cubeIndex] & 1024) != 0)
		    edgeVertices[10] = InterpolateEdge(voxels[x + 1, y, z + 1], voxels[x + 1, y + 1, z + 1]);
	    if ((edgeTable[cubeIndex] & 2048) != 0)
		    edgeVertices[11] = InterpolateEdge(voxels[x, y, z + 1], voxels[x, y + 1, z + 1]);
	    
	    
	    for (int i = 0; triTable[cubeIndex, i] != -1; i += 3)
	    {
		    vertices.Add(edgeVertices[triTable[cubeIndex, i]]);
		    vertices.Add(edgeVertices[triTable[cubeIndex, i + 1]]);
		    vertices.Add(edgeVertices[triTable[cubeIndex, i + 2]]);
		    AddTriangleIndices();
	    }
    }
    
    private Vector3 InterpolateEdge(Voxel v1, Voxel v2)
    {
	    float t = (isoLevel - v1.density) / (v2.density - v1.density);
	    return v1.position + t * (v2.position - v1.position);
    }
    
    private int CalculateCubeIndex(int x, int y, int z)
    {
	    int index = 0;
	    if (voxels[x, y, z].density < isoLevel) index |= 1;
	    if (voxels[x + 1, y, z].density < isoLevel) index |= 2;
	    if (voxels[x + 1, y, z + 1].density < isoLevel) index |= 4;
	    if (voxels[x, y, z + 1].density < isoLevel) index |= 8;
	    if (voxels[x, y + 1, z].density < isoLevel) index |= 16;
	    if (voxels[x + 1, y + 1, z].density < isoLevel) index |= 32;
	    if (voxels[x + 1, y + 1, z + 1].density < isoLevel) index |= 64;
	    if (voxels[x, y + 1, z + 1].density < isoLevel) index |= 128;
	    return index;
    }

    private bool IsVoxelHiddenInChunk(int x, int y, int z)
    {
        if (x < 0 || x >= chunkSize || y < 0 || y >= chunkSize || z < 0 || z >= chunkSize)
            return true;
        return !voxels[x, y, z].isActive;
    }
    
    private bool IsVoxelHiddenInWorld(Vector3 globalPos)
    {
        Chunk neighborChunk = World.Instance.GetChunkAt(globalPos);
        if (neighborChunk == null)
        {
            return true;
        }

        Vector3 localPos = neighborChunk.transform.InverseTransformPoint(globalPos);

        return !neighborChunk.IsVoxelActiveAt(localPos);
        
        
    }
    
    public bool IsVoxelActiveAt(Vector3 localPosition)
    {
        int x = Mathf.RoundToInt(localPosition.x);
        int y = Mathf.RoundToInt(localPosition.y);
        int z = Mathf.RoundToInt(localPosition.z);

        if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
        {
            return voxels[x, y, z].isActive;
        }

        return false;
    }

    
    private void HighlightVoxel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f)) // Adjust the max distance as needed
        {
            Vector3 hitPoint = hit.point - hit.normal * 0.01f; // Nudge into the voxel
            Vector3 hitVoxelPosition = hitPoint - transform.position;

            int x = Mathf.FloorToInt(hitVoxelPosition.x);
            int y = Mathf.FloorToInt(hitVoxelPosition.y);
            int z = Mathf.FloorToInt(hitVoxelPosition.z);

            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                if (voxelHighlightInstance != null)
                {
                    voxelHighlightInstance.SetActive(true);
                    voxelHighlightInstance.transform.position = transform.position + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f); // Center on voxel
                }
            }
        }
        else
        {
            if (voxelHighlightInstance != null)
            { 
                voxelHighlightInstance.SetActive(false);
            }
        }
    }

    private void TriggerExplosion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cast a ray from the camera to the mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 explosionCenter = hit.point; // The exact point of impact
            float explosionRadius = 5f; // Define the radius of your explosion

            // Convert the global hit point to local chunk coordinates
            Vector3 localHitPoint = this.transform.InverseTransformPoint(explosionCenter);

            // Explode the voxels in the chunk
            ExplodeVoxels(localHitPoint, explosionRadius);
        }
    }

    public void ExplodeVoxels(Vector3 explosionCenter, float explosionRadius)
    {
    
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3 voxelPosition = new Vector3(x, y, z);
                    float distance = Vector3.Distance(voxelPosition, explosionCenter);

                    if (distance <= explosionRadius)
                    {
                        voxels[x, y, z].isActive = false;
                    }
                }
            }
        }

        GenerateMesh();
    }
}
    
    


