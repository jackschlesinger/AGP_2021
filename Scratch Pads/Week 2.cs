// A way of making sure you don't end up with a null in Services Locator

private static AIController _ai;
public static AIController AIManager
{
    get
    {
        //Debug.Assert(_ai != null);
        
        if (_ai == null)
            _ai = new AIController();

        return _ai;
    }
    set => _ai = value;
}



public int x = 0;

var t = "Hello, my name is Jack";
int y;

void FunctionName(int x) {
    print(x);
}

Action<int> FunctionName = (x) => { print(x); };

Func<int, string,  float, string> TakesInIntAndReturnsString;