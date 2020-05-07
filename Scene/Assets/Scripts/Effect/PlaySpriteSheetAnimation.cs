/*
这是通过在明暗器主纹理上播放火焰动画来控制火焰效果燃烧的脚本。
它的工作原理是在主纹理上偏移sprite工作表上的UV。
为此，您需要从检查器中输入列和行的网格数
关闭时的火焰效果由SendMessage（“ToggleFlame”）从其他脚本调用ToggleFlame（）；到附加此脚本的火焰对象，或由getComponent（PlaySpriteSheetAnimation）.ToggleFire（）控制；
您可以通过切换bool playsanimation来控制spritesheet动画上的播放，
但这不会关闭粒子效果，也不会将动画倒回0。
*/
using UnityEngine;
using System.Collections;

public class PlaySpriteSheetAnimation : MonoBehaviour {
	
	
    public int numberOfRows = 16;                       //行数
    public int numberOfColumns = 8;                     //列数

    public int animationStartFrame = 40;                 //希望动画开始播放的帧。在火焰剑的例子中是0

    public int loopStartFrame = 55;                    	//循环的起始帧.将此设置为动画循环开始的帧
    public int loopEndFrame = 126;                     	//循环的结束帧。在此帧之后，如果enableLoop设置为true，则它将倒回loopStartFrame，否则它将作为最后一帧在此帧停止

    public bool enableLoop = true;                      //播放一次，或倒带到loopStartFrame并从那里播放

    public int playbackFPS = 40;						//向前播放时希望动画每秒具有的帧数
    public int shutDownFPS = 100;						//倒带时希望动画每秒具有的帧数

    private float framePerSecond = 40;                  //当前每秒帧数

    private bool playSSAnimation = true;				//控制开始播放效果
    private bool shutdown = false;                      //控制取消播放效果
    
	private int currentFame = 0;
    private ParticleSystem particleOnOff;

    private Vector2 sizeOfTheFrame;
    private float counter = 0.0f;
    private int xIndex = 0;
    private int yIndex = 0;
	
	
	//控制效果启动
	public void ToggleFlame()
	{
		if(playSSAnimation == true)
		{
			shutdown = true;
		}
		else
		{
			playSSAnimation = true;
		}
	}
	
	//计算当前帧
    void CalculateFrameIndex() 
    {
        if(shutdown)
        {
            framePerSecond = shutDownFPS;									//设置倒带时的FPS
            
            counter += Time.deltaTime * framePerSecond;
            
			//when counter go over 1
			if (counter >= 1.0f)
            {
                currentFame -= 1;											//当前帧减一
                counter = 0.0f;												//重置counter为0
            }
			
			//if current frame is less then 1
            if (currentFame <= 0)
            {
                currentFame = 0;											//将currentFrame设置为0。
                playSSAnimation = false;									//关闭播放动画
                shutdown = false;											//关闭状态转换
                particleOnOff.enableEmission = false;                       //禁用粒子系统
            }
        }
        else 
        {
            
            counter += Time.deltaTime * framePerSecond;

            //当计数器超过1时
			if (counter >= 1.0f)
            {
                currentFame += 1;											//当前帧减一
                counter = 0.0f;												//重置counter为0
            }
            
            
            //效果循环播放时
            if (enableLoop)
            {
				//当效果播放到最后一帧时，将当前帧设置为起始帧
                if (currentFame >= loopEndFrame)
                {
                    currentFame = loopStartFrame;
                }
            }
			//非循环播放时
            else 
            {
                //当效果播放到最后一帧时
                if (currentFame >= loopEndFrame)
                {
                    currentFame = animationStartFrame;						//将当前帧回放到动画开始帧
                    playSSAnimation = false;								//停止播放动画
                }

            }
            
        }

        //如果当前帧大于0
        if (currentFame > 0)
        {
            particleOnOff.enableEmission = true;							//启动粒子系统
        }
    }


    //此函数用于计算UV帧的大小
    void CalculateTheSizeOfTheFrame() 
    {
        sizeOfTheFrame = new Vector2(1.0f / numberOfColumns, 1.0f / numberOfRows); //size为1除以行列数之和，sizeX为1/列数，sizeY为1/行数
    }

	//设置偏移量，实现动画效果
    void PlayTheAnimation() 
    {
        CalculateFrameIndex();

        xIndex = currentFame % numberOfColumns;            									//当前行数
        yIndex = currentFame / numberOfColumns;            									//当前列数

        Vector2 offset;                                             						//用于存储纹理UV的x y偏移量的变量

        offset = new Vector2(xIndex * sizeOfTheFrame.x, ((numberOfRows - 1) - yIndex) * sizeOfTheFrame.y); //计算偏移量。在opengl中，v坐标是图像的底部，所以需要进行反转。

        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);                             //设置偏移量
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", sizeOfTheFrame);                      //在“UV纹理”中设置纹理比例
    }
	
	//start is called only once before update. it is used to initialize some variables
    void Start() 
    {
        particleOnOff = this.gameObject.GetComponent("ParticleSystem") as ParticleSystem;		//获取粒子系统并将其保存到particleOnOff变量
        CalculateTheSizeOfTheFrame();															//计算UV帧的大小
        currentFame = animationStartFrame;														//将当前帧回放到开始帧
    }

	// Update is called once per frame
	void Update ()
    {
        //每帧检测是否开启火焰效果
        if (playSSAnimation) 
        {
            framePerSecond = playbackFPS;														//设置FPS
            PlayTheAnimation();																	//播放动画
        }
        
	}
}
