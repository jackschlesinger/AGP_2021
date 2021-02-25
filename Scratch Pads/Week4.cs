Amount of information you can encode in a # of digits -

(2^ (# of digits)) - 1 -> highest number
(10 ^ (# of digits)) - 1 -> highest number

((# of possible digits) ^ (# of digits)) - 1 (highest number)


 

(Decimal - 10 digits)

1,482

2 - ones place (10^0)
8 - tens place (10^1)
4 - hundreds place (10^2)
1 - thousands place (10^3)

2 * 1 +
8 * 10 +
4 * 100 +
1 * 1000 = (1,482)

Binary - Two digits

Each digit is called a "bit"
8 digits is called a "byte"

0110 1100

0 - ones place
0 - twos place
1 - fours place
1 - eighths place
0 - 16th place
1 - 32nd place
1 - 64th place 
0 - 128th place

0 * 1 +
0 * 2 +
1 * 4 +
1 * 8 +
0 * 16 +
1 * 32 +
1 * 64 +
0 * 128 = (108 in decimal)

Hexadecimal - 16 (0 - 9, A - F) 

3D2F

F - ones place (16^0)
2 - sixteenths place (16^1)
D - two-hundred-fifty-six (16^2)
3 - four-thousand-ninety-six (16^3)

F * 1 (16)
2 * 16 (32)
D * 256 (3,584)
3 * 4096 (12,288) = 15,920





======== Floating point ========


32 bits
0     1010101 01010101 00101010      10001111
+/- | value rep. 7 decimal digits | 10 ^ # |






===  Passing by value =========

struct PhysicsDetail {
	float x, y, z;
}

var newVector3 = new Vector3(myPosition.x, myPosition.y, myPosition.z);

// Tuple<float, float, float> x;
// x.Item1, x.Item2, x.Item3


public void Update() {

	// MAKES A COPY
	var startingPosition = myPosition.Copy();

	var wallValue = NearestWallByValue(myPosition, myHeading);
	// wallValue != myPosition
	// startingPosition = myPosition;

	var wallReference = NearestWallByReference(myPosition, myHeading);
	// wallValue = myPosition
	// myPosition != startingPosition
}


public PhysicsDetail NearestWallByValue(PhysicsDetail myPosition, Vector3 direction) {
	while (!InWall(myPosition)) {
		myPosition += direciton.normalized;
	}
	return myPosition;
}


public Vector3 NearestWallByReference(Vector3 myPosition, Vector3 direction) {
	while (!InWall(myPosition)) {
		myPosition += direciton.normalized;
	}
	return myPosition;
}



======= Delegates

public void PrintNumbers(int x, float j) {
	// Does something
}

public void SwapNumbers(int x, float j) {

}

PrintNumbers(8, 3.003f);


// Tell event manager to run MyFunction in the future, if a condition is met.



public Action<int, float> myFunction;

myFunction = PrintNumbers;



myFunction(8, 3.003f);

...

myFunction = SwapNumbers;

myFunction(8, 3.003f);


// Funcs

Func<int> myFunc // same as int FunctionName() {}
Func<float, bool, string> otherFunc // same as string FunctionName(float f, bool b) {}





public static Action<AGP_Event> handler;

// other scripts
// game controller has a function "MyFunction"
// IN GAME CONTROLLER
EventManager.handler += MyFunction;
// audio system has a function playalert
// IN AUDIO SYSTEM
EventManager.handler += PlayAlert;
EventManager.handler -= PlayAlert; // to unsubscribe


EventManager.handler(new Achievement(example));


// Example runtime delegates



// In Application manager:

DisplayInformation debug;

public void Initialize() {

	#if UNITY_EDITOR
	debug = ConsoleLogInfo;
	#else
	debug = WriteOutInformation;
	#endif


	debug("Test");
}











