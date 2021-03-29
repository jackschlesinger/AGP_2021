public void AddWord(string word) {
	if (word.Length == 0) {
		endsInValidWord = true;
	}

	Node nextNode;

	if (HasChild(word[0])) 
		nextNode = GetChild(word[0]);
	else{
		
		nextNode = new Node { letter = word[0], endsInValidWord = false};
		children.Add(nextNode);
	}

	nextNode.AddWord(word.SubString(1, word.Length - 1));

}