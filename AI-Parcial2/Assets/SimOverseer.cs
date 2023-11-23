using AI.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Team
 {
     public PopulationManager populationManager;
     public SimConfig simConfig;
 }
public class SimOverseer : MonoBehaviour
{

    [SerializeField] private List<Team> teams = null;
    [SerializeField] private FoodManager foodHandler = null;
    //[SerializeField] private GameManager gameManager = null;
    [SerializeField] private gamegrid gameGrid = null;
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button stopButton = null;
    

    [SerializeField] private TMP_Text turnAmountText = null;
    [SerializeField] private int maxTurnsAllowed = 0;
    [SerializeField] private bool saveBestAgentOfEachTeam = false;

    private bool simulationStarted = false;
    private int teamsNeededForBegin = 0;
    private int currentTurn = 0;
    private float delayForNextTurn = 0f;
    private float time = 0f;

    private int totalFood = 0;
    private List<(string, float)> lastAgentSaved = new List<(string, float)>();

    void Start()
    {
        teamsNeededForBegin = teams.Count;
        Init();
    }


    void Update()
    {
        BeginSimulation();

        if (simulationStarted)
        {
            UpdateTurnWhenNeeded();
        }
    }

    private void FixedUpdate()
    {
        if (!simulationStarted)
            return;

        if (!CheckIfAllAgentsDone())
        {
            UpdateTeams();
        }
    }

    public void Init()
    {
        simulationStarted = false;
        lastAgentSaved.Clear();

        gameGrid.CreateGrid(new Vector2Int(100,100));
        
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        stopButton.onClick.AddListener(OnStopButtonClick);

        pauseButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(false);

        for (int i = 0; i < teams.Count; i++)
        {
           
           teams[i].simConfig.Init(HSSUtils.GetGridSize(), teams[i].populationManager);
           teams[i].populationManager.onTeamWipe += OnPauseButtonClick;
            
        }

        //gameManager.PopulateBoardWithAgents();
    }

    private void BeginSimulation()
    {
        if (simulationStarted)
            return;

        int teamsReady = 0;

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null && teams[i].simConfig.IsTeamReady)
            {
                teamsReady++;
            }
        }

        if (teamsReady == teamsNeededForBegin)
        {
            simulationStarted = true;
            OnStartedSimulation();
        }
    }


    private void UpdateTeams()
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                if (teams[i].populationManager != null)
                {
                    teams[i].populationManager.UpdatePopulation();
                }
            }
        }
    }

    private void SaveBestAgentOfEachTeam()
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                bool needUpdateBestAgent = true;
                Agent bestTeamAgent = teams[i].populationManager.GetBestAgent();

                if (lastAgentSaved != null)
                {
                    (string, float) correctSavedData = lastAgentSaved.Find(data => data.Item1 == teams[i].populationManager.ID);

                    if (bestTeamAgent != null && bestTeamAgent.AgentData.genome.fitness > correctSavedData.Item2)
                    {
                        lastAgentSaved.Remove(correctSavedData);
                        needUpdateBestAgent = true;
                    }
                    else
                    {
                        needUpdateBestAgent = false;
                    }
                }

                if (needUpdateBestAgent)
                {
                    if (bestTeamAgent != null)
                    {
                        if (!lastAgentSaved.Contains((teams[i].populationManager.ID,
                                bestTeamAgent.Genome.fitness)))
                        {
                            lastAgentSaved.Add((teams[i].populationManager.ID,
                                bestTeamAgent.Genome.fitness));
                        }

                        bestTeamAgent.AgentData.generation = teams[i].populationManager.generation;

                        DataHandler<AgentGenData>.Save(bestTeamAgent.AgentData, teams[i].populationManager.ID,
                            teams[i].populationManager.generation.ToString(), bestTeamAgent.Genome.fitness.ToString(),
                            bestTeamAgent.Genome.foodEaten.ToString());
                    }
                }
            }
        }
    }
    private AgentGenData LoadBestAgentFromTeam(int iterationTeam)
    {
        AgentGenData bestAgent = null;

        if (string.IsNullOrEmpty(teams[iterationTeam].simConfig.FileNameToLoad.text))
            return null;

        bestAgent = DataHandler<AgentGenData>.Load(teams[iterationTeam].populationManager.ID,
            teams[iterationTeam].simConfig.FileNameToLoad.text);

        return bestAgent;
    }

    private void OnStartedSimulation()
    {
        pauseButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(true);

        totalFood = 0;

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                totalFood += teams[i].populationManager.PopulationCount;
                teams[i].simConfig.gameObject.SetActive(false);

                List<Vector2Int> finalTeamPositions = new List<Vector2Int>();
                if (i == 0)
                {
                    for (int j = 0; j < HSSUtils.gridSize; j++)
                    {
                        finalTeamPositions.Add(new Vector2Int(j, 0));

                    }
                }
                else {
                    for (int j = 0; j < HSSUtils.gridSize; j++)
                    {
                        finalTeamPositions.Add(new Vector2Int(j, HSSUtils.gridSize-1));

                    }
                }

                teams[i].populationManager.StartSimulation(finalTeamPositions, gameGrid, foodHandler, LoadBestAgentFromTeam(i));
            }
        }

        foodHandler.PopulateGridWithFood();
    }

    private void OnEndedAllTurns()
    {
        currentTurn = 0;
        turnAmountText.text = "Turn: " + currentTurn.ToString();

        if (saveBestAgentOfEachTeam)
        {
            SaveBestAgentOfEachTeam();
        }

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
                teams[i].populationManager.EndedGeneration();
        }

        foodHandler.ClearFood();
        foodHandler.PopulateGridWithFood();
    }

    private void OnPauseButtonClick()
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                teams[i].populationManager.PauseSimulation();
            }
        }
    }

    private void OnStopButtonClick()
    {
        if (saveBestAgentOfEachTeam)
        {
            SaveBestAgentOfEachTeam();
        }

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                teams[i].simConfig.gameObject.SetActive(true);
                teams[i].simConfig.OnStopSimulation();
                teams[i].populationManager.StopSimulation();
            }
        }

        simulationStarted = false;

        pauseButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(false);

        foodHandler.ClearFood();

        currentTurn = 0;
        turnAmountText.text = "Turn: " + currentTurn.ToString();
    }

    private void UpdateTurnWhenNeeded()
    {
        if (currentTurn < maxTurnsAllowed)
        {
            if (CheckIfAllAgentsDone())
            {
                if (time < delayForNextTurn)
                {
                    time += Time.deltaTime;
                }
                else
                {
                    time = 0;
                    currentTurn++;

                    turnAmountText.text = "Turn: " + currentTurn.ToString();

                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i] != null && teams[i].populationManager != null)
                        {
                            teams[i].populationManager.UpdateTurn(currentTurn);
                        }
                    }
                }
            }
        }
        else
        {
            currentTurn = maxTurnsAllowed;

            if (currentTurn == maxTurnsAllowed)
            {
                turnAmountText.text = "Turns: Simulation Finished";
                OnEndedAllTurns();
            }
        }
    }

    private bool CheckIfAllAgentsDone()
    {
        int completedTeams = 0;

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null && teams[i].populationManager != null)
            {
                if (teams[i].populationManager.AllAgentsDone())
                    completedTeams++;
            }
        }

        return completedTeams == teams.Count;
    }

}
