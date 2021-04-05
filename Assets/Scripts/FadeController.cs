using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要

public class FadeController : MonoBehaviour
{


	public float fadeSpeed = 0.002f;        //透明度が変わるスピードを管理

	float red, green, blue, alfa;   //パネルの色、不透明度を管理
	float redT, greenT, blueT, alfaT;   //パネルの色、不透明度を管理
	float red2, green2, blue2, alfa2;   //パネルの色、不透明度を管理

	public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ
	Image fadeImage, fadeImage2,fadeImageT;                //透明度を変更するパネルのイメージ
	public GameObject targetObject, targetObject2;
	void Start()
	{

		fadeImage = GetComponent<Image>();
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;

		fadeImage2 = targetObject2.GetComponent<Image>();
		red2   = fadeImage2.color.r;
		green2  = fadeImage2.color.g;
		blue2   = fadeImage2.color.b;
		alfa2   = fadeImage2.color.a;

		fadeImageT = targetObject.GetComponent<Image>();
		redT = fadeImageT.color.r;
		greenT = fadeImageT.color.g;
		blueT = fadeImageT.color.b;
		alfaT = fadeImageT.color.a;
	}

	void Update()
	{
		if (isFadeIn)
		{
			StartFadeIn();
		}

		if (isFadeOut)
		{
			StartFadeOut();
		}

	}

	void StartFadeIn()
	{

		alfaT -= fadeSpeed;                //a)不透明度を徐々に下げる
		SetAlphaT();                      //b)変更した不透明度パネルに反映する
		if (alfaT <= 0)
		{
			fadeImageT.enabled = false;    //d)パネルの表示をオフにする
			alfa -= fadeSpeed;                //a)不透明度を徐々に下げる
			SetAlpha();                      //b)変更した不透明度パネルに反映する
			if (alfa <= 0)
			{                    //c)完全に透明になったら処理を抜ける
				
				fadeImage.enabled = false;    //d)パネルの表示をオフにする
				alfa2 -= fadeSpeed;                //a)不透明度を徐々に下げる
				SetAlpha2();                      //b)変更した不透明度パネルに反映する
				if (alfa2 <= 0)
				{                    //c)完全に透明になったら処理を抜ける
					isFadeIn = false;
					fadeImage2.enabled = false;    //d)パネルの表示をオフにする
				}
				
			}
		}
	}

	void StartFadeOut()
	{
		fadeImage.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
		SetAlpha();               // c)変更した透明度をパネルに反映する
		if (alfa >= 1)
		{             // d)完全に不透明になったら処理を抜ける
			isFadeOut = false;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}

	void SetAlphaT()
	{
		fadeImageT.color = new Color(redT, greenT, blueT, alfaT);
	}

	void SetAlpha2()
	{
		fadeImage2.color = new Color(red2, green2, blue2, alfa2);
	}
}