using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using AI.Utils;
using UnityEngine.EventSystems;


public class AgentGenData
{
    public Genome genome;
    public NeuralNetwork neuralNetwork;
    public int generation;
}

public enum State
{
    Alive,
    Dead
}

public class Agent : MonoBehaviour
{
    #region ACTUAL ALGORITHM STUFF
    protected Genome genome;
    public Genome Genome => genome;

    protected NeuralNetwork neuralNetwork;
    public NeuralNetwork NeuralNetwork => neuralNetwork;

    protected AgentBehaviour agentBehaviour;
    public AgentBehaviour AgentBehaviour => agentBehaviour;

    protected int currentTurn = 0;
    public int CurrentTurn => currentTurn;

    protected int foodEaten = 0;
    public int FoodEaten => foodEaten;

    protected int currentIteration = 0;
    public int CurrentIteration => currentIteration;

    protected AgentGenData agentData;
    public AgentGenData AgentData => agentData;
    private int generationsSurvived = 0;
    #endregion



    [SerializeField] Material[] teamMaterials = new Material[2];
    [SerializeField] Vector2Int position;
    Vector2Int lastPosition;
    Vector2Int initialPosition;

    [SerializeField] AgentBehaviour behaviour;
    FoodManager foodHandler;
    int foodCollected;
    gamegrid map;

    public State state
    {
        get; private set;
    }
    private void Awake()
    {
        agentBehaviour = GetComponent<AgentBehaviour>();
        lastPosition = default;
        generationsSurvived = 0;
    }

    public void SetInitialPosition(Vector2Int initialPosition)
    {
        this.initialPosition = initialPosition;
    }

    public void SetBrain(Genome genome, NeuralNetwork neuralNetwork, bool resetAI = true)
    {
        this.genome = genome;
        this.neuralNetwork = neuralNetwork;
        state = State.Alive;

        agentData = new AgentGenData();

        agentData.genome = genome;
        agentData.neuralNetwork = neuralNetwork;

        lastPosition = position;

        if (resetAI)
        {
            OnReset();
        }
    }

    public void Think(float dt, int turn, int iteration, gamegrid map, FoodManager food)
    {
        if (state == State.Alive)
        {
            currentTurn = turn;
            currentIteration = iteration;

            this.map = map;

            agentBehaviour.SetBehaviourNeeds(OnAteFood, food);

            OnThink(dt, map, food);
            
        }
    }

    private void OnAteFood()
    {
        Debug.Log("OnAteFood()");
        foodCollected++;
        genome.fitness = genome.fitness > 0 ? genome.fitness * 2 : 10;
       
    }

    public virtual void OnGenerationEnded(out Genome genome)
    {
        genome = null;

        if (foodCollected < 1)
        {
            state = State.Dead;
            return;
        }
        else
        {
            state = State.Alive;
        }

        genome = this.genome;
    }
    protected virtual void OnThink(float dt, gamegrid map, FoodManager food)
    {

        List<GridCell> agentAdjancentCells = map.FindAdjacents(map.GetGridCell(position));

        List<float> inputs = new List<float>();
        float[] outputs;

        lastPosition = position;

        inputs.Add(foodCollected);
        //inputs.Add(agentBehaviour.transform.position.magnitude);
        inputs.Add(FindClosestFood(food));

        if (agentAdjancentCells.Any()) //Existe literalmente cualquier cosa aca?
        {
            for (int i = 0; i < agentAdjancentCells.Count; i++)
            {
                inputs.Add(Vector3.Distance(agentBehaviour.transform.position,
                    new Vector3(agentAdjancentCells[i].GetPosition().x, agentAdjancentCells[i].GetPosition().y,
                    HSSUtils.defaultZ)));

                
            }
        }

        outputs = neuralNetwork.Synapsis(inputs.ToArray());

        Vector2Int resultingMove = new Vector2Int();
        for (int i = 0; i < outputs.Length; i++)
        {

            if (outputs[i] < 1.0f && outputs[i] > 0.75f)
            {
                
                resultingMove = agentBehaviour.Movement(MoveDirection.Up, position.x, position.y, 100,100); //Still hardcoded to 100 gridsize
                
            }
            if (outputs[i] < 0.75f && outputs[i] > 0.50f)
            {

                resultingMove = agentBehaviour.Movement(MoveDirection.Down, position.x, position.y, 100, 100);
                
            }
            if (outputs[i] < 0.50f && outputs[i] > 0.25f)
            {

                resultingMove = agentBehaviour.Movement(MoveDirection.Right, position.x, position.y, 100, 100);
                
            }
            if (outputs[i] < 0.25f && outputs[i] > 0.00f)
            {

                resultingMove = agentBehaviour.Movement(MoveDirection.Left, position.x, position.y, 100, 100);
                
            }
            if (outputs[i] < 0)
            {

                resultingMove = agentBehaviour.Movement(MoveDirection.None, position.x, position.y, 100, 100);
                
            }
        }
        UpdatePosition(resultingMove);
    }

    public void UpdatePosition(Vector2Int pos) {
        position.x = pos.x;
        position.y = pos.y;

        transform.position = HSSUtils.GetWorldFromPosition(position);
    }

    protected virtual void OnReset()
    {
        genome.fitness = 0.0f;
        foodCollected = 0;
        position = initialPosition;
        transform.position = HSSUtils.GetWorldFromPosition(position);
    }


    private float FindClosestFood(FoodManager food)
    {
        if (food == null ||  food.GetCurrentFood() < 1)
            return 0f;

        float closestFood = Vector3.Distance(agentBehaviour.transform.position,
            new Vector3(food.foodList[0].GetPosition().x, food.foodList[0].GetPosition().y, HSSUtils.defaultZ));

        for (int i = 0; i < food.GetCurrentFood(); i++)
        {
            if (food.foodList[i] != null)
            {
                float newDistance = Vector3.Distance(agentBehaviour.transform.position,
                    new Vector3(food.foodList[0].GetPosition().x, food.foodList[0].GetPosition().y, HSSUtils.defaultZ));

                if (closestFood < newDistance)
                {
                    closestFood = newDistance;
                }
            }
        }

        return closestFood;
    }


    public void Init(Vector2Int pos, bool team = true)
    {
        UpdatePosition(pos);
        if (team)
        {
            this.transform.GetComponent<MeshRenderer>().material = teamMaterials[0];
        }
        else {
            this.transform.GetComponent<MeshRenderer>().material = teamMaterials[1];
        }
    }
}
