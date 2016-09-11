using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FairyGUI;


public class DialogueTest : MonoBehaviour {

	// Use this for initialization
	void Start1 () {
        //GComponent view = UIPackage.CreateObject("DialogPanel", "Main") as GComponent;
        UIPackage.AddPackage("DialogPanel");
        gameObject.transform.position = new Vector3(0f, 0f, 0f);
        gameObject.AddComponent<UIPanel>();
        gameObject.layer = 5;
        UIPanel panel = gameObject.GetComponent<UIPanel>();

        panel.packageName = "DialogPanel";
        panel.componentName = "Main";
        panel.CreateUI();
        GComponent view = panel.ui;

    }


    GComponent view;
    void OnEnable()
    {

        Dictionary<string, GComponent> dic = new Dictionary<string, GComponent>();
        UIPackage.AddPackage("DialogPanel");
        //DialogPanel.DialogPanelBinder.BindAll();
        //DialogPanel.UI_Main uimain = DialogPanel.UI_Main.CreateInstance();
        view = UIPackage.CreateObject("DialogPanel", "Main") as GComponent;
        
        GRoot.inst.AddChild(view);
        if (!dic.ContainsKey(string.Format("{0}", view.GetType().FullName)))
        {
            dic.Add(string.Format("{0}", view.GetType().FullName), view);
        }
        GTextField T = ((GComponent)view).GetChild("Text") as GTextField;
        T.text = "213421351354243324421343241243214";
        targetUIText = T;
        textCache = targetUIText.text;
        T.text = "";
        Speed = 0.05f;
        StartCoroutine(ProcessType());
        //view.Dispose();



    }

    void OnDisable()
    {
        view.Dispose();
    }

    private GTextField targetUIText;
    private string textCache;
    public AudioClip TypeSound;
    public AudioClip TypeFinishedSound;
    public float Speed;

    IEnumerator ProcessType()
    {
        // type text till done all.
        while (targetUIText.text.Length < textCache.Length)
        {
            targetUIText.text += textCache[targetUIText.text.Length];
            if (TypeSound != null)
            {
                //AudioManager.Instance.Play(TypeSound);
            }
            yield return new WaitForSeconds(Speed);
        }
        if (TypeFinishedSound != null)
        {
           // AudioManager.Instance.Play(TypeFinishedSound);
        }
    }
}
