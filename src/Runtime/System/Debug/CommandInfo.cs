using System;
using UnityEngine;

namespace PurpleFlowerCore.PFCDebug
{
    //todo:改为支持拓展任意类型
    public interface ICommand
    {
        public Type ParamType{ get; }
        public void Invoke(string input);
    }
    
    public class CommandInfo : ICommand
    {
        public Action Action;
        
        public CommandInfo(Action action)
        {
            Action = action;
        }
        
        public static implicit operator CommandInfo(Action a) => new(a);


        public Type ParamType => null;

        public void Invoke(string _)
        {
            Action?.Invoke();
        }
    }
    
    public class CommandInfo_Int : ICommand
    {
        public Action<int> Action;
        
        public CommandInfo_Int(Action<int> action)
        {
            Action = action;
        }
        
        public static implicit operator CommandInfo_Int(Action<int> a) => new(a);

        public Type ParamType => typeof(int);

        public void Invoke(string input)
        {
            if (int.TryParse(input, out int result))
            {
                Action?.Invoke(result);
            }
            else
            {
                PFCLog.Error("DebugMenu", "Invalid input: " + input);
            }
        }
    }
    
    public class CommandInfo_Float : ICommand
    {
        public Action<float> Action;
        
        public CommandInfo_Float(Action<float> action)
        {
            Action = action;
        }
        
        public static implicit operator CommandInfo_Float(Action<float> a) => new(a);

        public Type ParamType => typeof(float);

        public void Invoke(string input)
        {
            if (float.TryParse(input, out float result))
            {
                Action?.Invoke(result);
            }
            else
            {
                PFCLog.Error("DebugMenu", "Invalid input: " + input);
            }
        }
    }
    
    public class CommandInfo_String : ICommand
    {
        public Action<string> Action;
        
        public CommandInfo_String(Action<string> action)
        {
            Action = action;
        }
        
        public static implicit operator CommandInfo_String(Action<string> a) => new(a);

        public Type ParamType => typeof(string);

        public void Invoke(string input)
        {
            Action?.Invoke(input);
        }
    }
    
    public class CommandInfo_Bool : ICommand
    {
        public Action<bool> Action;
        
        public CommandInfo_Bool(Action<bool> action)
        {
            Action = action;
        }
        
        public static implicit operator CommandInfo_Bool(Action<bool> a) => new(a);

        public Type ParamType => typeof(bool);

        public void Invoke(string input)
        {
            if (bool.TryParse(input, out bool result))
            {
                Action?.Invoke(result);
            }
            else
            {
                PFCLog.Error("DebugMenu", "Invalid input: " + input);
            }
        }
    }
}