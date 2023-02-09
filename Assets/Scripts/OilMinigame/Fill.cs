using Obi;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObiSolver))]

public class Fill : MonoBehaviour
{
    ObiSolver solver;
    public int counter = 0;
    public Collider2D targetCollider = null;

    ObiSolver.ObiCollisionEventArgs collisionEvent;

    void Awake()
    {
        solver = GetComponent<ObiSolver>();

    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        foreach (Oni.Contact contact in e.contacts)
        {
            // this one is an actual collision:
            if (contact.distance < 0.01)
            {
                Component collider;
                if (ObiCollider.idToCollider.TryGetValue(contact.other, out collider))
                {
                    counter++;
                }
            }
        }
    }
}