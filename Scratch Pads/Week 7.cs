 // Ternary operator 
// (bool) ? [Value if true] : [Value if false]
/
var toDebug = response.ToUpper() == "AVAILABLE" ? "Server is available." : "Server not available.";
Debug.Log(toDebug);

Action toDo = response.ToUpper() == "AVAILABLE" ? onSuccess : onFailure;
if (toDo != null) toDo();



// null check:
if (onSuccess != null)
	onSuccess.Invoke();





public string PrintString(string toPrint, int index) {
	if (index >= toPrint.Length) return "";

	return toPrint[index] + "!\n" + PrintString(toPrint, index + 1);
}

public void Start() {
	PrintString("Hello World", 0);
}

returns "H!\n" + PrintString("Hello World", 1);
	return "e!\n" + PrintString("Hello World", 2);
	return "l!\n" + PrintString("Hello World", 3);
	return "l!\n" + PrintString("Hello World", 4);
	return "o!\n" + PrintString("Hello World", 5);
	return " !\n" + PrintString("Hello World", 6); 
	return "W!\n" + PrintString("Hello World", 7);
	return "o!\n" + PrintString("Hello World", 8);
	return "r!\n" + PrintString("Hello World", 9);
	return "l!\n" + PrintString("Hello World", 10);
	return "d!\n" + PrintString("Hello World", 11); 
	return "";

"H!\n" +
"e!\n" +
"l!\n" +
"l!\n" +
"o!\n" +
" !\n" +
"W!\n" +
"o!\n" +
"r!\n" +
"l!\n" +
"d!\n" + "";


		Veronica
	|			  |
Nathan		    Sally	
   |	   	   |     |
   |	      Bob Joseph
   |           |     |
Daniel       Susan Betty

/*

EmployeesUnderCount("Sally", orgChart);

Veronica :
counter 0;

Nathan :
counter 0;

Daniel :
counter: 0

Sally : counter 0;

Bob :
counter: 1
counter: 1 + (EmployeesUnderCount("Bob", bob's childTree))
	- Susan: 1
	-> 2

Susan :
counter: 2

Joseph:
counter: 3
counter 3 + (EmployeesUnderCount("Joseph", Joseph's childTree))
	- Betty: 1
	- 4

Bettys:
counter: 4

return counter (4)
*/



int EmployeesUnderCount(string employeeName, Tree orgChart) 
{
	var counter = 0;

	foreach (var employee in orgChart) {
		if (employee.manager == employeeName) {
			counter++; 
		
			counter = counter + EmployeesUnderCount(employee.Name, employee.childrenTree);
		}
	}

	return counter;
}

int EmployeesUnderCount(string employeeName, Tree orgChart) {
	return EmployeesUnderCountRecursive(employeeName, orgChart);
}

void EmployeesUnderCountRecursive(string employeeName, Tree subTree) {
	if (root.childrenCount == 0) {
		return (root.manager == employeeName) ? 1 : 0;;
	}

	foreach (var child of subTree.root) {
		if (child.manager == employeeName) {
			counter += 1;
			return 1 + EmployeesUnderCountRecursive(child.name, child.subTree);
		}
		else
		{
			return EmployeesUnderCountRecursive(employeeName, child.subTree);
		}
	}
}






	