using UnityEngine;

public class DrawingSystemParticle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DrawingSystem drawingSystem;

    [SerializeField] private ParticleDrawer[] drawers;

    [System.Serializable]
    private class ParticleDrawer
    {
        [SerializeField] private ParticleSystem particleSystem;
        private ParticleSystem.Particle[] particles;
        public int ColourIndex => colourIndex;
        public int SecondaryColourIndex => secondaryColourIndex;
        [SerializeField] private int colourIndex;
        [SerializeField] private int secondaryColourIndex;

        [Space]
        [SerializeField] private bool drawLinesToParticles;
        public bool DrawLines => drawLinesToParticles;

        [SerializeField] private int lineStrength;
        public int LineStrength => lineStrength;

        [Space]
        [SerializeField, Range(0.01f, 1f)] private float timeBetweenDraws;
        private float timeSinceLastDraw;

        [SerializeField, Range(0f, 1f)] private float drawChance;

        public bool WillDrawThisFrame()
        {
            timeSinceLastDraw += Time.deltaTime;

            if (particleSystem.isPlaying)
            {
                if (timeSinceLastDraw <= timeBetweenDraws)
                {
                    return false;
                }
                else if (Random.Range(0f, 1f) < drawChance)
                {
                    timeSinceLastDraw = 0f;
                    return true;
                }
                else
                {
                    timeSinceLastDraw = 0f;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public ParticleSystem.Particle[] GetParticles()
        {
            if (particles == null) particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

            particleSystem.GetParticles(particles);
            return particles;
        }

        private ParticleSystem.ShapeModule shape;

        public Vector3 GetPosition()
        {
            shape = particleSystem.shape;
            shape.position = particleSystem.transform.position;

            return particleSystem.transform.position;
        }
    }
    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].WillDrawThisFrame())
            {
                if (!drawers[i].DrawLines)
                {
                    Draw(drawers[i].GetParticles(), Random.Range(drawers[i].ColourIndex, drawers[i].SecondaryColourIndex));
                }
                else
                {
                    DrawLinesToParticles(drawers[i].GetParticles(), drawers[i].ColourIndex, drawers[i].SecondaryColourIndex, drawers[i].LineStrength, drawers[i].GetPosition());
                }
            }
        }
    }

    private void Draw(ParticleSystem.Particle[] particles, int colourIndex)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            drawingSystem.ApplyColourToPixel(colourIndex, particles[i].position);
        }
    }

    private void DrawLinesToParticles(ParticleSystem.Particle[] particles, int colourIndex1, int colourIndex2, int lineStrength, Vector3? startPosition)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 start = startPosition == null ? transform.position : (Vector3)startPosition;

            Debug.DrawLine(start, particles[i].position, Color.red, 0.1f);

            for (int t = 0; t < lineStrength; t++)
            {
                drawingSystem.ApplyColourToPixel(Random.Range(colourIndex1, colourIndex2), Vector3.Lerp(start, particles[i].position, (float)t / lineStrength));
            }
        }
    }
}
