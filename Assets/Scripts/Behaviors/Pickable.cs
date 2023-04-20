using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pickable : MonoBehaviour
{
    [SerializeField]float speedMultiplier = 0.5f;
    [SerializeField]int numberToCarryMin = 0;
    [SerializeField]int numberToCarryMax = 2;
    [SerializeField]bool isFood = true;
    [SerializeField]float foodQuantity = 60;
    private int currentlyHolding = 0;
    private Rigidbody m_Rigidbody;
    private Transform m_transform;
    private float eatingSpeed = 10;
    private bool isFish = false;

    void Start(){
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_transform = GetComponentInChildren<Transform>();
    }

    public void InitAsFish(){
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_transform = GetComponentInChildren<Transform>();    
        speedMultiplier = 1f;
        numberToCarryMin = 0;
        numberToCarryMax = 0;
        isFood = true;
        foodQuantity = 60;
        isFish = true;
    }

    public void SetKinematic(bool g){
        m_Rigidbody.isKinematic = g;
    }

    public float GetSpeedMultiplier(){
        Debug.Log((transform.childCount/(Math.Max(numberToCarryMax, 1))));
        return speedMultiplier+ (1-speedMultiplier)*(transform.childCount/(Math.Max(numberToCarryMax, 1)));
    }

    public void AddFish(){
        currentlyHolding ++;
    }

    public int GetCurrentlyHolding(){
        return currentlyHolding;
    }

    public Transform GetTransform(){
        return m_transform;
    }

    public int getNumberToCarryMin(){
        return numberToCarryMin;
    }

    public int getNumberToCarryMax(){
        return numberToCarryMax;
    }

    public void IsLetDown(){
        if(!isFish){
            int cc = transform.childCount;
            for(int i = cc-1; i >= 0; i--)
            {
                m_transform.GetChild(i).GetComponentInChildren<Boid>().LetGo();
                Destroy(m_transform.GetChild(i).gameObject);
            }
        }
        currentlyHolding = 0;
    }

    public bool GetIsFood(){
        return isFood;
    }

    public float GetFoodQuantity(){
        return foodQuantity;
    }

    public float Eat(float time){
        foodQuantity -= time*(currentlyHolding+1) * eatingSpeed; 
        return time*(currentlyHolding+1) * eatingSpeed;
    }
}
