using UnityEngine;

public class DrawingSystemParticle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem particleSystemRef;
    [SerializeField] private DrawingSystem drawingSystem;

    private ParticleSystem.Particle[] particles;

    [Space]

    [SerializeField, Range(0f, 1f)] private float timeBetweenDraws;
    private float timeSinceLastDraw;

    [SerializeField, Range(0f, 1f)] private float drawChance;

    [Space]

    [SerializeField] private int colourIndex;


    private float timeSinceSystemStarted;
    private void Start()
    {
        if (particleSystemRef == null) particleSystemRef = GetComponent<ParticleSystem>();
        if (particles == null) particles = new ParticleSystem.Particle[particleSystemRef.main.maxParticles];
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            particleSystemRef.Play();
            particleSystemRef.Emit(50);

            timeSinceSystemStarted = 0f;
        }
        else if (timeSinceSystemStarted > particleSystemRef.main.startLifetimeMultiplier)
        {
            particleSystemRef.Stop();
        }

        timeSinceSystemStarted += Time.deltaTime;

        UpdateDrawings();
    }

    private void DrawOverParticles()
    {        
        particleSystemRef.GetParticles(particles);

        if (particles.Length > 1)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                drawingSystem.ApplyColourToPixel(colourIndex, particles[i].position);
            }
        }
    }

    private void UpdateDrawings()
    {
        if (particleSystemRef.isPlaying)
        {
            if (Random.Range(0f, 1f) < drawChance)
            {
                timeSinceLastDraw += Time.deltaTime;
            }

            if (timeSinceLastDraw > timeBetweenDraws)
            {
                DrawOverParticles();
                timeSinceLastDraw = 0f;
            }
        }
    }
}
