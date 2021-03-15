

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



	public string GetString()
        {
            var ms = new MemoryStream(NumberOfBytes());
            var bw = new BinaryWriter(ms);
            
            bw.Write(Version);

            // Version 1 data
            bw.Write(walls.Length);
            foreach (var wall in walls)
            {
                bw.Write(wall.x);
                bw.Write(wall.y);
            }

            bw.Write(items.Length);

            foreach (var item in items)
            {
                bw.Write(item.GetString());
            }
            
            bw.Write(levelScore);
            
            bw.Write(player.GetString());

            bw.Write(width);
            bw.Write(height);

            var saveData = new ByteSerializer(ms.GetBuffer());

            bw.Close();
            ms.Close();

            return saveData.GetAsString();
        }