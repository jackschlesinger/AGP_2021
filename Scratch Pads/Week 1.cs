public static class Services {
	
	public static void InitializeServices() {
		Services.GameManager = this;
		Services.EnemyManager = new EnemyManager();
		Services.SpellDatabase = UnityEngine.Resources.Load<SpellDatabase>("test_database");
	}

	public static GameManager GameManager;
	public static EnemyManager EnemyManager;
	public static SpellDatabase SpellDatabase;
}


public class GameManager : Monbehavior 
{
	private void Start() {
		// Initialization
		Services.InitializeServices();
	}

	public void Update() {
		// Update
		Services.EnemyManager.Update();
		Services.Player.Update();
		Services.EnemyManager.Update();
		Services.Network.Call();
		Services.EnemyManager.Update(true);
	}

	public void OnDestroy() {
		// Destruction
		Services.EnemyManager.Destroy();
	}
}

public class EnemyManager {

	public List<Enemy> allEnemies = new List<Enemy>();

	public void Initialization() {

	}

	public void Update(bool moveEnemies = false)  {
		foreach (var enemy in SpawnedEnemies()) {
			enemy.Update();

			if (!moveEnemies) continue;

			enemy.Move();
		}
	}

	public Enemy[] SpawnedEnemies() { }

	public void Destroy() {

	}
}

public class SpellDatabase : ScriptableObject {

}






// *****// OLD singleton example
public class GameManager :Monobehavior {   
	public static GameManager Instance;
	public const int MaxHP = 10;

	private void Start() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			UnityEngine.Destroy(this);
		}
	}
}


public class Player {
	public int hp { get; private set; }

	public void Start() {
		hp = GameManager.Instance.MaxHP;
	}
}

