using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandsInput : MonoBehaviour
{
    Animator handAnim;
    InputDevice input;
    public XRNode xrNodo;
    float gripFloat;
    bool gripBool;
    bool gripPress;

    public InputDeviceCharacteristics caracteristicas;
    List<InputDevice> listaInput = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        handAnim = GetComponent<Animator>();
        //input = InputDevices.GetDeviceAtXRNode(xrNodo);
        InputDevices.GetDevicesWithCharacteristics(caracteristicas, listaInput);
        if(listaInput.Count == 1)
        {
            input = listaInput[0];
            
        }
        else if (listaInput.Count > 1)
        {
            Debug.LogError("Hay mas de un contorl");
        }
        else
        {
            Debug.LogError("No detecta ningun control");
        }
    }

    // Update is called once per frame
    void Update()
    {
        input.TryGetFeatureValue(CommonUsages.grip, out gripFloat);
        handAnim.SetFloat("3Finger", gripFloat);

        input.TryGetFeatureValue(CommonUsages.trigger, out gripFloat);
        handAnim.SetFloat("indexFinger", gripFloat);

        input.TryGetFeatureValue(CommonUsages.primaryTouch, out gripBool);
        input.TryGetFeatureValue(CommonUsages.primaryButton, out gripPress);

        if (gripBool)
            gripFloat = 0.5f;
        else if (gripPress)
            gripFloat = 1;
        else
            gripFloat = 0;

        handAnim.SetFloat("thumbFinger", gripFloat);

        
        
    }
}
