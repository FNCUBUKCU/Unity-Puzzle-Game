using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class PuzzleSection : MonoBehaviour{

    Vector2 m_goodOffset; //fotonun bulunması gereken ilk yeri gösterir
    Vector2 m_offset;//bozuk olan foto.yeri değiştirme
    Vector2 m_scale; //2 ye 2 puzzle ise iki yöne hareket eder

    GameBoard m_gameBoard; ///her foto parçasının gameboard a erişmesi için tanımlandı
        //onclick yazmak içi lazım alttaki
    void Start()
    {
        m_gameBoard = (GameBoard)GameObject.FindObjectOfType<Canvas>().GetComponentInChildren<GameBoard>();
    }

    public void CreatePuzzlePiece(int size)
    {
        transform.localScale = new Vector3(1.0f * transform.localScale.x / size, 1.0f * transform.localScale.z / size, 1);
    }
    public void AssignImage(Vector2 scale,Vector2 offset) //bir kere çalışan assi. ımage //scale parçalar ne kadar küçülecek offset parç.nekadar kaydırılacak
    {
        m_goodOffset = offset;
        m_scale = scale;
        AssignImage(offset); //puzzle oynarken surekli çalışacak olan ass ımage
    }
    public void AssignImage(Vector2 offset)
    {
        m_offset = offset;
        GetComponent<RawImage>().uvRect = new Rect(offset.x, offset.y, m_scale.x, m_scale.y);
    }

    public void OnClick()
    {
        PuzzleSection previousSelection = m_gameBoard.GetSelection();
        if (previousSelection != null)
        {
            previousSelection.GetComponent<RawImage>().color = Color.white; //bir önceki parça rengini beyaza dönürme
            Vector2 tempOffset = previousSelection.GetImageOffset(); //bir önceki tıklananı sakladık
            previousSelection.AssignImage(m_offset); //ikinci tıklanana gönderdik
            AssignImage(tempOffset);
            m_gameBoard.SetSelection(null);

            //kontrol panel yukarı kalksın
            if(m_gameBoard.CheckBoard()==true)
                 m_gameBoard.Win();
            
        }else  //ilk tıklama
        {
            GetComponent<RawImage>().color = Color.gray;
            m_gameBoard.SetSelection(this);
        }
    }

    public Vector2 GetImageOffset()
    {
        return m_offset;
    }

    public bool CheckGoodPlacement()
    {
        return (m_goodOffset == m_offset);
    }

}
