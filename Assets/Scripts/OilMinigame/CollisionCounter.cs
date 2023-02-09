using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiSolver))]
public class CollisionCounter : MonoBehaviour
{

    ObiSolver solver;
    public ObiEmitter emitter;
    public int counter = 0;
    public Collider targetCollider = null;

    Obi.ObiSolver.ObiCollisionEventArgs frame;
    HashSet<int> particles = new HashSet<int>();
    public FluidGameManager gameManager;

    void Awake()
    {
        solver = GetComponent<Obi.ObiSolver>();
        gameManager = GameObject.Find("GameManager").GetComponent<FluidGameManager>();
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        HashSet<int> currentParticles = new HashSet<int>();

        for (int i = 0; i < e.contacts.Count; ++i)
        {
            if (e.contacts.Data[i].distance < 0.01f)
            {
                Component collider;

                if (ObiCollider.idToCollider.TryGetValue(e.contacts.Data[i].other, out collider))
                {
                    if (collider == targetCollider)
                    {
                        currentParticles.Add(e.contacts.Data[i].particle);

                        /*if(emitter != null)
                            emitter.life[e.contacts.Data[i].particle] = 0;*/
                    }
                }
            }
        }

        particles.ExceptWith(currentParticles);
        counter += particles.Count;
        particles = currentParticles;
        gameManager.actualizarValorSlider(counter);
    }

}