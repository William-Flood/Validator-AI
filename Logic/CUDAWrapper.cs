using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class CUDAWrapper
    {
        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int blockSize();

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void release(IntPtr toRelease);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void cuda_release(IntPtr toRelease);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr declare_transfer_block(int totalSize);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr establish_net_block(IntPtr toEstablish, int totalSize);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr findIntermediateBlock(IntPtr sensoryBlock, int sensoryCount, int intermediateCount, int motorCount, int netCount);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr findTallyBlock(IntPtr intermediateBlock, int intermediateCount, int motorCount, int netCount);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr findTempTallyBlock(IntPtr tallyBlock, int intermediateCount, int motorCount, int netCount);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void runCycle(int sensoryCount, int intermediateCount, int motorCount, int netCount, IntPtr sensoryBlock, IntPtr intermediateBlock, IntPtr tallyBlock, IntPtr tempTally, IntPtr sensoryInput, int sensoryInputLength);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getTally(IntPtr tallyBlock, int tallyingNeuronCount);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getNet(IntPtr encodedNet, int size);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CalculateBlockCount(int firingNeuronCount);

        [DllImport("CUDANetRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr establish_sensory_input(IntPtr toEstablish, int totalSize);
    }
}
