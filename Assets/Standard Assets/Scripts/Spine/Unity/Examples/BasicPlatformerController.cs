using UnityEngine;

namespace Spine.Unity.Examples
{
	[RequireComponent(typeof(CharacterController))]
	public class BasicPlatformerController : MonoBehaviour
	{
		[Header("Controls")]
		public string XAxis = "Horizontal";

		public string YAxis = "Vertical";

		public string JumpButton = "Jump";

		[Header("Moving")]
		public float walkSpeed = 1.5f;

		public float runSpeed = 7f;

		public float gravityScale = 6.6f;

		[Header("Jumping")]
		public float jumpSpeed = 25f;

		public float jumpDuration = 0.5f;

		public float jumpInterruptFactor = 100f;

		public float forceCrouchVelocity = 25f;

		public float forceCrouchDuration = 0.5f;

		[Header("Graphics")]
		public SkeletonAnimation skeletonAnimation;

		[Header("Animation")]
		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string walkName = "Walk";

		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string runName = "Run";

		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string idleName = "Idle";

		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string jumpName = "Jump";

		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string fallName = "Fall";

		[SpineAnimation("", "skeletonAnimation", true, false)]
		public string crouchName = "Crouch";

		[Header("Effects")]
		public AudioSource jumpAudioSource;

		public AudioSource hardfallAudioSource;

		public AudioSource footstepAudioSource;

		public ParticleSystem landParticles;

		[SpineEvent("", "", true, false)]
		public string footstepEventName = "Footstep";

		private CharacterController controller;

		private Vector3 velocity = default(Vector3);

		private float jumpEndTime;

		private bool jumpInterrupt;

		private float forceCrouchEndTime;

		private Vector2 input;

		private bool wasGrounded;

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		private void Start()
		{
			skeletonAnimation.AnimationState.Event += HandleEvent;
		}

		private void HandleEvent(TrackEntry trackEntry, Event e)
		{
			if (e.Data.Name == footstepEventName)
			{
				footstepAudioSource.Stop();
				footstepAudioSource.pitch = GetRandomPitch(0.2f);
				footstepAudioSource.Play();
			}
		}

		private static float GetRandomPitch(float maxOffset)
		{
			return 1f + UnityEngine.Random.Range(0f - maxOffset, maxOffset);
		}

		private void Update()
		{
			input.x = UnityEngine.Input.GetAxis(XAxis);
			input.y = UnityEngine.Input.GetAxis(YAxis);
			bool flag = (controller.isGrounded && input.y < -0.5f) || forceCrouchEndTime > Time.time;
			velocity.x = 0f;
			float deltaTime = Time.deltaTime;
			if (!flag)
			{
				if (Input.GetButtonDown(JumpButton) && controller.isGrounded)
				{
					jumpAudioSource.Stop();
					jumpAudioSource.Play();
					velocity.y = jumpSpeed;
					jumpEndTime = Time.time + jumpDuration;
				}
				else
				{
					jumpInterrupt |= (Time.time < jumpEndTime && Input.GetButtonUp(JumpButton));
				}
				if (input.x != 0f)
				{
					velocity.x = ((!(Mathf.Abs(input.x) > 0.6f)) ? walkSpeed : runSpeed);
					velocity.x *= Mathf.Sign(input.x);
				}
				if (jumpInterrupt)
				{
					if (velocity.y > 0f)
					{
						velocity.y = Mathf.MoveTowards(velocity.y, 0f, deltaTime * jumpInterruptFactor);
					}
					else
					{
						jumpInterrupt = false;
					}
				}
			}
			Vector3 vector = Physics.gravity * gravityScale * deltaTime;
			if (controller.isGrounded)
			{
				jumpInterrupt = false;
			}
			else if (wasGrounded)
			{
				if (velocity.y < 0f)
				{
					velocity.y = 0f;
				}
			}
			else
			{
				velocity += vector;
			}
			wasGrounded = controller.isGrounded;
			controller.Move(velocity * deltaTime);
			if (!wasGrounded && controller.isGrounded)
			{
				if (0f - velocity.y > forceCrouchVelocity)
				{
					forceCrouchEndTime = Time.time + forceCrouchDuration;
					hardfallAudioSource.Play();
				}
				else
				{
					footstepAudioSource.Play();
				}
				landParticles.Emit((int)(velocity.y / -9f) + 2);
			}
			if (controller.isGrounded)
			{
				if (flag)
				{
					skeletonAnimation.AnimationName = crouchName;
				}
				else if (input.x == 0f)
				{
					skeletonAnimation.AnimationName = idleName;
				}
				else
				{
					skeletonAnimation.AnimationName = ((!(Mathf.Abs(input.x) > 0.6f)) ? walkName : runName);
				}
			}
			else
			{
				skeletonAnimation.AnimationName = ((!(velocity.y > 0f)) ? fallName : jumpName);
			}
			if (input.x != 0f)
			{
				skeletonAnimation.Skeleton.FlipX = (input.x < 0f);
			}
		}
	}
}
