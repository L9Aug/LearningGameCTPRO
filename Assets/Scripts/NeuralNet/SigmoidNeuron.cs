// core neural net functionality based on work from : http://neuralnetworksanddeeplearning.com/chap1.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{

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

        public SigmoidNeuron(string NeuronData, NeuronLayer PreviousLayer)
        {
            if (PreviousLayer != null)
            {
                string[] DataSplit = NeuronData.Split('|');
                string[] WeightsData = DataSplit[0].Split(',');

                Bias = float.Parse(DataSplit[1]);

                Weights = new float[WeightsData.Length];
                Inputs = new SigmoidNeuron[WeightsData.Length];
                NablaWeights = new float[PreviousLayer.Nodes.Length];

                for (int i = 0; i < WeightsData.Length; ++i)
                {
                    Weights[i] = float.Parse(WeightsData[i]);
                    Inputs[i] = PreviousLayer.Nodes[i];
                    NablaWeights[i] = 0;
                }
            }
            else
            {
                Bias = 0;
            }
        }

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

        public void ResetNablas()
        {
            NablaBias = 0;
            for(int i = 0; i < NablaWeights.Length; ++i)
            {
                NablaWeights[i] = 0;
            }
        }

    }

}