

	public void ExampleMethod()
	{
		Task a = new Task();
		Task b = new Task();
		Task c = new Task();
		Task d = new Task();

		a.Then(b).Then(c).Then(d);
	}

	public Task UpdateMethod(System.Action methodToUpdate)
	{
		// do something

		return this;
	}
	
	public void FactoryExample()
	{
		Task a = new Task();

		a.UpdateMethod(CleanUp).UpdateMethod(Update).UpdateMethod(OnAbort);
		
	}


	