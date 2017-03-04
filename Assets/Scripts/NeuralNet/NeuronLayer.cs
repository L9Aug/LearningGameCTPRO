using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    [System.Serializable]
    public class NeuronLayer
    {

        public SigmoidNeuron[] Nodes;        

        public NeuronLayer(int NumNodes, NeuronLayer PreviousLayer)
        {
            Nodes = new SigmoidNeuron[NumNodes];
            for(int i = 0; i < NumNodes; ++i)
            {
                Nodes[i] = new SigmoidNeuron(PreviousLayer);
            }
        }
    }
}