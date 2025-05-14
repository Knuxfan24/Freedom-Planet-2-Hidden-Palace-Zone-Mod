namespace FP2_Hidden_Palace_Mod.CustomObjectScripts
{
    public class HPZSinkingPlatform : FPBaseObject
    {
        // FPBaseObject stuff.
        public static int classID = -1;
        public FPObjectState state;
        private bool isValidatedInObjectList;

        // Where this platform started.
        private Vector2 returnPosition;

        // The collider for this platform.
        private BoxCollider2D colliderPlatform;
        
        // Whether this platform can currently be moved by the player's touch.
        private bool canMove = true;

        // How fast the platform rises.
        private float upwardsVelocity = 1f;

        private new void Start()
        {
            // Set the state to the default one.
            state = State_Default;

            // Start the FPBaseObject setup.
            base.Start();
            classID = FPStage.RegisterObjectType(this, GetType(), 0);
            objectID = classID;

            // Get the collider for this platform.
            colliderPlatform = GetComponent<BoxCollider2D>();

            // Store this platform's starting position.
            returnPosition = position;
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
            // Check for the player object.
            if (FPStage.ConfirmClassWithPoolTypeID(typeof(FPPlayer), FPPlayer.classID))
            {
                // Set up a value to hold an object.
                FPBaseObject objRef = null;

                // Loop through each player object in the stage.
                while (FPStage.ForEach(FPPlayer.classID, ref objRef))
                {
                    // Get a reference to this player.
                    FPPlayer fPPlayer = (FPPlayer)objRef;

                    // Check if the player is grounded and colliding with this platform's collider.
                    if (fPPlayer.colliderGround == colliderPlatform && fPPlayer.onGround)
                    {
                        // Check if the platform can move, if so, push it down by two units multiplied by the delta time.
                        // This does mean that the platform can stick if the player ends up on both it and another surface, but oh well.
                        if (canMove)
                            position = new Vector2(position.x, position.y - (2f * FPStage.deltaTime));

                    }
                    else
                    {
                        // Check if this platform is lower down than its start position.
                        if (position.y < returnPosition.y)
                        {
                            // Disable the move flag.
                            canMove = false;

                            // Add the upwards velocity to this platform's y position.
                            position = new Vector2(position.x, position.y + upwardsVelocity);

                            // Increment the upwards velocity by the delta time if its below eight.
                            if (upwardsVelocity < 8)
                                upwardsVelocity += FPStage.deltaTime;
                        }
                        else
                        {
                            // Snap to the return position.
                            position = returnPosition;

                            // Reset the upwards velocity and can move flag.
                            upwardsVelocity = 1f;
                            canMove = true;
                        }

                    }
                }
            }
        }
    }
}
