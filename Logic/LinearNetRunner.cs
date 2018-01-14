using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;
using System.IO;

namespace Logic
{
    public static class LinearNetRunner
    {
        public static void RunNet(int[,,] sensoryNet, int[,,] intermediateNet, int[,] tally, int sensoryIndex, int threshhold)
        {
            int[,] tempTally = new int[tally.GetLength(0), sensoryNet.GetLength(2)];
            for (int netIndex = 0; netIndex < sensoryNet.GetLength(2); netIndex++)
            {
                for (int sensorySynapse = 0; sensorySynapse < sensoryNet.GetLength(0); sensorySynapse++)
                {
                    tally[sensorySynapse, netIndex] += sensoryNet[sensorySynapse, sensoryIndex, netIndex];
                }
                for (int cycleIndex = 0; cycleIndex < 10; cycleIndex++)
                {
                    bool intermediateFired = false;
                    for (int intermediateNeuron = 0; intermediateNeuron < intermediateNet.GetLength(1); intermediateNeuron++)
                    {
                        if (tally[intermediateNeuron, netIndex] >= threshhold)
                        {
                            intermediateFired = true;
                            for (int intermediateSynapse = 0; intermediateSynapse < intermediateNet.GetLength(0); intermediateSynapse++)
                            {
                                tempTally[intermediateSynapse, netIndex] += intermediateNet[intermediateSynapse, intermediateNeuron, netIndex];
                            }
                        }
                    }
                    if (!intermediateFired && cycleIndex > 0)
                    {
                        break;
                    }
                    for (int i = 0; i < tally.GetLength(0); i++)
                    {
                        if (threshhold <= tally[i, netIndex] && i < intermediateNet.GetLength(1))
                        {
                            tally[i, netIndex] = 0;
                        }
                        tally[i, netIndex] += tempTally[i, netIndex];
                        tempTally[i, netIndex] = 0;
                        if (0 > tally[i, netIndex])
                        {
                            tally[i, netIndex] = 0;
                        }
                    }
                }
            }
        }


        public static void RunNet(int[,,] sensoryNet, int[,,] intermediateNet, int[,] tally, int sensoryIndex, int threshhold, String fileName, bool fileMode)
        {
            using (var debugWriter = new StreamWriter(fileName,fileMode))
            {
                int[,] tempTally = new int[tally.GetLength(0), sensoryNet.GetLength(2)];
                for (int netIndex = 0; netIndex < sensoryNet.GetLength(2); netIndex++)
                {
                    debugWriter.WriteLine("---Net: " + netIndex);
                    debugWriter.WriteLine("tally:");
                    debugWriter.WriteLine(WriteRow(tally, netIndex));
                    debugWriter.WriteLine("Sensory row at index " + sensoryIndex + ":");
                    debugWriter.WriteLine(WriteRow(CollapseBrick(sensoryNet, netIndex),sensoryIndex));
                    for (int sensorySynapse = 0; sensorySynapse < sensoryNet.GetLength(0); sensorySynapse++)
                    {
                        tempTally[sensorySynapse, netIndex] = sensoryNet[sensorySynapse, sensoryIndex, netIndex];
                    }
                    for (int cycleIndex = 0; cycleIndex < 11; cycleIndex++)
                    {
                        bool intermediateFired = false;
                        for (int intermediateNeuron = 0; intermediateNeuron < intermediateNet.GetLength(1); intermediateNeuron++)
                        {
                            if (tally[intermediateNeuron, netIndex] >= threshhold)
                            {
                                debugWriter.WriteLine("Intermediate row:");
                                debugWriter.WriteLine(WriteRow(CollapseBrick(intermediateNet, netIndex), intermediateNeuron));
                                intermediateFired = true;
                                for (int intermediateSynapse = 0; intermediateSynapse < intermediateNet.GetLength(0); intermediateSynapse++)
                                {
                                    tempTally[intermediateSynapse, netIndex] += intermediateNet[intermediateSynapse, intermediateNeuron, netIndex];
                                }
                            }
                        }
                        if (!intermediateFired && cycleIndex > 0)
                        {
                            break;
                        }
                        for (int i = 0; i < tally.GetLength(0); i++)
                        {
                            if (threshhold <= tally[i, netIndex] && i < intermediateNet.GetLength(1))
                            {
                                tally[i, netIndex] = 0;
                            }
                            tally[i, netIndex] += tempTally[i, netIndex];
                            tempTally[i, netIndex] = 0;
                            if (0 > tally[i, netIndex])
                            {
                                tally[i, netIndex] = 0;
                            }
                        }
                        debugWriter.WriteLine("New tally:");
                        debugWriter.WriteLine(WriteRow(tally, netIndex));
                    }
                }
            }
        }

        public static String WriteRow(int[,] grid, int index)
        {
            if(grid.Length == 0)
            {
                throw new Exception("Empty grid");
            }
            if(grid.GetLength(1) < index)
            {
                throw new IndexOutOfRangeException();
            }
            StringBuilder rowWriter = new StringBuilder(grid[0,index].ToString());
            for(int i = 1; i<grid.GetLength(0); i++)
            {
                rowWriter.Append("," + grid[i, index]);
            }
            return rowWriter.ToString();
        }

        static int[,] CollapseBrick(int[,,] brick, int index)
        {
            var grid = new int[brick.GetLength(0), brick.GetLength(1)];
            for(int rowIndex = 0; rowIndex < brick.GetLength(1); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < brick.GetLength(0); columnIndex++)
                {
                    grid[columnIndex, rowIndex] = brick[columnIndex, rowIndex, index];
                }
            }
            return grid;
        }
    }
}
