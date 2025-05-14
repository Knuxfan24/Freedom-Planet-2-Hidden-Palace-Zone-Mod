using FP2_Hidden_Palace_Mod;
using FP2_Hidden_Palace_Mod.Patchers;

public enum MonitorTypes
{
    WoodShield = 0,
    EarthShield = 1,
    AquaShield = 2,
    FireShield = 3,
    MetalShield = 4,
    Crystals = 10,
    LifePetals = 11,
    Invincibility = 12,
    BloodCrystals = 13,
    ExtraLife = 20,
    Lilac1UP = 21,
    Carol1UP = 22,
    Milla1UP = 23,
    Neera1UP = 24,
    Merga = 30
}

public class ItemMonitor : FPBaseObject
{
    // FPBaseObject stuff.
    public static int classID = -1;
    public FPObjectState state;
    private bool isValidatedInObjectList;

    // The hitboxes for this monitor.
    public FPHitBox hitbox;
    private BoxCollider2D boxCollider;

    // A game object to do things to upon collecting this monitor's item.
    public FPBaseObject targetObject;

    // The animator for the monitor frame and the display.
    private Animator animator;
    private Animator childAnimator;

    // The game object for this monitor's display.
    private GameObject itemDisplay;

    // The type of item in this monitor.
    public MonitorTypes itemType;

    // The animation to play for this monitor's display from its animator.
    private string animationName;

    // A timer used for moving/destroying the monitor display.
    private float timer;

    // A value used to determine if the Rainbow Charm has already changed this monitor's type.
    private bool rainbowCharmSwapped;

    private new void Start()
    {
        state = State_Idle;

        // Set up the hitbox, depending on if the box collider exists or not.
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            hitbox.left = boxCollider.size.x * -0.5f - 4f + boxCollider.offset.x;
            hitbox.top = boxCollider.size.y * 0.5f + 4f + boxCollider.offset.y;
            hitbox.right = boxCollider.size.x * 0.5f + 4f + boxCollider.offset.x;
            hitbox.bottom = boxCollider.size.y * -0.5f - 4f + boxCollider.offset.y;
        }
        else
        {
            hitbox.left = -30f;
            hitbox.top = 28f;
            hitbox.right = 30f;
            hitbox.bottom = -32f;
        }
        hitbox.enabled = true;

        // Get a reference to this monitor's own animator.
        animator = GetComponent<Animator>();

        // Get this monitor's display and the animator for said display.
        itemDisplay = gameObject.transform.GetChild(0).gameObject;
        childAnimator = itemDisplay.GetComponent<Animator>();

        // Start the FPBaseObject setup.
        base.Start();
        classID = FPStage.RegisterObjectType(this, GetType(), 0);
        objectID = classID;
    }

    private void Update()
    {
        // Validate this object in the stage list if it hasn't already been.
        if (!isValidatedInObjectList && FPStage.objectsRegistered)
            isValidatedInObjectList = FPStage.ValidateStageListPos(this);

        // Invoke the current state if it isn't null.
        state?.Invoke();
    }

    private void State_Idle()
    {
        // Set up a value to hold an object.
        FPBaseObject objRef = null;

        // Loop through each player object in the stage.
        while (FPStage.ForEach(FPPlayer.classID, ref objRef))
        {
            // Get a reference to this player.
            FPPlayer fPPlayer = (FPPlayer)objRef;

            // If the player has the Items To Bombs Brave Stone equipped, then change this monitor's type to the Merga one.
            if (fPPlayer.IsPowerupActive(FPPowerup.ITEMS_TO_BOMBS))
                itemType = MonitorTypes.Merga;

            // Check if the player has the Rainbow Charm equipped.
            if (fPPlayer.IsPowerupActive(FPPowerup.RAINBOW_CHARM))
            {
                // Check if this monitor's type is one that should be swapped.
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                {
                    // Check that we haven't already swapped this monitor's type.
                    if (!rainbowCharmSwapped)
                    {
                        // Swap this monitor's type depending on a random number.
                        switch (UnityEngine.Random.Range(0, 6))
                        {
                            case 0: itemType = MonitorTypes.WoodShield; break;
                            case 1: itemType = MonitorTypes.EarthShield; break;
                            case 2: itemType = MonitorTypes.AquaShield; break;
                            case 3: itemType = MonitorTypes.FireShield; break;
                            case 4: itemType = MonitorTypes.MetalShield; break;
                            case 5: itemType = MonitorTypes.Invincibility; break;
                        }

                        // Flag this monitor as one that has been swapped already.
                        rainbowCharmSwapped = true;
                    }
                }
            }

            // Handle swapping this monitor if a shield charm is equipped.
            if (fPPlayer.IsPowerupActive(FPPowerup.EARTH_CHARM))
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                    itemType = MonitorTypes.EarthShield;
            if (fPPlayer.IsPowerupActive(FPPowerup.FIRE_CHARM))
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                    itemType = MonitorTypes.FireShield;
            if (fPPlayer.IsPowerupActive(FPPowerup.METAL_CHARM))
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                    itemType = MonitorTypes.MetalShield;
            if (fPPlayer.IsPowerupActive(FPPowerup.WATER_CHARM))
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                    itemType = MonitorTypes.AquaShield;
            if (fPPlayer.IsPowerupActive(FPPowerup.WOOD_CHARM))
                if (itemType == MonitorTypes.WoodShield || itemType == MonitorTypes.EarthShield || itemType == MonitorTypes.AquaShield || itemType == MonitorTypes.FireShield || itemType == MonitorTypes.MetalShield || itemType == MonitorTypes.Invincibility)
                    itemType = MonitorTypes.WoodShield;

            // Handle the petal based Brave Stones.
            if (fPPlayer.IsPowerupActive(FPPowerup.NO_PETALS) && itemType == MonitorTypes.LifePetals)
                itemType = MonitorTypes.Crystals;
            if (fPPlayer.IsPowerupActive(FPPowerup.MORE_PETALS) && itemType == MonitorTypes.Crystals)
                itemType = MonitorTypes.LifePetals;
            if (fPPlayer.IsPowerupActive(FPPowerup.MORE_PETALS) && fPPlayer.IsPowerupActive(FPPowerup.NO_PETALS) && (itemType == MonitorTypes.Crystals || itemType == MonitorTypes.LifePetals))
                itemType = MonitorTypes.BloodCrystals;
        }

        // Select the correct animation depending on the monitor type.
        switch (itemType)
        {
            case MonitorTypes.WoodShield: animationName = "WoodShield"; break;
            case MonitorTypes.EarthShield: animationName = "EarthShield"; break;
            case MonitorTypes.AquaShield: animationName = "AquaShield"; break;
            case MonitorTypes.FireShield: animationName = "FireShield"; break;
            case MonitorTypes.MetalShield: animationName = "MetalShield"; break;

            case MonitorTypes.Crystals: animationName = "Crystals"; break;
            case MonitorTypes.LifePetals: animationName = "LifePetals"; break;
            case MonitorTypes.Invincibility: animationName = "Invincibility"; break;
            case MonitorTypes.BloodCrystals: animationName = "BloodCrystals"; break;

            case MonitorTypes.ExtraLife:
                switch (FPSaveManager.character)
                {
                    case FPCharacterID.LILAC: animationName = "1UP_Lilac"; break;
                    case FPCharacterID.CAROL:
                    case FPCharacterID.BIKECAROL: animationName = "1UP_Carol"; break;
                    case FPCharacterID.MILLA: animationName = "1UP_Milla"; break;
                    case FPCharacterID.NEERA: animationName = "1UP_Neera"; break;
                    default: animationName = "1UP"; break;
                }
                break;
            case MonitorTypes.Lilac1UP: animationName = "1UP_Lilac"; break;
            case MonitorTypes.Carol1UP: animationName = "1UP_Carol"; break;
            case MonitorTypes.Milla1UP: animationName = "1UP_Milla"; break;
            case MonitorTypes.Neera1UP: animationName = "1UP_Neera"; break;

            case MonitorTypes.Merga: animationName = "Merga"; break;
        }

        // Play the selected animation on the display's animator.
        childAnimator.Play(animationName);

        // Run the collision check.
        InteractWithObjects();
    }

    // Copied from the base game's ItemBox class, with the check for the regular explosion removed.
    private void InteractWithObjects()
    {
        FPBaseObject objRef = null;
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(FPPlayer), FPPlayer.classID))
        {
            while (FPStage.ForEach(FPPlayer.classID, ref objRef))
            {
                FPPlayer fPPlayer = (FPPlayer)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, fPPlayer.hbAttack))
                {
                    fPPlayer.hitStun = Mathf.Max(fPPlayer.hitStun, 4f);
                    FPStage.CreateStageObject(HitSpark.classID, (fPPlayer.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (fPPlayer.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    targetObject = fPPlayer;
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(BoostExplosion), BoostExplosion.classID))
        {
            objRef = null;
            while (FPStage.ForEach(BoostExplosion.classID, ref objRef))
            {
                BoostExplosion boostExplosion = (BoostExplosion)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, boostExplosion.hbAttack))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (boostExplosion.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (boostExplosion.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    targetObject = boostExplosion.parentObject;
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(SmallShockwave), SmallShockwave.classID))
        {
            objRef = null;
            while (FPStage.ForEach(SmallShockwave.classID, ref objRef))
            {
                SmallShockwave smallShockwave = (SmallShockwave)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, smallShockwave.hbTouch))
                {
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(CarolJumpDisc), CarolJumpDisc.classID))
        {
            objRef = null;
            while (FPStage.ForEach(CarolJumpDisc.classID, ref objRef))
            {
                CarolJumpDisc carolJumpDisc = (CarolJumpDisc)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, carolJumpDisc.hbTouch))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (carolJumpDisc.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (carolJumpDisc.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    if (carolJumpDisc.parentObject != null)
                    {
                        targetObject = carolJumpDisc.parentObject;
                    }
                    else if (carolJumpDisc.parentBoss != null)
                    {
                        targetObject = carolJumpDisc.parentBoss;
                    }
                    else
                    {
                        targetObject = null;
                    }
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(MillaShield), MillaShield.classID))
        {
            objRef = null;
            while (FPStage.ForEach(MillaShield.classID, ref objRef))
            {
                MillaShield millaShield = (MillaShield)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, millaShield.hbAttack))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (millaShield.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (millaShield.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    if (millaShield.parentObject != null)
                    {
                        targetObject = millaShield.parentObject;
                    }
                    else if (millaShield.parentBoss != null)
                    {
                        targetObject = millaShield.parentBoss;
                    }
                    else if (millaShield.parentShadow != null)
                    {
                        targetObject = millaShield.parentShadow;
                    }
                    else
                    {
                        targetObject = null;
                    }
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(MillaCube), MillaCube.classID))
        {
            objRef = null;
            while (FPStage.ForEach(MillaCube.classID, ref objRef))
            {
                MillaCube millaCube = (MillaCube)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, millaCube.hbTouch))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (millaCube.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-10f, 10f), (millaCube.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-10f, 10f));
                    millaCube.enemyCollision = true;
                    targetObject = millaCube.parentObject;
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(NeeraFreeze), NeeraFreeze.classID))
        {
            objRef = null;
            while (FPStage.ForEach(NeeraFreeze.classID, ref objRef))
            {
                NeeraFreeze neeraFreeze = (NeeraFreeze)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, neeraFreeze.hbTouch))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (neeraFreeze.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (neeraFreeze.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    targetObject = neeraFreeze.parentObject;
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(ProjectileBasic), ProjectileBasic.classID))
        {
            objRef = null;
            while (FPStage.ForEach(ProjectileBasic.classID, ref objRef))
            {
                ProjectileBasic projectileBasic = (ProjectileBasic)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, projectileBasic.hbTouch))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (projectileBasic.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-10f, 10f), (projectileBasic.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-10f, 10f));
                    projectileBasic.collision = true;
                    targetObject = projectileBasic.parentObject;
                    BoxHit();
                }
            }
        }
        if (FPStage.ConfirmClassWithPoolTypeID(typeof(PHDrillRide), PHDrillRide.classID))
        {
            objRef = null;
            while (FPStage.ForEach(PHDrillRide.classID, ref objRef))
            {
                PHDrillRide pHDrillRide = (PHDrillRide)objRef;
                if (FPCollision.CheckOOBB(this, hitbox, objRef, pHDrillRide.hbAttack))
                {
                    FPStage.CreateStageObject(HitSpark.classID, (pHDrillRide.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (pHDrillRide.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                    targetObject = pHDrillRide.targetPlayer;
                    BoxHit();
                }
            }
        }
        if (!FPStage.ConfirmClassWithPoolTypeID(typeof(PlayerBFF2000), PlayerBFF2000.classID))
        {
            return;
        }
        objRef = null;
        while (FPStage.ForEach(PlayerBFF2000.classID, ref objRef))
        {
            PlayerBFF2000 playerBFF = (PlayerBFF2000)objRef;
            if (FPCollision.CheckOOBB(this, hitbox, objRef, playerBFF.collisionBox))
            {
                FPStage.CreateStageObject(HitSpark.classID, (playerBFF.position.x + position.x * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f), (playerBFF.position.y + position.y * 2f) * (1f / 3f) + UnityEngine.Random.Range(-20f, 20f));
                targetObject = playerBFF.targetPlayer;
                BoxHit();
            }
        }
    }

    private void State_ItemMove()
    {
        // Increment the timer based on the delta time.
        timer += Time.deltaTime;

        // While the timer is below or equal to 0.45, move the display up by 3 pixels.
        if (timer <= 0.45f)
            itemDisplay.transform.position = new Vector3(itemDisplay.transform.position.x, itemDisplay.transform.position.y + (3f * FPStage.deltaTime), itemDisplay.transform.position.z);
        
        // If the timer is equal to or above 0.55, then move to the GetItem state instead.
        if (timer >= 0.55f)
            state = State_GetItem;
    }

    private void State_GetItem()
    {
        // Reset the timer.
        timer = 0f;

        // Handle this monitor's item type.
        switch (itemType)
        {
            // If this monitor is a shield, then use the GiveShield function.
            case MonitorTypes.WoodShield: GiveShield("Wood"); break;
            case MonitorTypes.EarthShield: GiveShield("Earth"); break;
            case MonitorTypes.AquaShield: GiveShield("Water"); break;
            case MonitorTypes.FireShield: GiveShield("Fire"); break;
            case MonitorTypes.MetalShield: GiveShield("Metal"); break;

            // If this is a crystal monitor, then play the ring sound and give the player 10 crystals.
            case MonitorTypes.Crystals:
                FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_ring"));
                for (int i = 0; i < 10; i++)
                    GiveCrystal();
                break;

            // If this is a life petal monitor, then play the Mania hyper ring sound and give the player 3.5 health points.
            case MonitorTypes.LifePetals:
                FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("mania_hyperring"));

                // Loop through so we can check if the player should be given a health point or a crystal.
                for (int i = 0; i < 7; i++)
                {
                    if (FPPlayerPatcher.player.health >= FPPlayerPatcher.player.healthMax || FPPlayerPatcher.player.IsPowerupActive(FPPowerup.ONE_HIT_KO))
                        GiveCrystal();
                    else
                        FPPlayerPatcher.player.health = Mathf.Min(FPPlayerPatcher.player.health + 0.5f, FPPlayerPatcher.player.healthMax);
                }
                break;

            // Invincibility, mostly copied from the game's own invincibility item box.
            case MonitorTypes.Invincibility:
                // Set the invincibility and flash timers.
                FPPlayerPatcher.player.invincibilityTime = Mathf.Max(FPPlayerPatcher.player.invincibilityTime, 1200f);
                FPPlayerPatcher.player.flashTime = Mathf.Max(FPPlayerPatcher.player.flashTime, 1200f);

                // Spawn the two stars.
                InvincibilityStar invincibilityStar = (InvincibilityStar)FPStage.CreateStageObject(InvincibilityStar.classID, -100f, -100f);
                invincibilityStar.parentObject = FPPlayerPatcher.player;
                InvincibilityStar invincibilityStar2 = (InvincibilityStar)FPStage.CreateStageObject(InvincibilityStar.classID, -100f, -100f);
                invincibilityStar2.parentObject = FPPlayerPatcher.player;
                invincibilityStar2.rotation = 180f;

                // Play the unused invincibility jingle, but stop any playing ones first so they don't overlap.
                FPAudio.StopJingle();
                FPAudio.PlayJingle(0);
                break;

            // If this monitor is a blood crystal one, then just do the same behaviour as both the crystal and life petal ones.
            case MonitorTypes.BloodCrystals:
                FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("mania_hyperring"));
                for (int i = 0; i < 3; i++)
                {
                    if (FPPlayerPatcher.player.health >= FPPlayerPatcher.player.healthMax || FPPlayerPatcher.player.IsPowerupActive(FPPowerup.ONE_HIT_KO))
                        GiveCrystal();
                    else
                        FPPlayerPatcher.player.health = Mathf.Min(FPPlayerPatcher.player.health + 0.5f, FPPlayerPatcher.player.healthMax);
                }
                FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_ring"));
                for (int i = 0; i < 5; i++)
                    GiveCrystal();
                break;

            // Extra life, using a lot of code from the base game.
            case MonitorTypes.ExtraLife:
            case MonitorTypes.Lilac1UP:
            case MonitorTypes.Carol1UP:
            case MonitorTypes.Milla1UP:
            case MonitorTypes.Neera1UP:
                // Check that the player has less than nine lives before adding one.
                if (FPPlayerPatcher.player.lives < 9)
                    FPPlayerPatcher.player.lives++;

                // Create the +1 that appears beneath the life counter.
                CrystalBonus lifeUpIndicator = (CrystalBonus)FPStage.CreateStageObject(CrystalBonus.classID, 292f, -64f);
                lifeUpIndicator.animator.Play("HUD_Add");
                lifeUpIndicator.duration = 40f;

                // Create the two stars.
                InvincibilityStar extraLifeStar = (InvincibilityStar)FPStage.CreateStageObject(InvincibilityStar.classID, -100f, -100f);
                extraLifeStar.parentObject = FPPlayerPatcher.player;
                extraLifeStar.distance = 320f;
                extraLifeStar.descend = true;
                InvincibilityStar extraLifeStar2 = (InvincibilityStar)FPStage.CreateStageObject(InvincibilityStar.classID, -100f, -100f);
                extraLifeStar2.parentObject = FPPlayerPatcher.player;
                extraLifeStar2.rotation = 180f;
                extraLifeStar2.distance = 320f;
                extraLifeStar2.descend = true;

                // Play the 1UP jingle.
                FPAudio.PlayJingle(3);
                break;

            // If this monitor is a Merga one, then deal one point of damage, knock the player back and create a hit spark on top of them.
            case MonitorTypes.Merga:
                FPPlayerPatcher.player.healthDamage += 1f;
                FPPlayerPatcher.player.hurtKnockbackX = velocity.x * 0.6f;
                FPPlayerPatcher.player.hurtKnockbackY = velocity.y * 0.6f;
                FPPlayerPatcher.player.Action_HitSpark(FPPlayerPatcher.player);
                break;
        }

        // Swap to the Wait to Despawn state.
        state = State_ItemWaitToDespawn;

        // Internal function to give the player a shield.
        void GiveShield(string type)
        {
            // Play the sound and set the shield type depending on which one has been collected.
            switch (type)
            {
                case "Wood":
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_shield"));
                    FPPlayerPatcher.player.shieldID = 0;
                    break;
                case "Earth":
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_shield_thunder"));
                    FPPlayerPatcher.player.shieldID = 1;
                    break;
                case "Water":
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_shield_bubble"));
                    FPPlayerPatcher.player.shieldID = 2;
                    break;
                case "Fire":
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_shield_fire"));
                    FPPlayerPatcher.player.shieldID = 3;
                    break;
                case "Metal":
                    FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_shield_metal"));
                    FPPlayerPatcher.player.shieldID = 4;
                    break;
            }

            // Create the shield orb.
            ShieldOrb shieldOrb = (ShieldOrb)FPStage.CreateStageObject(ShieldOrb.classID, position.x, position.y + 60f);
            shieldOrb.spawnLocation = this;
            shieldOrb.parentObject = FPPlayerPatcher.player;
            shieldOrb.animator.Play(type, 0, 0f);

            // Calculate and set the shield health.
            int shieldHealth = 2 + FPPlayerPatcher.player.potions[5];
            if (FPPlayerPatcher.player.IsPowerupActive(FPPowerup.STRONG_SHIELDS))
                shieldHealth += 3;
            FPPlayerPatcher.player.shieldHealth = Mathf.Min(FPPlayerPatcher.player.shieldHealth + shieldHealth, (int)FPPlayerPatcher.player.healthMax * 2);

            // Create the flash over the player.
            ShieldHit shieldHit = (ShieldHit)FPStage.CreateStageObject(ShieldHit.classID, FPPlayerPatcher.player.position.x, FPPlayerPatcher.player.position.y);
            shieldHit.SetParentObject(FPPlayerPatcher.player);
            shieldHit.remainingDuration = 15f;
        }

        // Internal function to give the player a crystal, mostly copied from the game's own Life Petal code.
        void GiveCrystal()
        {
            FPSaveManager.AddCrystal(FPPlayerPatcher.player);
            FPPlayerPatcher.player.crystalBonusTimer = 40f;
            CrystalBonus crystalBonus = (CrystalBonus)FPStage.CreateStageObject(CrystalBonus.classID, 32f, -64f - (float)FPPlayerPatcher.player.crystalBonus * 16f);
            crystalBonus.duration = 20f;
            FPPlayerPatcher.player.crystalBonus = (FPPlayerPatcher.player.crystalBonus + 1) % 14;
        }
    }

    private void State_ItemWaitToDespawn()
    {
        // Increment the timer based on the delta time.
        timer += Time.deltaTime;

        // Check if the timer has reached 0.5.
        if (timer >= 0.5f)
        {
            // Deactivate the display object.
            itemDisplay.SetActive(false);

            // Swap into the destroyed state.
            state = State_Destroyed;
        }
    }

    private void State_Destroyed()
    {
        // Both of these are done entirely to fix an issue where scrolling a monitor off screen and back on could cause it to reappear but be empty and intangible.
        // Force this monitor to always be active.
        activationMode = FPActivationMode.ALWAYS_ACTIVE;

        // Force this monitor to play the broken animation.
        animator.Play("Broken");
    }

    public void BoxHit()
    {
        // Play the monitor pop sound.
        FPAudio.PlaySfx(Plugin.hpzAssetBundle.LoadAsset<AudioClip>("classic_pop"));

        // Switch to the Item Move state.
        state = State_ItemMove;

        // If the box collider exists, then disable it to make this monitor intangible.
        if (boxCollider != null)
            boxCollider.enabled = false;

        // Disable this monitor's hitbox.
        hitbox.enabled = false;

        // Swap this monitor to the broken animation.
        animator.Play("Broken");

        // Bump up the sorting order of this monitor's display so it appears in front of the remains.
        itemDisplay.GetComponent<SpriteRenderer>().sortingOrder = 1;

        // Set the display's animation to the first frame of whatever it should be displaying.
        childAnimator.speed = 0;
        childAnimator.Play(animationName, -1, 0f);

        // Create an explosion atop this monitor.
        FPStage.CreateStageObject(Explosion.classID, position.x, position.y);
    }
}
