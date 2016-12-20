using System.Diagnostics;
using cn.bmob.api;
using cn.bmob.tools;
using KyGreenPet.bean;

namespace KyGreenPet.util
{
    class CloudServiceHelper
    {
        private string _filePath = string.Empty;
        private string _fileKey = string.Empty;
        private string _uploadToken = string.Empty;
        private static BmobWindows _bmobWindows;


        public CloudServiceHelper()
        {
            BmobBuilder();
        }

        /// <summary>
        /// Bmob初始化
        /// </summary>
        public static BmobWindows BmobBuilder()
        {
            if (_bmobWindows == null)
            {
                _bmobWindows = new BmobWindows();
                _bmobWindows.initialize(PresetInfo.BmobApplicationId, PresetInfo.BmobRestApiKey);
                BmobDebug.Register(msg => { Debug.WriteLine("BmobDebug:" + msg); });
            }
            return _bmobWindows;
        }
    }
}