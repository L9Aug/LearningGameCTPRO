using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    [System.Serializable]
    public class NeuralNetLayer
    {

        public NeuralNetNode[] Nodes;
        

        public NeuralNetLayer(int NumNodes, int NextLayerNodeCount)
        {
            Nodes = new NeuralNetNode[NumNodes];
            for(int i = 0; i < NumNodes; ++i)
            {
                Nodes[i] = new NeuralNetNode(NextLayerNodeCount);
            }
        }
    }
}