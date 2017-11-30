using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;

namespace Logic
{
    public static class LinearNetRunner
    {
        public static void RunNet(int[,,] sensoryNet, int[,,] intermediateNet, int[,] tally, int sensoryIndex, int threshhold)
        {
            int[,] tempTally = new int[tally.Length, sensoryNet.GetLength(2)];
            for(int netIndex = 0; netIndex < sensoryNet.GetLength(2); netIndex++)
            {
                for(int sensorySynapse = 0; sensorySynapse < sensoryNet.GetLength(0); sensorySynapse++)
                {
                    tempTally[sensorySynapse, netIndex] = sensoryNet[sensorySynapse, sensoryIndex, netIndex];
                }
                for(int cycleIndex = 0; cycleIndex<11; cycleIndex++)
                {
                    for(int intermediateNeuron = 0; intermediateNeuron < intermediateNet.GetLength(1); intermediateNeuron++)
                    {
                        if(tally[intermediateNeuron, netIndex] > threshhold)
                        {
                            for (int intermediateSynapse = 0; intermediateSynapse < intermediateNet.GetLength(0); intermediateSynapse++)
                            {
                                tempTally[intermediateSynapse, netIndex] += intermediateNet[intermediateSynapse, intermediateNeuron, netIndex];
                            }
                            tally[intermediateNeuron, netIndex] = 0;
                        }
                    }

                    for(int i = 0; i<tally.Length; i++)
                    {
                        if (threshhold >= tally[i, netIndex])
                        {
                            tally[i, netIndex] = 0;
                        }
                        tally[i, netIndex] += tempTally[i, netIndex];
                        if(0 < tally[i, netIndex])
                        {
                            tally[i, netIndex] = 0;
                        }
                    }
                }
            }
        }
    }
}
