using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace SpriteClasses
{
    public class AnimationDictionary 
    {
        //dictionary of animations rather than list - with a dictionary, they can have names
        public Dictionary<string, Animation> animationDictionary = new Dictionary<string, Animation>();
        //which animation is currently playing
        public string CurrentAnimation { get; set; }

        public AnimationDictionary()
        {       }

        // This method adds an animation to the dictionary
        public void AddAnimation(String animationName, Animation animation)
        {
            animationDictionary.Add(animationName, animation);
        }
    }
}
