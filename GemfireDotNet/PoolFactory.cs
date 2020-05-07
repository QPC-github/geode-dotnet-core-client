using System;
using System.Runtime.InteropServices;

namespace Apache 
{
    namespace Geode
    {
        namespace DotNetCore
        {
            public class PoolFactory : GemfireNativeObject
            {
                [DllImport(Constants.libPath,
                    CharSet = CharSet.Auto)]
                private static extern IntPtr apache_geode_Cache_GetPoolManager(IntPtr cache);

                [DllImport(Constants.libPath,
                    CharSet = CharSet.Auto)]
                private static extern IntPtr apache_geode_PoolManager_CreateFactory(IntPtr poolManager);

                [DllImport(Constants.libPath,
                    CharSet = CharSet.Auto)]
                private static extern IntPtr apache_geode_PoolFactory_AddLocator(IntPtr poolManager, IntPtr hostname, int port);

                [DllImport(Constants.libPath,
                    CharSet = CharSet.Auto)]
                private static extern IntPtr apache_geode_DestroyPoolFactory(IntPtr poolManager);

                internal PoolFactory(IntPtr poolManager)
                {
                    _containedObject = apache_geode_PoolManager_CreateFactory(poolManager);
                }

                public PoolFactory AddLocator(string hostname, int port)
                {
                    IntPtr hostnamePtr = Marshal.StringToCoTaskMemUTF8(hostname);
                    apache_geode_PoolFactory_AddLocator(_containedObject, hostnamePtr, port);
                    Marshal.FreeCoTaskMem(hostnamePtr);
                    return this;
                }

                public Pool CreatePool(string poolName)
                {
                    return new Pool(_containedObject, poolName);
                }

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                protected override void DestroyContainedObject()
                {
                    apache_geode_DestroyPoolFactory(_containedObject);
                    _containedObject = IntPtr.Zero;
                }
            }
        }
    }
}