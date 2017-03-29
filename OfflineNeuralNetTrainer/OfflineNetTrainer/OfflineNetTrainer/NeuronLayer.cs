// core neural net functionality based on work from : http://neuralnetworksanddeeplearning.com/chap1.html

using System.Collections;
using System.Collections.Generic;

namespace NeuralNet
{

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

        public NeuronLayer(int NumNodes, string[] NeuronData, NeuronLayer PreviousLayer)
        {
            Nodes = new SigmoidNeuron[NumNodes];

            for(int i = 0; i < NumNodes; ++i)
            {
                Nodes[i] = new SigmoidNeuron((NeuronData.Length > 0) ? NeuronData[i] : null, PreviousLayer);
            }
        }
    }
}