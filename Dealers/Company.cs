using GTA.Math;

namespace VeoAutoMod.Dealers
{
    enum CompanyState
    {
        Spawned,
        Spawning,
        Removed,
    }

    abstract class Company
    {
        private readonly string name;
        private readonly Vector3 position;

        private bool isLoaded = false;
        
        private float spawnRadius = 150f;

        private CompanyState state = CompanyState.Removed;

        public Company(string name, Vector3 position) {
            this.name = name;
            this.position = position;

            if (World.IsDebug()) spawnRadius = 20f;
        }

        public string GetName() => this.name;
        public Vector3 GetPosition() => this.position;

        public void SetLoaded() => this.isLoaded = true;
        public bool IsLoaded() => this.isLoaded;

        public void SetSpawnRadius(float radius) => this.spawnRadius = radius;
        public float GetSpawnRadius() => this.spawnRadius;

        public void SetState(CompanyState state) => this.state = state;
        public CompanyState GetState() => this.state;
        public bool Is(CompanyState inState) => this.state == inState;
        public bool IsSpawning() => this.state == CompanyState.Spawning;
        public bool IsSpawned() => this.state == CompanyState.Spawned;
        public bool IsRemoved() => this.state == CompanyState.Removed;

        public void Update()
        {             
            bool isPlayerNear = World.IsPlayerInRadius(position, spawnRadius);

            if (isPlayerNear && Is(CompanyState.Removed))
            {                
                Spawn();                                
            }

            if (!isPlayerNear && Is(CompanyState.Spawned))
            {
                Remove();                
            }

            if (IsSpawned()) UpdateSpawned(); 
        }

        protected abstract void Spawn();

        protected abstract void Remove();

        protected abstract void UpdateSpawned();
    }
}
