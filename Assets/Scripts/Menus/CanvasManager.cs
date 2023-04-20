using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    //[SerializeField] private Text text1;
    [SerializeField] private Text text2;
    //[SerializeField] private MainFishManager player;
    private List<float> listfps = new List<float>();
    private float count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //text1.text = "food : "+ player.GetFood();
        listfps.Add(1/Time.deltaTime);
        count += Time.deltaTime;
        if(count > 0.3){
            text2.text = "fps : "+average();
            listfps = new List<float>();
            count = 0;
        }
    }

    float average(){
        float tot = 0;
        int tot2 = 0;
        foreach(float f in listfps){
            tot += f;
            tot2 ++;
        }
        return tot/tot2;
    }
}
