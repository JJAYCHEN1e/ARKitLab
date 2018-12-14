﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;


public class HitResultController : MonoBehaviour {
    const int MAX = 2;
    public GameObject obj;
    private GameObject[] currentObj = new GameObject[MAX];
    //public Rigidbody[] currentObjRigid = new Rigidbody[MAX];
    private bool hitTestEnabled = false;
    public Slider m_slider;
    public Text text;
    public Text text2;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mHit;
        //射线检验  
        LayerMask mask = 1 << LayerMask.NameToLayer("Sphere");
        if (Physics.Raycast(mRay, out mHit, 20f, mask.value))
        {
            ;
        }else{

        if (hitTestEnabled && Input.touchCount > 0)
            {//如果有触摸
                var touch = Input.GetTouch(0); //获取第一个触摸点
                if (touch.phase == TouchPhase.Began)//触摸点的状态是开始或者移动中
                {
                    //屏幕坐标转为视口坐标
                    //Screenspace is defined in pixels.The bottom-left of the screen is (0, 0); 
                    //the right-top is (pixelWidth, pixelHeight).The z position is in world units from the camera.
                    //Viewport space is normalized and relative to the camera. The bottom-left of the camera is (0, 0);
                    //the top-right is (1, 1).The z position is in world units from the camera.
                    Vector3 screenPos = Camera.main.ScreenToViewportPoint(touch.position);
                    ARPoint point = new ARPoint//ARPoint 是个结构体
                    {
                        x = screenPos.x,
                        y = screenPos.y
                    };

                    /*
                    ARHitTestResultType[] resultTypes = {
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane,
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
                    };

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        List<ARHitTestResult> hitTestResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
                        if(hitTestResults.Count > 0){
                            foreach(var hitResult in hitTestResults){
                                if (currentObj[0] == null)
                                {
                                    currentObj[0] = Instantiate(obj);
                                    //currentObj[0].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform) + Vector3.up * currentObj[0].transform.localScale.y;
                                    currentObj[0].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                                    currentObj[0].transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                                    if (currentObj[1] != null) currentObj[0].transform.position.Set(currentObj[0].transform.position.x, currentObj[1].transform.position.y, currentObj[0].transform.position.z);
                                    //设置颜色还没有完成
                                    currentObj[0].mass = 5;
                                }else if (currentObj[1] == null)
                                {
                                    currentObj[1] = Instantiate(obj);
                                    //currentObj[1].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform) + Vector3.up * currentObj[0].transform.localScale.y;
                                    currentObj[1].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                                    currentObj[1].transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                                    if (currentObj[0] != null) currentObj[1].transform.position.Set(currentObj[1].transform.position.x, currentObj[0].transform.position.y, currentObj[1].transform.position.z);
                                    //设置颜色还没有完成
                                }
                            }
                        }
                    }
                    */

                    List<ARHitTestResult> hitTestResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
                    if (currentObj[0] == null)
                    {
                        currentObj[0] = Instantiate(obj);

                        //currentObj[0].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform) + Vector3.up * currentObj[0].transform.localScale.y;
                        currentObj[0].transform.position = UnityARMatrixOps.GetPosition(hitTestResults[hitTestResults.Count - 1].worldTransform);
                        currentObj[0].transform.rotation = UnityARMatrixOps.GetRotation(hitTestResults[hitTestResults.Count - 1].worldTransform);
                        text2.text = text2.text + " b1:" + currentObj[0].transform.position.ToString("F3");
                        if (currentObj[1] != null) currentObj[0].transform.position = new Vector3(currentObj[0].transform.position.x, currentObj[1].transform.position.y, currentObj[0].transform.position.z);
                        //设置颜色
                        //currentObj[0].GetComponent<Material>().color = Color.blue;


                        currentObj[0].GetComponent<Rigidbody>().mass = 5;
                        text2.text = text2.text + " a1:" + currentObj[0].transform.position.ToString("F3");
                        //text2.text = text2.text + " b1:" + currentObj[0].GetComponent<SphereCollider>().material.bounciness.ToString();
                    }
                    else if (currentObj[1] == null)
                    {
                        currentObj[1] = Instantiate(obj);
                        //currentObj[1].transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform) + Vector3.up * currentObj[0].transform.localScale.y;
                        currentObj[1].transform.position = UnityARMatrixOps.GetPosition(hitTestResults[hitTestResults.Count - 1].worldTransform);
                        currentObj[1].transform.rotation = UnityARMatrixOps.GetRotation(hitTestResults[hitTestResults.Count - 1].worldTransform);
                        text2.text = text2.text + " b2:" + currentObj[1].transform.position.ToString("F3");
                        if (currentObj[0] != null) currentObj[1].transform.position = new Vector3(currentObj[1].transform.position.x, currentObj[0].transform.position.y, currentObj[1].transform.position.z);
                        //设置颜色还没有完成
                        text2.text = text2.text + " a2:" + currentObj[1].transform.position.ToString("F3");
                        //text2.text = text2.text + " b2:" + currentObj[1].GetComponent<SphereCollider>().material.bounciness.ToString();
                    }
                }
            }
            text.text = currentObj[0].GetComponent<Rigidbody>().mass.ToString() + " " + currentObj[1].GetComponent<Rigidbody>().mass.ToString();
        }


    }

    public void EnableHitTest(){
        hitTestEnabled = true;
    }

    public void OnEnableHitTestClick()
    {
        EnableHitTest();
    }

    public void HitAnother(){
        if(currentObj[0]!=null && currentObj[1]!=null){
            Vector3 direction = currentObj[1].transform.position - currentObj[0].transform.position;
            //currentObj[0].AddForce(direction.normalized*0.5f, ForceMode.VelocityChange);
            currentObj[0].GetComponent<Rigidbody>().velocity = direction.normalized*0.5f;
            text2.text = text2.text+ " 方向:" + direction.ToString("F3");
            text2.text = text2.text + " V0:" + currentObj[0].GetComponent<Rigidbody>().velocity.ToString("F3");
            text2.text = text2.text + " V1:" + currentObj[1].GetComponent<Rigidbody>().velocity.ToString("F3");
        }
    }

    public void Reset()
    {
        foreach (var item in currentObj)
        {
            if (item != null) DestroyImmediate(item.gameObject);
        }
    }

    public void ChangeMass(float a){
        if (currentObj[0] != null) currentObj[0].GetComponent<Rigidbody>().mass = a;
    }

    public void ChangeMassT(string a){
        if (currentObj[0] != null) currentObj[0].GetComponent<Rigidbody>().mass = float.Parse(a);
    }

    public void showv(){
        text2.text = text2.text + " V0:" + currentObj[0].GetComponent<Rigidbody>().velocity.ToString("F3");
        text2.text = text2.text + " V1:" + currentObj[1].GetComponent<Rigidbody>().velocity.ToString("F3");
    }

    public void showpos(){
        text2.text = text2.text + " pos0:" + currentObj[0].transform.position.ToString("F3");
        text2.text = text2.text + " pos1:" + currentObj[1].transform.position.ToString("F3");
    }

    public void clear(){
        text2.text = "";
    }
}