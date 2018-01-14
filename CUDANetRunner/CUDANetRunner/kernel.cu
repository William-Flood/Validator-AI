
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <iostream>
#include <cstring>

#define THRESHHOLD 20;
#define BLOCKSIZE 512;


extern "C" {

	__declspec(dllexport) int CalculateBlockCount(int firingNeuronCount) {
		unsigned int blockSize = BLOCKSIZE;
		unsigned int firingBlocks = firingNeuronCount / blockSize;
		if (0 != firingNeuronCount % blockSize) {
			firingBlocks++;
		}
		return firingBlocks;
	}
}


__global__ void activateSensory(const int *sensoryGridBlock, int *activeBlock, const int sensoryIndex, const int neuronCount) {
	activeBlock[threadIdx.x + blockIdx.x*neuronCount*blockDim.x] =
		sensoryGridBlock[threadIdx.x + sensoryIndex*neuronCount + blockIdx.x*neuronCount*blockDim.x];
}

//use dim3 to set multidimensional thread count
//dim3 blockDim(xNeuronCount, yNeuronCount, netCount);
//collapse<<<1,blockDim>>>(activationGrid, span);

__global__ void setValues(int *activationGrid, const int width, const int depth) {
	int neuronIndex = threadIdx.x + blockDim.x * blockIdx.x;
	int collapseOn = threadIdx.y;
	int netIndex = threadIdx.z;
	activationGrid[neuronIndex + collapseOn*width + netIndex*width*depth] = 1;
}

__global__ void increment(int *activationGrid, const int width, const int depth, const int size, const int pad_to) {
	int neuronIndex = threadIdx.x + blockDim.x * blockIdx.x;
	int collapseOn = threadIdx.y;
	int netIndex = threadIdx.z;
	int currentIndex = neuronIndex + collapseOn*width + netIndex*width*depth;
	if (size > currentIndex) {
		for (int i = 0; i < pad_to; i++) {
			activationGrid[currentIndex] += 1;
		}
	}
}

//Successively split the shared memory array in half, adding the values from the second half to the first half 
//(eg adding the value at index 15 to index 7, then the value at index 7 to index 3, then the value at index 3 to index 1, 
//and then adding the value at index 1 to index 0 - at the end, the value at index 0 will contain the sum of all the values in the original list)
__device__ void
reduceBlock(volatile int *sdata, int mySum, const unsigned int tid, unsigned int firingNeuronCount, int* intermediates, unsigned int intermediateIndex)
{
	sdata[tid] = mySum;
	__syncthreads();
	int blockSize = blockDim.x;
	if (firingNeuronCount < threadIdx.x + blockSize*blockIdx.x) {
		return;
	}
	// do reduction in shared mem
	int nextSummedIndex = 256;
	int indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 512)
	//{
	//	if (tid < 256)
	//	{
	//		sdata[tid] = mySum = mySum + sdata[tid + 256];
	//	}
	//
	//	__syncthreads();
	//}

	__syncthreads();
	nextSummedIndex = 128;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 256)
	//{
	//	if (tid < 128)
	//	{
	//		sdata[tid] = mySum = mySum + sdata[tid + 128];
	//	}
	//
	//	__syncthreads();
	//}

	__syncthreads();
	nextSummedIndex = 64;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 128)
	//{
	//	if (tid <  64)
	//	{
	//		sdata[tid] = mySum = mySum + sdata[tid + 64];
	//	}
	//
	//	__syncthreads();
	//}

	//if (tid < 32)
	//{
	__syncthreads();
	nextSummedIndex = 32;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 64)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 32];
	//}

	__syncthreads();
	nextSummedIndex = 16;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 32)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 16];
	//}

	nextSummedIndex = 8;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 16)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 8];
	//}

	nextSummedIndex = 4;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 8)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 4];
	//}

	nextSummedIndex = 2;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 4)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 2];
	//}

	nextSummedIndex = 1;
	indexEscape = (tid + nextSummedIndex) < blockSize;
	mySum += indexEscape * sdata[(tid + nextSummedIndex) * indexEscape];
	sdata[tid] = mySum;
	//if (blockSize >= 2)
	//{
	//	sdata[tid] = mySum = mySum + sdata[tid + 1];
	//}
	//}
	//intermediates[intermediateIndex] = sdata[tid];
}

/*template <unsigned int blockSize, bool nIsPow2>
__device__ void
reduceBlocks(const int *g_idata, int *g_odata, unsigned int n)
{
	extern __shared__ int sdata[];

	// perform first level of reduction,
	// reading from global memory, writing to shared memory
	unsigned int tid = threadIdx.x;
	unsigned int i = blockIdx.x*(blockSize * 2) + threadIdx.x;
	unsigned int gridSize = blockSize * 2 * gridDim.x;
	int mySum = 0;

	// we reduce multiple elements per thread.  The number is determined by the
	// number of active thread blocks (via gridDim).  More blocks will result
	// in a larger gridSize and therefore fewer elements per thread
	while (i < n)
	{
		mySum += g_idata[i];

		// ensure we don't read out of bounds -- this is optimized away for powerOf2 sized arrays
		if (nIsPow2 || i + blockSize < n)
			mySum += g_idata[i + blockSize];

		i += gridSize;
	}

	// do reduction in shared mem
	reduceBlock<blockSize>(sdata, mySum, tid);

	// write result for this block to global mem
	if (tid == 0) { g_odata[blockIdx.x] = sdata[0]; }
}*/



__device__ void
tallyNeurons(int* intermediateBlock, int* tallyBlock, int* tempTally, unsigned int firingNeuronCount)
{
	extern __shared__ int sdata[];
	int threshhold = THRESHHOLD;
	// perform first level of reduction,
	// reading from global memory, writing to shared memory

	unsigned int firingNeuron = threadIdx.x + blockIdx.x*blockDim.x;
	unsigned int countEscape = firingNeuronCount > firingNeuron;
	
	unsigned int tallyingNeuron = blockIdx.y;
	unsigned int tallyingNeuronCount = gridDim.y;
	unsigned int network = blockIdx.z;
	unsigned int networkIndex = network*tallyingNeuronCount*firingNeuronCount;
	unsigned int firingNeuronIndex = networkIndex + (firingNeuron)*tallyingNeuronCount;
	unsigned int synapseIndex = tallyingNeuron + firingNeuronIndex;
	int mySum = intermediateBlock[synapseIndex] * countEscape * (threshhold <= tallyBlock[(firingNeuron)+network*tallyingNeuronCount]);
	//intermediateBlock[synapseIndex] += 1000* intermediateBlock[synapseIndex] * countEscape * (threshhold <= tallyBlock[(firingNeuron)+network*tallyingNeuronCount]);;
	// do reduction in shared mem
	reduceBlock(sdata, mySum, threadIdx.x, firingNeuronCount, intermediateBlock, synapseIndex);

	// write result for this block to global mem
	unsigned int tempTallyIndex = network*gridDim.x*tallyingNeuronCount + tallyingNeuron*gridDim.x + blockIdx.x;
	if (threadIdx.x == 0) {
		tempTally[tempTallyIndex] = sdata[0];
		//Debug
		//intermediateBlock[synapseIndex] += (sdata[0] + 1) * 1000;
	}

}


__global__ void
AddSensoryTally(int* sensoryBlock, int sensoryCount, int* tallyBlock, int* tempTally, unsigned int sensoryIndex, unsigned int tallyingNeuronCount)
{
	// perform first level of reduction,
	// reading from global memory, writing to shared memory
	unsigned int tallyingNeuronID = threadIdx.x + blockIdx.x * blockDim.x;
	unsigned int inRangeFlag = tallyingNeuronID < tallyingNeuronCount;
	unsigned int sensoryNeuronIndex = blockIdx.y * tallyingNeuronCount * sensoryCount + sensoryIndex * tallyingNeuronCount;
	unsigned int sensoryValue = sensoryBlock[sensoryNeuronIndex+ tallyingNeuronID] * inRangeFlag;
	//sensoryBlock[sensoryNeuronIndex + tallyingNeuronID] = 1000 * inRangeFlag - 500;
	int* storageRoot = tallyBlock + (tempTally - tallyBlock) * (1 - inRangeFlag);
	unsigned int tallyingNeuronIndex = tallyingNeuronID + blockIdx.y * tallyingNeuronCount;
	storageRoot[tallyingNeuronIndex] += sensoryValue;
}


/*template <unsigned int blockSize, bool nIsPow2>
__global__ void
reduceMultiPass(const int *g_idata, int *g_odata, unsigned int n)
{
	reduceBlocks<blockSize, nIsPow2>(g_idata, g_odata, n);
}*/


/*template <unsigned int blockSize, bool nIsPow2>
__global__ void
reduceMultiPass(const int *g_idata, int *g_odata, unsigned int n, unsigned int sensoryIndex)
{
	reduceBlocks<blockSize, nIsPow2>(g_idata, g_odata, n, sensoryIndex);
}*/




__global__ void
tallyMultiPass(int* intermediateBlock, int* tallyBlock, int* tempTally, unsigned int blockSize)
{
	tallyNeurons(intermediateBlock, tallyBlock, tempTally, blockSize);
}


__global__ void finalizeTally(int* tallyBlock, int* tempTally, int intermediateCount) {
	unsigned int blockSize = BLOCKSIZE;
	int threshhold = THRESHHOLD;
	unsigned int blockCount = intermediateCount / blockSize + (0 != intermediateCount % blockSize);

	//Sets tally at the tallied neuron index at the net index to zero, should it be an intermediate neurorn with a tally greater than the threshhold
	tallyBlock[threadIdx.x + blockDim.x*blockIdx.x] = tallyBlock[threadIdx.x + blockDim.x*blockIdx.x] * 
		(!(threadIdx.x <intermediateCount && threshhold <= tallyBlock[threadIdx.x + blockDim.x*blockIdx.x]));

	int totalDelta = 0;
	int* tempNeuronTally = tempTally + blockDim.x*blockIdx.x * blockCount + threadIdx.x*blockCount;
	for (int i = 0; i < blockCount; i++) {
		totalDelta += tempNeuronTally[i];
	}

	int rawSet = tallyBlock[threadIdx.x + blockDim.x*blockIdx.x] + totalDelta;

	tallyBlock[threadIdx.x + blockDim.x*blockIdx.x] = rawSet * (rawSet > 0);


}

bool isPow2(unsigned int x)
{
	return ((x&(x - 1)) == 0);
}

/*void reduce(int size, int threads, int blocks, int *d_idata, int *d_odata)
{
	dim3 dimBlock(threads, 1, 1);
	dim3 dimGrid(blocks, 1, 1);
	int smemSize = (threads <= 32) ? 2 * threads * sizeof(float) : threads * sizeof(float);

	// choose which of the optimized versions of reduction to launch
	if (isPow2(size))
	{
		switch (threads)
		{
		case 512:
			reduceMultiPass<512, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 256:
			reduceMultiPass<256, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 128:
			reduceMultiPass<128, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 64:
			reduceMultiPass< 64, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 32:
			reduceMultiPass< 32, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 16:
			reduceMultiPass< 16, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  8:
			reduceMultiPass<  8, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  4:
			reduceMultiPass<  4, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  2:
			reduceMultiPass<  2, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  1:
			reduceMultiPass<  1, true> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;
		}
	}
	else
	{
		switch (threads)
		{
		case 512:
			reduceMultiPass<512, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 256:
			reduceMultiPass<256, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 128:
			reduceMultiPass<128, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 64:
			reduceMultiPass< 64, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 32:
			reduceMultiPass< 32, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case 16:
			reduceMultiPass< 16, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  8:
			reduceMultiPass<  8, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  4:
			reduceMultiPass<  4, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  2:
			reduceMultiPass<  2, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;

		case  1:
			reduceMultiPass<  1, false> << < dimGrid, dimBlock, smemSize >> >(d_idata, d_odata, size);
			break;
		}
	}
}
*/


__device__ void tally(int firingNeuronCount, int tallyingNeuronCount, int netCount, int* intermediateBlock, int* tallyBlock, int* tempTally)
{
	unsigned int blockSize = BLOCKSIZE;
	blockSize = firingNeuronCount * (firingNeuronCount <= blockSize) + blockSize * (firingNeuronCount > blockSize);
	unsigned int firingBlocks = firingNeuronCount / blockSize + (0 != firingNeuronCount % blockSize);
	dim3 dimBlock(blockSize, 1, 1);
	dim3 dimGrid(firingBlocks, tallyingNeuronCount, netCount);
	int sharedMemSize = blockSize * sizeof(int);

	tallyMultiPass<< < dimGrid, dimBlock, sharedMemSize>> >(intermediateBlock, tallyBlock, tempTally, firingNeuronCount);

}


__device__ void sensoryTally(int netCount, int* sensoryBlock, unsigned int sensoryCount, int* tallyBlock, int* tempTally, unsigned int sensoryIndex, int tallyingNeuronCount )
{
	unsigned int blockSize = BLOCKSIZE;
	blockSize = tallyingNeuronCount * (tallyingNeuronCount <= blockSize) + blockSize * (blockSize < tallyingNeuronCount);
	unsigned int tallyingBlockCount = tallyingNeuronCount / blockSize + (0 != tallyingNeuronCount % blockSize);
	dim3 dimGrid(tallyingBlockCount, netCount);

	AddSensoryTally << < dimGrid, blockSize>> >(sensoryBlock, sensoryCount, tallyBlock, tempTally, sensoryIndex, tallyingNeuronCount);

}

__global__ void cudaRunCycle(int sensoryCount, int intermediateCount, int motorCount, int netCount, int* sensoryBlock, int* intermediateBlock, int* tallyBlock, int* tempTally, int* sensoryInput, int sensoryInputLength) {
	for (int i = 0; i < sensoryInputLength; i++) {
		int sensoryIndex = *(sensoryInput + i);
		int tallyingNeuronCount = intermediateCount + motorCount;
		sensoryTally(netCount, sensoryBlock, sensoryCount, tallyBlock, tempTally, sensoryIndex, tallyingNeuronCount);
		for (int insideLoop = 0; insideLoop < 10; insideLoop++) {
			tally(intermediateCount, tallyingNeuronCount, netCount, intermediateBlock, tallyBlock, tempTally);
			finalizeTally << <netCount, tallyingNeuronCount >> >(tallyBlock, tempTally, intermediateCount);
		}
	}
}


extern "C"
{

	/*__declspec(dllexport) int* testCollapse(int size, int pad_to) {
		int *toCollapse = (int*)malloc(size *sizeof(int));
		int *g_toCollapse;
		cudaMalloc(&g_toCollapse, size * sizeof(int));
		int *g_collapseResult;
		cudaMalloc(&g_collapseResult, size * sizeof(int));

		int threads, blocks;
		if (size < 512) {
			threads = size / 4;
			if ((size & 3) == 0) {
				blocks = 4;
			}
			else {
				blocks = 5;
			}
		}
		else {
			threads = 512;
			if (size % 512 == 0) {
				blocks = size / 512;
			}
			else {
				blocks = size / 512 + 1;
			}
		}
		dim3 blockDim(threads, 1, 1);
		increment <<<blocks, blockDim >>>(g_toCollapse, 1, 1,size,pad_to);
		
		
		reduce(size,threads,blocks,g_toCollapse,g_collapseResult);
		

		cudaMemcpy(toCollapse, g_collapseResult, size * sizeof(int), cudaMemcpyDeviceToHost);
		cudaFree(g_toCollapse);
		cudaFree(g_collapseResult);
		return toCollapse;
	}*/




	__declspec(dllexport) int blockSize() {
		return BLOCKSIZE;
	}

	__declspec(dllexport) void release(int* toRelease) {
		free(toRelease);
	}

	__declspec(dllexport) void cuda_release(int* toRelease) {
		cudaFree(toRelease);
	}

	//int synapseListLength = (intermediateCount + motorCount)*netCount;
	//int sensoryBlockSize = sensoryCount*synapseListLength;
	//int intermediateBlockSize = intermediateCount;
	//int tallyGridSize = (intermediateCount + motorCount)*netCount;
	//int activeBlockSize = synapseListLength*(intermediateCount + 1)*netCount;
	//int totalSize = sensoryBlockSize + intermediateBlockSize + tallyGridSize + activeBlockSize;
	__declspec(dllexport) int* declare_transfer_block(int totalSize) {
		int* transfer_pointer = (int*)malloc(totalSize*sizeof(int));
		cudaError_t err = cudaGetLastError();

		if (cudaSuccess != err)
		{
			const char * errorString = cudaGetErrorString(err);
		}
		return transfer_pointer;


	}

	__declspec(dllexport) int* establish_net_block(int* toEstablish, int totalSize) {
		int* net_block;
		int memSize = totalSize * sizeof(int);
		cudaMalloc(&net_block, memSize);

		cudaMemcpy(net_block, toEstablish, memSize,cudaMemcpyHostToDevice);
		return net_block;
	}

	__declspec(dllexport) int* findIntermediateBlock(int* sensoryBlock, int sensoryCount, int intermediateCount, int motorCount, int netCount) {
		int sensoryBlockSize = sensoryCount*(intermediateCount + motorCount) * netCount;
		return sensoryBlock + sensoryBlockSize;
	}

	__declspec(dllexport) int* findTallyBlock(int* intermediateBlock, int intermediateCount, int motorCount, int netCount) {
		int intermediateSize = intermediateCount*(intermediateCount + motorCount)*netCount;
		return intermediateBlock + intermediateSize;
	}

	__declspec(dllexport) int* findTempTallyBlock(int* tallyBlock, int intermediateCount, int motorCount, int netCount) {
		int tallySize = (intermediateCount + motorCount)*netCount;
		return tallyBlock + tallySize;
	}

	__declspec(dllexport) void runCycle(int sensoryCount, int intermediateCount, int motorCount, int netCount, int* sensoryBlock, int* intermediateBlock, int* tallyBlock, int* tempTally, int* sensoryInput, int sensoryInputLength) {
		cudaRunCycle << <1, 1 >> > (sensoryCount, intermediateCount, motorCount, netCount, sensoryBlock, intermediateBlock, tallyBlock, tempTally, sensoryInput, sensoryInputLength);
		
		const char * errorString;
		cudaError_t err = cudaGetLastError();

		if (cudaSuccess != err)
		{
			errorString = cudaGetErrorString(err);
		}
		/*int firingNeuronCount = intermediateCount+1;
		int tallyingNeuronCount = intermediateCount + motorCount;
		tally(firingNeuronCount, tallyingNeuronCount, netCount, sensoryBlock, intermediateBlock, tallyBlock, tempTally, sensoryIndex, sensoryCount);
		firingNeuronCount = intermediateCount;
		for (int insideLoop = 0; insideLoop < 10; insideLoop++) {
			tally(firingNeuronCount, tallyingNeuronCount, netCount, intermediateBlock, tallyBlock, tempTally);
			finalizeTally << <netCount, tallyingNeuronCount >> >(tallyBlock, tempTally, intermediateCount, firingNeuronCount);
		}*/

	}

	__declspec(dllexport) int* getTally(int* tallyBlock, int tallyingNeuronCount) {
		int* tallyReport = (int*)malloc(tallyingNeuronCount*sizeof(int));
		cudaMemcpy(tallyReport, tallyBlock, tallyingNeuronCount * sizeof(int), cudaMemcpyDeviceToHost);
		return tallyReport;
	}

	__declspec(dllexport) int* getNet(int* encodedNet, int netSize) {
		int* netReport = (int*)malloc(netSize * sizeof(int));
		cudaMemcpy(netReport, encodedNet, netSize * sizeof(int), cudaMemcpyDeviceToHost);
		return netReport;
	}


	__declspec(dllexport) int* establish_sensory_input(int* toEstablish, int totalSize) {
		int* net_block;
		int memSize = totalSize * sizeof(int);
		cudaMalloc(&net_block, memSize);

		cudaMemcpy(net_block, toEstablish, memSize, cudaMemcpyHostToDevice);
		return net_block;
	}
}
