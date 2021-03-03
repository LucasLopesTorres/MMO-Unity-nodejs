using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public static class ConverJsonVector 
{
    public static Vector3 JsonToVector3(string jsonVector){
        
        Vector3 convert;

        string[] jsonVector_ = Regex.Split(jsonVector, ",");
        
        convert = new Vector3(float.Parse(jsonVector_[0]), float.Parse(jsonVector_[1]));
        
        return convert; 
    }

    public static Vector4 JsonToVector4(string jsonVector){

        Vector4 convert;
        string[] jsonVector_ = Regex.Split(jsonVector, ",");

         convert = new Vector4(float.Parse(jsonVector_[0]), float.Parse(jsonVector_[1]),  float.Parse(jsonVector_[2]),
          float.Parse(jsonVector_[3]));

          return convert;
    }
}
