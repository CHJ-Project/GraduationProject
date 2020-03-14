/*
 this is the script that controls the burning of the fire effect by playing the fire animation on the shader main texture.
 it works by offsetting the UV on the sprite sheet on the main texture.
 for this to work you need to input the grid number of columns and rows from the inspector
 the fire effect on off is controlled by calling ToggleFlame(); from other script by SendMessage("ToggleFlame"); to the fire object where this script is attached, or by getComponent(PlaySpriteSheetAnimation).ToggleFire();
 you can control the playback on spritesheet animation by toggling the bool playSSAnimation,
 but that will not turn off the particle effect and it will not rewind the animation to 0.
*/
using UnityEngine;
using System.Collections;

public class PlaySpriteSheetAnimation : MonoBehaviour {
	
	
    public int numberOfRows = 16;                       //number of rows in your sprite sheet
    public int numberOfColumns = 8;                     //number of columns in your sprite sheet
    
	public int animationStartFrame = 0;                 //the frame you want the animation to start playing. in the flame sword example it is 0
	
	public int loopStartFrame = 55;                    	//starting frame of the loop. set this to the frame that is loop start of your animation
    public int loopEndFrame = 126;                     	//end frame of the loop. after this frame if you have enableLoop set to true it will rewind to the loopStartFrame, if not it will stop at this frame as a last frame
	
	public bool enableLoop = true;                      //play it once, or rewind to loopStartFrame and play it from there
    
    public int playbackFPS = 40;						//the number of frames per second you wish the animation to have when playing forward
    public int shutDownFPS = 100;						//the number of frames per second you wish the animation to have when rewinding backward

	private float framePerSecond = 40;                  //the current number of frame per second
	
	private bool playSSAnimation = true;				//bool that starts playing the animation
    private bool shutdown = false;                      //bool that rewinds the animation to 0. and stooping the play when reach to 0 frame
    
	private int currentFame = 0;                        //index of the frame at given moment
    private ParticleSystem particleOnOff; 				//my fire effect have shiruken particle system on the model. this variable hold that system for easy access. used to turn of the particles when animation is not playing. if you use this script for playing other sprite sheet you will have to comment all code related for turning on or off the particle system

    private Vector2 sizeOfTheFrame;                     //on normal UV size is 1,1
    private float counter = 0.0f;                       //used to calculate frames per sec
    private int xIndex = 0;								//current row
    private int yIndex = 0;								//current column
	
	
	//use this to toggle the flame effect on the sword. you just need to call ToggleFlame() from code 
	public void ToggleFlame()
	{
		//if animation is playing
		if(playSSAnimation == true)
		{
			
			shutdown = true;												//set shutdown to true. this will rewind the animation to 0 and stop the animation and shuriken particle effect from playing
		}
		else
		{
			
			playSSAnimation = true;											//if animation is not playing then play the animation
		}
	}
	
	//this function is calculating the current frame. it is called in PlayTheAnimation() which is called in update and you don't have to use it or pass data to it
    void CalculateFrameIndex() 
    {
        if(shutdown)
        {
			
            framePerSecond = shutDownFPS;									//if you have different FPS when rewinding this line will set it
            
            counter += Time.deltaTime * framePerSecond;						//Time.deltaTime is returning 1 in a second. when you multiply it to the FPS it will return FPS in 1 sec
            
			//when counter go over 1
			if (counter >= 1.0f)
            {
                currentFame -= 1;											//reduce the current frame by 1
                counter = 0.0f;												//reset the counter to 0
            }
			
			//if current frame is less then 1
            if (currentFame <= 0)
            {
                currentFame = 0;											//set currentFrame to 0. you don't want negative number for current frame
                playSSAnimation = false;									//turn off playing the animation
                shutdown = false;											//turn of shutdown state
                particleOnOff.enableEmission = false;//and turn off particle from emitting particles. if you don't have particle effect you will need to comment this line out
            }
        }
		//if it is not the shutdown state it means that the sprite sheet is playing
        else 
        {
            
            counter += Time.deltaTime * framePerSecond;						//Time.deltaTime is returning 1 in a second. when you multiply it to the FPS it will return FPS in 1 sec
            
			//when counter go over 1
			if (counter >= 1.0f)
            {
                currentFame += 1;											//reduce the current frame by 1
                counter = 0.0f;												//reset the counter to 0
            }
            
            
            //if animation is looping 
            if (enableLoop)
            {
				//and current frame is at the loop end frame
                if (currentFame >= loopEndFrame)
                {
                    currentFame = loopStartFrame;							//rewind the current frame to the loop start frame
                }
            }
			//if the animation is not looping
            else 
            {
				//and current frame is at the loop end frame
                if (currentFame >= loopEndFrame)
                {
                    currentFame = animationStartFrame;						//rewind the current frame to the animation start frame
                    playSSAnimation = false;								//ad stop the playing of the animation
                }

            }
            
        }
		
		//if current frame is more then 0
        if (currentFame > 0)
        {
            particleOnOff.enableEmission = true;							//then particle effect should emit particles. if your effect don't have particles, you should comment out this all if statement 
        }
    }
	
	
	//this function is used to calculate the size of the UV frame
    void CalculateTheSizeOfTheFrame() 
    {
        sizeOfTheFrame = new Vector2(1.0f / numberOfColumns, 1.0f / numberOfRows); //the size is 1 devided by the number of collumns and rows. the size is rectangle so sizeX =  1 / number of columns and sizeY = 1 / num of rows
    }

	//this function is the main one that calculate the offset of the UV and its used to play the animation by setting the offset in the _MainTex of the first material on the game object that contain this script
    void PlayTheAnimation() 
    {
        CalculateFrameIndex();

        
        xIndex = currentFame % numberOfColumns;            									//current row
        yIndex = currentFame / numberOfColumns;            									//current column

        Vector2 offset;                                             						//the variable used to store x y offset for the texture UV

        
        
        offset =new Vector2 (xIndex * sizeOfTheFrame.x, ((numberOfRows-1)-yIndex)*sizeOfTheFrame.y); // calculate offset.  v coordinate is the bottom of the image in opengl so we need to invert.


        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);                             //set the offset in UV texture
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", sizeOfTheFrame);                      //Set the texture scale in UV texture
    }
	
	//start is called only once before update. it is used to initialize some variables
    void Start() 
    {
        particleOnOff = this.gameObject.GetComponent("ParticleSystem")as ParticleSystem;		//get the particle system and save it to particleOnOff variable
        CalculateTheSizeOfTheFrame();															//calculate the size of the UV frame
        currentFame = animationStartFrame;														//rewind the current frame to the start frame
    }

	// Update is called once per frame
	void Update ()
    {
		//if true
        if (playSSAnimation) 
        {
            framePerSecond = playbackFPS;														//set the FPS to playback fps
            PlayTheAnimation();																	//play the animation
        }
        
	}
}
