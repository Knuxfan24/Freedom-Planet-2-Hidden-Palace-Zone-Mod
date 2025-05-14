namespace FP2_Hidden_Palace_Mod.CustomObjectScripts
{
    public class HPZWaterfall : FPBaseObject
    {
        // How many segments this waterfall needs.
        public int segmentCount;

        private new void Start()
        {
            // The position of the top piece (just where the template object itself is placed).
            Vector3 topPosition = gameObject.transform.position;

            // The templates for the waterfall pieces.
            GameObject topSegment = UnityEngine.GameObject.Find("WaterfallTop");
            GameObject middleSegment = UnityEngine.GameObject.Find("WaterfallMiddle");
            GameObject bottomSegment = UnityEngine.GameObject.Find("WaterfallBottom");

            // Create and set the position of the top segment.
            GameObject waterfallTop = UnityEngine.GameObject.Instantiate(topSegment);
            waterfallTop.transform.position = new Vector3(topPosition.x, topPosition.y, topPosition.z);

            // Loop through, create and set the position of each middle segment (we start at 1 in this loop for the sake of the maths).
            for (int segmentIndex = 1; segmentIndex < segmentCount; segmentIndex++)
            {
                GameObject waterfallSegment = UnityEngine.GameObject.Instantiate(middleSegment);
                waterfallSegment.transform.position = new Vector3(topPosition.x, topPosition.y - (32 * segmentIndex), topPosition.z);
            }

            // Create and set the position of the bottom segment.
            GameObject waterfallBottom = UnityEngine.GameObject.Instantiate(bottomSegment);
            waterfallBottom.transform.position = new Vector3(topPosition.x, topPosition.y - (32 * segmentCount) + 16, topPosition.z);

            // Destroy the template object we attached the script to, as its useless now.
            GameObject.Destroy(gameObject);
        }
    }
}
