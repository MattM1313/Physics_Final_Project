//Robert Evola
//Created March 23, 2013
//Modified March 23, 2013
//Physics Engine 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using SpriteClasses;

namespace ThePhysics
{
    public class PhysX
    {

        //Force Vectors
        private static Vector2 totalForce;
        private static Vector2 forceOfGravity;
        private static Vector2 forceOfWind;
        private static Vector2 forceOfFriction;
       


        //----------Math Methods-----------//
        
        //Sums all of the forces together
        public static void sumForces()
        {
            totalForce = forceOfGravity + forceOfWind + forceOfFriction;
        }

        public static void sumForces(bool grav, bool wind, bool friction)
        {
            if (grav)
                totalForce += forceOfGravity;

            if (wind)
                totalForce += forceOfWind;

            if (friction)
                totalForce += forceOfFriction;
        }

        //public static void updatePosition(Sprite obj, float timeInterval)
        //{
        //    obj.Force = totalForce;
        //    obj.Acceleration = calculateAcceleration1(obj.Force, obj.Mass);
        //    obj.Velocity = calculateVelocity1(obj.InitialVelocity, obj.Acceleration, timeInterval);
        //    obj.Position += calculateDisplacement2(obj.InitialVelocity, obj.Acceleration, timeInterval); 
        //    obj.InitialVelocity = obj.Velocity; //Resetting velocities
        //}

        //----------Calcuate Force------------//
        public static Vector2 calculateForce(Vector2 accel, float mass)
        {
            return mass * accel;
        }


        //---------Calculate Acceleration-------------//
        public static Vector2 calculateAcceleration1(Vector2 force, float mass)
        {
            return force / mass;
        }

        public static Vector2 calculateAcceleration2(Vector2 vI, Vector2 vF, float time)
        {
            return ((vF - vI) / time);
        }

        public static Vector2 calculateAcceleration3(Vector2 displacement, Vector2 vI, float time)
        {
            return ((2 * (displacement - (vI * time))) / (time * time));
        }

        public static Vector2 calculateAcceleration4(Vector2 displacement, Vector2 vI, Vector2 vF)
        {
            return ((vF * vF) - (vI * vI)) / (2 * displacement);
        }

        //End of Acceleration


        //-----------Calculate Velocity-----------//
        public static Vector2 calculateVelocity1(Vector2 vI, Vector2 accel, float time)
        {
            return (vI + (accel * time));
        }

        public static Vector2 calculateVelocity2(Vector2 displacement, Vector2 accel, float time)
        {
            return ( (displacement - ((accel/2) * (time * time)) ) / time);
        }

        public static Vector2 calculateVelocity3(Vector2  displacement, Vector2 vI, Vector2 accel)
        {
            Vector2 v = (vI * vI) + (2 * accel * displacement);
            v.X = (float)Math.Sqrt(v.X);
            v.Y = (float)Math.Sqrt(v.Y);
            return v;
        }

        public static Vector2 calculateVelocity4(Vector2 displacement, Vector2 vF, Vector2 accel)
        {
            Vector2 v = (vF * vF) - (2 * accel * displacement);
            v.X = (float)Math.Sqrt(v.X);
            v.Y = (float)Math.Sqrt(v.Y);
            return v;
        }
        //End of Velocity


        //------------Calculate Displacement-----------//
        public static Vector2 calculateDisplacement1(Vector2 vI, Vector2 vF, float time)
        {
            return ((vF + vI) / 2) * time;
        }

        public static Vector2 calculateDisplacement2(Vector2 vI, Vector2 accel, float time)
        {
            return ((vI * time) + ((accel / 2) * (time * time)));
        }

        public static Vector2 calculateDisplacement3(Vector2 vI, Vector2 vF, Vector2 accel)
        {
            return ((vF * vF) - (vI * vI)) / (2 * accel);
        }
        //End of Displacement



        //----------Get and Set Methods----------//
        
        //Force of Gravity
        public static void setGravityForce(Vector2 force)
        {
            forceOfGravity = force;
        }

        public static Vector2 getGravityForce()
        {
            return forceOfGravity;
        }

        //Force of Wind
        public static void setWindForce(Vector2 force)
        {
            forceOfWind = force;
        }

        public static Vector2 getWindForce()
        {
            return forceOfWind;
        }

        //Force of Friction
        public static void setFrictionForce(Vector2 force)
        {
            forceOfFriction = force;
        }

        public static Vector2 getFrictionForce()
        {
            return forceOfFriction;
        }

        //Total Force
        public static Vector2 getTotalForce()
        {
            return totalForce;
        }
        //-------End of Get and Set Methods-------//

    }//End of PhysX
}//End of File
