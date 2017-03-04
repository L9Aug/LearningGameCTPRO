using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    [System.Serializable]
    public class SigmoidNeuron
    {
        public float[] Weights;
        public SigmoidNeuron[] Inputs;
        public float Bias;
        public float Activation;

        public float NablaBias;
        public float[] NablaWeights;
        public float Delta;

        public float z;

        public SigmoidNeuron(NeuronLayer PreviousLayer)
        {
            if (PreviousLayer != null)
            {
                Weights = new float[PreviousLayer.Nodes.Length];
                Inputs = new SigmoidNeuron[PreviousLayer.Nodes.Length];
                NablaWeights = new float[PreviousLayer.Nodes.Length];

                for (int i = 0; i < PreviousLayer.Nodes.Length; ++i)
                {
                    Inputs[i] = PreviousLayer.Nodes[i];
                    Weights[i] = Random.Range(-1f, 1f);
                    NablaWeights[i] = 0;
                }

                Bias = Random.Range(-1f, 1f);
            }
            else
            {
                Bias = 0;
            }
        }

        public float GetZ()
        {
            float SumWI = 0;

            for(int i = 0; i < Inputs.Length; ++i)
            {
                SumWI += Weights[i] * Inputs[i].Activation;
            }

            z = SumWI + Bias;

            return z;
        }

    }

}