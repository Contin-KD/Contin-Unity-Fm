using System;
using System.Collections.Generic;
using System.Reflection;

namespace TGame.ECS
{
    public class SystemManager
    {
        private List<IAwakeSystem> list;

        public void LoadAllSystems()
        {
            foreach (var type in Assembly.GetCallingAssembly().GetTypes())
            {
                if (type.Name == "MoveAwakeSystem")
                {
                    Type awakeType = typeof(AwakeSystem<,>);
                    if (type.IsSubclassOf(awakeType))
                    {

                    }
                    if (awakeType.IsAssignableFrom(type))
                    {

                    }
                }
            }
        }

        public void Awake()
        {
        }
    }
}
