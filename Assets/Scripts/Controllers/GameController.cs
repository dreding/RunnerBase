using System;
using System.Linq;
using Collision;
using Configs;
using CustomInput;
using Cysharp.Threading.Tasks;
using Data;
using DefaultNamespace;
using Interfaces;
using Movers;
using Pool;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        // Serialized fields accessible in the Unity Editor
        [SerializeField] private LevelConfig level;
        [SerializeField] private Transform playerViewPrefab;
        [SerializeField] private CameraFollow cameraController;

        // Private variables for internal use
        private RoadController roadController;
        private BasePlayerMover playerMover;
        private PlayerController player;
        private CollisionChecker collisionChecker;
        private CollisionResolver collisionResolver;
        private BonusPool pool;
        private BonusSpawner bonusSpawner;
        private SwipeDetector inputController;
        public PlayerData PlayerData { get; private set; }

        // Flag to indicate whether the game is paused
        private bool isPaused;

        // Event triggered when player dies
        public Action OnPlayerDie;

        // Start is called before the first frame update
        void Awake()
        {
            // Initialize variables
            isPaused = true;
            roadController = new RoadController(level);
            var startPoint = ((StartPlatformData) level.startPlatform).playerSpawnPoint;
    
            var playerView = Instantiate(playerViewPrefab);
            playerView.transform.position = startPoint.position;
            startPoint.gameObject.SetActive(false);
            player = new PlayerController(playerView, 1, level.cellSize.x + level.cellSize.x / 2f);
            PlayerData = new PlayerData();
            inputController = new SwipeDetector();
            playerMover = new BasePlayerMover();        
            collisionResolver = new CollisionResolver();
            collisionChecker = new CollisionChecker(level.cellSize);
            bonusSpawner = new BonusSpawner(level.bonusSpawnChance, level.flyHeight, level.bonuses);

            // Initialize camera controller
            cameraController.InitCameraFollow(playerView);

            // Subscribe to input events
            inputController.OnSwipeLeft += DoMoveLeft;
            inputController.OnSwipeRight += DoMoveRight;
            collisionResolver.OnBonusCollided += AddingBonus;
            collisionResolver.OnPlayerDie += PlayerDie;
            roadController.OnPlatformCreated += NewPlatformCreated;
        }

        // Update is called once per frame
        void Update()
        {
            if (isPaused)
            {
                return;
            }
            // Using defines for different input based on platform.
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                DoMoveLeft();
            } else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                DoMoveRight();
            }

#else
            inputController.OnUpdate();
#endif
            
            // Update player movement
            if (playerMover is not null)
            {
                if (playerMover.MovePlayer(player, Time.deltaTime, out var newPos))
                {
                    var distance = player.UpdatePlayerPosition(newPos);
                    if (collisionChecker.IsPlayerCollided(player.PlayerBounds, out var result))
                    {
                        collisionResolver.ResolveCollision(player.PlayerBounds, result);
                    }
                    roadController.PlayerPositionChanged(newPos);
                
                    PlayerData.ChangeDistance(distance);
                }
            }
        }

        public void ChangePauseState(bool _pauseState)
        {
            isPaused = _pauseState;
            player.MoveState(!_pauseState);
        }

        /// <summary>
        /// Handle bonus collection
        /// </summary>
        /// <param name="_bonusData"></param>
        private void AddingBonus(FieldBonusData _bonusData)
        {
            switch (_bonusData.BonusType)
            {
                case BonusType.Coin:
                    AddCurrency();
                    break;
                case BonusType.SlowDown:
                    OnChangingMover();
                    playerMover = new SlowPlayerMover(playerMover);
                    break;
                case BonusType.SpeedUp:
                    OnChangingMover();
                    playerMover = new FastPlayerMover(playerMover);
                    break;
                case BonusType.Fly:
                    OnChangingMover();
                    playerMover = new FlyPlayerMover(playerMover, level.flyHeight);
                    player.FlyState(true);
                    break;
                default:
                    Debug.LogError("");
                    playerMover = new BasePlayerMover();
                    break;
            }

            if (playerMover is ExpiringPlayerMover expiringPlayerMover)
            {
                expiringPlayerMover.onMoverExpired += PlayerMoverExpired;
            }

            collisionChecker.RemoveObject(_bonusData);
        }

        private void AddCurrency()
        {
            PlayerData.ChangeCoinsAmount(1);
        }

        private void PlayerDie()
        {
            OnPlayerDie?.Invoke();
            ChangePauseState(true);
            player.PlayerDie();
        }

        // Handle expiration of player mover
        private void PlayerMoverExpired(BasePlayerMover _mover)
        {
            if (_mover is FlyPlayerMover)
            {
                player.FlyState(false);
            }

            if (_mover is ExpiringPlayerMover expiringMover)
            {
                expiringMover.Dispose();
            }
            playerMover = new BasePlayerMover();
        }

        // Clean up when changing player mover
        private void OnChangingMover()
        {
            if (playerMover is FlyPlayerMover)
            {
                player.FlyState(false);
            }

            if (playerMover is ExpiringPlayerMover expiringMover)
            {
                expiringMover.Dispose();
            }
        }

        // Handle platform creation
        private async void NewPlatformCreated(PlatformData _newPlatform)
        {
            await UniTask.NextFrame(PlayerLoopTiming.LastFixedUpdate);
            var bonuses = bonusSpawner.SpawnBonuses(_newPlatform);
            collisionChecker.AddObjects(bonuses.Cast<IFieldObject>().ToList());
            collisionChecker.AddNewPlatform(_newPlatform);
        }

        private void DoMoveRight()
        {
            if (player.TargetLine < level.maxBlocksInRowCount - 1)
            {
                var newLine = player.TargetLine + 1;
                player.ChangeLine(newLine, newLine * level.cellSize.x + level.cellSize.x / 2f);
            }
        }

        private void DoMoveLeft()
        {
            if (player.TargetLine > 0)
            {
                var newLine = player.TargetLine - 1;
                player.ChangeLine(newLine, newLine * level.cellSize.x + level.cellSize.x / 2f);
            }
        }
    }
}
