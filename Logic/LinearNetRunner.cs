using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;

namespace Logic
{
    static class LinearNetRunner
    {
        static void RunNet(int[,] sensoryNet, int[,] intermediateNet, int[] tally, int sensoryIndex, int threshhold)
        {
            int[] tempTally = new int[tally.Length];
            for(int sensorySynapse = 0; sensorySynapse < sensoryNet.GetLength(0); sensorySynapse++)
            {
                tempTally[sensorySynapse] = sensoryNet[sensorySynapse, sensoryIndex];
            }
            for(int intermediateNeuron = 0; intermediateNeuron < intermediateNet.GetLength(1); intermediateNeuron++)
            {
                if(tally[intermediateNeuron] > threshhold)
                {
                    for (int intermediateSynapse = 0; intermediateSynapse < intermediateNet.GetLength(0); intermediateSynapse++)
                    {
                        tempTally[intermediateSynapse] += intermediateNet[intermediateSynapse, intermediateNeuron];
                    }
                    tally[intermediateNeuron] = 0;
                }
            }

            for(int i = 0; i<tally.Length; i++)
            {
                tally[i] += tempTally[i];
                if(0 < tally[i])
                {
                    tally[i] = 0;
                }
            }
        }
    }
}
