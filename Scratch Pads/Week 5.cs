
Bitwise operators &, |, <<, >>
        
Unity physics represents layers in a bitmask

32 layers - one int worth of bytes 00000000 00000000 00000000 00000000
- ray that only hits 1st and 6th layers -> "00000000 00000000 00000000 00100001"

1 32 or 64 bit address to represent the start of the array,
At most 4 bytes
        
32 layers - each as a byte in an array of bytes

1 32 or 64 bit address to represent the start of the array,
1 32 or 64 bit number to represent the length of the array,
32 bytes, one for each layer to hit.



"Left shift operator" <<

 0010 0101 (37)

  0010 0101 << 1 = 0100 1010 (74)

  Mask layers 6 and 1

  var toMask = (1 << (6 - 1)) + (1 << (1 - 1));

   1 << 5 = 32
   0000 0001 << 5
   0000 0010 << 4
   0000 0100 << 3
   0000 1000 << 2
   0001 0000 << 1
   0010 0000

   0000 0001 << 0 = 1

   33

"Right shift operator" >>

Same but the other direction

	0010 0010 >> 1
	0001 0001 = 17

"And operator" &

Works the same as "and" boolean operator, on individual bits.


  0010 0011
 &1010 1110
-------------
  0010 0010

  "Both values in the same position are 1"


For example, I want to know the layers that two raycasts have in common

layerMask1 & layerMask2 == commonLayersInMasks



"Or operator" |

Wors the same as the "or" boolean operator, on individual bits.


  0010 0011
 |1010 1110
-------------
  1010 1111

  For example, I want to know ALL the layers that two raycasts use (union)

  layerMask1 | layerMask2 == allLayersIncludedInEitherMask






List<Node> dungeon = new List<Node> dungeon;



public class Node {
	public List<Node> neighbors;

	public Node() {
		this.neighbors = new List<Node>();
	}

	public AddNeighbor(Node node) {
		neighbors.Add(node);
	}
}





BoardPiece[,] board = new BoardPiece[8,8];

board[4, 5] = new EmptySpace();






BoardPiece[][] board = new BoardPiece[8][];
for (var i = 0; i < 8; i++) {
	board[i] = new BoardPiece[8];
}

board[4][5];






