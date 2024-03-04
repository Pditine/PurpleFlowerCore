using System;
using PurpleFlowerCore.Save;

namespace PurpleFlowerCore
{
    public static class SaveSystem
    {
#if PFC_SAVE_JSON
        private static SaveJsonModule _saveJsonModule;
        private static SaveJsonModule SaveJsonModule
        {
            get
            {
                if (_saveJsonModule is not null)return _saveJsonModule;
                _saveJsonModule = new SaveJsonModule();
                return _saveJsonModule;
            }
        }
        
        public static void Save(string fileName, object data)
        {
            SaveJsonModule.Save(fileName,data);
        }

        public static T Load<T>(string fileName) where T: class
        {
            return SaveJsonModule.Load<T>(fileName);
        }

        public static object Load(string fileName, Type type)
        {
            return SaveJsonModule.Load(fileName, type);
        }
        
#endif
        
#if PFC_SAVE_LITJSON
        private static SaveLitJsonModule _saveJsonModule;
        private static SaveLitJsonModule SaveJsonModule
        {
            get
            {
                if (_saveJsonModule is not null)return _saveJsonModule;
                _saveJsonModule = new SaveLitJsonModule();
                return _saveJsonModule;
            }
        }
        
        public static void Save(string fileName, object data)
        {
            SaveJsonModule.Save(fileName,data);
        }

        public static T Load<T>(string fileName) where T: class
        {
            return SaveJsonModule.Load<T>(fileName);
        }

        public static object Load(string fileName, Type type)
        {
            return SaveJsonModule.Load(fileName, type);
        }
#endif
    }
}