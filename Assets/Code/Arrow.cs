using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CONSTANTS
{
    public static float K = 9; 
}

public class Arrow : MonoBehaviour
{
    const int MIN_DISTANCE = 1;  // the minimum distance that a particle has to be away to be accounted for in calc ; This is used to avoid infinite bugs and the such

    public float maxAlpha = .8f, minAlpha = 0f; 
    public float minDist = 7f, maxDist =  14f;


    public void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

    }

    /*getElectricField(position )
    {
        const electricField = new Vector2(0, 0);

        this.activeChargedParticles.forEach(chargedParticle => {
            const distanceSquared = chargedParticle.positionProperty.get().distanceSquared(position);

            // Avoid bugs stemming from large or infinite fields (#82, #84, #85).
            // Assign the E-field an angle of zero and a magnitude well above the maximum allowed value.
            if (distanceSquared < MIN_DISTANCE_SCALE)
            {
                electricField.x = 10 * ChargesAndFieldsConstants.MAX_EFIELD_MAGNITUDE;
                electricField.y = 0;
                return;
            }

            const distancePowerCube = Math.pow(distanceSquared, 1.5);

            // For performance reasons, we don't want to generate more vector allocations
            const electricFieldContribution = {
        x: (position.x - chargedParticle.positionProperty.get().x) * (chargedParticle.charge) / distancePowerCube,
        y: (position.y - chargedParticle.positionProperty.get().y) * (chargedParticle.charge) / distancePowerCube
          };
        electricField.add(electricFieldContribution);
    } );
    electricField.multiplyScalar(K_CONSTANT ); // prefactor depends on units
    return electricField;
  }*/


    public void calcEField(List<GameObject> particles)
    {
        Vector2 netElectricField = Vector2.zero;

        foreach ( GameObject particle in particles)
        {
            float dist = Vector2.Distance(particle.transform.position, transform.position);


            // Now check for min distance if needed

            float distCubed = Mathf.Pow(Mathf.Abs(dist), 1.5f);

            // contribution to the net electric field 
            Vector2 contribution = new Vector2(
                (transform.position.x - particle.transform.position.x) * particle.GetComponent<Particle>().charge / distCubed,
                (transform.position.y - particle.transform.position.y) * particle.GetComponent<Particle>().charge / distCubed);


            // if contribution is large that means is should have a higher alpha 
            // take the larger alpha for this specific arrow (if lower don't change alpha) 
            

            if(dist <=minDist) // just give it max alpha 
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, maxAlpha);
            }
            else
            {

                Debug.Log("Distance: " + dist); 

                float fraction = (dist - minDist) / (maxDist - minDist);

                float val = Mathf.Lerp(maxAlpha, minAlpha, fraction);

                if (GetComponent<SpriteRenderer>().color.a < val)
                {
                    GetComponent<SpriteRenderer>().color = new Color(val, val, val, val);
                }

            }

            netElectricField += contribution;


            float rotationZ = Mathf.Atan2(netElectricField.y, netElectricField.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            
        }

        netElectricField *= CONSTANTS.K;

    }



    // returns angle with the horizontal 
    float returnAngle(Vector2 sum)
    {
 
        return Mathf.Atan2(sum.y, sum.x) * Mathf.Rad2Deg;
    }


    Vector3 calcDistance(Vector3 first ,  Vector3 second)
    {
        Vector3 resultant = second - first;

        // multiply it by the  square of the distance (mag ) ; closer it is the stronger it is 
        Debug.Log("Resultant vector prior: ");  
        Debug.Log("Multiplicant: " + (1.0f / (Mathf.Pow(resultant.magnitude, 4)))); 
        return resultant * (1.0f /(Mathf.Pow(resultant.magnitude, 4)));
    }
}   
