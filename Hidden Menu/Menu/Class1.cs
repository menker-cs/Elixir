/*
using BepInEx;
using System;
using UnityEngine;

namespace SimpleMemoryLeakTracker
{
    [BepInPlugin("com.yourname.simplememoryleaktracker", "Simple Memory Leak Tracker", "1.0.0")]
    public class MemoryLeakPlugin : BaseUnityPlugin
    {
        private long lastMemoryUsed;

        private void Awake()
        {
            lastMemoryUsed = GetUsedMemory();
            Logger.LogInfo("Memory Leak Tracker Initialized.");
        }

        private void Update()
        {
            long currentMemoryUsed = GetUsedMemory();

            if (currentMemoryUsed - lastMemoryUsed > 1 * 1024 * 1024)
            {
                Logger.LogWarning($"Memory increased! Current memory usage: {currentMemoryUsed / 1024f / 1024f} MB");
            }

            lastMemoryUsed = currentMemoryUsed;
        }

        private long GetUsedMemory()
        {
            return GC.GetTotalMemory(false);
        }
    }
}*/