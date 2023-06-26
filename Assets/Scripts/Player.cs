using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class Player : MonoBehaviour, IHasStats
	{

		public static Player Instance;
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		public Transform GunPosition;
		public EntityStats Stats;
		public GameObject bulletPrefab;

		public delegate void PlayerHpDelegate(int hp, int max);

		public static PlayerHpDelegate OnHpChanged;
		public static PlayerHpDelegate OnShooted;
		public ParticleSystem shotEffect;
		public int bullets = 50;
		public AudioSource shootAudio;

		public Animator gunAnimator;
		public CinemachineVirtualCamera camera;
		public int exp;
		public int maxExp;
		public int gold;
		public static PlayerHpDelegate OnExpChanged;
		public static PlayerHpDelegate OnLevelChanged;

		public bool ACTIVE = true;
	
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		public float timeToHeal = 10;
		public int HealBonus = 0;


		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			Instance = this;
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			
			Stats = PlayerClass.GetStats(ClassType.Attacker);
			
		}

		public void AddAmmo(int count)
		{
			bullets += count;
			OnShooted?.Invoke(bullets, 0);
		}

		public void AddHpWithBonus(int count)
		{
			AddHp(count + HealBonus);
		}
		public void AddHp(int count)
		{
			Stats.Health += count;
			if (Stats.Health > Stats.MaxHealth)
				Stats.Health = Stats.MaxHealth;
			
			OnHpChanged?.Invoke(Stats.Health,Stats.MaxHealth);
		}
		
		public void AddMaxHp()
		{
			Stats.MaxHealth += 25;
			OnHpChanged?.Invoke(Stats.Health,Stats.MaxHealth);
		}
		
		public void AddDamage()
		{
			Stats.Damage +=  (int)(Stats.Damage * 0.4f) + Random.Range(1,10);
			
		}

		public void AddHealSpeed()
		{
			timeToHeal -= 2f;
			HealBonus++;
			if (timeToHeal <= 0) timeToHeal = 1f;
		}

		public void AddSpeed()
		{
			MoveSpeed += 0.7f;
			SprintSpeed += 0.7f;
		}
		public void AddJump()
		{
			JumpHeight += 0.4f;
		}

		public void AddExp(int count)
		{
			exp += count;
			start:
			if (exp >= maxExp)
			{
				int dif = exp - maxExp;

				maxExp = (int)(maxExp + (maxExp * 0.3f));
				exp = dif;
				AddLevel();
				goto start;
			}
			
			OnExpChanged?.Invoke(exp, maxExp);
		}

		public void AddLevel()
		{
			gold++;
			OnLevelChanged?.Invoke(gold, 0);
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			GameInput.Input.Player.Shot.performed += _ => OnShoot();

			StartCoroutine(Heal());
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			if(!ACTIVE) return;
			Zoom();
			Move();

		}

		IEnumerator Heal()
		{
			yield return new WaitForSeconds(timeToHeal);
			if(HealBonus == 0)
			AddHp(0);
			else
			{
				AddHp(1);
			}
			StartCoroutine(Heal());
		}

		private void Zoom()
		{
			
			if (GameInput.Input.Player.Zoom.IsPressed())
			{
				if(camera.m_Lens.FieldOfView != 60)
				camera.m_Lens.FieldOfView = 60;
				return;
			}
			else
			{
				if(camera.m_Lens.FieldOfView != 80)
					camera.m_Lens.FieldOfView = 80;
			}
		}

		private void OnShoot()
		{
			if(!ACTIVE) return;
			if(bullets <= 0 ) return;
			
				gunAnimator.SetBool("attack",true);
				if(bullets==0) return;
				Shoot();
			
		}

		IEnumerator PlayShot()
		{
			shotEffect.Play();
			yield return new WaitForSeconds(0.1f);
			shotEffect.Stop();
			gunAnimator.SetBool("attack",false);
		}

		private void Shoot()
		{
			StartCoroutine(PlayShot());
			bullets--;
			OnShooted?.Invoke(bullets, 0);

			GameObject ga = Instantiate(Resources.Load<GameObject>("Bullet"));
			ga.transform.position =  GunPosition.position; 
			ga.transform.rotation =  GunPosition.rotation;
			
			//Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/2, 0));
			
			ga.GetComponent<Bullet>().AddForce(GunPosition);
			
		
			shootAudio.Play();
	
		}

		private void LateUpdate()
		{
			if(!ACTIVE) return;
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			if (GameInput.Input.Player.Sprint.IsPressed())
			{
				gunAnimator.SetBool("run",true);
			}
			else
			{
				gunAnimator.SetBool("run",false);
			}

			

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void SetDamage(int damage)
		{
       
			Stats.Health -= damage;
			OnHpChanged?.Invoke(Stats.Health, Stats.MaxHealth);
			if (Stats.Health <= 0)
			{
				Stats.Health = 0;
				OnHpChanged?.Invoke(Stats.Health, Stats.MaxHealth);
				
				Flag.Instance.Lose();
			}
		}

		public Vector3 GetCenter()
		{
			return transform.position + Vector3.up;
		}

		public EntityStats GetStats()
		{
			return Stats;
		}
	}
}