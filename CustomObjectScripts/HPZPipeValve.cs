namespace FP2_Hidden_Palace_Mod.CustomObjectScripts
{
    internal class HPZPipeValve : FPBaseObject
    {
        // FPBaseObject stuff.
        public static int classID = -1;
        public FPObjectState state;
        private bool isValidatedInObjectList;
        public FPHitBox hbTouch;

        // The animator for this valve.
        private Animator animator;

        // Timer used for the water sprout.
        private float timeCounter;

        // A list of game objects that make up this valve's water sprout.
        private readonly List<GameObject> sproutPieces = [];

        // The index of the currently active water sprout segment.
        private int pieceIndex;

        private new void Start()
        {
            // Set the state to the default one.
            state = State_Default;

            // Set up the valve's hitbox.
            hbTouch.left = -32;
            hbTouch.top = 32;
            hbTouch.right = 32;
            hbTouch.bottom = -32;
            hbTouch.enabled = true;
            hbTouch.visible = true;

            // Start the FPBaseObject setup.
            base.Start();
            classID = FPStage.RegisterObjectType(this, GetType(), 0);
            objectID = classID;

            // Get the animator for the valve sprite.
            animator = GetComponent<Animator>();

            // Loop through each child object and add it to the sprout pieces list.
            foreach (Transform child in gameObject.transform)
                sproutPieces.Add(child.gameObject);
        }

        private void Update()
        {
            // Validate this object in the stage list if it hasn't already been.
            if (!isValidatedInObjectList && FPStage.objectsRegistered)
                isValidatedInObjectList = FPStage.ValidateStageListPos(this);

            // Invoke the current state if it isn't null.
            state?.Invoke();
        }

        private void State_Default()
        {
            // Set up a value to hold an object.
            FPBaseObject objRef = null;

            // Loop through each player object in the stage.
            while (FPStage.ForEach(FPPlayer.classID, ref objRef))
            {
                // Get a reference to this player.
                FPPlayer fPPlayer = (FPPlayer)objRef;

                // Check if the player's collider has overlapped this valve's.
                if (FPCollision.CheckOOBB(this, hbTouch, objRef, fPPlayer.hbTouch))
                {
                    // Code copied from the Spring object.
                    if (fPPlayer.state == new FPObjectState(fPPlayer.State_Lilac_Blink))
                        fPPlayer.Action_StopBlink();
                    fPPlayer.state = fPPlayer.State_InAir;
                    fPPlayer.SetPlayerAnimation("Spring");
                    fPPlayer.velocity.y = 32;
                    fPPlayer.jumpReleaseFlag = false;
                    fPPlayer.jumpAbilityFlag = false;
                    fPPlayer.boostCount = 0;
                    if (fPPlayer.onGround)
                    {
                        fPPlayer.angle = 0f;
                        fPPlayer.velocity.x = fPPlayer.groundVel * FPCommon.GetCleanCos(fPPlayer.groundAngle * ((float)Math.PI / 180f));
                        fPPlayer.onGround = false;
                    }

                    // Play the valve's spinning animation.
                    animator.Play("Spin");

                    // Play the geyser sound from the asset bundle.
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_watergeyser"));

                    // Reset the piece index.
                    pieceIndex = 0;

                    // Reset the time counter.
                    timeCounter = 0;

                    // Swap to the sprout state.
                    state = State_Sprout;
                }
            }
        }

        private void State_Sprout()
        {
            // Increment the time counter by the delta time in FPStage.
            timeCounter += FPStage.deltaTime;

            // Stop the function here if we haven't reached a time of 1.
            // This doesn't help if the game is running slower than 60FPS, but should help framerates above that.
            if (timeCounter < 1)
                return;

            // Check if piece index is less than 17.
            if (pieceIndex < 17)
            {
                // If we're not processing the first piece (the actual splash), then move that piece up by 32 units.
                if (pieceIndex != 0)
                    sproutPieces[0].transform.position = new Vector3(sproutPieces[0].transform.position.x, sproutPieces[0].transform.position.y + 32, sproutPieces[0].transform.position.z);

                // Enable the sprite for the current piece of the sprout.
                sproutPieces[pieceIndex].GetComponent<SpriteRenderer>().enabled = true;

                // Increment our piece index.
                pieceIndex++;

                // Stop the function here so we stay in the sprout state.
                return;
            }

            // Reset the time counter.
            timeCounter = 0;

            // Swap to the waiting state.
            state = State_Wait;
        }

        private void State_Wait()
        {
            // Increment the time counter by the game's delta time.
            timeCounter += Time.deltaTime;

            // Check if the time counter has reached 1.
            if (timeCounter >= 1f)
            {
                // Swap to the revert state.
                state = State_Revert;

                // Set piece index down to 16.
                pieceIndex = 16;

                // Reset the time counter back to 0.
                timeCounter = 0;
            }
        }

        private void State_Revert()
        {
            // Increment the time counter by the delta time in FPStage.
            timeCounter += FPStage.deltaTime;

            // Stop the function here if we haven't reached a time of 1.
            // This doesn't help if the game is running slower than 60FPS, but should help framerates above that.
            if (timeCounter < 1)
                return;

            // Disable the sprite renderer on this piece.
            sproutPieces[pieceIndex].GetComponent<SpriteRenderer>().enabled = false;

            // Check if we're processing a sprout piece other than the first one.
            if (pieceIndex > 0)
            {
                // Move the first piece (the actual splash) down by 32 units.
                sproutPieces[0].transform.position = new Vector3(sproutPieces[0].transform.position.x, sproutPieces[0].transform.position.y - 32, sproutPieces[0].transform.position.z);
                
                // Decrement our piece index.
                pieceIndex--;
            }

            // If we have just processed the first piece, then revert back to the default state.
            else
                state = State_Default;
        }
    }
}
