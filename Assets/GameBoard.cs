using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    // değişken tanımla yabboz ölçüsü 
    public int m_size;
    public GameObject m_puzzlePiece;

    //ek3 
    public int m_randomPasses = 12;

    PuzzleSection[,] m_puzzle; //dizi tanım
    PuzzleSection m_puzzleSelection; //tıklanılan parçayı saklar


    //her eleman için 
    private void Start()
    {
        GameObject temp;

        m_puzzle = new PuzzleSection[m_size, m_size]; //atama

        for (int i = 0; i < m_size; i++)
        {
            for (int j = 0; j < m_size; j++)
            {
                temp = (GameObject)Instantiate(m_puzzlePiece, new Vector2(i * 500 / m_size, j * 250 / m_size), Quaternion.identity);
                temp.transform.SetParent(transform); //Parent olaylarında transform kullanılır

                m_puzzle[i, j] = (PuzzleSection)temp.GetComponent("PuzzleSection"); //saklanır
                m_puzzle[i, j].CreatePuzzlePiece(m_size);//fonksiyonunu çağır fotoları normal boyuta getirmek için
            }
        }
        SetupBoard();

        //ek1  
        RandomizePlacement(); // parçalar rastgele gelsin 


    }
    //ek2 
    void RandomizePlacement()
    {
        VectorInt2[] puzzleLocation = new VectorInt2[2];
        Vector2[] puzzleOffset = new Vector2[2];

        do
        {
            for (int i = 0; i < m_randomPasses; i++)
            {
                puzzleLocation[0].x = Random.Range(0, m_size);
                puzzleLocation[0].y = Random.Range(0, m_size);
                puzzleLocation[1].x = Random.Range(0, m_size);
                puzzleLocation[1].y = Random.Range(0, m_size);
                puzzleOffset[0] = m_puzzle[puzzleLocation[0].x, puzzleLocation[0].y].GetImageOffset();
                puzzleOffset[1] = m_puzzle[puzzleLocation[1].x, puzzleLocation[1].y].GetImageOffset();


                //fotolar karıştı altta
                m_puzzle[puzzleLocation[0].x, puzzleLocation[0].y].AssignImage(puzzleOffset[1]);
                m_puzzle[puzzleLocation[1].x, puzzleLocation[1].y].AssignImage(puzzleOffset[0]);
            }
        } while (CheckBoard() == true);
    }


    void SetupBoard()
    {
        Vector2 offset;
        Vector2 m_scale = new Vector2(1f / m_size, 1f / m_size);
        for (int i = 0; i < m_size; i++)
        {
            for (int j = 0; j < m_size; j++)
            {
                offset = new Vector2(i * (1f / m_size), j * (1f / m_size));
                m_puzzle[i, j].AssignImage(m_scale, offset);  //her parçanın puzzlesection kodu yazıldı puzzle sectionda
            }
        }
    }

    public PuzzleSection GetSelection()
    {
        return m_puzzleSelection;
    }
    public void SetSelection(PuzzleSection selection)
    {
        m_puzzleSelection = selection;
    }


    public bool CheckBoard()
    {
        for (int i = 0; i < m_size; i++)
        {
            for (int j = 0; j < m_size; j++)
            {
                if (m_puzzle[i, j].CheckGoodPlacement() == false)
                    return false;
            }
        }
        return true;
    }
    public void Win()
    {
        GetComponent<Animator>().SetTrigger("moveUp");
    }
}