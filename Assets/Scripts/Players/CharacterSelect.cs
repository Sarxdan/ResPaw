using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum MoveTo
{
    left = 0,
    right,
    up,
    down
}
public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    Image p1Icon;
    [SerializeField]
    Image p2Icon;

    [SerializeField]
    Transform[] iconsPositions;

    private int playerOnePosition;
    private int playerTwoPosition;


    private void Start()
    {
        PlaceTheIconsOnDefaultPlaces();
    }

    private void Update()
    {
        SelectAnimal();
    }

    private void SelectAnimal()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {

            playerOnePosition = MoveSelectIcon(playerOnePosition, MoveTo.right);
            p1Icon.transform.position = iconsPositions[playerOnePosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

            playerOnePosition = MoveSelectIcon(playerOnePosition, MoveTo.left);
            p1Icon.transform.position = iconsPositions[playerOnePosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

            playerOnePosition = MoveSelectIcon(playerOnePosition, MoveTo.up);
            p1Icon.transform.position = iconsPositions[playerOnePosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            playerOnePosition = MoveSelectIcon(playerOnePosition, MoveTo.down);
            p1Icon.transform.position = iconsPositions[playerOnePosition].position;
            SetPlayers();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerTwoPosition = MoveSelectIcon(playerTwoPosition, MoveTo.right);
            p2Icon.transform.position = iconsPositions[playerTwoPosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerTwoPosition = MoveSelectIcon(playerTwoPosition, MoveTo.left);
            p2Icon.transform.position = iconsPositions[playerTwoPosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerTwoPosition = MoveSelectIcon(playerTwoPosition, MoveTo.up);
            p2Icon.transform.position = iconsPositions[playerTwoPosition].position;
            SetPlayers();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerTwoPosition = MoveSelectIcon(playerTwoPosition, MoveTo.down);
            p2Icon.transform.position = iconsPositions[playerTwoPosition].position;
            SetPlayers();
        }



    }

    private void PlaceTheIconsOnDefaultPlaces()
    {
        p1Icon.transform.position = iconsPositions[0].position;
        p2Icon.transform.position = iconsPositions[1].position;

        playerOnePosition = 0;
        playerTwoPosition = 1;
        SetPlayers();
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(2);
    }

    private int MoveSelectIcon(int currentPosition, MoveTo moveTo)
    {
        int goToPosition = 0;
        switch (moveTo)
        {
            case MoveTo.left:
                if (currentPosition == 0)
                {
                    goToPosition = iconsPositions.Length - 1;
                }
                else
                {
                    goToPosition = currentPosition - 1;
                }
                break;
            case MoveTo.right:
                if (currentPosition == iconsPositions.Length - 1)
                {
                    goToPosition = 0;
                }
                else
                {
                    goToPosition = currentPosition + 1;
                }
                break;
            case MoveTo.up:
                if (currentPosition > 2)
                    goToPosition = currentPosition - 3;
                else
                    goToPosition = currentPosition + 3;
                break;
            case MoveTo.down:
                if (currentPosition < 3)
                    goToPosition = currentPosition + 3;
                else
                    goToPosition = currentPosition - 3;
                break;
            default:
                break;
        }

        if ((IsTheTheNewPositionTakenByOtherPlayer(goToPosition)) && (moveTo <= MoveTo.right))
        {
            goToPosition = moveTo == MoveTo.right ? goToPosition + 1 : goToPosition - 1;

            goToPosition = goToPosition < 0 ? iconsPositions.Length - 1 : goToPosition <= iconsPositions.Length - 1 ? goToPosition : 0;
        }
        else if (IsTheTheNewPositionTakenByOtherPlayer(goToPosition))
        {
            goToPosition = currentPosition;
        }

        return goToPosition;
    }

    bool IsTheTheNewPositionTakenByOtherPlayer(int goToPosition)
    {
        return goToPosition == playerOnePosition || goToPosition == playerTwoPosition;
    }

    void SetPlayers()
    {
        SelectedPlayersSingleton.Instance.SelectedPlayerOne = (EnumAnimals)playerOnePosition;
        SelectedPlayersSingleton.Instance.SelectedPlayerTwo = (EnumAnimals)playerTwoPosition;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
