using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum MoveTo
{
    left = 0,
    right
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
        PlaceTheIconsOnDefultPlaces();
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



    }

    private void PlaceTheIconsOnDefultPlaces()
    {
        p1Icon.transform.position = iconsPositions[0].position;
        p2Icon.transform.position = iconsPositions[1].position;

        playerOnePosition = 0;
        playerTwoPosition = 1;
        SetPlayers();
    }

    public void loadLevel()
    {
        SceneManager.LoadScene((int)SceneEnum.Level1Scene);
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
            default:
                break;
        }

        if (goToPosition == playerOnePosition || goToPosition == playerTwoPosition)
        {
            goToPosition = moveTo == MoveTo.right ? goToPosition + 1 : goToPosition - 1;

            goToPosition = goToPosition < 0 ? iconsPositions.Length - 1 : goToPosition <= iconsPositions.Length - 1 ? goToPosition : 0;
        }

        return goToPosition;
    }

    void SetPlayers()
    {
        SelectedPlayersSingleton.Instance.SelectedPlayerOne = (EnumAnimals)playerOnePosition;
        SelectedPlayersSingleton.Instance.SelectedPlayerTwo = (EnumAnimals)playerTwoPosition;
    }

}
