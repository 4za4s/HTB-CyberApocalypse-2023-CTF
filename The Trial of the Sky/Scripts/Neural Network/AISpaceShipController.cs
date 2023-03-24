using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AISpaceShipController : MonoBehaviour
{
    public NeuralNetwork network = null;

    public SpaceShipController controller;
    private Vector3 lastPosition;
    public float timeElapsed;
    private float avgSensor;
    public bool alive = true;
    public float overallFitness;
    float totalDistanceTravelled= 0;



    void Start(){
        timeElapsed = 0;
        lastPosition = this.controller.spaceship.position;

        network = new NeuralNetwork(File.ReadAllText("./nn1282.txt"));
    }

    void Update(){
        timeElapsed = timeElapsed + Time.deltaTime;
        if (alive)
        {
            
            List<double> input = new List<double>();
            for (int i = 0; i < controller.sensors.Count; i++)
            {
                input.Add(controller.sensors[i].hitNormal);
            }
            input.Add(controller.speed / controller.acceleration);

           
            double[] output = network.Run(input);
          
            controller.spaceshipTurn = (float)output[0];
            controller.spaceshipDrive = (float)output[1]+1;

            if (controller.playerHitWall)
            {
                int last_checkpoint = controller.spaceshipCheckPoint.nextCheckpoint;
                Debug.Log("The spaceship crashed before reaching checkpoint " + last_checkpoint);
                Stop();
            }
            if (controller.playerStopped)
            {
                Stop();
            }
            //CalculateFitness();
        }
    }

    // private void Death()
    // {
    //     GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    // }

    // private void CalculateFitness() {

    //     float checkpointsPassed = controller.checkpointsPassed;
    //     totalDistanceTravelled += Vector3.Distance(transform.position,lastPosition);
    //     lastPosition = transform.position;

    //     overallFitness = (checkpointsPassed*100 + totalDistanceTravelled);
    //     Debug.Log("Fitness: " + overallFitness);
    //     if (timeElapsed > 60*5)
    //     {
    //         Death();
    //     }
    // }


    public void Stop(){
        alive = false;
        controller.spaceshipTurn = 0;
        controller.spaceshipDrive = 0;
        controller.spaceship.isKinematic = true;
        controller.spaceship.velocity = Vector3.zero;
    }

    // public void ResetWithNetwork(NeuralNetwork nn)
    // {
    //     network = nn;
    //     this.controller.ResetPosition(controller.startingPos, controller.carRotation);
    //     alive = true;
    //     controller.spaceship.isKinematic = false;
    //     timeElapsed = 0;
    //     totalDistanceTravelled = 0;
    //     lastPosition = controller.startingPos;
    // }

    public void Reset(Vector3 startingPos, Quaternion rotation)
    {
        this.controller.ResetPosition(startingPos, rotation);
        alive = true;
        controller.spaceship.isKinematic = false;
    }
}
