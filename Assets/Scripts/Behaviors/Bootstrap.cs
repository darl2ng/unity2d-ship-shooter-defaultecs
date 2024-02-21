using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Command;

using Factory;
using Resource;

using Model.Resource;
using Model.UI;
using Model.Shooter;

using System.Network;
using System.UI;
using System.Asset;
using System.Shooter;

using Transform = Model.UI.Transform;
using Screen = Model.UI.Screen;
using UILayer = Model.UI.Layer;
using Collision = Model.Shooter.Collision;
using ColliderLayer = Model.Shooter.Layer;
using Collider2D = Model.Shooter.Collider2D;


namespace Behaviors
{
    /// <summary>
    /// Quick implementation to set up the game.
    /// The main set up of the generic world and loading of assets should be in loading scene that shall load other scenes.
    /// Other scenes should then hold specific gameplay models and processes as serializable components to facilitate edition.
    /// Processes should be exposed to have references from the scene instead of declaring them here.
    /// </summary>
    public sealed class Bootstrap : MonoBehaviour
    {
        [Header("Registries")]
        [SerializeField]
        private AssetRegistry[] _assetRegistries;

        [SerializeField]
        private LevelRegistry _levelRegistry;

        [Header("Input Actions")]
        [SerializeField]
        private InputActionReference _movementAction;

        [SerializeField]
        private InputActionReference _firingAction;

        [Header("Layers")]
        [SerializeField]
        private GameObject _UILayer;

        [SerializeField]
        private GameObject _UnitLayer;

        [Header("Camera")]
        [SerializeField]
        private Camera _camera;

        private readonly List<ISystem<float>> _systems = new();

        private ISystem<float> _scheduler;
        private World _world;
        private EntityCommandRecorder _recorder;
        private IEntityFactory _entityFactory;
        private IAssetFactory _assetFactory;
        private IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<UILayer, GameObject> _containerByLayer = new(2);

        private void Awake()
        {
            _world = new World();
            _recorder = new EntityCommandRecorder();

            _entityFactory = new EntityFactory(_world, (Entity entity) => entity.SetSameAsWorld<Device>());
            _assetFactory = new AssetFactory(_assetRegistries);
            _gameObjectFactory = new GameObjectFactory();

            _containerByLayer.Add(UILayer.Ui, _UILayer);
            _containerByLayer.Add(UILayer.Unit, _UnitLayer);

            var mainScreen = new Screen() { Name = "Main" };
            var menuScreen = new Screen() { Name = "Menu" };

            _world.Set<Device>(new Device() { ActiveScreen = mainScreen });

            var level1 = new Level() { Name = "Level1" };
            var level2 = new Level() { Name = "Level2" };
            _world.Set<Game>(new Game() { ActiveLevel = level1 });

            _world.Set<Score>(new Score() { Value = 0, MaxValue = 0 });
            _world.Set<Lives>(new Lives() { Value = 3, MaxValue = 3 });

            _entityFactory.Create<Level>(level1);
            _entityFactory.Create<Level>(level2);

            void OnGameEnded(in bool message)
            {

                _recorder.Execute();

                if (message)
                {
                    _world.Get<Game>().Result = "Well done!";
                    Debug.Log("Game completed");
                }
                else
                {
                    _world.Get<Game>().Result = "Game Over!";
                    var lives = _world.Get<Lives>();
                    if (lives.Value > 0) lives.Value--;
                    Debug.Log("Game Failed");
                }
                _world.Get<Score>().Value = 0;
                _world.Get<Device>().ActiveScreen = menuScreen;
            }

            void OnReplayClicked()
            {
                var device = _world.Get<Device>();
                device.ActiveScreen = mainScreen;
                var game = _world.Get<Game>();
                game.Completed = false;
                game.ActiveLevel = level1;
                _world.Get<Score>().Value = 0;
                _world.Get<Lives>().Value = 3;                
            }

            void OnCollisionDetected(Entity entity, Entity collidedEntity)
            {
                if (entity.Has<Attack>() && collidedEntity.Has<Health>())
                {
                    var collisionEntity = _entityFactory.Create<Collision>();
                    collisionEntity.SetSameAs<Attack>(entity);
                    collisionEntity.SetSameAs<Health>(collidedEntity);
                }
            }

            ButtonHelper.Register("replay", OnReplayClicked);
            LabelHelper.Register("CurrentScore", () => _world.Get<Score>().Value.ToString());
            LabelHelper.Register("MaxScore", () => _world.Get<Score>().MaxValue.ToString());
            LabelHelper.Register("CurrentLives", () => _world.Get<Lives>().Value.ToString());
            LabelHelper.Register("MaxLives", () => _world.Get<Lives>().MaxValue.ToString());
            LabelHelper.Register("GameResult", () => _world.Get<Game>().Result);

            _world.Subscribe<bool>(OnGameEnded);

            _systems.Add(new NetworkSystem(_world));
            _systems.Add(new AssetSystem(_world, _assetRegistries, _gameObjectFactory));
            _systems.Add(new ScreenSystem(_world));
            _systems.Add(new MovementSystem(_world));
            _systems.Add(new LayeredRenderSystem(_world, _containerByLayer));
            _systems.Add(new SpawningSystem(_world, _entityFactory, _assetFactory));
            _systems.Add(new RoundSystem(_world, _recorder));
            _systems.Add(new LevelSystem(_world, _levelRegistry));
            _systems.Add(new GameSystem(_world));
            _systems.Add(new PlayerSystem(_world, _movementAction));
            _systems.Add(new BulletSystem(_world, _entityFactory, _assetFactory, _firingAction));
            _systems.Add(new DisposalSystem(_world, _camera));
            _systems.Add(new AttackingSystem(_world));
            _systems.Add(new HealthSystem(_world));
            _systems.Add(new CollisionSystem(_world, OnCollisionDetected));

            _entityFactory.Create<Asset, Transform, UILayer, Screen>(
                _assetFactory.GetOrCreate(mainScreen.Name),
                new Transform() { Position = new Vector3(0, 0, 0) },
                UILayer.Ui,
                mainScreen);
            _entityFactory.Create<Asset, Transform, UILayer, Screen>(
                _assetFactory.GetOrCreate(menuScreen.Name),
                new Transform() { Position = new Vector3(0, 2, 0) },
                UILayer.Ui,
                menuScreen);

            var playerEntity = _entityFactory.Create<Asset, Transform, UILayer, Movement, Screen, Player, Collider2D, ColliderLayer, Health>(
                _assetFactory.GetOrCreate(Player.ASSET),
                new Transform() { Position = new Vector3(0, -3, 0) },
                UILayer.Unit,
                new Movement() { Power = Player.POWER },
                mainScreen,
                new Player(),
                new Collider2D(Player.SIZE, Player.OFFSET),
                    ColliderLayer.Friend,
                    new Health() { Value = Player.HEALTH }
                );

            LabelHelper.Register("CurrentHealth", () => playerEntity.Get<Health>().Value.ToString("0.00"));
            LabelHelper.Register("MaxHealth", () => Player.HEALTH.ToString("0.00"));
        }

        void Start() => _scheduler = new SequentialSystem<float>(_systems);

        void Update() => _scheduler?.Update(Time.deltaTime);
    }
}
