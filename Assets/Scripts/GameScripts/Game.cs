using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    enum EPhase
    {
        PLACING,
		PREMOVING,
        MOVING,
        REMOVING,
        FLYING,
        ENDING
    }

    enum EPlayer
    {
        EMPTY = -1,
        FIRST = 0,
        SECOND = 1,
    }

    private const int NB_PIECES_MAX = 9;
    public GameObject Selector;
    public GameObject MovingSelector;
    public GameObject[] Second;
    public GameObject[] First;
    public GameObject Ending;

    private float timeT;
    private bool isPlayerFirst = true;
    private int nbFirstPiecePlaced = 0;
    private int nbSecondPiecePlaced = 0;
	private int firstPiecePlaced = 0;
	private int secondPiecePlaced = 0;
	private Vector2 pieceSelected = Vector2.zero;
    private EPhase phase = EPhase.PLACING;

    private Dictionary<Vector2, EPlayer> dicoOfPlayerPosition = new Dictionary<Vector2, EPlayer>();
    private Dictionary<Vector2, List<int>> dicoOfAdjacent = new Dictionary<Vector2, List<int>>();
    private List<List<int>> listOfMillsCombinaison = new List<List<int>>();
    private List<Vector2> listFirstStartPiecesPosition = new List<Vector2>();
    private List<Vector2> listSecondStartPiecesPosition = new List<Vector2>();
    private ArrayList arrayPosition = new ArrayList
    {
        new Vector2(-4.5f,  4.5f),
        new Vector2(0f,     4.5f),
        new Vector2(4.5f,   4.5f),
        new Vector2(-3f,    3f),
        new Vector2(0f,     3f),
        new Vector2(3f,     3f),
        new Vector2(-1.5f,  1.5f),
        new Vector2(0f,     1.5f),
        new Vector2(1.5f,   1.5f),
        new Vector2(-4.5f,  0f),
        new Vector2(-3f,    0f),
        new Vector2(-1.5f,  0f),
        new Vector2(1.5f,   0f),
        new Vector2(3f,     0f),
        new Vector2(4.5f,   0f),
        new Vector2(-1.5f,  -1.5f),
        new Vector2(0f,     -1.5f),
        new Vector2(1.5f,   -1.5f),
        new Vector2(-3f,    -3f),
        new Vector2(0f,     -3f),
        new Vector2(3f,     -3f),
        new Vector2(-4.5f,  -4.5f),
        new Vector2(0f,     -4.5f),
        new Vector2(4.5f,   -4.5f),
    };

	private void Start()
	{
		InitStartPosition();
		InitDicoOfPlayersPosition();
		InitDicoOfAdjacent();
		InitListOfMillsCombinaison();
		Selector.SetActive(false);
		MovingSelector.SetActive(false);
		Ending.SetActive(false);
	}

	private void InitStartPosition()
	{
		foreach (GameObject piece in First)
		{
			listFirstStartPiecesPosition.Add(piece.transform.position);
		}
		foreach (GameObject piece in Second)
		{
			listSecondStartPiecesPosition.Add(piece.transform.position);
		}
	}

	private void InitDicoOfPlayersPosition()
	{
		foreach (Vector2 pos in arrayPosition)
		{
			if (dicoOfPlayerPosition.ContainsKey(pos))
			{
				dicoOfPlayerPosition[pos] = EPlayer.EMPTY;
			}
			else
			{
				dicoOfPlayerPosition.Add(pos, EPlayer.EMPTY);
			}

		}
	}

	/*
     *     0-------------1-------------2
     *     |             |             |
     *     |   3---------4---------5   |
     *     |   |         |         |   |
     *     |   |    6----7----8    |   |
     *     |   |    |         |    |   |
     *     |   |    |         |    |   |
     *     9---10---11        12---13--14
     *     |   |    |         |    |   |
     *     |   |    |         |    |   |
     *     |   |    15---16---17   |   |
     *     |   |         |         |   |
     *     |   18--------19--------20  |
     *     |             |             |
     *     21------------22------------23
     */

	private void InitDicoOfAdjacent()
	{
		dicoOfAdjacent.Add((Vector2)arrayPosition[0], new List<int> { 1, 9 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[1], new List<int> { 0, 2, 4 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[2], new List<int> { 1, 14 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[3], new List<int> { 4, 10 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[4], new List<int> { 1, 3, 5, 7 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[5], new List<int> { 4, 13 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[6], new List<int> { 7, 11 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[7], new List<int> { 4, 6, 8 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[8], new List<int> { 7, 12 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[9], new List<int> { 0, 10, 21 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[10], new List<int> { 3, 9, 11, 18 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[11], new List<int> { 6, 10, 15 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[12], new List<int> { 8, 13, 17 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[13], new List<int> { 5, 12, 14, 20 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[14], new List<int> { 2, 13, 23 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[15], new List<int> { 11, 16 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[16], new List<int> { 15, 17, 19 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[17], new List<int> { 12, 16 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[18], new List<int> { 10, 19 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[19], new List<int> { 16, 18, 20, 22 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[20], new List<int> { 13, 19 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[21], new List<int> { 9, 22 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[22], new List<int> { 19, 21, 23 });
		dicoOfAdjacent.Add((Vector2)arrayPosition[23], new List<int> { 14, 22 });
	}

	private void InitListOfMillsCombinaison()
	{
		listOfMillsCombinaison.Add(new List<int> { 0, 1, 2 });
		listOfMillsCombinaison.Add(new List<int> { 0, 9, 21 });
		listOfMillsCombinaison.Add(new List<int> { 1, 4, 7 });
		listOfMillsCombinaison.Add(new List<int> { 2, 14, 23 });
		listOfMillsCombinaison.Add(new List<int> { 3, 4, 5 });
		listOfMillsCombinaison.Add(new List<int> { 3, 10, 18 });
		listOfMillsCombinaison.Add(new List<int> { 5, 13, 20 });
		listOfMillsCombinaison.Add(new List<int> { 6, 7, 8 });
		listOfMillsCombinaison.Add(new List<int> { 6, 11, 15 });
		listOfMillsCombinaison.Add(new List<int> { 8, 12, 17 });
		listOfMillsCombinaison.Add(new List<int> { 9, 10, 11 });
		listOfMillsCombinaison.Add(new List<int> { 12, 13, 14 });
		listOfMillsCombinaison.Add(new List<int> { 15, 16, 17 });
		listOfMillsCombinaison.Add(new List<int> { 16, 19, 22 });
		listOfMillsCombinaison.Add(new List<int> { 18, 19, 20 });
		listOfMillsCombinaison.Add(new List<int> { 21, 22, 23 });
	}

	private void Update()
	{
		timeT += Time.deltaTime;
		Selector.SetActive(false);
		MovingSelector.SetActive(false);
		Vector2 mousePosition = MouseSelector();

		if (phase != EPhase.PLACING && ImpossibleToMove(GetPlayer()))
		{
			phase = EPhase.ENDING;
			isPlayerFirst = !isPlayerFirst;
		}

		switch (phase)
		{
			case EPhase.PLACING:
				Place(mousePosition);
				break;
			case EPhase.PREMOVING:
				PlaceRemoveOpponentPiece();
				break;
			case EPhase.MOVING:
				Move(mousePosition);
				break;
			case EPhase.REMOVING:
				RemoveOpponentPiece();
				break;
			case EPhase.FLYING:
				Fly(mousePosition);
				break;
			case EPhase.ENDING:
				End();
				break;
		}
	}

	private Vector2 MouseSelector()
	{
		float camDis = Camera.main.transform.position.y - GetComponent<Transform>().position.y;
		Vector2 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

		foreach (Vector2 position in arrayPosition)
		{
			if (IsCloseToPosition(mouse, position))
				return position;
		}
		return Vector2.zero;
	}

	private bool IsCloseToPosition(Vector2 mousePosition, Vector2 squarePosition)
	{
		float xmin = squarePosition.x - 0.2f;
		float xmax = squarePosition.x + 0.2f;
		float ymin = squarePosition.y - 0.2f;
		float ymax = squarePosition.y + 0.2f;

		return (mousePosition.x >= xmin &&
				mousePosition.x <= xmax &&
				mousePosition.y >= ymin &&
				mousePosition.y <= ymax);
	}

	private bool ImpossibleToMove(EPlayer player)
	{
		foreach (Vector2 position in arrayPosition)
		{
			if (dicoOfPlayerPosition[position] == player)
			{
				foreach (int indexOfAdjacent in dicoOfAdjacent[position])
				{
					if (GetPlayerInSquare(indexOfAdjacent) == EPlayer.EMPTY)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private EPlayer GetPlayerInSquare(int squareIndex)
	{
		Vector2 squarePosition = (Vector2)arrayPosition[squareIndex];
		return dicoOfPlayerPosition[squarePosition];
	}

	private EPlayer GetPlayer()
	{
		return (isPlayerFirst ? EPlayer.FIRST : EPlayer.SECOND);
	}

	private void Place(Vector2 mousePosition)
	{
		if (firstPiecePlaced == NB_PIECES_MAX && nbSecondPiecePlaced == NB_PIECES_MAX)
		{
			phase = EPhase.MOVING;
			return;
		}

		Selector.SetActive(false);

		if (mousePosition != Vector2.zero && dicoOfPlayerPosition[mousePosition] == EPlayer.EMPTY)
		{
			Selector.SetActive(true);
			Selector.transform.position = mousePosition;
			if (isPlayerFirst)
			{
				if (Input.GetMouseButtonDown(0) && firstPiecePlaced < NB_PIECES_MAX)
				{
					First[firstPiecePlaced].transform.position = mousePosition;
					dicoOfPlayerPosition[mousePosition] = EPlayer.FIRST;
					firstPiecePlaced++;
					nbFirstPiecePlaced++;
					pieceSelected = mousePosition;

					if (MillCreated())
					{
						phase = EPhase.PREMOVING;
					}
					else
					{
						isPlayerFirst = false;
					}
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0) && secondPiecePlaced < NB_PIECES_MAX)
				{
					Second[secondPiecePlaced].transform.position = mousePosition;
					dicoOfPlayerPosition[mousePosition] = EPlayer.SECOND;
					secondPiecePlaced++;
					nbSecondPiecePlaced++;
					pieceSelected = mousePosition;

					if (MillCreated())
					{
						phase = EPhase.PREMOVING;
					}
					else
					{
						isPlayerFirst = true;
					}
				}
			}
		}
	}

	private void Move(Vector2 mousePosition)
	{
		if (GetNbPieceOnBoardCurrentPlayer() == 3)
		{
			phase = EPhase.FLYING;
			return;
		}

		if (pieceSelected == Vector2.zero)
		{
			pieceSelected = SelectPiece(mousePosition, GetPlayer());
		}
		if (pieceSelected != Vector2.zero)
		{
			if (MovePiece(mousePosition, GetPlayer()))
			{
				if (MillCreated())
				{
					phase = EPhase.REMOVING;

				}
				else
				{
					isPlayerFirst = !isPlayerFirst;
					pieceSelected = Vector2.zero;
					Selector.SetActive(false);
				}
			}
		}
	}

	private int GetNbPieceOnBoardCurrentPlayer()
	{
		return isPlayerFirst ? nbFirstPiecePlaced : nbSecondPiecePlaced;
	}

	private Vector2 SelectPiece(Vector2 mousePosition, EPlayer player)
	{
		if (mousePosition != Vector2.zero && dicoOfPlayerPosition[mousePosition] == player)
		{
			Selector.transform.position = mousePosition;
			Selector.SetActive(true);
			if (Input.GetMouseButtonDown(0))
			{
				foreach (int indexOfAdjacent in dicoOfAdjacent[mousePosition])
				{
					if (GetPlayerInSquare(indexOfAdjacent) == EPlayer.EMPTY || phase == EPhase.FLYING)
					{
						timeT = 0f;
						return (mousePosition);
					}
				}
			}
		}
		return Vector2.zero;
	}

	private bool MovePiece(Vector2 mousePosition, EPlayer player)
	{
		GameObject pieceToMove = null;

		Selector.SetActive(true);
		MovingSelector.SetActive(false);

		foreach (int indexOfAdjacent in dicoOfAdjacent[pieceSelected])
		{
			Vector2 adjacent = (Vector2)arrayPosition[indexOfAdjacent];
			if (IsCloseToPosition(mousePosition, adjacent) && GetPlayerInSquare(indexOfAdjacent) == EPlayer.EMPTY)
			{
				MovingSelector.transform.position = adjacent;
				MovingSelector.SetActive(true);
				if (Input.GetMouseButtonDown(0))
				{
					pieceToMove = FindPieceWithPosition(pieceSelected, player);
					if (pieceToMove != null)
					{
						pieceToMove.transform.position = adjacent;
						dicoOfPlayerPosition[pieceSelected] = EPlayer.EMPTY;
						dicoOfPlayerPosition[adjacent] = player;
						pieceSelected = adjacent;
						return true;
					}
				}
			}
		}

		if (mousePosition == pieceSelected && Input.GetMouseButtonDown(0) && timeT > 0.5f)
		{
			timeT = 0f;
			pieceSelected = Vector2.zero;
		}
		return false;
	}

	private GameObject FindPieceWithPosition(Vector2 position, EPlayer player)
	{
		if (player == EPlayer.FIRST)
		{
			foreach (GameObject piece in First)
			{
				if ((Vector2)piece.transform.position == position)
					return piece;
			}
		}
		else
		{
			foreach (GameObject piece in Second)
			{
				if ((Vector2)piece.transform.position == position)
					return piece;
			}
		}
		return null;
	}

	private bool MillCreated()
	{
		int index = arrayPosition.IndexOf(pieceSelected);
		List<List<int>> listTmp = listOfMillsCombinaison.FindAll(list => list.Contains(index));

		foreach (List<int> list in listTmp)
		{
			int count = 0;
			foreach (int i in list)
			{
				Vector2 squarePosition = (Vector2)arrayPosition[i];
				if (dicoOfPlayerPosition[squarePosition] == GetPlayer())
				{
					count++;
				}
			}
			if (count == 3)
				return true;
		}
		return false;
	}

	private void PlaceRemoveOpponentPiece()
	{
		GameObject pieceToRemove = null;

		Vector2 mousePosition = MouseSelector();
		if (mousePosition != Vector2.zero && dicoOfPlayerPosition[mousePosition] == GetOpponent())
		{
			Selector.transform.position = mousePosition;
			Selector.SetActive(true);
			if (Input.GetMouseButtonDown(0))
			{
				pieceToRemove = FindPieceWithPosition(mousePosition, GetOpponent());
				if (pieceToRemove != null)
				{
					dicoOfPlayerPosition[mousePosition] = EPlayer.EMPTY;
					if (GetPlayer() == EPlayer.FIRST)
					{
						pieceToRemove.transform.position = listFirstStartPiecesPosition[nbSecondPiecePlaced - 1];
						nbSecondPiecePlaced--;
					}
					else
					{
						pieceToRemove.transform.position = listSecondStartPiecesPosition[nbFirstPiecePlaced - 1];
						nbFirstPiecePlaced--;
					}
					phase = EPhase.PLACING;
					isPlayerFirst = !isPlayerFirst;
					pieceSelected = Vector2.zero;
					MovingSelector.SetActive(false);
					Selector.SetActive(false);
				}
			}
		}
	}

	private void RemoveOpponentPiece()
	{
		GameObject pieceToRemove = null;

		Vector2 mousePosition = MouseSelector();
		if (mousePosition != Vector2.zero && dicoOfPlayerPosition[mousePosition] == GetOpponent())
		{
			Selector.transform.position = mousePosition;
			Selector.SetActive(true);
			if (Input.GetMouseButtonDown(0))
			{
				pieceToRemove = FindPieceWithPosition(mousePosition, GetOpponent());
				if (pieceToRemove != null)
				{
					dicoOfPlayerPosition[mousePosition] = EPlayer.EMPTY;
					if (GetPlayer() == EPlayer.FIRST)
					{
						pieceToRemove.transform.position = listFirstStartPiecesPosition[nbSecondPiecePlaced - 1];
						nbSecondPiecePlaced--;
					}
					else
					{
						pieceToRemove.transform.position = listSecondStartPiecesPosition[nbFirstPiecePlaced - 1];
						nbFirstPiecePlaced--;
					}
					if (nbFirstPiecePlaced == 2 || nbSecondPiecePlaced == 2)
					{
						phase = EPhase.ENDING;
					}
					else
					{
						phase = EPhase.MOVING;
						isPlayerFirst = !isPlayerFirst;
						pieceSelected = Vector2.zero;
					}
					MovingSelector.SetActive(false);
					Selector.SetActive(false);
				}
			}
		}
	}

	private EPlayer GetOpponent()
	{
		return (isPlayerFirst ? EPlayer.SECOND : EPlayer.FIRST);
	}

	private void Fly(Vector2 mousePosition)
	{
		if (GetNbPieceOnBoardCurrentPlayer() > 3)
		{
			phase = EPhase.MOVING;
			return;
		}
		if (pieceSelected == Vector2.zero)
		{
			pieceSelected = SelectPiece(mousePosition, GetPlayer());
		}
		if (pieceSelected != Vector2.zero)
		{
			if (FlyingPiece(mousePosition, GetPlayer()))
			{
				if (MillCreated())
				{
					phase = EPhase.REMOVING;
				}
				else
				{
					isPlayerFirst = !isPlayerFirst;
					pieceSelected = Vector2.zero;
					Selector.SetActive(false);
				}
			}
		}
	}

	private bool FlyingPiece(Vector2 mousePosition, EPlayer player)
	{
		GameObject pieceToMove = null;

		Selector.SetActive(true);
		MovingSelector.SetActive(false);
		if (mousePosition != Vector2.zero && dicoOfPlayerPosition[mousePosition] == EPlayer.EMPTY)
		{
			MovingSelector.SetActive(true);
			MovingSelector.transform.position = mousePosition;
			if (Input.GetMouseButtonDown(0))
			{
				pieceToMove = FindPieceWithPosition(pieceSelected, player);
				pieceToMove.transform.position = mousePosition;
				dicoOfPlayerPosition[mousePosition] = player;
				dicoOfPlayerPosition[pieceSelected] = EPlayer.EMPTY;
				pieceSelected = mousePosition;
				return true;
			}
		}
		if (mousePosition == pieceSelected && Input.GetMouseButtonDown(0) && timeT > 1f)
		{
			Debug.Log(timeT);
			timeT = 0f;
			pieceSelected = Vector2.zero;
		}
		return false;
	}

	private void End()
	{
		Ending.SetActive(true);
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Restart();
		}
	}

	private void Restart()
	{
		InitPiecesPosition();
		InitDicoOfPlayersPosition();
		Selector.SetActive(false);
		MovingSelector.SetActive(false);
		Ending.SetActive(false);

		pieceSelected = Vector2.zero;
		isPlayerFirst = true;
		nbSecondPiecePlaced = 0;
		nbFirstPiecePlaced = 0;
		phase = EPhase.PLACING;
		timeT = 0;
	}

	private void InitPiecesPosition()
	{
		for (int i = 0; i < NB_PIECES_MAX; i++)
		{
			First[i].transform.position = listFirstStartPiecesPosition[i];
			Second[i].transform.position = listSecondStartPiecesPosition[i];
		}
	}
}
